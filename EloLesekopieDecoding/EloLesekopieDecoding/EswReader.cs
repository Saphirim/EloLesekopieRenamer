using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using EloExportConverter.Model;

namespace EloLesekopieDecoding
{
    public class EswReader
    {

        public static EloObjectMetaData ReadFile(string filePath)
        {
            if (!File.Exists(filePath) || !filePath.EndsWith(".ESW"))
            {
                throw new ArgumentException("File doesn't exist or is not a *.esw-file");
            }
            var eswLines = File.ReadAllLines(filePath, CodePagesEncodingProvider.Instance.GetEncoding(1252));

            var eloObjectMetaData = new EloObjectMetaData();

            eloObjectMetaData.Name = Regex.Replace(eswLines.Single(l => l.StartsWith("SHORTDESC=")).Substring(("SHORTDESC=").Length), "[\\\\/\\:\\*\\?\\\"<>\\|]", "");

            var dateArray = eswLines.Single(l => l.StartsWith("ABLDATE=")).Substring(("ABLDATE=").Length).Split('.');

            eloObjectMetaData.ArchiveDate = new DateTime(int.Parse(dateArray[2]), int.Parse(dateArray[1]), int.Parse(dateArray[0]));

            return eloObjectMetaData;
        }
    }
}
