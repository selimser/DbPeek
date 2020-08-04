using System;
using System.IO;

namespace DbPeek.Helpers.Editor
{
    internal static class FileHelper
    {
        //create a temp file with guid name.sql and open that.

        private static string DbPeekDumpPath;

        static FileHelper()
        {
            SetDumpPath();
            CreateDumpFolder();
        }

        private static void SetDumpPath()
        {
            DbPeekDumpPath = Path.Combine(Path.GetTempPath(), "DbPeek_Dump");
        }

        private static void CreateDumpFolder()
        {
            if (!Directory.Exists(DbPeekDumpPath))
            {
                Directory.CreateDirectory(DbPeekDumpPath);
            }
        }

        internal static string CreateFileWithContents(string fileContents)
        {
            var dumpFileName = $@"{DbPeekDumpPath}\{Guid.NewGuid().ToString("d")}.sql";
            File.WriteAllText(dumpFileName, fileContents);
            return dumpFileName;
        }
    }
}
