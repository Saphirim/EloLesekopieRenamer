using System;
using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace EloLesekopieDecoding
{
    public class FolderConverter
    {
        private readonly DirectoryInfo _workingFolder;
        private readonly DirectoryInfo _targetFolder;
        private readonly XElement _xmlFolderStructure;
        private const string MetaDataFileExtension = ".ESW";

        public FolderConverter(DirectoryInfo workingDirectory, DirectoryInfo targetDirectory, XElement xmlFolderStructure)
        {
            _workingFolder = workingDirectory;
            _targetFolder = targetDirectory;
            _xmlFolderStructure = xmlFolderStructure;
        }

        public void ProcessFolder()
        {
            var eswFiles = _workingFolder.GetFiles().Where(f => f.Extension.Equals(MetaDataFileExtension, StringComparison.OrdinalIgnoreCase)).ToList();

            var dataFiles = _workingFolder.GetFiles().Where(f => !f.Extension.Equals(MetaDataFileExtension, StringComparison.InvariantCultureIgnoreCase));
            
            foreach (var dataFile in dataFiles)
            {
                if (eswFiles.Any(f => f.Name.Substring(0, f.Name.Length - 4).Equals(dataFile.Name.Substring(0, dataFile.Name.Length - dataFile.Extension.Length), StringComparison.InvariantCultureIgnoreCase)))
                {
                    var eswFile =
                        eswFiles.Single(f => f.Name.Substring(0, f.Name.Length - 4).Equals(dataFile.Name.Substring(0, dataFile.Name.Length - dataFile.Extension.Length), StringComparison.InvariantCultureIgnoreCase));
                    new FileConverter(eswFile, dataFile, _targetFolder).ProcessFile();
                }
            }

            var subDirectories = _workingFolder.GetDirectories();

            foreach (var subDirectory in subDirectories)
            {

                try
                {
                    var eswFile =
                        eswFiles.SingleOrDefault(f => f.Name.Substring(0, f.Name.Length - 4).Equals(subDirectory.Name, StringComparison.InvariantCultureIgnoreCase));

                    if (eswFile != null)
                    {
                        var metaData = EswReader.ReadFile(eswFile.FullName);


                        var nextSubDirectory = new DirectoryInfo(_targetFolder.FullName + "\\" + metaData.Name);

                        Directory.CreateDirectory(nextSubDirectory.FullName);

                        var folderXmlElement = new XElement("folder");
                        folderXmlElement.Add(new XAttribute("name", metaData.Name));
                        folderXmlElement.Add(new XAttribute("key", metaData.Name));
                        folderXmlElement.Add(new XAttribute("buzzwords", ""));
                        _xmlFolderStructure.Add(folderXmlElement);

                        new FolderConverter(subDirectory, nextSubDirectory, folderXmlElement).ProcessFolder();
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

    }
}