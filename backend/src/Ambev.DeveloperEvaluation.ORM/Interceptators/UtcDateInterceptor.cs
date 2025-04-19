using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ambev.DeveloperEvaluation.ORM.Interceptators;

public class UtcDateInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;

        foreach (var entry in context?.ChangeTracker.Entries() ?? [])
        {
            foreach (var prop in entry.Properties
                .Where(p => p.Metadata.ClrType == typeof(DateTime)))
            {
                if (prop.CurrentValue is DateTime dt && dt.Kind != DateTimeKind.Utc)
                {
                    prop.CurrentValue = DateTime.SpecifyKind(dt, DateTimeKind.Utc);
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
