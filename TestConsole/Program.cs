﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Clear();                

                LogInstructions("Starts getting file");
                GetFile();

                LogInstructions("Starts saving file");
                SaveFile();

                Console.WriteLine($"\nExit? (Y/N)");
                var result = Console.ReadKey();
                if (result.Key == ConsoleKey.Y)
                {
                    break;
                }
            }
        }

        static void SaveFile()
        {
            var fileFullName = FileManager.TestFileFullName;
            var fileUploadFolder = FileManager.UploadFolderPath;
            Console.Write($"Starts upload file:\n From: {fileFullName} \n   To: {fileUploadFolder}\n\n");
            var result = FileManager.SaveFile(fileFullName, fileUploadFolder);

            result.Wait();
            LogTaskResult(result);
        }

        static void GetFile()
        {
            var fileFullName = FileManager.TestFileFullName;
            Console.Write($"Starts getting file:\n at: {fileFullName} \n\n");
            var result = FileManager.GetFile(fileFullName);

            result.Wait();
            LogTaskResult(result);
        }

        static void LogInstructions(string instruction)
        {
            Console.WriteLine();
            Console.WriteLine(new String('-', 50));
            Console.WriteLine($"Press Enter to {instruction}:");
            Console.Write(new String('-', 50));
            Console.ReadLine();
        }

        static void LogTaskResult(Task<bool> task)
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
