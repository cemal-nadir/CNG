using Microsoft.EntityFrameworkCore.Diagnostics;
using CNG.EntityFrameworkCore.Extensions;

namespace CNG.EntityFrameworkCore.Interceptors
{
    public class CreateUpdateInterceptor : SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(
            DbContextEventData eventData,
            InterceptionResult<int> result)
        {
            eventData.SignSignatures();
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
        {
            eventData.SignSignatures();
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }
    }
}
