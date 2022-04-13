namespace IssueSystem.Areas.Identity.Pages.Account
{
#nullable disable

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Text;
    using System.Text.Encodings.Web;
    using System.Threading;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authentication;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;
    using Microsoft.AspNetCore.WebUtilities;
    using Microsoft.Extensions.Logging;

    using IssueSystem.Data.Models;
    using IssueSystem.Services.HelpersServices.SendGrid;
    using IssueSystem.Common;
    using System.Security.Claims;

    public class RegisterModel : PageModel
    {
        private readonly SignInManager<Employee> signInManager;
        private readonly UserManager<Employee> userManager;
        private readonly IUserStore<Employee> userStore;
        private readonly IUserEmailStore<Employee> emailStore;
        private readonly ILogger<RegisterModel> logger;
        private readonly IEmailSender emailSender;

        public RegisterModel(
            UserManager<Employee> userManager,
            IUserStore<Employee> userStore,
            SignInManager<Employee> signInManager,
            ILogger<RegisterModel> logger, IEmailSender emailSender)
        {
            this.userManager = userManager;
            this.userStore = userStore;
            this.emailStore = this.GetEmailStore();
            this.signInManager = signInManager;
            this.logger = logger;
            this.emailSender = emailSender;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [Required]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            this.ReturnUrl = returnUrl;
            this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!User.Identity.IsAuthenticated)
            {
                returnUrl ??= this.Url.Content("~/");

                this.ExternalLogins = (await this.signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

                if (this.ModelState.IsValid)
                {
                    var user = new Employee
                    {
                        UserName = this.Input.Email,
                        Email = this.Input.Email,
                        FirstName = this.Input.FirstName,
                        LastName = this.Input.LastName,
                    };

                    await this.userStore.SetUserNameAsync(user, this.Input.FirstName, CancellationToken.None);

                    var claimType = "Surname";
                    await this.userManager.AddClaimAsync(user, new Claim(claimType, user.LastName));

                    await this.emailStore.SetEmailAsync(user, this.Input.Email, CancellationToken.None);
                    var result = await this.userManager.CreateAsync(user, this.Input.Password);

                    if (result.Succeeded)
                    {
                        this.logger.LogInformation("User created a new account with password.");

                        var userId = await this.userManager.GetUserIdAsync(user);

                        var code = await this.userManager.GenerateEmailConfirmationTokenAsync(user);

                        code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                        var callbackUrl = this.Url.Page(
                            "/Account/ConfirmEmail",
                            pageHandler: null,
                            values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                            protocol: this.Request.Scheme);

                        await emailSender.SendEmailAsync(
                        GlobalConstants.IssueSystemEmail,
                        GlobalConstants.IssueSystemName,
                        Input.Email,
                        "Reset Password",
                        $"Please confirm your Account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        //await this.emailSender.SendEmailAsync(this.Input.Email, "Confirm your email", $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                        if (this.userManager.Options.SignIn.RequireConfirmedAccount)
                        {
                            return this.RedirectToPage("RegisterConfirmation", new { email = this.Input.Email, returnUrl = returnUrl });
                        }
                        else
                        {
                            await this.signInManager.SignInAsync(user, isPersistent: false);
                            return this.LocalRedirect(returnUrl);
                        }
                    }
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                // If we got this far, something failed, redisplay form
                return this.Page();

            }

            return Redirect("~/");
        }

        private Employee CreateUser()
        {
            try
            {
                return Activator.CreateInstance<Employee>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(Employee)}'. " +
                    $"Ensure that '{nameof(Employee)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<Employee> GetEmailStore()
        {
            if (!this.userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }

            return (IUserEmailStore<Employee>)this.userStore;
        }
    }
}
