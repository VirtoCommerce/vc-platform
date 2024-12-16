using System;

namespace VirtoCommerce.Platform.Core.Logger
{
    [Obsolete("Use Serilog's static Log.Logger or inject ILogger instead")]
    public static class ConsoleLog
    {
        public static void BeginOperation(string message)
        {
            Console.Write(@"[{0:HH:mm:ss}] {1} ", DateTime.Now, message);
        }

        public static void EndOperation()
        {
            Console.WriteLine("OK");
        }

    }
}
