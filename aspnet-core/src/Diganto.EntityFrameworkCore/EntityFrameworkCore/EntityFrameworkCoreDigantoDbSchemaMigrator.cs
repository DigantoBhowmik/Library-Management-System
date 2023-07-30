using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Diganto.Data;
using Volo.Abp.DependencyInjection;

namespace Diganto.EntityFrameworkCore
{
    public class EntityFrameworkCoreDigantoDbSchemaMigrator
        : IDigantoDbSchemaMigrator, ITransientDependency
    {
        private readonly IServiceProvider _serviceProvider;

        public EntityFrameworkCoreDigantoDbSchemaMigrator(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task MigrateAsync()
        {
            /* We intentionally resolving the DigantoDbContext
             * from IServiceProvider (instead of directly injecting it)
             * to properly get the connection string of the current tenant in the
             * current scope.
             */

            await _serviceProvider
                .GetRequiredService<DigantoDbContext>()
                .Database
                .MigrateAsync();
        }
    }
}
