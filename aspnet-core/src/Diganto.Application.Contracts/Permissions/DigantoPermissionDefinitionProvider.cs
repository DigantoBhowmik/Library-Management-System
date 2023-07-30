using Diganto.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Diganto.Permissions
{
    public class DigantoPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var bookStoreGroup = context.AddGroup(DigantoPermissions.GroupName, L("Permission:Diganto"));
            var booksPermission = bookStoreGroup.AddPermission(DigantoPermissions.Books.Default, L("Permission:Books"));
            booksPermission.AddChild(DigantoPermissions.Books.Create, L("Permission:Books.Create"));
            booksPermission.AddChild(DigantoPermissions.Books.Edit, L("Permission:Books.Edit"));
            booksPermission.AddChild(DigantoPermissions.Books.Delete, L("Permission:Books.Delete"));
            //Define your own permissions here. Example:
            //myGroup.AddPermission(DigantoPermissions.MyPermission1, L("Permission:MyPermission1"));


        }


        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DigantoResource>(name);
        }
    }
}
