using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace CNG.EntityFrameworkCore
{
    public class SoftDeleteModelCustomizer : ModelCustomizer
    {
        public SoftDeleteModelCustomizer(ModelCustomizerDependencies dependencies)
            : base(dependencies)
        {
        }

        public override void Customize(ModelBuilder modelBuilder, DbContext context)
        {
            base.Customize(modelBuilder, context);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var isDeletedProperty = entityType.FindProperty("IsDeleted");
                if (isDeletedProperty == null || isDeletedProperty.ClrType != typeof(bool)) continue;
                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var propertyMethod = typeof(EF).GetMethod("Property")?.MakeGenericMethod(typeof(bool));
                if(propertyMethod is null)continue;
                var propertyAccess = Expression.Call(propertyMethod, parameter, Expression.Constant("IsDeleted"));
                var compareExpression = Expression.Equal(propertyAccess, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                // Global query filter olarak ekle
                entityType.SetQueryFilter(lambda);
            }
        }
    }
}
