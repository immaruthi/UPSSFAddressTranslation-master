/// <summary> 
/// IoCContainer.cs-
/// Pallamalli Maruthi - Pactera IDC (Hyderabad)
/// </summary>

using AtService.Extensions;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.ObjectBuilder;
using Quincus.Implementations;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using UPS.Application.CustomLogs;

namespace AtService.CustomConatiner
{
    public static class IoCContainer
    {
        private static readonly Lazy<Dictionary<ImplementationType, int>> ImplementationTypePriorities = new Lazy<Dictionary<ImplementationType, int>>(() =>
           Enum.GetValues(typeof(ImplementationType)).Cast<ImplementationType>().ToDictionary(i => i, i => (int)i));


        private static readonly Lazy<UnityContainer> LazyUnityContainer = new Lazy<UnityContainer>(() =>
        {
            var unityContainer = new UnityContainer();

            // Add the extension to auto inject properties of type IAction
            unityContainer.AddNewExtension<PropertyInjectorUnityContainerExtension>();

            // Auto register all IActions
            RegisterTypes(unityContainer, new APIServiceAssemblyReference());
            RegisterTypes(unityContainer, new LogServiceAssemblyReference());


            return unityContainer;
        });


        private static void RegisterTypes(UnityContainer unityContainer, params AssemblyReferenceBase[] assemblyReferences)
        {
            // First validate that there aren't 2 assemblies with the same ImplementationType
            var duplicateImplementationTypes = assemblyReferences.GroupBy(r => r.ImplementationType).Where(r => r.Count() > 1);
            if (duplicateImplementationTypes.Any())
            {
                throw new Exception(duplicateImplementationTypes.Select(d => String.Format("Both {1} are registered as ImplementationType '{0}', there can be only a single assemble registered with any given ImplementationType.",
                                                                                            d.Key,
                                                                                            d.Select(r => "'" + r.GetType().Name + "'").Aggregate((s1, s2) => s1 + " and " + s2)))
                    .Aggregate((s1, s2) => s1 + "\r\n-----------------------------------\r\n" + s2));
            }

            // Get all types that implement IAction
            var actionImplementations = assemblyReferences
                .SelectMany(r => AllClasses.FromAssemblies(r.GetType().Assembly)
                    .Where(t => typeof(IAction).IsAssignableFrom(t))
                    .Select(t => new
                    {
                        Type = t,
                        r.ImplementationType
                    }));

            // Get all interfaces from those types that inherit from IAction
            var actionsToRegister = actionImplementations
                .SelectMany(t => WithMappings.FromAllInterfaces(t.Type)
                    .Where(i => i != typeof(IAction) && typeof(IAction).IsAssignableFrom(i))
                    .Concat(new[] { t.Type }) // We also want to register the type directly
                    .Select(i => new
                    {
                        Interface = i,
                        t.Type,
                        t.ImplementationType
                    }));

            // Group the actions together and sort the implementations by priority
            var actionsGroupedByPriority = actionsToRegister
                .GroupBy(i => i.Interface)
                .Select(i => new
                {
                    Interface = i.Key,
                    Implementations = i.OrderBy(t => ImplementationTypePriorities.Value[t.ImplementationType]).ToList()
                });

            foreach (var action in actionsGroupedByPriority)
            {
                // Register the default implementation (the implementation with the highest priority)
                unityContainer.RegisterType(action.Interface, action.Implementations.First().Type);

                foreach (var implementation in action.Implementations)
                {
                    // Register all implementations as named registrations
                    unityContainer.RegisterType(action.Interface, implementation.Type, implementation.ImplementationType.ToString());
                }
            }
        }

        private class PropertyInjectorUnityContainerExtension : UnityContainerExtension
        {
            protected override void Initialize()
            {
                Context.Strategies.Add(new PropertyInjectionBuilderStrategy(Container), UnityBuildStage.Initialization);
            }

            private class PropertyInjectionBuilderStrategy : BuilderStrategy
            {
                private static readonly ConcurrentDictionary<Type, Lazy<List<PropertyInfo>>> InjectableProperties = new ConcurrentDictionary<Type, Lazy<List<PropertyInfo>>>();

                private readonly IUnityContainer UnityContainer;

                public PropertyInjectionBuilderStrategy(IUnityContainer unityContainer)
                {
                    UnityContainer = unityContainer;
                }

                public override void PreBuildUp(IBuilderContext context)
                {
                    foreach (var property in GetInjectableProperties(context.BuildKey.Type))
                    {
                        property.SetValue(context.Existing, UnityContainer.Resolve(property.PropertyType));
                    }
                }

                public override void PreTearDown(IBuilderContext context)
                {
                    foreach (var property in GetInjectableProperties(context.Existing.GetType()))
                    {
                        var disposable = property.GetValue(context.Existing) as IDisposable;

                        if (disposable != null)
                            disposable.Dispose();
                    }
                }

                private List<PropertyInfo> GetInjectableProperties(Type t)
                {
                    return InjectableProperties.GetOrAdd(t, k => new Lazy<List<PropertyInfo>>(() =>
                        k.GetRuntimeProperties()
                            .Where(p => p.SetMethod != null
                                        && p.GetMethod != null
                                        && p.SetMethod.IsPublic
                                        && typeof(IAction).IsAssignableFrom(p.PropertyType)).ToList())).Value;
                }
            }
        }

        public static void BuildUp(object obj)
        {
            BuildUp(obj.GetType(), obj);
        }

        public static void BuildUp(Type t, object obj)
        {
            var builtUpObj = LazyUnityContainer.Value.BuildUp(t, obj);

            if (obj != builtUpObj)
                throw new Exception("BuildUp created it's own object and did not just add property dependencies.");
        }

        public static void Teardown(object obj)
        {
            LazyUnityContainer.Value.Teardown(obj);
        }

    }

}
