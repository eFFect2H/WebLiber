using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MyWebLib
{
    /// <summary>
    /// Валидатор, проверяющий доступность сущностей по атрибуту RequiredAvailable.
    /// </summary>
    public class AttributeAvailabilityValidator<TDto> : IAvailabilityValidator<TDto>
    {
        private readonly DbContext _context;

        public AttributeAvailabilityValidator(DbContext context)
        {
            _context = context;
        }

        public async Task<bool> IsAvailableAsync(TDto dto)
        {
            var dtoType = typeof(TDto);
            var attributes = dtoType.GetCustomAttributes<RequiredAvailableAttribute>();

            foreach (var attr in attributes)
            {
                var fkProp = dtoType.GetProperty(attr.ForeignKeyProperty);
                if (fkProp == null) return false;

                var fkValue = fkProp.GetValue(dto);
                if (fkValue == null) return false;

                var setMethod = typeof(DbContext).GetMethod(nameof(DbContext.Set), Type.EmptyTypes);
                var genericSet = setMethod!.MakeGenericMethod(attr.EntityType);
                var dbSet = genericSet.Invoke(_context, null);

                var findAsync = dbSet!.GetType().GetMethod("FindAsync", new[] { typeof(object[]) });
                var task = (Task)findAsync!.Invoke(dbSet, new object[] { new object[] { fkValue! } })!;
                await task.ConfigureAwait(false);

                var resultProp = task.GetType().GetProperty("Result");
                var entity = resultProp!.GetValue(task);
                if (entity == null) return false;

                var isAvailableProp = attr.EntityType.GetProperty("IsAvailable");
                if (isAvailableProp == null || isAvailableProp.PropertyType != typeof(bool)) return false;

                var isAvailable = (bool?)isAvailableProp.GetValue(entity);
                if (isAvailable != true)
                    return false;
            }

            return true;
        }



        /// Пример DTO с атрибутом
        /*
        [RequiredAvailable("TableID", typeof(Table))]
        public class BookingDTO
        {
            public int BookingID { get; set; }
            public int UserID { get; set; }
            public int TableID { get; set; }
            public DateTime BookingDate { get; set; }
            public TimeSpan StartTime { get; set; }
            public TimeSpan EndTime { get; set; }
            public string Status { get; set; } = "booked";
        }
        */

        /// Регистрация в DI
        // builder.Services.AddScoped(typeof(IAvailabilityValidator<>), typeof(AttributeAvailabilityValidator<>));
    }

}

