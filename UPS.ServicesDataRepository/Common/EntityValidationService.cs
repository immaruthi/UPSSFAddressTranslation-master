using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using UPS.ServicesAsyncActions;

namespace UPS.ServicesDataRepository.Common
{
    public class EntityValidationServic : IEntityValidationService
    {
        public List<T> FilterValidEntity<T>(List<T> entities) where T : class
        {
            List<T> validEntities = new List<T>();
            entities.ForEach(entity =>
            {
                if (IsValidEntity<T>(entity))
                {
                    validEntities.Add(entity);
                }
            });

            return validEntities;
        }

        public bool IsValidEntity<T>(T entity) where T : class
        {
            var context = new ValidationContext(entity, null, null);
            var validationResult = new List<ValidationResult>();
            var isValid = Validator.TryValidateObject(entity, context, validationResult, true);
            foreach (var str in validationResult)
            {
                //Need to log error
                Console.WriteLine(str.ErrorMessage.ToString());
            }
            return isValid;
        }
    }
}
