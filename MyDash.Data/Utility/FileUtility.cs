using Microsoft.Identity.Client.Extensions.Msal;
using System.IO;

namespace MyDash.Data.Utility;

public static class FileUtility
{
    public static string UserRootDirectory
    {
        get
        {
            string dir = Path.Combine(MsalCacheHelper.UserRootDirectory, "MyDash");
            Directory.CreateDirectory(dir);
            return dir;
        }
    }

    public static string AppModelFile => Path.Combine(FileUtility.UserRootDirectory, "AppModel.json");
}
