using Car_Renting.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Car_Renting.Controllers.Tests
{
    public class MockUserManager : UserManager<User>
    {
        private HashSet<string> _adminUserIds;

        public User CurrentUser { get; set; }

        public AppDbContext Context { get; private set; }

        public User AdminUser { get; set; }

        public User NormalUser { get; set; }

        private MockUserManager()
            : base(null, null, null, null, null, null, null, null, null)
        {
            throw new NotSupportedException();
        }

        public static MockUserManager Create(AppDbContext context)
        {
            // UserManager methods that are used are fully mocked out so we don't need a functional base class
            var userManager = (MockUserManager)FormatterServices.GetUninitializedObject(typeof(MockUserManager));
            userManager.Context = context;
            return userManager;
        }

        public User AddUser(string email, string firstName, string lastName, string phoneNumber, bool isAdmin)
        {
            var user = new User { Id = email, UserName = email, Email = email, FirstName = firstName, LastName = lastName, PhoneNumber = phoneNumber };

            if (isAdmin)
            {
                _adminUserIds.Add(email);
                AdminUser = user;
            }
            else
            {
                NormalUser = user;
            }

            Context.Users.Add(user);
            Context.SaveChanges();

            return user;
        }

        public override Task<User> GetUserAsync(ClaimsPrincipal principal)
        {
            return Task.FromResult(CurrentUser);
        }

        public override Task<bool> IsInRoleAsync(User user, string role)
        {
            return Task.FromResult(role == "Admin" && _adminUserIds.Contains(user.Email)));
        }
    }
}
