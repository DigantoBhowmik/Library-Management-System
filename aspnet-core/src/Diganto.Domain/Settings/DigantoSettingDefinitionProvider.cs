using Volo.Abp.Settings;

namespace Diganto.Settings
{
    public class DigantoSettingDefinitionProvider : SettingDefinitionProvider
    {
        public override void Define(ISettingDefinitionContext context)
        {
            //Define your own settings here. Example:
            //context.Add(new SettingDefinition(DigantoSettings.MySetting1));
        }
    }
}
