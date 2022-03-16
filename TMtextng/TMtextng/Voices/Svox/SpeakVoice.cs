using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using TMtextng.Konfig;
using System.Windows;

namespace TMtextng
{
    class SpeakVoice
    {
        public static String pil_path = "TMspeak_DLL.dll";

        [DllImport("TMspeak_DLL.dll", EntryPoint = "Init", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int init(string code_pass, string str);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ReadFromBuffer", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int readFromBuffer(string str, string wave_path);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ReadFromBufferToFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int readFromBufferToFile(string str, string wave_path);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ReadFromBufferAsync", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern int readFromBufferAsync(string str);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ReadFromBufferAsyncToFile", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int readFromBufferAsyncToFile(string str, string wave_path);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ReadFromFileW", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        public static extern int readFromFile(string file_path);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "Stop", CallingConvention = CallingConvention.Cdecl)]
        private static extern int stop();



        [DllImport("TMspeak_DLL.dll", EntryPoint = "SetSpeed", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool setSpeed(int nspeed);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "SetPitch", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool setPitch(int npitch);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "SetVolume", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool setVolume(int nvolume);



        [DllImport("TMspeak_DLL.dll", EntryPoint = "Terminate", CallingConvention = CallingConvention.Cdecl)]
        private static extern int terminate();

        [DllImport("TMspeak_DLL.dll", EntryPoint = "Is_Speaking", CallingConvention = CallingConvention.Cdecl)]
        private static extern bool is_Speaking();

        [DllImport("TMspeak_DLL.dll", EntryPoint = "ConvertWavToMP3", CharSet = CharSet.Ansi, CallingConvention = CallingConvention.Cdecl)]
        private static extern bool convertWavToMP3(string pszoutfile, string pszinfile, int nbrate);

        [DllImport("TMspeak_DLL.dll", EntryPoint = "CheckLicenseCommandLine", CallingConvention = CallingConvention.Cdecl)]
        public static extern bool CheckLicenseCommandLine();

        [DllImport("TMspeak_DLL.dll", EntryPoint = "SetTextW", CharSet = CharSet.Unicode)]
        public static extern bool readAsyncClipBoard(string str);

        public static bool ConvertWavToMP3(string pszoutfile, string pszinfile, int nbrate)
        {
            return convertWavToMP3(pszoutfile, pszinfile, nbrate);
        }

        // Global Init method for SVox Initialization
        public static int Init_SVOX()
        {
            int svox_code = 0;

#if DEBUG
            svox_code = Init_Debug();
#else
            svox_code = Init_Release();    
#endif


            if (svox_code != 0)
                System.Windows.MessageBox.Show("Fehler beim Initialisieren der Sprachausgabe!");

            return svox_code;
        }

        
        // This Init method is only for Debug mode
        public static int Init_Debug()
        {
            IniReader iniReader = new IniReader();
           
            SpeakVoice.setPitch(iniReader.voicePitch + 50);
            SpeakVoice.setSpeed(iniReader.voiceSpeed + 50);
            SpeakVoice.setVolume(iniReader.voiceVolume);

            string selectedLanguageShortcut = "";
            string[] svoxLanguagesShortcut = { "ENG_W", "FRA_W", "GER_M", "GER_W", "ITA_W", "NED_W", "POR_W", "SPA_W" };
            string[] svoxLanguages = { "Englisch W", "Französisch W", "Deutsch M", "Deutsch W", "Italienisch W", "Niederländisch W", "Portugiesisch W", "Spanisch W" };

            for (int i = 0; i < svoxLanguages.Length; i++)
            {
                if (iniReader.selectedLanguage == svoxLanguages[i])
                    selectedLanguageShortcut = svoxLanguagesShortcut[i];
            }
            pil_path = iniReader.dataPath + @"\TMdocuLang\" + selectedLanguageShortcut + "\\";
            return init("1234567890ABCDE_Fhufs_dpi230239ewsdj93874JFOadfjsfiojfrlj_vff", pil_path);

            //RegistryValue reg_val = new RegistryValue();

            //SpeakVoice.setPitch(reg_val.ReadPitch());
            //SpeakVoice.setSpeed(reg_val.ReadSpeed());
            //SpeakVoice.setVolume(reg_val.ReadVolume());
            //return init("1234567890ABCDE_Fhufs_dpi230239ewsdj93874JFOadfjsfiojfrlj_vff", "C:\\TMND-GMBH\\daisy_dll\\");
        }

        // This Init method is only valid, if TMspeakDocu in installed
        public static int Init_Release()
        {
            SetPilPath();

            RegistryValue reg_val = new RegistryValue();

            SpeakVoice.setPitch(reg_val.ReadPitch());
            SpeakVoice.setSpeed(reg_val.ReadSpeed());
            SpeakVoice.setVolume(reg_val.ReadVolume());
            return init("1234567890ABCDE_Fhufs_dpi230239ewsdj93874JFOadfjsfiojfrlj_vff", pil_path);
            //return init("1234567890ABCDE_Fhufs_dpi230239ewsdj93874JFOadfjsfiojfrlj_vff", "C:\\TMND-GMBH\\daisy_dll\\");
        }

        public static bool SetPilPath()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            String tmdocu_path = (String)rk.GetValue("StrPath") + "TMdocuLang\\";

            int tmdocu_val = (int)Convert.ToInt32(rk.GetValue("ValSpeak"));
            //Int64 temp = (Int64)(tmdocu_val / 2147483648);
            int temp = (int)(tmdocu_val >> 31);

            if ((temp % 2) == 0)
            {
                pil_path = tmdocu_path + "GER_W" + "\\";
            }
            else
            {
                pil_path = tmdocu_path + "GER_M" + "\\";
            }

            return true;
        }

        public static int GenerateWave(String buffer, String wave_path)
        {
            return readFromBufferToFile(buffer, wave_path);
        }

        public static int GenerateWaveAsync(String buffer, String wave_path)
        {
            return readFromBufferAsyncToFile(buffer, wave_path);
        }

        public static int ReadFromBuffer(String buffer, String wave_path)
        {
            return readFromBuffer(buffer, wave_path);
        }

        public static int ReadFromBufferAsync(String buffer)
        {
            return readFromBufferAsync(buffer);
        }

        public static bool IsSpeaking()
        {
            return is_Speaking();
        }

        public static int Stop()
        {
            return stop();
        }

        public static int Terminate()
        {
            return terminate();
        }

        public static void ReadAsyncClipBoard(string str)
        {
#if !WIN32
            RegistryKey rk = Registry.LocalMachine.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu", true);
            rk.SetValue("ValRdState", 1);
            readAsyncClipBoard(str);
#endif
        }
    }
}
