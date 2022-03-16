using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TMtextng
{
    class IniReader
    {
        public int svox_voice_active;
        public int keyboard_active;
        public int readPressedButtonTextMode_active;

        public int GerW_active;
        public int GerM_active;
        public int FraW_active;
        public int SpaW_active;
        public int ItaW_active;
        public int NedW_active;
        public int PorW_active;
        public int EngW_active;

        private string TMTextng_path_DEV;
        private string TMTextng_path_RUN;
        private string iniPath;
        private string iniPath_ESP;
        private string iniPath_ITA;
        private string iniPath_NLD;
        private string iniPath_ENG;
        private string iniPath_POR;
        private string iniPath_FRA;
        private string userIniPath;       
        public string dataPath;


        // Config Settings
        public readonly int windowWidth;
        public readonly int windowHeight;
        public int windowStyle;
        public int keyButtonImageVisibility;
        public string keyboardImagesPath;
        public string metacomImagePath;
        public int voiceSpeed;
        public int voiceSpeed_MS;
        public int voicePitch;
        public int voiceVolume;
        public int amount_of_App_Starts;
        public int min_Suggested_Word_Uses;
        public int scan_cycles_amount;
        public int scan_duration_seconds;
        public string scanningType;
        public string scanningInterval;
        public string selectedLanguage;
        public string textReadMode;

        //Buttons import count 
        public int configureButton_count;
        public int scanButton_count;
        public int menuKonfigButtons_count;
        public int scanOpionsButtons_count;
        public int readOptionsButtons_count;
        public int speechOptionsButtons_count;
        public int fontSettingsLabel_count;
        public int textInputButtons_count;
        public int redewendungKonfigButtons_count;
        public int imageChoiceButtons_count;
        public int metacomImageButtons_count;
        public int textSymbolButtons_count;



        // Config Settings
        public List<string> fontSettingsText = new List<string>();
        public List<int> ButtonColor = new List<int>();
        public List<int> fontsizesValues = new List<int>();


        //Config Buttons Properties
        public List<bool> configureButtonEnabled = new List<bool>();
        public List<byte> configureButtonRed = new List<byte>();
        public List<byte> configureButtonGreen = new List<byte>();
        public List<byte> configureButtonBlue = new List<byte>();
        public List<string> configureButtonLetter = new List<string>();
        public List<string> configureButtonVisible = new List<string>();

        /////Cfg Buttons (these buttons are used in most windows,
        /////             if you want to change the value of any of them you can do it in the ini file)
        //Buttons Text
        public List<string> menuKonfigButtonsText = new List<string>();
        public List<string> scanOpitonsButtonsText = new List<string>();
        public List<string> readOptionsButtonsText = new List<string>();
        public List<string> textInputButtonsText = new List<string>();
        public List<string> speechOptionsButtonText = new List<string>();
        public List<string> redewendungKonfigButtonsText = new List<string>();
        public List<string> imageChoiceButtonsText = new List<string>();
        public List<string> metacomImageButtonsText = new List<string>();
        public List<string> textSymbolKeyboardButtonsText = new List<string>();


        //Buttons Text Visibility
        public List<int> menuKonfigButtonsTextVisible = new List<int>();
        public List<int> scanOpitonsButtonsTextVisible = new List<int>();
        public List<int> readOptionsButtonsTextVisible = new List<int>();
        public List<int> textInputButtonsTextVisible = new List<int>();
        public List<int> speechOptionsButtonsTextVisible = new List<int>();
        public List<int> redewendungKonfigButtonsTextVisible = new List<int>();
        public List<int> imageChoiceButtonsTextVisible = new List<int>();
        public List<int> metacomImageButtonsTextVisible = new List<int>();
        public List<int> textSymbolKeyboardButtonsTextVisible = new List<int>();


        //Buttons Image Visibility
        public List<int> menuKonfigButtonsImageVisible = new List<int>();
        public List<int> scanOpitonsButtonsImageVisible = new List<int>();
        public List<int> readOptionsButtonsImageVisible = new List<int>();
        public List<int> textInputButtonsImageVisible = new List<int>();
        public List<int> speechOptionsButtonsImageVisible = new List<int>();
        public List<int> redewendungKonfigButtonsImageVisible = new List<int>();
        public List<int> imageChoiceButtonsImageVisible = new List<int>();
        public List<int> metacomImageButtonsImageVisible = new List<int>();
        public List<int> textSymbolKeyboardButtonsImageVisible = new List<int>();



        /////Main Keyboard Buttons
        //Numeric Keyboard Buttons
        public List<string> numericKeyboardLetter = new List<string>();
        //Main Lower abc Buttons
        public List<string> abcButtonLetter = new List<string>();
        public List<string> abcButtonLetter_ESP = new List<string>();
        public List<string> abcButtonLetter_ITA = new List<string>();
        public List<string> abcButtonLetter_NLD = new List<string>();
        public List<string> abcButtonLetter_ENG = new List<string>();
        public List<string> abcButtonLetter_POR = new List<string>();
        public List<string> abcButtonLetter_FRA = new List<string>();
        //Main Lower qwertz Buttons
        public List<string> qwertzButtonLetter = new List<string>();
        public List<string> qwertzButtonLetter_ESP = new List<string>();
        public List<string> qwertzButtonLetter_ITA = new List<string>();
        public List<string> qwertzButtonLetter_NLD = new List<string>();
        public List<string> qwertzButtonLetter_ENG = new List<string>();
        public List<string> qwertzButtonLetter_POR = new List<string>();
        public List<string> qwertzButtonLetter_FRA = new List<string>();
        public List<string> upperQWERTZButtonLetter = new List<string>();
        //Main Upper ABC Buttons
        public List<string> upperABCButtonLetter = new List<string>();
        public List<string> upperABCButtonLetter_ESP = new List<string>();
        public List<string> upperABCButtonLetter_ITA = new List<string>();
        public List<string> upperABCButtonLetter_NLD = new List<string>();
        public List<string> upperABCButtonLetter_ENG = new List<string>();
        public List<string> upperABCButtonLetter_POR = new List<string>();
        public List<string> upperABCButtonLetter_FRA = new List<string>();
        //Main Upper QWERTZ Buttons
        public List<string> upperQWERTZButtonLetter_ESP = new List<string>();
        public List<string> upperQWERTZButtonLetter_ITA = new List<string>();
        public List<string> upperQWERTZButtonLetter_NLD = new List<string>();
        public List<string> upperQWERTZButtonLetter_ENG = new List<string>();
        public List<string> upperQWERTZButtonLetter_POR = new List<string>();
        public List<string> upperQWERTZButtonLetter_FRA = new List<string>();

        //Main Keyboard Buttons Images 
        public List<int> abcButtonsImages = new List<int>();
        public List<int> numericButtonImages = new List<int>();
        public List<int> upperABCButtonsImages = new List<int>();
        public List<int> qwertzButtonsImages = new List<int>();
        public List<int> upperQWERTZButtonImages = new List<int>();


        public List<string> redewendungabcButtonLetter = new List<string>();
        public List<string> redewendungABCButtonLetter = new List<string>();



        public IniReader()
        {
            iniPath = "TMtextng_config.ini";
            iniPath_ESP = "TMtextng_config_ESP.ini";
            iniPath_ITA = "TMtextng_config_ITA.ini";
            iniPath_NLD = "TMtextng_config_NLD.ini";
            iniPath_ENG = "TMtextng_config_ENG.ini";
            iniPath_POR = "TMtextng_config_POR.ini";
            iniPath_FRA = "TMtextng_config_FRA.ini";
            userIniPath = "TMtextng_config_user.ini";

            var parser = new FileIniDataParser();
            IniData data = parser.ReadFile(iniPath, Encoding.UTF8);
            IniData data_ESP = parser.ReadFile(iniPath_ESP, Encoding.UTF8);
            IniData data_ITA = parser.ReadFile(iniPath_ITA, Encoding.UTF8);
            IniData data_NLD = parser.ReadFile(iniPath_NLD, Encoding.UTF8);
            IniData data_ENG = parser.ReadFile(iniPath_ENG, Encoding.UTF8);
            IniData data_POR = parser.ReadFile(iniPath_POR, Encoding.UTF8);
            IniData data_FRA = parser.ReadFile(iniPath_FRA, Encoding.UTF8);
            IniData userData = parser.ReadFile(userIniPath, Encoding.UTF8);

            PathChecker pathCheck = new PathChecker();

            TMTextng_path_DEV = data["Main"]["TMtextng_PATH-Dev"];
            TMTextng_path_RUN = data["Main"]["TMtextng_PATH-Run"];

            if (pathCheck.CheckDebuggerPresence())
                dataPath = TMTextng_path_DEV;
            else
                dataPath = TMTextng_path_RUN;


            windowWidth = int.Parse(data["Main"]["tmtext_delivery_resolution_x"]);
            windowHeight = int.Parse(data["Main"]["tmtext_delivery_resolution_y"]);

            keyboard_active = int.Parse(userData["User"]["Keyboard_active"]);
            readPressedButtonTextMode_active = int.Parse(userData["User"]["ReadPressedButtonTextMode_active"]);


            configureButton_count = 0;
            scanButton_count = 0;
            menuKonfigButtons_count = 0;
            readOptionsButtons_count = 0;
            textInputButtons_count = 0;
            scanOpionsButtons_count = 0;
            redewendungKonfigButtons_count = 0;
            imageChoiceButtons_count = 0;
            metacomImageButtons_count = 0;
            textSymbolButtons_count = 0;
            int validationCount = 0;

            ReadConfigureButtonValues(data, validationCount);
            ReadMenuKonfigWindowButtons(data, validationCount);
            ReadScanOptionsButtons(data, validationCount);
            ReadReadOptionsWindowButtons(data, validationCount);
            TextInputWindowWindowButtons(data, validationCount);
            ReadSpeechOptionsWindowButtons(data, validationCount);
            ReadRedewendungKonfigWIndowButtons(data, validationCount);
            ReadImageChoiceButtons(data, validationCount);
            ReadMetacomImageButtons(data, validationCount);
            ReadTextSymbolKeyboardButtons(data, validationCount);
            ReadFontSettingsWindowLabels(data);
            ReadFontSettingsWindowButtons(userData);
            ReadKeyboardButtonValues(data, 0, 0, "Main", "_os", abcButtonLetter);
            ReadKeyboardButtonValues(data, 0, 0, "Main", "_ms", upperABCButtonLetter);
            ReadKeyboardButtonValues(data, 0, 0, "Main", "_osq", qwertzButtonLetter);
            ReadKeyboardButtonValues(data_ESP, 0, 0, "ESP", "_os", abcButtonLetter_ESP);
            ReadKeyboardButtonValues(data_ESP, 0, 0, "ESP", "_ms", upperABCButtonLetter_ESP);
            ReadKeyboardButtonValues(data_ESP, 0, 0, "ESP", "_osq", qwertzButtonLetter_ESP);
            ReadKeyboardButtonValues(data_ESP, 0, 0, "ESP", "_msq", upperQWERTZButtonLetter_ESP);
            ReadKeyboardButtonValues(data_ITA, 0, 0, "ITA", "_os", abcButtonLetter_ITA);
            ReadKeyboardButtonValues(data_ITA, 0, 0, "ITA", "_ms", upperABCButtonLetter_ITA);
            ReadKeyboardButtonValues(data_ITA, 0, 0, "ITA", "_osq", qwertzButtonLetter_ITA);
            ReadKeyboardButtonValues(data_ITA, 0, 0, "ITA", "_msq", upperQWERTZButtonLetter_ITA);
            ReadKeyboardButtonValues(data_NLD, 0, 0, "NLD", "_os", abcButtonLetter_NLD);
            ReadKeyboardButtonValues(data_NLD, 0, 0, "NLD", "_ms", upperABCButtonLetter_NLD);
            ReadKeyboardButtonValues(data_NLD, 0, 0, "NLD", "_osq", qwertzButtonLetter_NLD);
            ReadKeyboardButtonValues(data_NLD, 0, 0, "NLD", "_msq", upperQWERTZButtonLetter_NLD);
            ReadKeyboardButtonValues(data_ENG, 0, 0, "ENG", "_os", abcButtonLetter_ENG);
            ReadKeyboardButtonValues(data_ENG, 0, 0, "ENG", "_ms", upperABCButtonLetter_ENG);
            ReadKeyboardButtonValues(data_ENG, 0, 0, "ENG", "_osq", qwertzButtonLetter_ENG);
            ReadKeyboardButtonValues(data_ENG, 0, 0, "ENG", "_msq", upperQWERTZButtonLetter_ENG);
            ReadKeyboardButtonValues(data_POR, 0, 0, "POR", "_os", abcButtonLetter_POR);
            ReadKeyboardButtonValues(data_POR, 0, 0, "POR", "_ms", upperABCButtonLetter_POR);
            ReadKeyboardButtonValues(data_POR, 0, 0, "POR", "_osq", qwertzButtonLetter_POR);
            ReadKeyboardButtonValues(data_POR, 0, 0, "POR", "_msq", upperQWERTZButtonLetter_POR);
            ReadKeyboardButtonValues(data_FRA, 0, 0, "FRA", "_os", abcButtonLetter_FRA);
            ReadKeyboardButtonValues(data_FRA, 0, 0, "FRA", "_ms", upperABCButtonLetter_FRA);
            ReadKeyboardButtonValues(data_FRA, 0, 0, "FRA", "_osq", qwertzButtonLetter_FRA);
            ReadKeyboardButtonValues(data_FRA, 0, 0, "FRA", "_msq", upperQWERTZButtonLetter_FRA);
            ReadKeyboardButtonValues(data, 0, 0, "Main", "_msq", upperQWERTZButtonLetter);
            //ReadKeyboardButtonValues(data, 0, 0, "Main", "_so", numericKeyboardLetter);
            ReadKeybordButtonImage(data, 0, 0, "Main", "_os", abcButtonsImages);
            ReadKeybordButtonImage(data, 0, 0, "Main", "_ms", upperABCButtonsImages);
            ReadKeybordButtonImage(data, 0, 0, "Main", "_osq", qwertzButtonsImages);
            ReadKeybordButtonImage(data, 0, 0, "Main", "_msq", upperQWERTZButtonImages); 
            ReadRedewendungKeyboardButtonValues(data, 0, 0, "Main", "_os", redewendungabcButtonLetter);
            ReadRedewendungKeyboardButtonValues(data, 0, 0, "Main", "_ms", redewendungABCButtonLetter);
            ReadSpecialSymbols(data);
            ReadSpecialSymbolsImage(data);
            ReadWindowStyle(data);
            ReadSpeechOptions(userData);
            ReadTextInputOptions(userData);
            ReadScanningOptionsUser(userData);
            ReadButtonColorRGB(data);
            ReadSvoxVoiceSettings(userData, validationCount);
            GetTextReadMode(userData);
            ReadSvoxVoiceActive(userData);
            ReadKeyButtonImageVisibility(data);
            ReadKeyButtonImagePath(data);
            ReadMetacomImagePath(data);
        }

        void ReadSvoxVoiceActive(IniData data)
        {
            GerW_active = int.Parse(data["User"]["Lang - GerW - on - off"]); ;
            GerM_active = int.Parse(data["User"]["Lang - GerM - on - off"]); ;
            FraW_active = int.Parse(data["User"]["Lang - FraW - on - off"]); ;
            SpaW_active = int.Parse(data["User"]["Lang - SpaW - on - off"]); ;
            ItaW_active = int.Parse(data["User"]["Lang - ItaW - on - off"]); ;
            NedW_active = int.Parse(data["User"]["Lang - NedW - on - off"]); ;
            PorW_active = int.Parse(data["User"]["Lang - PorW - on - off"]); ;
            EngW_active = int.Parse(data["User"]["Lang - EngW - on - off"]); ;
        }

        void ReadSpecialSymbolsImage(IniData data)
        {
            int buttonRowCount = 0;
            int buttonColumnCount = 0;

            foreach (SectionData section in data.Sections)
            {
                foreach (KeyData key in section.Keys)
                {
                    if (key.KeyName == "tmtext_area_keyboard_area_front_keyboardbuttonImageVisibility_" + (buttonRowCount + 3).ToString("00") + "_" + (buttonColumnCount + 1).ToString("00") + "_so")
                    {
                        numericButtonImages.Add(int.Parse(key.Value));

                        buttonColumnCount++;
                    }

                    if (buttonColumnCount == 12)
                    {
                        buttonColumnCount = 0;
                        buttonRowCount++;
                    }
                }
            }
        }

        public void ReadSpecialSymbols(IniData data)
        {
            int buttonColumnCount = 0;
            int buttonRowCount = 0;
            
            foreach (SectionData section in data.Sections)
            {
                foreach (KeyData key in section.Keys)
                {
                    if (key.KeyName == "tmtext_area_keyboard_area_front_keyboardbutton_char_de_" + (buttonRowCount + 3).ToString("00") + "_" + (buttonColumnCount + 1).ToString("00") + "_so")
                    {
                        if (key.Value == "\"\"\"")
                            numericKeyboardLetter.Add(("\""));
                        else
                            numericKeyboardLetter.Add(key.Value);

                        buttonColumnCount++;
                    }

                    if (buttonColumnCount == 12)
                    {
                        buttonColumnCount = 0;
                        buttonRowCount++;
                    }
                }
            }
        }

        public void GetTextReadMode(IniData data)
        {
            textReadMode = data["User"]["textReadMode"];
        }
        public void ReadSvoxVoiceSettings(IniData data, int validationCount)
        {
            svox_voice_active = int.Parse(data["User"]["svox_voice_active"]);

            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "User")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "selected_voice")
                        {
                            selectedLanguage = key.Value;
                        }                           
                    }
                }
            }
        }

        public void ReadTextInputOptions(IniData data)
        {
            try
            {
                amount_of_App_Starts = int.Parse(data["User"]["TMTextng_Amount_Of_Starts"]);
                min_Suggested_Word_Uses = int.Parse(data["User"]["TMTextng_Minimum_Amount_Of_Suggestested_Word_Uses"]);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Incorrect ini Data");
                Application.Current.Shutdown();
            }           
        }

        public void ReadSpeechOptions(IniData data)
        {
            voiceSpeed = int.Parse(data["User"]["voiceSpeed"]);
            voiceSpeed_MS = int.Parse(data["User"]["voiceSpeed_MS"]);
            voicePitch = int.Parse(data["User"]["voicePitch"]);
            voiceVolume = int.Parse(data["User"]["voiceVolume"]);
        }

        public void ReadScanningOptionsUser(IniData data)
        {
            try
            {
                scanningType = data["User"]["scan_type"];
                scanningInterval = data["User"]["scan_interval"];
                scan_cycles_amount = int.Parse(data["User"]["scan_cycles_amount"]);
                scan_duration_seconds = int.Parse(data["User"]["scan_duration_seconds"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Incorrect ini Data - Scanning Section");
                Application.Current.Shutdown();
            }
        }

        public void ReadScanOptionsButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_scanoptions_window_button_" + (scanOpionsButtons_count + 1).ToString("00") + "_text")
                        {
                            scanOpitonsButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_scanoptions_window_button_" + (scanOpionsButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            scanOpitonsButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_scanoptions_window_button_" + (scanOpionsButtons_count + 1).ToString("00") + "_image")
                        {
                            scanOpitonsButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            scanOpionsButtons_count++;
                            validationCount = 0;
                        }
                    }
                    scanOpitonsButtonsText.Add(data["Main"]["tmtext_scanoptions_window_label_01_text"]);
                    scanOpitonsButtonsText.Add(data["Main"]["tmtext_scanoptions_window_label_02_text"]);
                }
            }
        }

        void ReadMenuKonfigWindowButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_konfig_window_button_" + (menuKonfigButtons_count + 1).ToString("00") + "_text")
                        {
                            menuKonfigButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_konfig_window_button_" + (menuKonfigButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            menuKonfigButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_konfig_window_button_" + (menuKonfigButtons_count + 1).ToString("00") + "_image")
                        {
                            menuKonfigButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            menuKonfigButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }
        void ReadReadOptionsWindowButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_readoptions_window_button_" + (readOptionsButtons_count + 1).ToString("00") + "_text")
                        {
                            readOptionsButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_readoptions_window_button_" + (readOptionsButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            readOptionsButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_readoptions_window_button_" + (readOptionsButtons_count + 1).ToString("00") + "_image")
                        {
                            readOptionsButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            readOptionsButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }
        void ReadSpeechOptionsWindowButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_speechoptions_window_button_" + (speechOptionsButtons_count + 1).ToString("00") + "_text")
                        {
                            speechOptionsButtonText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_speechoptions_window_button_" + (speechOptionsButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            speechOptionsButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_speechoptions_window_button_" + (speechOptionsButtons_count + 1).ToString("00") + "_image")
                        {
                            speechOptionsButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            speechOptionsButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }
        void ReadRedewendungKonfigWIndowButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_redewendungkonfigwindow_window_button_" + (redewendungKonfigButtons_count + 1).ToString("00") + "_text")
                        {
                            redewendungKonfigButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_redewendungkonfigwindow_window_button_" + (redewendungKonfigButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            redewendungKonfigButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_redewendungkonfigwindow_window_button_" + (redewendungKonfigButtons_count + 1).ToString("00") + "_image")
                        {
                            redewendungKonfigButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            redewendungKonfigButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }

        void ReadImageChoiceButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_imagechoice_window_button_" + (imageChoiceButtons_count + 1).ToString("00") + "_text")
                        {
                            imageChoiceButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_imagechoice_window_button_" + (imageChoiceButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            imageChoiceButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_imagechoice_window_button_" + (imageChoiceButtons_count + 1).ToString("00") + "_image")
                        {
                            imageChoiceButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            imageChoiceButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }

        void ReadMetacomImageButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_metacomimage_window_button_" + (metacomImageButtons_count + 1).ToString("00") + "_text")
                        {
                            metacomImageButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_metacomimage_window_button_" + (metacomImageButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            metacomImageButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_metacomimage_window_button_" + (metacomImageButtons_count + 1).ToString("00") + "_image")
                        {
                            metacomImageButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            metacomImageButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }

        void ReadTextSymbolKeyboardButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_textsymbolkeyboard_window_button_" + (textSymbolButtons_count + 1).ToString("00") + "_text")
                        {
                            textSymbolKeyboardButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_textsymbolkeyboard_window_button_" + (textSymbolButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            textSymbolKeyboardButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_textsymbolkeyboard_window_button_" + (textSymbolButtons_count + 1).ToString("00") + "_image")
                        {
                            textSymbolKeyboardButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            textSymbolButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }


        void ReadFontSettingsWindowLabels(IniData data)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_textsettings_window_button_" + (fontSettingsLabel_count + 1).ToString("00") + "_text")
                        {
                            fontSettingsText.Add(key.Value);
                            fontSettingsLabel_count++;
                        }                        
                    }
                }
            }
        }

        void ReadFontSettingsWindowButtons(IniData data)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "User")
                {
                    fontsizesValues.Add(int.Parse(data["User"]["Input_fontsize"]));
                    fontsizesValues.Add(int.Parse(data["User"]["Konfig_fontsize"]));
                    fontsizesValues.Add(int.Parse(data["User"]["WordSuggestion_fontsize"]));
                    fontsizesValues.Add(int.Parse(data["User"]["Keyboard_fontsize"]));
                }
            }
        }

        void TextInputWindowWindowButtons(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_textinput_window_button_" + (textInputButtons_count + 1).ToString("00") + "_text")
                        {
                            textInputButtonsText.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_textinput_window_button_" + (textInputButtons_count + 1).ToString("00") + "_text_visible")
                        {
                            textInputButtonsTextVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_textinput_window_button_" + (textInputButtons_count + 1).ToString("00") + "_image")
                        {
                            textInputButtonsImageVisible.Add(int.Parse(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 3)
                        {
                            textInputButtons_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }


        void ReadConfigureButtonValues(IniData data, int validationCount)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_text")
                        {
                            configureButtonLetter.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_Enabled")
                        {
                            configureButtonEnabled.Add(StringToBoolean(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_Visible")
                        {
                            configureButtonVisible.Add(key.Value);
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_color_red")
                        {
                            configureButtonRed.Add(StringToByte(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_color_green")
                        {
                            configureButtonGreen.Add(StringToByte(key.Value));
                            validationCount++;
                        }

                        if (key.KeyName == "tmtext_area_controlbutton_area_front_button_" + (configureButton_count + 1).ToString("00") + "_color_blue")
                        {
                            configureButtonBlue.Add(StringToByte(key.Value));
                            validationCount++;
                        }

                        if (validationCount == 6)
                        {
                            configureButton_count++;
                            validationCount = 0;
                        }
                    }
                }
            }
        }

        void ReadKeyboardButtonValues(IniData data, int buttonColumnCount, int buttonRowCount, string sectionName, string keyboardDeterminant, List<string> keyboardList)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == sectionName)
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_area_keyboard_area_front_keyboardbutton_char_de_" + (buttonRowCount + 3).ToString("00") + "_" + (buttonColumnCount + 1).ToString("00") + keyboardDeterminant)
                        {
                            if (key.Value == "\"\"\"")
                                keyboardList.Add(("\""));
                            else
                                keyboardList.Add(key.Value);

                            buttonColumnCount++;
                        }

                        if (buttonColumnCount == 11)
                        {
                            buttonColumnCount = 0;
                            buttonRowCount++;
                        }
                    }
                }
            }
        }

        void ReadKeybordButtonImage(IniData data, int buttonColumnCount, int buttonRowCount, string sectionName, string keyboardDeterminant, List<int> keyboardList)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == sectionName)
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_area_keyboard_area_front_keyboardbuttonImageVisibility_" + (buttonRowCount + 3).ToString("00") + "_" + (buttonColumnCount + 1).ToString("00") + keyboardDeterminant)
                        {
                            keyboardList.Add(int.Parse(key.Value));

                            buttonColumnCount++;
                        }

                        if (buttonColumnCount == 11)
                        {
                            buttonColumnCount = 0;
                            buttonRowCount++;
                        }
                    }
                }
            }
        }

        void ReadRedewendungKeyboardButtonValues(IniData data, int buttonColumnCount, int buttonRowCount, string sectionName, string keyboardDeterminant, List<string> keyboardList)
        {
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == sectionName)
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if (key.KeyName == "tmtext_area_redewendung_keyboard_area_front_keyboardbutton_char_de_" + (buttonRowCount + 1).ToString("00") + "_" + (buttonColumnCount + 1).ToString("00") + keyboardDeterminant)
                        {
                            if (key.Value == "\"\"\"")
                                keyboardList.Add(("\""));
                            else
                                keyboardList.Add(key.Value);

                            buttonColumnCount++;
                        }

                        if (buttonColumnCount == 10)
                        {
                            buttonColumnCount = 0;
                            buttonRowCount++;
                        }
                    }
                }
            }
        }
        public void ReadWindowStyle(IniData data)
        {
            windowStyle = int.Parse(data["Main"]["tmtext_windowstyle"]);
        }

        public void ReadKeyButtonImageVisibility(IniData data)
        {
            keyButtonImageVisibility = int.Parse(data["Main"]["tmtext_keyboard_buttons_images_visibility"]);
        }

        public void ReadKeyButtonImagePath(IniData data)
        {
            keyboardImagesPath = data["Main"]["tmtext_keyboard_buttons_images_path"];
        }

        public void ReadMetacomImagePath(IniData data)
        {
            metacomImagePath = data["Main"]["tmtext_tmsymlib_german_path"];
        }

        void ReadButtonColorRGB(IniData data)
        {
            string[] buttonType = { "zuruck", "default" };
            string[] buttonColor = { "red", "green", "blue" };

            int i = 0;
            int j = 0;
            
            foreach (SectionData section in data.Sections)
            {
                if (section.SectionName == "Main")
                {
                    foreach (KeyData key in section.Keys)
                    {
                        if(key.KeyName == "tmtext_" + buttonType[j] + "_button_color_" + buttonColor[i])
                        {
                            ButtonColor.Add(int.Parse(key.Value));
                            i++;
                        }

                        if(i == 3)
                        {
                            i = 0;
                            j++;
                        }
                    }
                }
            }
        }


        byte StringToByte(string stringValue)
        {
            byte value;

            bool parseResult = byte.TryParse(stringValue, out value);

            if (parseResult)
                return value;

            else
                return 255;
        }

        bool StringToBoolean(string value)
        {
            string[] trueStrings = { "1", "y", "yes", "true" };
            string[] falseStrings = { "0", "n", "no", "false" };


            if (trueStrings.Contains(value, StringComparer.OrdinalIgnoreCase))
                return true;
            else if (falseStrings.Contains(value, StringComparer.OrdinalIgnoreCase))
                return false;

            else
                return false;
        }
    }
}
