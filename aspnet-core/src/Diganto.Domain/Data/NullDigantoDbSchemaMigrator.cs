using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Diganto.Data
{
    /* This is used if database provider does't define
     * IDigantoDbSchemaMigrator implementation.
     */
    public class NullDigantoDbSchemaMigrator : IDigantoDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}