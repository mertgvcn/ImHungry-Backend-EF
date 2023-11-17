using AutoMapper.Internal;
using ImHungryBackendER.Services.OtherServices.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using TypeSupport.Extensions;

namespace ImHungryBackendER.Services.OtherServices
{
    public class DbOperationHelperService : IDbOperationHelperService
    {
        public void MarkModifiedProperties<T>(T entity, ImHungryContext context)
        {
            PropertyInfo[] Properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in Properties)
            {
                //Skip if property value is not null.
                if (property.GetValue(entity) != null) continue;

                //Finding out whether the property is a collection, reference or normal property
                if (property.PropertyType.IsCollection())
                {
                    context.Entry(entity).Collection(property.Name).IsModified = false;
                }
                else if (property.PropertyType.GetExtendedType().IsReferenceType && property.PropertyType != typeof(string))
                {
                    context.Entry(entity).Reference(property.Name).IsModified = false;
                }
                else
                {
                    context.Entry(entity).Property(property.Name).IsModified = false;
                }
            }
        }

        //_context.Entry(user).Property(a => a.Password).IsModified = false;
    }
}
