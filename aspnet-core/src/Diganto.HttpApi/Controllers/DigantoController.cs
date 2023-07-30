using Diganto.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Diganto.Controllers
{
    /* Inherit your controllers from this class.
     */
    public abstract class DigantoController : AbpController
    {
        protected DigantoController()
        {
            LocalizationResource = typeof(DigantoResource);
        }
    }
}