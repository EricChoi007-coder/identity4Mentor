using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;

namespace Sample.IdentityServer
{
    public static class Config
    {
        // API Scope
        public static IEnumerable<ApiScope> ApiScopes => new[]
        {
            // Claim 用户信息单元，证件单元
            new ApiScope
            {
                Name = "sample_api",
                DisplayName = "Sample API",
            },
        };

        public static IEnumerable<Client> Clients => new[]
        {
            new Client
            {
                ClientId = "sample_client",
                ClientSecrets =
                {
                    new Secret("sample_client_secret".Sha256())  //加密
                },
                AllowedGrantTypes = GrantTypes.ClientCredentials, //允许的授权类型
                AllowedScopes = {"sample_api"}
            },
            new Client
            {
                ClientId = "sample_pass_client",
                ClientSecrets =
                {
                    new Secret("sample_client_secret".Sha256())
                },
                AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,
                AllowedScopes = {"sample_api"}
            },
            new Client
            {
                ClientId = "sample_mvc_client",
                ClientName = "Sample MVC Client",
                ClientSecrets =
                {
                    new Secret("sample_client_secret".Sha256())
                },
                AllowedGrantTypes = GrantTypes.Code,
                RedirectUris = {"https://localhost:4001/signin-oidc"},
                PostLogoutRedirectUris = {"https://localhost:4001/sigout-callback-oidc"},
                AllowedScopes = new List<string>
                {
                    IdentityServerConstants.StandardScopes.OpenId,
                    IdentityServerConstants.StandardScopes.Profile
                },
                RequireConsent = true
            }
        };

        public static List<TestUser> Users = new List<TestUser>
        {
            new TestUser
            {
                SubjectId = "1",
                Username = "admin",
                Password = "123"
            }
        };

        public static IEnumerable<IdentityResource> IdentityResources => new List<IdentityResource>
        {
            new IdentityResources.OpenId(), 
            new IdentityResources.Profile()
        };
    }
}
