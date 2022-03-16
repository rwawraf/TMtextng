using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMtextng.Voices
{
    class MyVoice
    {
        public string GetMyVoiceSoundFile(string mainDirectory, string textFragment)
        {
            IniReader iniReader = new IniReader();

            string[] subdirectoryEntries = Directory.GetDirectories(iniReader.dataPath + mainDirectory);
            string fullPath = "";

            foreach (string element in subdirectoryEntries)
            {
                string checkedFile = element + "\\" + textFragment + ".wav";

                if (File.Exists(checkedFile))
                {
                    fullPath = checkedFile;
                }
            }

            return fullPath;
        }
    }
}
