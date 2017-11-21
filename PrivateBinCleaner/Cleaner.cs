using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace PrivateBinCleaner
{
    public class Cleaner
    {
        private static readonly Logger Log = new Logger(typeof(Cleaner));
        private double _unixTime;

        public void Clean(string filename)
        {
            string root;
            using (var configReader = File.OpenRead(filename))
            {
                var reader = XElement.Load(configReader);
                root = reader.Element("root")?.Value;
            }

            var span = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
            _unixTime = span.TotalSeconds;

            Traverse(new DirectoryInfo(root));
        }

        private void Traverse(DirectoryInfo directory)
        {
            foreach (var dir in directory.GetDirectories())
                Traverse(dir);

            foreach (var file in directory.GetFiles("*.php"))
            {
                var expireDate = "";
                using (var item = File.OpenRead(file.FullName))
                {
                    var reader = new StreamReader(item);
                    var expire = Regex.Match(reader.ReadToEnd(), "\"expire_date\":(.*?),");
                    if (expire.Success)
                        expireDate = expire.Groups[1].Value;
                }

                if (expireDate == "") continue;
                if (!(Convert.ToDouble(expireDate) < _unixTime)) continue;
                Log.Info("Expiring " + file.Name);
                File.Delete(file.FullName);
            }
        }
    }
}