using System;
using System.IO;
using System.Text.RegularExpressions;

namespace EloLesekopieDecoding
{
    public class FileConverter
    {
        private readonly FileInfo _eswFile;
        private readonly FileInfo _dataFile;
        private readonly DirectoryInfo _targetDirectory;

        public FileConverter(FileInfo eswFile, FileInfo dataFile, DirectoryInfo targetDirectory)
        {
            _eswFile = eswFile;
            _dataFile = dataFile;
            _targetDirectory = targetDirectory;
        }

        public void ProcessFile()
        {
            var metaData = EswReader.ReadFile(_eswFile.FullName);

            var newFileName = $"{metaData.ArchiveDate:ddMMyyyy}_{metaData.Name}{_dataFile.Extension}";

            try
            {
                var exportPath = _targetDirectory + "\\" + newFileName;
                if (File.Exists(exportPath))
                {
                    exportPath = exportPath + "Kopie";
                }
                _dataFile.CopyTo(exportPath);
            }
            catch (Exception ex)
            {

            }
        }
    }
}