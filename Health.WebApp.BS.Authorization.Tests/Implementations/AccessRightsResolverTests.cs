using HealthManager.WebApp.BS.Authorization.Implementations;
using HealthManager.WebApp.BS.Service.Contracts;
using HealthManager.WebApp.BS.Shared.Constants;
using HealthManager.WebApp.BS.Shared.DataTransferObjects.User;
using HealthManager.WebApp.BS.Shared.Exceptions.Auth0;
using HealthManager.WebApp.BS.Shared.Exceptions.Authorization;
using HealthManager.WebApp.BS.Shared.Exceptions.User;
using Moq;
using System.Security.Claims;
using Xunit;

namespace HealthManager.WebApp.BS.Authorization.Tests.Implementations
{
    public class AccessRightsResolverTests
    {
        private readonly Mock<IUserService> _mockUser = new Mock<IUserService>();

        [Fact]
        public void CheckPrincipalsRightsOnSubscription__NullSubscriptionEndUser__ThrowsUnauthorized()
        {
            // Arrange
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            string subID = null;
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.UserCategory, UserCategory.EndUser.ToString()),
                new Claim(TokenClaims.SubscriptionId, "456"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            Assert.Throws<InsufficientSubscriptionException>(() => resolver.CheckPrincipalsRightsOnSubscription(claimsPrincipal, subID));
        }

        [Fact]
        public void CheckPrincipalsRightsOnSubscription__InvalidCategoryID__ThrowsTokenInvalidException()
        {
            // Arrange
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            string subID = "123";
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.UserCategory, "somestring"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            Assert.Throws<TokenInvalidException>(() => resolver.CheckPrincipalsRightsOnSubscription(claimsPrincipal, subID));
        }

        [Fact]
        public void CheckPrincipalsRightsOnSubscription__InternalUser__AssertDoesntThrow()
        {
            // Arrange
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            string subID = "123";
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.UserCategory, UserCategory.Internal.ToString()),
                new Claim(TokenClaims.SubscriptionId, "456"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            resolver.CheckPrincipalsRightsOnSubscription(claimsPrincipal, subID);
        }
        [Fact]
        public void CheckPrincipalsUsername__MatchingUsername__DoesntThrow()
        {
            // Arrange
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            string passedUsername = "Aca";
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.Username, "Aca"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
             resolver.CheckPrincipalsUsername(claimsPrincipal, passedUsername);
        }

        [Fact]
        public void CheckPrincipalsUsername__NonMatchingUsername__ThrowUnauthorized()
        {
            // Arrange
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            string passedUsername = "Djape";
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.Username, "Aca"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            Assert.Throws<InvalidPrincipalUsernameException>(() => resolver.CheckPrincipalsUsername(claimsPrincipal, passedUsername));
        }
     

        [Fact]
        public async void CheckPrincipalsRightsOnDelete__SelfDelete__ThrowsPrincipalSelfDeleteException()
        {
            // Arrange
            string subID = "456";
            string username = "Aca";
            UserDto user = new UserDto
            {
                Username = "Aca",
                SubscriptionId = "456"
            };

            _mockUser.Setup(x => x.GetUserByUsernameAsync(username, false)).Returns(async () => user);
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.Username, username),
                new Claim(TokenClaims.UserCategory, UserCategory.Partner.ToString()),
                new Claim(TokenClaims.SubscriptionId, "456"),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            await Assert.ThrowsAsync<PrincipalSelfDeleteException>(() => resolver.CheckPrincipalsRightsOnDelete(claimsPrincipal, "Aca"));
        }

        [Fact]
        public async void CheckPrincipalsRightsOnDelete__InsufficientSubscription__ThrowsInsufficientSubscriptionException()
        {
            // Arrange
            string subID = "123";
            string username = "Aca";
            UserDto user = new UserDto
            {
                Username = "Joca",               
                SubscriptionId = "456"
            };

            _mockUser.Setup(x => x.GetUserByUsernameAsync(user.Username, false)).Returns(async () => user);
            AccessRightsResolver resolver = new AccessRightsResolver(_mockUser.Object);
            var claims = new List<Claim>()
            {
                new Claim(TokenClaims.Username, username),
                new Claim(TokenClaims.UserCategory, UserCategory.EndUser.ToString()),
                new Claim(TokenClaims.SubscriptionId, subID),
            };
            ClaimsIdentity identity = new ClaimsIdentity(claims, "Test");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            //Act assert
            await Assert.ThrowsAsync<InsufficientSubscriptionException>(() => resolver.CheckPrincipalsRightsOnDelete(claimsPrincipal, "Joca"));
        }
    }
}
