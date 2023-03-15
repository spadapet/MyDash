using Microsoft.Identity.Client.Extensions.Msal;
using System.IO;

namespace MyDash.Data.Utility;

public static class FileUtility
{
    public static string UserRootDirectory => MsalCacheHelper.UserRootDirectory;
    public static string AppModelFile => Path.Combine(FileUtility.UserRootDirectory, "AppModel.json");
}
