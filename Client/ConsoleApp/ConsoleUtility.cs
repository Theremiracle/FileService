using System;
using System.Threading.Tasks;

namespace Client.ConsoleApp
{
    public static class ConsoleUtility
    {
        public static void LogInstructions(string instruction)
        {
            Console.WriteLine();
            Console.WriteLine(new String('-', 50));
            Console.WriteLine($"Press Enter to {instruction}:");
            Console.Write(new String('-', 50));
            Console.ReadLine();
        }

        public static void LogTaskResult(Task<bool> task)
        {
            if (task.IsCompleted && task.Result)
            {
                Console.Write($"\nSucceeds!!!\n");
            }
            else
            {
                Console.Write($"\nFailed...\n");
            }
        }
    }
}
