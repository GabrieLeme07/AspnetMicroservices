using IdentityServer4.Models;
using IdentityServer4.Test;

namespace IdentityServer.API;

public class Config
{
    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            new Client
            {
                ClientId = "basketClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "movieAPI" }
            },
            new Client
            {
                ClientId = "catalogClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "catalogAPI" }
            },
            new Client
            {
                ClientId = "discountClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "discountAPI" }
            },
            new Client
            {
                ClientId = "orderClient",
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                ClientSecrets =
                {
                    new Secret("secret".Sha256())
                },
                AllowedScopes = { "orderAPI" }
            }
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("basketAPI", "Basket API"),
            new ApiScope("catalogAPI", "Catalog API"),
            new ApiScope("discountAPI", "Discount API"),
            new ApiScope("orderAPI", "Order API")

        };

    public static IEnumerable<ApiResource> ApiResources =>
        Array.Empty<ApiResource>();

    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Address(),
                new IdentityResources.Email()
            };

    public static List<TestUser> TestUsers =>
     new() { };
}
