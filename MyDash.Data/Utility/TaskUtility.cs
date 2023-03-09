using System;
using System.Threading.Tasks;

namespace MyDash.Data.Utility;

public static class TaskUtility
{
    public static async void FileAndForget(Func<Task> func)
    {
        try
        {
            await func?.Invoke();
        }
        catch
        {
            // Forget about it, maybe log telemetry if that ever gets added
        }
    }
}
