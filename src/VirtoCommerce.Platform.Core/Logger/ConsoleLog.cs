using System;

namespace VirtoCommerce.Platform.Core.Logger
{
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
