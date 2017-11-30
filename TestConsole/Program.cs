using System;
using System.Collections.Generic;
using System.IO;
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

                LogInstructions("Starts saving file");
                SaveFile();

                LogInstructions("Starts deleting file");
                DeleteFile();

                LogInstructions("Starts getting file");
                GetFile();

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
            var fileFullName = FileServiceProxy.FileToUpload;
            var fileUploadFolder = FileServiceProxy.UploadFolderPath;
            Console.Write($"Starts upload file:\n From: {fileFullName} \n   To: {fileUploadFolder}\n\n");
            var result = FileServiceProxy.SaveFile(fileFullName, fileUploadFolder);

            result.Wait();
            LogTaskResult(result);
        }

        static void DeleteFile()
        {
            var fileFullName = FileServiceProxy.UploadFolderPath + @"\" + Path.GetFileName(FileServiceProxy.FileToUpload);
            Console.Write($"Starts deleting file:\n at: {fileFullName} \n\n");
            var result = FileServiceProxy.DeleteFile(fileFullName);

            result.Wait();
            LogTaskResult(result);
        }

        static void GetFile()
        {
            var fileFullName = FileServiceProxy.FileToDownload;
            Console.Write($"Starts getting file:\n at: {fileFullName} \n\n");
            var result = FileServiceProxy.GetFile(fileFullName);

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
