using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;
using Volo.Abp.TextTemplating;

namespace Diganto.Emailing
{
    public class EmailTemplateDefinitionProvider : TemplateDefinitionProvider, ITransientDependency
    {
        public override void Define(ITemplateDefinitionContext context)
        {
            context.Add(new TemplateDefinition(
                name: CustomEmailTemplates.Layout,
                isLayout: true
                ).WithVirtualFilePath("/Emailing/Templates/Layout/CustomLayout.html", isInlineLocalized: true));

            context.Add(new TemplateDefinition(
                name: CustomEmailTemplates.ConfirmationEmail,
                layout: CustomEmailTemplates.Layout
                ).WithVirtualFilePath("/Emailing/Templates/ConfirmationEmail/ConfirmationEmail.html", isInlineLocalized: true));
        }
    }
}
