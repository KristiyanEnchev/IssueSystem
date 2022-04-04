namespace IssueSystem.Areas.Identity.Pages.Account.Manage
{
    using System.ComponentModel.DataAnnotations.Schema;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    using IssueSystem.Data.Models;
    using IssueSystem.Services.Contracts.File;
    using IssueSystem.Models.Image;
    using IssueSystem.Common;
    using IssueSystem.Models.User;
    using IssueSystem.Services.Contracts.Users;
    using IssueSystem.Infrastructure.ValidationAttributes;

    public class IndexModel : PageModel
    {
        private readonly UserManager<Employee> _userManager;
        private readonly IFileService _fileService;
        private readonly IUserPersonalService _userService;

        public IndexModel(
            UserManager<Employee> userManager,
            IFileService fileService, IUserPersonalService userService)
        {
            _userManager = userManager;
            _fileService = fileService;
            _userService = userService;
        }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public ProfileViewModel Profile { get; set; }

        [BindProperty]
        public Image image { get; set; }

        public class Image
        {
            public int id { get; set; }
            public string path { get; set; }
            public string description { get; set; }

            [NotMapped]
            [FileSize(1024)]
            [FileTypes("png, jpeg, jpg")]
            public IFormFile file { get; set; }
        }


        public async Task<IActionResult> OnGetAsync()
        {
            Profile = new ProfileViewModel();

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                //return NotFound($"User Not Found'.");
                return RedirectToPage("/NotFound");
            }

            if (User.IsInRole(IssueSystemRoles.AdministratorRoleName))
            {
                return Redirect("~/Administration/");
            }

            await LoadAsync(user);

            return Page();
        }

        private async Task LoadAsync(Employee user)
        {
            Profile.FirstName = user.FirstName;
            Profile.LastName = user.LastName;
            Profile.Email = user.Email;
            if (user.Department != null)
            {
                Profile.Department = user.Department.DepartmentName;
            }
            else
            {
                Profile.Department = "Talk to your Manager to set your Department";
            }

            (bool exists, ResponseImageViewModel image) = await _fileService.GetUserImage(user.Id);

            if (exists)
            {
                Profile.ProfilePicture = image.Content;
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            // Here we check the form with the picture 
            if (this.image.file != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(image.file.FileName);
                var extension = Path.GetExtension(image.file.FileName);

                var imageModel = new RequestImageViewModel
                {
                    Name = fileName,
                    FileExtension = extension,
                    EmployeeId = user.Id,
                };
                using (var dataStream = new MemoryStream())
                {
                    await image.file.CopyToAsync(dataStream);
                    imageModel.Content = dataStream.ToArray();
                }

                bool isSuccsessful = false;

                if (user.ProfilePicture != null)
                {
                    isSuccsessful = await _userService.UpdateUserProfilePicture(imageModel, user.Id);
                }
                else
                {
                    isSuccsessful = await _userService.UploadProfilePicture(imageModel);
                }

                if (!isSuccsessful)
                {
                    StatusMessage = "Somthing Went Wrong While Trying To Update Your Profile Picture";

                    return Page();
                }

                await LoadAsync(user);

                return Page();
            }

            // Here we check the form with Data

            //if (!ModelState.IsValid)
            //{
            //    await LoadAsync(user);
            //    return Page();
            //}

            if (Profile.FirstName != user.FirstName
                || Profile.LastName != user.LastName
                || Profile.Email != user.Email)
            {
                var editModel = new EditProfileDataModel
                {
                    FirstName = Profile.FirstName,
                    LastName = Profile.LastName,
                    Email = Profile.Email,
                };

                var isUpdatedUserData = await _userService.UpdateUserData(editModel);

                if (!isUpdatedUserData)
                {
                    StatusMessage = "Somthing Went Wrong While Trying To Update Your Personal Data";

                    return Page();
                }

                await LoadAsync(user);

                return Page();
            }

            await LoadAsync(user);

            TempData[MessageConstant.ErrorMessage] = "Your profile has been updated";

            return RedirectToPage();
        }
    }
}
