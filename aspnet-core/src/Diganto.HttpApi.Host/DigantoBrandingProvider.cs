using Volo.Abp.DependencyInjection;
using Volo.Abp.Ui.Branding;

namespace Diganto
{
    [Dependency(ReplaceServices = true)]
    public class DigantoBrandingProvider : DefaultBrandingProvider
    {
        public override string AppName => "Diganto";
    }
}
