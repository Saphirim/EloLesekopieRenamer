using System;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EloLesekopieDecoding
{
    public class ArchiveCrawler
    {
        private const string ExportInfoFileName = "ExpInfo.ini";

        private readonly DirectoryInfo _sourceDirectory;
        private readonly DirectoryInfo _targetDirectory;
        private readonly EswReader _eswReader;

        private DirectoryInfo _exportDir;
        private DirectoryInfo _currentDirectory;



        public ArchiveCrawler(DirectoryInfo sourcePath, DirectoryInfo targetPath)
        {
            _sourceDirectory = sourcePath;
            _targetDirectory = targetPath;
            _eswReader = new EswReader();

            CheckExportInfoFile();
            CheckTargetPath();
        }

        private void CheckExportInfoFile()
        {
            if (!File.Exists(_sourceDirectory.FullName + "\\" + ExportInfoFileName))
            {
                throw new ArgumentException($"The source directory doesn't contain the {ExportInfoFileName} File. Pass the top-level directory of the ELO export folder.");
            }
        }

        private void CheckTargetPath()
        {
            if (_targetDirectory.GetFiles().Any() || _targetDirectory.GetDirectories().Any())
            {
                throw new ArgumentException("Target directory is not emtpy. Pass an empty directory as target directory.");
            }
        }

        public void ProcessArchiveExport()
        {
            CreateArchiveTopLevelFolder();

            var xmlFolderStructure = new XDocument(new XElement("ecoDMSFolders"));
            var currentNode = xmlFolderStructure.Root;
            
            new FolderConverter(_sourceDirectory, _exportDir, currentNode).ProcessFolder();

            using (var textWriter = new XmlTextWriter(_targetDirectory + "\\" + "ecoFolders.xml", Encoding.UTF8))
            {
                xmlFolderStructure.Save(textWriter);
            }
        }


        private void CreateArchiveTopLevelFolder()
        {
            var exportIniLines = File.ReadAllLines(_sourceDirectory + "\\" + ExportInfoFileName);

            var exportTopLevelNameLine = exportIniLines.Single(l => l.StartsWith("PATH="));

            var exportTopLevelName = exportTopLevelNameLine.Substring(("PATH=").Length + 1);

            _exportDir = Directory.CreateDirectory(_targetDirectory.FullName + "\\" + exportTopLevelName);


        }
    }
}