using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    
    // Атрибут для автоматической проверки доступности связанной сущности по ID.

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class RequiredAvailableAttribute : Attribute
    {
        public string ForeignKeyProperty { get; }
        public Type EntityType { get; }

        public RequiredAvailableAttribute(string foreignKeyProperty, Type entityType)
        {
            ForeignKeyProperty = foreignKeyProperty;
            EntityType = entityType;
        }
    }
}
