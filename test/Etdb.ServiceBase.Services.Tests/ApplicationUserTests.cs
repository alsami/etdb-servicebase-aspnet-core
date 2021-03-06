using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Etdb.ServiceBase.Services.Tests
{
    public class ApplicationUserTests
    {
        [Fact]
        public void ApplicationUser_IsAuthenticated_User_Authenticated_True()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            var applicationUser = new ApplicationUser(mock.Object);

            Assert.True(applicationUser.IsAuthenticated());
        }

        [Fact]
        public void ApplicationUser_GetUserId_User_Authenticated_Returns_Id()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            var wantedUserId = Guid.NewGuid();

            mock.Setup(context => context.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                    new Claim(JwtClaimTypes.Subject, wantedUserId.ToString())
                });

            var applicationUser = new ApplicationUser(mock.Object);

            var receivedUserId = applicationUser.Id;

            Assert.Equal(wantedUserId, receivedUserId);
        }

        [Fact]
        public void ApplicationUser_GetUserName_User_Authenticated_Returns_UserName()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            var wantedUserName = Guid.NewGuid().ToString();

            mock.Setup(context => context.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                    new Claim(JwtClaimTypes.PreferredUserName, wantedUserName)
                });

            var applicationUser = new ApplicationUser(mock.Object);

            var receivedUserName = applicationUser.UserName;

            Assert.Equal(wantedUserName, receivedUserName);
        }

        [Fact]
        public void ApplicationUser_GetUserId_User_Not_Authenticated_Empty_Guid()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(false);

            var applicationUser = new ApplicationUser(mock.Object);

            Assert.Equal(Guid.Empty, applicationUser.Id);
        }

        [Fact]
        public void ApplicationUser_GetImageUrl_User_Authenticted_Returns_Url()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            mock.Setup(context => context.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                    new Claim(JwtClaimTypes.Profile, "https://someurl")
                });

            var applicationUser = new ApplicationUser(mock.Object);

            Assert.NotNull(applicationUser.ImageUrl);
        }

        [Fact]
        public void ApplicationUser_GetImageUrl_User_Authenticted_Has_No_Image_Returns_Null()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(true);

            mock.Setup(context => context.HttpContext.User.Claims)
                .Returns(new List<Claim>());

            var applicationUser = new ApplicationUser(mock.Object);

            Assert.Null(applicationUser.ImageUrl);
        }

        [Fact]
        public void ApplicationUser_GetImageUrl_User_Not_Authenticted_Returns_Null()
        {
            var mock = new Mock<IHttpContextAccessor>();

            mock.Setup(context => context.HttpContext.User.Identity.IsAuthenticated)
                .Returns(false);

            var applicationUser = new ApplicationUser(mock.Object);

            Assert.Null(applicationUser.ImageUrl);
        }
    }
}