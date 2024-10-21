using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace JuicyInternal
{
    public class JuicyFileSaver
    {
        const string FOLDER_NAME = "JuicySavedFiles";
        const string FILE_EXTENSION = ".juicy";
        static string directoryPath { get { return Path.Combine(Application.persistentDataPath, FOLDER_NAME); } }

        static void CreateDirectoryIfNeeded()
        {
            string DirectoryPath = Path.Combine(Application.persistentDataPath, FOLDER_NAME);
            if (!Directory.Exists(DirectoryPath))
                Directory.CreateDirectory(DirectoryPath);
        }

        public static int ReadIntValue(string fileName, int defaultValue = 0)
        {
            CreateDirectoryIfNeeded();
            string filePath = Path.Combine(directoryPath, fileName + FILE_EXTENSION);
            if (File.Exists(filePath))
                return int.Parse(File.ReadAllText(filePath));
            return defaultValue;
        }

        public static void WriteIntValue(string fileName, int value)
        {
            CreateDirectoryIfNeeded();
            string filePath = Path.Combine(directoryPath, fileName + FILE_EXTENSION);
            File.WriteAllText(filePath, value.ToString());
        }
    }
}
