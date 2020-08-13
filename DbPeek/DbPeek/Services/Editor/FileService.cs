using System;
using System.IO;

namespace DbPeek.Helpers.Editor
{
    internal static class FileService
    {
        private const string DumpFolderName = "DbPeek_Dump";
        private static string DbPeekDumpPath => Path.Combine(Path.GetTempPath(), DumpFolderName);
        

        static FileService()
        {
            CreateDumpFolder();
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
            var dumpFileName = $@"{DbPeekDumpPath}\{Guid.NewGuid():d}.sql";
            File.WriteAllText(dumpFileName, fileContents);
            return dumpFileName;
        }

        internal static int GetCacheFileCount()
        {
            return GetCachedFiles().Length;
        }

        internal static long GetTotalCacheSize()
        {
            var cacheFiles = GetCachedFiles();

            long totalBytes = 0;
            foreach (string name in cacheFiles)
            {
                var info = new FileInfo(name);
                totalBytes += info.Length;
            }

            return totalBytes;
        }

        private static string[] GetCachedFiles()
        {
            return Directory.GetFiles(DbPeekDumpPath, "*.*", SearchOption.AllDirectories);
        }

        internal static string AsFormatted(this long totalBytes)
        {
            return SizeSuffix(totalBytes, 2);
        }

        private static readonly string[] SuffixCollection = { "bytes", "KB", "MB", "GB" };
        private static string SizeSuffix(long value, int precision = 2)
        {
            if (precision < 0)
            {
                throw new ArgumentOutOfRangeException("decimalPlaces");
            }

            if (value < 0)
            {
                return "-" + SizeSuffix(-value);
            }

            if (value == 0)
            {
                return string.Format("{0:n" + precision + "} bytes", 0);
            }

            int mag = (int)Math.Log(value, 1024);
            decimal adjustedSize = (decimal)value / (1L << (mag * 10));

            if (Math.Round(adjustedSize, precision) >= 1000)
            {
                mag += 1;
                adjustedSize /= 1024;
            }

            return string.Format("{0:n" + precision + "} {1}",
                adjustedSize,
                SuffixCollection[mag]);
        }

        internal static void ClearCache()
        {
            foreach (var file in GetCachedFiles())
            {
                try
                {
                    File.Delete(file);
                }
                catch { } //don't bother with the ones that can't be deleted.
            }
        }
    }
}
