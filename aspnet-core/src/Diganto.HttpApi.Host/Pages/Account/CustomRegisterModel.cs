using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Diganto.Email;
using Diganto.Emailing;
using Diganto.Emailing.Templates.ConfirmationEmail;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Account;
using Volo.Abp.Account.Settings;
using Volo.Abp.Account.Web.Pages.Account;
using Volo.Abp.Auditing;
using Volo.Abp.Emailing;
using Volo.Abp.Emailing.Templates;
using Volo.Abp.Identity;
using Volo.Abp.Security.Claims;
using Volo.Abp.Settings;
using Volo.Abp.TextTemplating;
using Volo.Abp.Validation;
using static System.Net.WebRequestMethods;
using static Volo.Abp.Identity.Settings.IdentitySettingNames;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Diganto.Pages.Account
{
    public class CustomRegisterModel : AccountPageModel
    {

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        [BindProperty]
        public PostInput Input { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool IsExternalLogin { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ExternalLoginAuthSchema { get; set; }

        private readonly EmailService _emailService;
        private readonly IEmailSender _emailSender;
        private Volo.Abp.Identity.IdentityUser _abpIdentityUser;
        private readonly ISettingEncryptionService _settingEncryptionService;
        private readonly ITemplateRenderer _templateRenderer;
        private readonly IConfiguration _configuration;

        public CustomRegisterModel(IAccountAppService accountAppService, IdentityUserManager usermanager, EmailService emailService, IEmailSender emailSender, ISettingEncryptionService settingEncryptionService, ITemplateRenderer templateRenderer)
        {
            AccountAppService = accountAppService;
            UserManager = usermanager;
            _emailService = emailService;
            _emailSender = emailSender;
            _settingEncryptionService = settingEncryptionService;
            _templateRenderer = templateRenderer;

        }

        public virtual async Task<IActionResult> OnGetAsync()
        {
            var test = _settingEncryptionService.Encrypt(new SettingDefinition("Abp.Mailing.Smtp.Password"), "Cbl@12345");
            await CheckSelfRegistrationAsync();
            await TrySetEmailAsync();
            return Page();
        }

        private async Task TrySetEmailAsync()
        {
            if (IsExternalLogin)
            {
                var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    return;
                }

                if (!externalLoginInfo.Principal.Identities.Any())
                {
                    return;
                }

                var identity = externalLoginInfo.Principal.Identities.First();
                var emailClaim = identity.FindFirst(ClaimTypes.Email);

                if (emailClaim == null)
                {
                    return;
                }

                Input = new PostInput { EmailAddress = emailClaim.Value };
            }
        }

        public virtual async Task<IActionResult> OnPostAsync()
        {
            try
            {
                await CheckSelfRegistrationAsync();

                if (IsExternalLogin)
                {
                    var externalLoginInfo = await SignInManager.GetExternalLoginInfoAsync();
                    if (externalLoginInfo == null)
                    {
                        Logger.LogWarning("External login info is not available");
                        return RedirectToPage("./Login");
                    }

                    await RegisterExternalUserAsync(externalLoginInfo, Input.EmailAddress);
                }
                else
                {
                    await RegisterLocalUserAsync();
                    return RedirectToPage("RegisterConfirmation", new { email = Input.EmailAddress, returnUrl = ReturnUrl });
                }


                return Redirect(ReturnUrl ?? "~/"); //TODO: How to ensure safety? IdentityServer requires it however it should be checked somehow!
            }
            catch (BusinessException e)
            {
                Alerts.Danger(GetLocalizeExceptionMessage(e));
                return Page();
            }
        }

        protected virtual async Task RegisterLocalUserAsync()
        {
            //await DoItAsync();

            ValidateModel();
            //var userDto = new CustomRegisterDto();
            //userDto.Id = GuidGenerator.Create();
            //userDto.AppName = "MVC";
            //userDto.EmailAddress = Input.EmailAddress;
            //userDto.Password = Input.Password;
            //userDto.UserName = Input.UserName;
            //userDto.Name = Input.Name;
            //userDto.Surname = Input.SurName;
            var userDto = await AccountAppService.RegisterAsync(
                new RegisterDto
                {
                    AppName = "MVC",
                    EmailAddress = Input.EmailAddress,
                    Password = Input.Password,
                    UserName = Input.UserName
                }
            );
            //await _emailService.SendEmailAsync(Input.EmailAddress);
            var user = await UserManager.GetByIdAsync(userDto.Id);
            user.Name = Input.Name;
            user.Surname = Input.SurName;
            var phoneNumber = Input.Phone;
            if (!phoneNumber.IsNullOrWhiteSpace())
            {
                var phoneNumberConfirmed = string.Equals(Input.Phone, "true", StringComparison.InvariantCultureIgnoreCase);
                user.SetPhoneNumber(phoneNumber, phoneNumberConfirmed);
            }
            await UserManager.UpdateAsync(user);

            //await SignInManager.SignInAsync(user, isPersistent: true);
            await SendEmailToAskForEmailConfirmationAsync(user);
        }

        protected virtual async Task RegisterExternalUserAsync(ExternalLoginInfo externalLoginInfo, string emailAddress)
        {
            await IdentityOptions.SetAsync();

            var user = new IdentityUser(GuidGenerator.Create(), emailAddress, emailAddress, CurrentTenant.Id);

            (await UserManager.CreateAsync(user)).CheckErrors();
            (await UserManager.AddDefaultRolesAsync(user)).CheckErrors();

            var userLoginAlreadyExists = user.Logins.Any(x =>
                x.TenantId == user.TenantId &&
                x.LoginProvider == externalLoginInfo.LoginProvider &&
                x.ProviderKey == externalLoginInfo.ProviderKey);

            if (!userLoginAlreadyExists)
            {
                (await UserManager.AddLoginAsync(user, new UserLoginInfo(
                    externalLoginInfo.LoginProvider,
                    externalLoginInfo.ProviderKey,
                    externalLoginInfo.ProviderDisplayName
                ))).CheckErrors();
            }

            await SignInManager.SignInAsync(user, isPersistent: true);
        }

        protected virtual async Task CheckSelfRegistrationAsync()
        {
            if (!await SettingProvider.IsTrueAsync(AccountSettingNames.IsSelfRegistrationEnabled) ||
                !await SettingProvider.IsTrueAsync(AccountSettingNames.EnableLocalLogin))
            {
                throw new UserFriendlyException(L["SelfRegistrationDisabledMessage"]);
            }
        }
        private async Task SendEmailToAskForEmailConfirmationAsync(Volo.Abp.Identity.IdentityUser user)
        {
            var code = await UserManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            var url = "https://localhost:44319";

            var callbackUrl = $"{url}/Account/ConfirmEmail?userId={user.Id}&code={code}";
            callbackUrl = HtmlEncoder.Default.Encode(callbackUrl);
            try
            {
                var emailBody = await _templateRenderer.RenderAsync(
                 CustomEmailTemplates.ConfirmationEmail,
                    new ConfirmationEmailModel()
                    {
                        CallbackUrl = callbackUrl
                    }
                 );
                await _emailSender.SendAsync
                (
                Input.EmailAddress,
                "EmpoweRx- Confirmation Email",
                emailBody
                );
            }
            catch (Exception ex)
            {
                throw new UserFriendlyException(ex.Message);
            }
            //var callbackUrl = Url.Page("/Account/ConfirmEmail", pageHandler: null, values: new { userId = user.Id, code = code }, protocol: Request.Scheme);

            // TODO use EmailService instead of using IEmailSender directly
            //await _emailSender.SendAsync(Input.EmailAddress, "Confirm your email",
            //    $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
        public async Task DoItAsync()
        {
            //try
            //{
            var body = await _templateRenderer.RenderAsync(
            StandardEmailTemplates.Message,
            new
            {
                message = "This is email body..."
            }
        );

            await _emailSender.SendAsync(
                Input.EmailAddress,
                "Email subject",
                body
            );
            //}
            //catch(Exception e)
            //{
            //    throw new UserFriendlyException(e.Message);
            //}

        }
        public class PostInput
        {
            [Required]
            //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string UserName { get; set; }
            [Required]
            //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string Name { get; set; }
            [Required]
            // [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            public string SurName { get; set; }
            [Required]
            // [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
            [RegularExpression(@"^(?:\+?88|0088)?01[15-9]\d{8}$", ErrorMessage = "Enter Bangladeshi Number")]

            public string Phone { get; set; }
            [Required]
            [EmailAddress]
            //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
            public string EmailAddress { get; set; }

            [Required]
            //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
            [DataType(DataType.Password)]
            [DisableAuditing]
            public string Password { get; set; }
        }
    }
}