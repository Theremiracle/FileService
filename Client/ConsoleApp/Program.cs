﻿using ServiceProxy;
using System;
using System.IO;

namespace Client.ConsoleApp
{
    class Program
    {
        static FileServiceProxy FileServiceProxy = new FileServiceProxy();
        static void Main(string[] args)
        {
            while(true)
            {
                Console.Clear();

                Test();
                SaveFile();
                DeleteFile();
                GetFile();

                Console.WriteLine($"\nExit? (Y/N)");
                var result = Console.ReadKey();
                if (result.Key == ConsoleKey.Y)
                {
                    break;
                }
            }
        }

        static void Test()
        {
            ConsoleUtility.LogInstructions("Starts testing if service is ready");
            var result = FileServiceProxy.IsConnectionReadyAsync();

            result.Wait();
            ConsoleUtility.LogTaskResult(result);
        }

        static void SaveFile()
        {
            ConsoleUtility.LogInstructions("Starts saving file");
            var fileFullName = FileServiceProxy.FileToUpload;
            var fileUploadFolder = FileServiceProxy.UploadFolderPath;
            Console.Write($"Starts upload file:\n From: {fileFullName} \n   To: {fileUploadFolder}\n\n");
            var result = FileServiceProxy.SaveFileAsync(fileFullName, fileUploadFolder);

            result.Wait();
            ConsoleUtility.LogTaskResult(result);
        }

        static void DeleteFile()
        {
            ConsoleUtility.LogInstructions("Starts deleting file");
            var fileFullName = FileServiceProxy.UploadFolderPath + @"\" + Path.GetFileName(FileServiceProxy.FileToUpload);
            Console.Write($"Starts deleting file:\n at: {fileFullName} \n\n");
            var result = FileServiceProxy.DeleteFileAsync(fileFullName);

            result.Wait();
            ConsoleUtility.LogTaskResult(result);
        }

        static void GetFile()
        {
            ConsoleUtility.LogInstructions("Starts getting file");
            var fileFullName = FileServiceProxy.FileToDownload;
            Console.Write($"Starts getting file:\n at: {fileFullName} \n\n");
            var result = FileServiceProxy.GetFileAsync(fileFullName);

            result.Wait();
            ConsoleUtility.LogTaskResult(result);
        }        
    }
}
