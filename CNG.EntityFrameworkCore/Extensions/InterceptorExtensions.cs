using CNG.Core;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.Extensions.DependencyInjection;
using CNG.Abstractions.Signatures;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;

namespace CNG.EntityFrameworkCore.Extensions
{
    public static class InterceptorExtensions
    {
        public static string GetUserId()
        {
            var contextAccessor = ServiceTool.ServiceProvider?.GetService<IHttpContextAccessor>();
            if (contextAccessor?.HttpContext is null) return "SYSTEM";
            return contextAccessor.HttpContext.User.Claims
                .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value ?? "SYSTEM";
        }
        public static void SoftDelete(this DbContextEventData eventData)
        {
            var userId = GetUserId();
            var context = eventData.Context;
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry is not { State: EntityState.Deleted, Entity: ISoftDeletable softDeletable }) continue;

                entry.State = EntityState.Modified;
                softDeletable.IsDeleted = true;
                softDeletable.DeletedAt = DateTimeOffset.UtcNow;
                softDeletable.DeletedUserId = string.IsNullOrEmpty(softDeletable.DeletedUserId)
                    ? userId
                    : softDeletable.DeletedUserId;

            }
        }
        public static void SignSignatures(this DbContextEventData eventData)
        {
            var userId = GetUserId();
            var context = eventData.Context;
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry is { State: EntityState.Added, Entity: ICreated creatable })
                {
                    creatable.CreatedAt ??= DateTimeOffset.UtcNow;
                    creatable.CreatedUserId = string.IsNullOrEmpty(creatable.CreatedUserId)
                        ? userId
                        : creatable.CreatedUserId;
                }
                if (entry is { State: EntityState.Modified, Entity: IUpdated updatable })
                {
                    updatable.UpdatedAt ??= DateTimeOffset.UtcNow;
                    updatable.UpdatedUserId = string.IsNullOrEmpty(updatable.UpdatedUserId)
                        ? userId
                        : updatable.UpdatedUserId;
                }
            }
        }
    }
}
