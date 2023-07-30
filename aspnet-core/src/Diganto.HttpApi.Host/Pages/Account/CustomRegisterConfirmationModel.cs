using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;

namespace Diganto.Pages.Account
{
    public class CustomRegisterConfirmationModel : PageModel
    {
        private readonly IdentityUserManager _userManager;
        private readonly IEmailSender _sender;
        public bool DisplayConfirmAccountLink { get; set; }
        public string EmailConfirmationUrl { get; set; }
        private readonly IConfiguration _configuration;

        [BindProperty(SupportsGet = true)]
        public string ReturnUrl { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ReturnUrlHash { get; set; }

        public CustomRegisterConfirmationModel(IdentityUserManager userManager, IConfiguration configuration, IEmailSender sender)
        {
            _userManager = userManager;
            _sender = sender;
            _configuration = configuration;
        }

        public async Task<IActionResult> OnGetAsync(string email, string returnUrl = null)
        {
            if (email == null)
            {
                return RedirectToPage("/Index");
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return NotFound($"Unable to load user with email '{email}'.");
            }

            // TODO Set to false if you no longer want to display the Account/ConfirmEmail page
            DisplayConfirmAccountLink = false;
            if (DisplayConfirmAccountLink)
            {
                var userId = await _userManager.GetUserIdAsync(user);
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                var url = _configuration["App:SelfUrl"];
                EmailConfirmationUrl = $"{url}/Account/ConfirmEmail?userId={user.Id}&code={code}";
                //EmailConfirmationUrl = Url.Page(
                //    "/Account/ConfirmEmail",
                //    pageHandler: null,
                //    values: new { userId = userId, code = code },
                //    protocol: Request.Scheme);
            }
            return Page();
        }
    }
}