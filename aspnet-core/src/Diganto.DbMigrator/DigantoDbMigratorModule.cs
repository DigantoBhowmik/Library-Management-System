using Diganto.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Diganto.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(DigantoEntityFrameworkCoreModule),
        typeof(DigantoApplicationContractsModule)
        )]
    public class DigantoDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
