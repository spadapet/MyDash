using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using MyDash.Data.Model;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash.Data.Utility;

public static class AdoAuthenticationUtility
{
    // Cross platform Token Cache:
    // https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache

    // App settings
    private const string Authority = "https://login.windows.net/common";
    private const string VisualStudioIdeClientId = "872cd9fa-d31f-45e0-9eab-6e460a02d1f1";
    private const string AzureDevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/user_impersonation";
    private const string TokenCacheFileName = "MyDashTokenCache.bin";

    // For Mac
    public const string KeyChainServiceName = "myapp_msal_service";
    public const string KeyChainAccountName = "myapp_msal_account";

    // For Linux
    private const string LinuxKeyRingSchema = "com.peterspada.mydash.tokencache";
    private const string LinuxKeyRingCollection = MsalCacheHelper.LinuxKeyRingDefaultCollection;
    private const string LinuxKeyRingLabel = "MSAL token cache for MyDash app.";
    private static readonly KeyValuePair<string, string> LinuxKeyRingAttr1 = new("Version", "1");
    private static readonly KeyValuePair<string, string> LinuxKeyRingAttr2 = new("ProductGroup", "MyApps");

    public static async Task<AdoAuthentication> GetAuthenticationAsync(CancellationToken cancellationToken)
    {
        // Initialize the MSAL library by building a public client application
        IPublicClientApplication application = PublicClientApplicationBuilder
            .Create(AdoAuthenticationUtility.VisualStudioIdeClientId)
            .WithAuthority(AdoAuthenticationUtility.Authority)
            .WithDefaultRedirectUri()
            .Build();

        StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder(AdoAuthenticationUtility.TokenCacheFileName, MsalCacheHelper.UserRootDirectory)
            .WithLinuxKeyring(
                AdoAuthenticationUtility.LinuxKeyRingSchema,
                AdoAuthenticationUtility.LinuxKeyRingCollection,
                AdoAuthenticationUtility.LinuxKeyRingLabel,
                AdoAuthenticationUtility.LinuxKeyRingAttr1,
                AdoAuthenticationUtility.LinuxKeyRingAttr2)
            .WithMacKeyChain(
                AdoAuthenticationUtility.KeyChainServiceName,
                AdoAuthenticationUtility.KeyChainAccountName)
            .Build();

        MsalCacheHelper cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);
        cacheHelper.RegisterCache(application.UserTokenCache);

        string[] scopes = new string[] { AdoAuthenticationUtility.AzureDevOpsScope };
        AuthenticationResult authentication;
        try
        {
            IEnumerable<IAccount> accounts = await application.GetAccountsAsync();
            authentication = await application.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync(cancellationToken);
        }
        catch (MsalUiRequiredException ex)
        {
            authentication = await application.AcquireTokenInteractive(scopes).WithClaims(ex.Claims).ExecuteAsync();
        }

        return new AdoAuthentication()
        {
            AccessToken = authentication.AccessToken,
            UserName = authentication.Account.Username,
        };
    }
}
