using System;
using System.IO;
using EloLesekopieDecoding;

namespace EloExportConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            var sourceDirectory = "C:\\Users\\mhutt\\Downloads\\ELOExport";
            var targetDirectory = "C:\\Users\\mhutt\\Downloads\\ArchiveExporte\\" + DateTime.Now.ToString("hhmmss_ddMMyy");

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            try
            {
                var archiveCrawler = new ArchiveCrawler(new DirectoryInfo(sourceDirectory), new DirectoryInfo(targetDirectory));
                archiveCrawler.ProcessArchiveExport();
            }
            catch (Exception ex)
            {

            }
        }
    }
}
