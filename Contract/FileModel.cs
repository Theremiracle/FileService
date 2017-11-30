using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contract
{
    public class FileModel
    {
        public string FileFullName { get; set; }

        public static void CheckFileEixsts(string fileFullName)
        {
            if (string.IsNullOrEmpty(fileFullName))
            {
                throw new Exception("File full name is null or empty.");
            }

            if (!File.Exists(fileFullName))
            {
                throw new Exception($"{fileFullName} does not exit.");
            }
        }

        public static void CheckDirectoryEixsts(string directory)
        {
            if (string.IsNullOrEmpty(directory))
            {
                throw new Exception("Directory is null or empty.");
            }

            if (!Directory.Exists(directory))
            {
                throw new Exception($"{directory} does not exit.");
            }
        }
    }
}
