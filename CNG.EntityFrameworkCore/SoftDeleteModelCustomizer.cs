using System.Linq.Expressions;
using CNG.Abstractions.Signatures;
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
                if (!typeof(ISoftDeletable).IsAssignableFrom(entityType.ClrType))
                    continue;

                var parameter = Expression.Parameter(entityType.ClrType, "e");
                var converted = Expression.Convert(parameter, typeof(ISoftDeletable));
                var isDeletedProperty = Expression.Property(converted, nameof(ISoftDeletable.IsDeleted));
                var compareExpression = Expression.Equal(isDeletedProperty, Expression.Constant(false));
                var lambda = Expression.Lambda(compareExpression, parameter);

                entityType.SetQueryFilter(lambda);
            }
        }
    }
}
