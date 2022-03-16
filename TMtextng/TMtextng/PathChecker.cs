using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace TMtextng
{
    public class PathChecker
    {
        [DllImport("kernel32.dll", SetLastError = true, ExactSpelling = true)]
        static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
        bool isDebuggerPresent;
        public bool CheckDebuggerPresence()
        {
            isDebuggerPresent = false;
            CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
            return isDebuggerPresent;
        }

        public void CreateHardLinkToIniFile()
        {
            if (File.Exists(@"..\..\..\..\TMtextng_config.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config.ini", @"..\..\..\..\TMtextng_config.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_ESP.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_ESP.ini", @"..\..\..\..\TMtextng_config_ESP.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_ITA.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_ITA.ini", @"..\..\..\..\TMtextng_config_ITA.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_NLD.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_NLD.ini", @"..\..\..\..\TMtextng_config_NLD.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_ENG.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_ENG.ini", @"..\..\..\..\TMtextng_config_ENG.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_POR.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_POR.ini", @"..\..\..\..\TMtextng_config_POR.ini", IntPtr.Zero);
            }

            if (File.Exists(@"..\..\..\..\TMtextng_config_FRA.ini") && CheckDebuggerPresence() == true)
            {
                CreateHardLink("TMtextng_config_FRA.ini", @"..\..\..\..\TMtextng_config_FRA.ini", IntPtr.Zero);
            }
        }

        [DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool CreateHardLink(string lpFileName, string lpExistingFileName, IntPtr lpSecurityAttributes);
    }
}
