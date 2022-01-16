using Car_Renting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace Car_Renting.Controllers
{
    [Route("Setup")]
    public class SetupController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public SetupController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }


        [HttpGet]
        public async Task<IActionResult> Setup()
        {
            var message = string.Empty;

            try
            {
                if (!await _roleManager.RoleExistsAsync("Admin"))
                {
                    IdentityRole role = new IdentityRole("Admin");
                    await _roleManager.CreateAsync(role);
                    message += "Added 'Admin' role.\n";
                }
                else
                {
                    message += "Admin role already exists.\n";
                }

                if (!_userManager.Users.Any())
                {
                    User user = new User();
                    user.Email = "giura.emanuel@gmail.com";
                    user.UserName = user.Email;
                    user.FirstName = "Giura";
                    user.LastName = "Emanuel";
                    user.PhoneNumber = "555-555-555";

                    const string password = "emanuel96AX!";
                    IdentityResult result = await _userManager.CreateAsync(user, password);

                    if (result.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Admin");
                        message += $"Admin account {user.Email}  with default password {password} added.\n";
                    }
                    else
                    {
                        message += string.Join("\n", result.Errors.Select(e => e.Description));
                    }
                }
                else
                {
                    message += "Users already exist.\n";
                }

                return Ok(message);
            }
            catch (Exception ex)
            {
                return BadRequest(message + ex.Message);
            }
            
        }
    }
}
