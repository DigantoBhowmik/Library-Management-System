using System.Threading.Tasks;

namespace Diganto.Data
{
    public interface IDigantoDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
