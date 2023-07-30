using Diganto.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Diganto
{
    [DependsOn(
        typeof(DigantoEntityFrameworkCoreTestModule)
        )]
    public class DigantoDomainTestModule : AbpModule
    {

    }
}