using System;
using System.Collections.Generic;
using System.Text;

namespace UPS.ServicesAsyncActions
{
    public interface IEntityValidationService
    {
        bool IsValidEntity<T>(T entity) where T:class;
        List<T> FilterValidEntity<T>(List<T> entity) where T : class;
    }
}
