using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using Moq;
using SouthAmp.Infrastructure.Identity;
using SouthAmp.Infrastructure.Services;
using SouthAmp.Web.Controllers;
using Xunit;

namespace SouthAmp.Tests
{
    public class UsersControllerTests
    {
        private readonly Mock<UserManager<AppUser>> _userManagerMock;
        private readonly Mock<SignInManager<AppUser>> _signInManagerMock;
        private UsersController _controller;

        public UsersControllerTests()
        {
            var store = new Mock<IUserStore<AppUser>>();
            _userManagerMock = new Mock<UserManager<AppUser>>(store.Object, null!, null!, null!, null!, null!, null!, null!, null!);
            var contextAccessor = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var claimsFactory = new Mock<IUserClaimsPrincipalFactory<AppUser>>();
            _signInManagerMock = new Mock<SignInManager<AppUser>>(_userManagerMock.Object, contextAccessor.Object, claimsFactory.Object, null!, null!, null!, null!);
            _controller = new UsersController(_userManagerMock.Object, _signInManagerMock.Object, new TestJwtTokenService());
        }

        [Fact]
        public async Task Register_ReturnsOk_WhenSuccess()
        {
            var req = new RegisterRequest { UserName = "user", Email = "e@e.com", Password = "pass", Role = "guest" };
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), req.Password)).ReturnsAsync(IdentityResult.Success);
            _userManagerMock.Setup(u => u.AddToRoleAsync(It.IsAny<AppUser>(), req.Role)).ReturnsAsync(IdentityResult.Success);
            var result = await _controller.Register(req);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenFailed()
        {
            var req = new RegisterRequest { UserName = "user", Email = "e@e.com", Password = "pass" };
            var errors = new List<IdentityError> { new IdentityError { Description = "fail" } };
            _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<AppUser>(), req.Password)).ReturnsAsync(IdentityResult.Failed(errors.ToArray()));
            var result = await _controller.Register(req);
            var bad = Assert.IsType<BadRequestObjectResult>(result);
            Assert.NotNull(bad.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenUserNotFound()
        {
            var req = new LoginRequest { Email = "e@e.com", Password = "pass" };
            _userManagerMock.Setup(u => u.FindByEmailAsync(req.Email)).ReturnsAsync((AppUser)null);
            var result = await _controller.Login(req);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenPasswordInvalid()
        {
            var req = new LoginRequest { Email = "e@e.com", Password = "pass" };
            var user = new AppUser { Email = req.Email };
            _userManagerMock.Setup(u => u.FindByEmailAsync(req.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, req.Password, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);
            var result = await _controller.Login(req);
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsOk_WithToken_WhenSuccess()
        {
            var req = new LoginRequest { Email = "e@e.com", Password = "pass" };
            var user = new AppUser { Email = req.Email };
            _userManagerMock.Setup(u => u.FindByEmailAsync(req.Email)).ReturnsAsync(user);
            _signInManagerMock.Setup(s => s.CheckPasswordSignInAsync(user, req.Password, false)).ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);
            _userManagerMock.Setup(u => u.GetRolesAsync(user)).ReturnsAsync(new List<string> { "guest" });
            _controller = new UsersController(_userManagerMock.Object, _signInManagerMock.Object, new TestJwtTokenService());
            var result = await _controller.Login(req);
            var ok = Assert.IsType<OkObjectResult>(result);
            var value = ok.Value;
            var prop = value?.GetType().GetProperty("token");
            Assert.NotNull(prop);
            Assert.Equal("token", prop.GetValue(value));
        }
    }

    public class TestJwtTokenService : JwtTokenService
    {
        public TestJwtTokenService() : base(new TestConfiguration()) { }
        public override string GenerateToken(AppUser user, IList<string> roles) => "token";
    }

    public class TestConfiguration : IConfiguration
    {
        public string this[string key] { get => "test"; set { } }
        public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();
        public IConfigurationSection GetSection(string key) => new TestConfigurationSection();
        private static Microsoft.Extensions.Primitives.IChangeToken GetReloadTokenStatic() => new TestChangeToken();
        public Microsoft.Extensions.Primitives.IChangeToken GetReloadToken() => GetReloadTokenStatic();
    }

    public class TestConfigurationSection : IConfigurationSection
    {
        public string this[string key] { get => "test"; set { } }
        public string Key => "test";
        public string Path => "test";
        public string Value { get => "test"; set { } }
        public IEnumerable<IConfigurationSection> GetChildren() => Enumerable.Empty<IConfigurationSection>();
        public IConfigurationSection GetSection(string key) => this;
        public Microsoft.Extensions.Primitives.IChangeToken GetReloadToken() => new TestChangeToken();
    }

    public class TestChangeToken : Microsoft.Extensions.Primitives.IChangeToken
    {
        public bool ActiveChangeCallbacks => false;
        public bool HasChanged => false;
        public IDisposable RegisterChangeCallback(Action<object?> callback, object? state) => new TestDisposable();
    }

    public class TestDisposable : IDisposable
    {
        public void Dispose() { }
    }
}
