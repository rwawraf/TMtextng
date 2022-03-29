using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMtextng
{
    class IniCreator
    {
        public void CreateConfigUserIni()
        {
            var parser = new FileIniDataParser();
            IniData parsedINIDataToSave = new IniData();

            string iniPath = "TMtextng_config.ini";

            IniData data = parser.ReadFile(iniPath, Encoding.UTF8);

            parsedINIDataToSave["User"].AddKey("voiceSpeed", data["Main"]["voiceSpeed"]);
            parsedINIDataToSave["User"].AddKey("voicePitch", data["Main"]["voicePitch"]);
            parsedINIDataToSave["User"].AddKey("voiceVolume", data["Main"]["voiceVolume"]);
            parsedINIDataToSave["User"].AddKey("voiceSpeed_MS", data["Main"]["voiceSpeed_MS"]);
            parsedINIDataToSave["User"].AddKey("svox_voice_active", "1");
            parsedINIDataToSave["User"].AddKey("selected_voice", "Deutsch W");
            parsedINIDataToSave["User"].AddKey("TMTextng_Amount_Of_Starts", data["Main"]["TMTextng_Amount_Of_Starts"]);
            parsedINIDataToSave["User"].AddKey("TMTextng_Minimum_Amount_Of_Suggestested_Word_Uses", data["Main"]["TMTextng_Minimum_Amount_Of_Suggestested_Word_Uses"]);
            parsedINIDataToSave["User"].AddKey("scan_type", data["Main"]["tmtext_menu1_area_scan_type"]);
            parsedINIDataToSave["User"].AddKey("scan_interval", data["Main"]["tmtext_menu1_area_scan_interval"]);
            parsedINIDataToSave["User"].AddKey("scan_cycles_amount", "4");
            parsedINIDataToSave["User"].AddKey("scan_duration_seconds", "5");
            parsedINIDataToSave["User"].AddKey("textReadMode", "3");
            parsedINIDataToSave["User"].AddKey("Input_fontsize", "20");
            parsedINIDataToSave["User"].AddKey("Konfig_fontsize", "20");
            parsedINIDataToSave["User"].AddKey("WordSuggestion_fontsize", "20");
            parsedINIDataToSave["User"].AddKey("Keyboard_fontsize", "20");
            parsedINIDataToSave["User"].AddKey("Keyboard_active", "1");
            parsedINIDataToSave["User"].AddKey("ReadPressedButtonTextMode_active", "0");
            parsedINIDataToSave["User"].AddKey("Lang - GerW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - GerM - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - EngW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - FraW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - ItaW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - NedW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - PorW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("Lang - SpaW - on - off", "1");
            parsedINIDataToSave["User"].AddKey("SpeechRec", "0");
            parsedINIDataToSave["User"].AddKey("Control_panel_size", "0");

            parser.WriteFile("TMtextng_config_user.ini", parsedINIDataToSave);
        }

        public void SaveValue(string iniFile, string sectionName, string key, string value)
        {
            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniFile);
            data[sectionName].RemoveKey(key);
            data[sectionName].AddKey(key, value);
            parser.WriteFile(iniFile, data);
        }
    }
}
