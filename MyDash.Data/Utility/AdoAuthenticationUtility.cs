using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Extensions.Msal;
using MyDash.Data.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace MyDash.Data.Utility;

public static class AdoAuthenticationUtility
{
    // Cross platform Token Cache:
    // https://github.com/AzureAD/microsoft-authentication-extensions-for-dotnet/wiki/Cross-platform-Token-Cache


    // App settings
    public static readonly string[] Scopes = new[] { "user.read" };

    private const string Authority = "https://login.windows.net/common";
    private const string VisualStudioIdeClientId = "872cd9fa-d31f-45e0-9eab-6e460a02d1f1";
    private const string AzureDevOpsScope = "499b84ac-1321-427f-aa17-267ca6975798/user_impersonation";
    private const string TokenCacheFileName = "TokenCache.bin";
    //public readonly static string TokenCacheDir = MsalCacheHelper.UserRootDirectory;
    //
    //public const string KeyChainServiceName = "myapp_msal_service";
    //public const string KeyChainAccountName = "myapp_msal_account";
    //
    //public const string LinuxKeyRingSchema = "com.contoso.devtools.tokencache";
    //public const string LinuxKeyRingCollection = MsalCacheHelper.LinuxKeyRingDefaultCollection;
    //public const string LinuxKeyRingLabel = "MSAL token cache for all Contoso dev tool apps.";
    //public static readonly KeyValuePair<string, string> LinuxKeyRingAttr1 = new KeyValuePair<string, string>("Version", "1");
    //public static readonly KeyValuePair<string, string> LinuxKeyRingAttr2 = new KeyValuePair<string, string>("ProductGroup", "MyApps");
    //
    //// For Username / Password flow - to be used only for testing!
    //public const string Username = "liu.kang@bogavrilltd.onmicrosoft.com";
    //public const string Password = "";

    private static AuthenticationResult authentication;

    private static async Task<AuthenticationResult> GetAuthenticationAsync(CancellationToken cancellationToken)
    {
        if (AdoAuthenticationUtility.authentication == null || AdoAuthenticationUtility.authentication.ExpiresOn.AddMinutes(-10) < DateTimeOffset.UtcNow)
        {
            // Initialize the MSAL library by building a public client application
            IPublicClientApplication application = PublicClientApplicationBuilder
                .Create(VisualStudioIdeClientId)
                .WithAuthority(Authority)
                .WithDefaultRedirectUri()
                .Build();

            string directory = Path.Combine(MsalCacheHelper.UserRootDirectory, "MyDash");
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            StorageCreationProperties storageProperties = new StorageCreationPropertiesBuilder(TokenCacheFileName, Path.Combine(MsalCacheHelper.UserRootDirectory, "MyDash")).Build();
            MsalCacheHelper cacheHelper = await MsalCacheHelper.CreateAsync(storageProperties);
            cacheHelper.RegisterCache(application.UserTokenCache);

            string[] scopes = new string[] { AzureDevOpsScope };
            try
            {
                IEnumerable<IAccount> accounts = await application.GetAccountsAsync();
                AdoAuthenticationUtility.authentication = await application.AcquireTokenSilent(scopes, accounts.FirstOrDefault()).ExecuteAsync(cancellationToken);
            }
            catch (MsalUiRequiredException ex)
            {
                // If the token has expired, prompt the user with a login prompt
                AdoAuthenticationUtility.authentication = await application.AcquireTokenInteractive(scopes)
                    .WithClaims(ex.Claims)
                    .ExecuteAsync();
            }
        }

        return AdoAuthenticationUtility.authentication;
    }

    public static async Task<AuthenticationHeaderValue> GetAuthenticationHeaderValueAsync(CancellationToken cancellationToken)
    {
        AuthenticationResult authentication = await GetAuthenticationAsync(cancellationToken);
        return new AuthenticationHeaderValue("Bearer", authentication.AccessToken);
    }
}
