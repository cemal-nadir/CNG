using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CNG.EntityFrameworkCore.Interceptors
{
    public class SoftDeleteInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            SoftDelete(eventData);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            SoftDelete(eventData);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        private void SoftDelete(DbContextEventData eventData)
        {
            var context = eventData.Context;
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State != EntityState.Deleted ||
                    entry.Entity.GetType().GetProperty("IsDeleted") == null) continue;
                entry.State = EntityState.Modified;
                entry.CurrentValues["IsDeleted"] = true;
            }
        }
    }
}
