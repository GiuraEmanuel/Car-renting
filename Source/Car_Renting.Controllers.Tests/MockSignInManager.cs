using Car_Renting.Data;
using Microsoft.AspNetCore.Identity;
using System;
using System.Runtime.Serialization;
using System.Security.Claims;

namespace Car_Renting.Controllers.Tests
{
    public class MockSignInManager : SignInManager<User>
    {
        private MockUserManager _userManager;

        private MockSignInManager():base(null,null,null,null,null,null,null)
        {
            throw new NotSupportedException();
        }

        public static MockSignInManager Create(MockUserManager userManager)
        {
            // SignInManager methods that are used are fully mocked out so we don't need a functional base class
            var signInManager = (MockSignInManager)FormatterServices.GetUninitializedObject(typeof(MockSignInManager));
            signInManager._userManager = userManager;
            return signInManager;
        }

        public override bool IsSignedIn(ClaimsPrincipal principal)
        {
            return _userManager.CurrentUser != null;
        }
    }
}
