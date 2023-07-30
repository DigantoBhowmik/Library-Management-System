using Volo.Abp.Modularity;

namespace Diganto
{
    [DependsOn(
        typeof(DigantoApplicationModule),
        typeof(DigantoDomainTestModule)
        )]
    public class DigantoApplicationTestModule : AbpModule
    {

    }
}