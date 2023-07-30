using System;
using System.Collections.Generic;
using System.Text;
using Diganto.Localization;
using Volo.Abp.Application.Services;

namespace Diganto
{
    /* Inherit your application services from this class.
     */
    public abstract class DigantoAppService : ApplicationService
    {
        protected DigantoAppService()
        {
            LocalizationResource = typeof(DigantoResource);
        }
    }
}
