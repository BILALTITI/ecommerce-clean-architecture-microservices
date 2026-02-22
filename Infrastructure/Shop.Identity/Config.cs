using Duende.IdentityServer.Models;

namespace Shop.Identity;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    // Scopes
    public static IEnumerable<ApiScope> ApiScopes =>
        new List<ApiScope>
        {
            new ApiScope("catalog.read"),
            new ApiScope("catalog.write"),

            new ApiScope("basket.read"),
            new ApiScope("basket.write")
        };

    // APIs
    public static IEnumerable<ApiResource> ApiResources =>
        new List<ApiResource>
        {
            new ApiResource("catalog")
            {
                Scopes =
                {
                    "catalog.read",
                    "catalog.write"
                }
            },

            new ApiResource("basket")
            {
                Scopes =
                {
                    "basket.read",
                    "basket.write"
                }
            }
        };

    // Clients
    public static IEnumerable<Client> Clients =>
        new List<Client>
        {
            new Client
            {
                ClientId = "gateway.client",
                ClientName = "Gateway Client",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("gateway-secret".Sha256())
                },

                AllowedScopes =
                {
                    "catalog.read",
                    "catalog.write",
                    "basket.read",
                    "basket.write"
                }
            },

            new Client
            {
                ClientId = "catalog.client",
                ClientName = "Catalog Service",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("catalog-secret".Sha256())
                },

                AllowedScopes =
                {
                    "catalog.read",
                    "catalog.write"
                }
            },

            new Client
            {
                ClientId = "basket.client",
                ClientName = "Basket Service",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("basket-secret".Sha256())
                },

                AllowedScopes =
                {
                    "basket.read",
                    "basket.write"
                }
            },

            new Client
            {
                ClientId = "EshoppingGateway.client",
                ClientName = "EshoppingGateway",

                AllowedGrantTypes = GrantTypes.ClientCredentials,

                ClientSecrets =
                {
                    new Secret("Eshopping-Gateway".Sha256())
                },

                AllowedScopes =
                {
                    "basket.read",
                    "basket.write"
                }
            }
        };
}