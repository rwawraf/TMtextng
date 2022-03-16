using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TMnote;

namespace TMtextng.Konfig
{
    /// <summary>
    /// Interaktionslogik für SpeechOptionsWindow.xaml
    /// </summary>
    public partial class VoiceOptionsWindow : Window
    {
        Label[] optionLabels = new Label[5];
        static TextBox[] textBoxes = new TextBox[3];
        ComboBox standardVoiceCombobox = new ComboBox();
        IniReader iniReader = new IniReader();
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        public string voiceSpeed, voicePitch, voiceVolume, voiceSpeed_MS, selectedLanguage;

        string[] speechOptions = { "Sprachgeschwindigkeit (1 - 20)", "Tonhöhe (0 - 450)", "Lautstärke (1 - 100)", "Standartsprache", "Read Pressed Button Text Mode" };

        //List<string> svoxLanguages = new List<string>{ "Deutsch M", "Deutsch W", "Englisch W", "Französisch W", "Italienisch W", "Niederländisch W", "Portugiesisch W", "Spanisch W" };

        List<string> svoxLanguages = new List<string>();
        List<string> languagesToSelect = new List<string>();

        public VoiceOptionsWindow()
        {
            InitializeComponent();
            MainWindowViewModel buttonMVVM = new MainWindowViewModel();

            if (iniReader.GerM_active == 1)
                svoxLanguages.Add("Deutsch M");

            if (iniReader.GerW_active == 1)
                svoxLanguages.Add("Deutsch W");

            if (iniReader.EngW_active == 1)
                svoxLanguages.Add("English W");

            if (iniReader.FraW_active == 1)
                svoxLanguages.Add("Französisch W");

            if (iniReader.ItaW_active == 1)
                svoxLanguages.Add("Italienisch W");

            if (iniReader.NedW_active == 1)
                svoxLanguages.Add("Niederländisch W");

            if (iniReader.PorW_active == 1)
                svoxLanguages.Add("Portugiesisch W");

            if (iniReader.SpaW_active == 1)
                svoxLanguages.Add("Spanisch W");

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            languagesToSelect.AddRange(svoxLanguages);
            languagesToSelect.AddRange(GetInstalledMSVoices());

            voiceSpeed = iniReader.voiceSpeed.ToString();
            voicePitch = iniReader.voicePitch.ToString();
            voiceVolume = iniReader.voiceVolume.ToString();
            voiceSpeed_MS = iniReader.voiceSpeed_MS.ToString();
            selectedLanguage = iniReader.selectedLanguage;       

            for (int i = 0; i < 2; i++)
            {
                CfgButton konfigButton = new CfgButton(iniReader.speechOptionsButtonText[i], CfgButton.TYPE.Default, iniReader.speechOptionsButtonsTextVisible[i], iniReader.speechOptionsButtonsImageVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                //if (iniReader.speechOptionsButtonText[i] == "Svox")
                //{
                //    konfigButton.btnImage = ImageHelper.LoadBitmapImage(@"C:\projekte\TMtextng_dir\TMtextng\Tastatur_Bilder\!.png");
                //}
                if (iniReader.speechOptionsButtonText[i] == "Zurück")
                {
                    myCCCplace1.Children.Add(konfigButton);
                }
                konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);
            }         


            for (int i = 0; i < 5; i++)
            {
                optionLabels[i] = new Label();
                optionLabels[i].FontSize = 30;

                if (iniReader.svox_voice_active == 1 && i == 0)
                    optionLabels[i].Content = "Sprachgeschwindigkeit (0 - 450)";

                else
                    optionLabels[i].Content = speechOptions[i];

                if (iniReader.svox_voice_active == 0 && i == 1)
                    optionLabels[i].IsEnabled = false;

                else
                    optionLabels[i].IsEnabled = true;

                optionLabels[i].VerticalContentAlignment = VerticalAlignment.Center;

                Grid.SetRow(optionLabels[i], i);
                Grid.SetColumn(optionLabels[i], 0);
                SpeechOptionsGrid.Children.Add(optionLabels[i]);
            }

            CheckBox checkBox = new CheckBox();

            checkBox.Checked += TurnOn_ReadPressedButtonTextMode;
            checkBox.Unchecked += TurnOff_ReadPressedButtonTextMode;

            if (iniReader.readPressedButtonTextMode_active == 1)
                checkBox.IsChecked = true;

            else
                checkBox.IsChecked = false;

            Viewbox viewbox = new Viewbox();
            viewbox.Child = checkBox;

            Grid.SetRow(viewbox, 4);
            Grid.SetColumn(viewbox, 1);
            SpeechOptionsGrid.Children.Add(viewbox);


            for (int i = 0; i < 3; i++)
            {
                textBoxes[i] = new TextBox();
                textBoxes[i].PreviewTextInput += TextBoxInput;
                textBoxes[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                textBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;
                textBoxes[i].FontSize = 40;

                if (i == 0)
                {                   
                    if(iniReader.svox_voice_active == 0)
                    {
                        textBoxes[i].MaxLength = 2;
                        textBoxes[i].Text = voiceSpeed_MS;
                    }
                        
                    else
                    {
                        textBoxes[i].MaxLength = 3;
                        textBoxes[i].Text = voiceSpeed;
                    }                  

                    textBoxes[i].TextChanged += SetVoiceSpeed;
                }

                if (i == 1)
                {
                    if(iniReader.svox_voice_active == 0)
                        textBoxes[i].IsEnabled = false;

                    textBoxes[i].MaxLength = 3;
                    textBoxes[i].Text = voicePitch;
                    textBoxes[i].TextChanged += SetVoicePitch;
                }

                if (i == 2)
                {
                    textBoxes[i].MaxLength = 3;
                    textBoxes[i].Text = voiceVolume;
                    textBoxes[i].TextChanged += SetVoiceVolume;
                }


                Grid.SetRow(textBoxes[i], i);
                Grid.SetColumn(textBoxes[i], 1);
                SpeechOptionsGrid.Children.Add(textBoxes[i]);
            }

            for (int i = 0; i < svoxLanguages.Count; i++)
                standardVoiceCombobox.Items.Add(svoxLanguages[i]);

            for (int i = 0; i < GetInstalledMSVoices().Count; i++)
                standardVoiceCombobox.Items.Add(GetInstalledMSVoices()[i]);

            standardVoiceCombobox.FontSize = 20;
            standardVoiceCombobox.SelectionChanged += new SelectionChangedEventHandler(ComboBoxChangedEventHandler);
            Grid.SetRow(standardVoiceCombobox, 3);
            Grid.SetColumn(standardVoiceCombobox, 1);
            SpeechOptionsGrid.Children.Add(standardVoiceCombobox);
            SetComboBoxValue();
        }

        private void TurnOn_ReadPressedButtonTextMode(object sender, RoutedEventArgs e)
        {
            IniReader iniReader = new IniReader();
            IniCreator iniCreator = new IniCreator();

            if (iniReader.readPressedButtonTextMode_active == 0)
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "ReadPressedButtonTextMode_active", "1");
        }

        private void TurnOff_ReadPressedButtonTextMode(object sender, RoutedEventArgs e)
        {
            IniReader iniReader = new IniReader();
            IniCreator iniCreator = new IniCreator();

            if (iniReader.readPressedButtonTextMode_active == 1)
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "ReadPressedButtonTextMode_active", "0");
        }

        private void SetComboBoxValue()
        {
            for(int i = 0; i < languagesToSelect.Count; i++)
            {
                if (selectedLanguage == languagesToSelect[i])
                    standardVoiceCombobox.SelectedIndex = i;
            }
        }

        public void ComboBoxChangedEventHandler(object sender, SelectionChangedEventArgs e)
        {
            IniCreator iniCreator = new IniCreator();
            var parser = new FileIniDataParser();
            string userIniPath = "TMtextng_config_user.ini";
            IniData userData = parser.ReadFile(userIniPath, Encoding.UTF8);
            IniReader iniReader = new IniReader();

            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "selected_voice", standardVoiceCombobox.SelectedValue.ToString());

            for(int i = 0; i < svoxLanguages.Count; i++)
            {
                if(standardVoiceCombobox.SelectedValue.ToString() == svoxLanguages[i])
                {
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "svox_voice_active", "1");
                    optionLabels[0].Content = "Sprachgeschwindigkeit (0 - 450)";
                    optionLabels[1].IsEnabled = true;
                    textBoxes[0].MaxLength = 3;
                    textBoxes[0].Text = iniReader.voiceSpeed.ToString();
                    textBoxes[1].IsEnabled = true;
                    textBoxes[1].Text = iniReader.voicePitch.ToString();
                }

                else if(!svoxLanguages.Contains(standardVoiceCombobox.SelectedValue.ToString()))
                {
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "svox_voice_active", "0");
                    optionLabels[0].Content = "Sprachgeschwindigkeit (1 - 20)";
                    optionLabels[1].IsEnabled = false;
                    textBoxes[0].MaxLength = 2;
                    textBoxes[0].Text = iniReader.voiceSpeed_MS.ToString();
                    textBoxes[1].IsEnabled = false;
                    textBoxes[1].Text = "100";                 
                }                   
            }         
        }
        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }

        private void FontSizeChange(object sender, SizeChangedEventArgs e)
        {
            double minFontSize = 20.0;
            double maxFontSize = 40.0;
            double maxMinFontSizeDiff = maxFontSize - minFontSize;

            double gridMinHeight = 294;
            double gridMaxHeight = 707.3;
            double gridMaxMinHeightDiff = gridMaxHeight - gridMinHeight;

            double gridMinWidth = 624;
            double gridMaxWidth = 1924;
            double gridMaxMinWidthDiff = gridMaxWidth - gridMinWidth;

            //Linear equation considering "max/min FontSize" and "max/min GridHeight/GridWidth"
            double heightFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinHeightDiff) * this.SpeechOptionsGrid.ActualHeight + maxMinFontSizeDiff / 2;
            double widthFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinWidthDiff) * this.SpeechOptionsGrid.ActualWidth + maxMinFontSizeDiff / 2;

            foreach (var children in this.SpeechOptionsGrid.Children)
            {
                if(children.GetType() == typeof(Label))
                {
                    (children as Label).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);
                }
                else if(children.GetType() == typeof(TextBox))
                    (children as TextBox).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);

                else if (children.GetType() == typeof(Button))
                    (children as Button).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);
            }
        }

        public static void SetVoiceSpeed(object sender, RoutedEventArgs e)
        {
            IniReader iniReader = new IniReader();
            IniCreator iniCreator = new IniCreator();
            int voiceSpeed = 0;
            int voiceSpeed_MS = 0;

            if (String.IsNullOrEmpty(textBoxes[0].Text))
            {
                voiceSpeed = 10;
            }
            else
            {               

                if(iniReader.svox_voice_active == 0)
                {
                    if (int.Parse(textBoxes[0].Text) > 20)
                    {
                        voiceSpeed_MS = 20;
                        textBoxes[0].Text = voiceSpeed_MS.ToString();
                    }

                    else if (int.Parse(textBoxes[0].Text) <= 0)
                    {
                        voiceSpeed_MS = 1;
                        textBoxes[0].Text = voiceSpeed_MS.ToString();
                    }

                    else
                        voiceSpeed_MS = int.Parse(textBoxes[0].Text);

                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "voiceSpeed_MS", voiceSpeed_MS.ToString());
                }

                else if (iniReader.svox_voice_active == 1)
                {
                    if (int.Parse(textBoxes[0].Text) > 450)
                    {
                        voiceSpeed = 450;
                        textBoxes[0].Text = voiceSpeed.ToString();
                    }

                    else if (int.Parse(textBoxes[0].Text) < 0)
                    {
                        voiceSpeed = 0;
                        textBoxes[0].Text = voiceSpeed.ToString();
                    }

                    else
                        voiceSpeed = int.Parse(textBoxes[0].Text);

                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "voiceSpeed", voiceSpeed.ToString());
                }
                                
            }
        }

        public static void SetVoiceVolume(object sender, RoutedEventArgs e)
        {
            int voiceVolume = 0;

            if (String.IsNullOrEmpty(textBoxes[2].Text))
            {
                voiceVolume = 50;
            }
            else
            {
                if (int.Parse(textBoxes[2].Text) > 100)
                {
                    voiceVolume = 100;
                    textBoxes[2].Text = voiceVolume.ToString();
                }                   

                else if (int.Parse(textBoxes[2].Text) <= 0)
                {
                    voiceVolume = 1;
                    textBoxes[2].Text = voiceVolume.ToString();
                }                  

                else
                    voiceVolume = int.Parse(textBoxes[2].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "voiceVolume", voiceVolume.ToString());
        }

        public static void SetVoicePitch(object sender, RoutedEventArgs e)
        {
            int voicePitch = 0;

            IniReader iniReader = new IniReader();

            if (String.IsNullOrEmpty(textBoxes[1].Text))
            {
                voicePitch = 100;
            }
            else
            {
                if (int.Parse(textBoxes[1].Text) > 200)
                {
                    voicePitch = 200;
                    textBoxes[1].Text = voicePitch.ToString();
                }                    

                else if (int.Parse(textBoxes[1].Text) <= 0)
                {
                    voicePitch = 1;
                    textBoxes[1].Text = voicePitch.ToString();
                }                   

                else
                    voicePitch = int.Parse(textBoxes[1].Text);
            }
            IniCreator iniCreator = new IniCreator();
            
            if(iniReader.svox_voice_active == 1)
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "voicePitch", voicePitch.ToString());
        }

        public void TextBoxInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static List<string> GetInstalledMSVoices()
        {
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            List<string> msVoices = new List<string>();

            foreach (InstalledVoice installedVoice in speechSynthesizer.GetInstalledVoices())
            {
                VoiceInfo info = installedVoice.VoiceInfo;
                msVoices.Add(info.Name);
            }

            return msVoices;
        }

        private void onWindowDeactivated(object sender, EventArgs e)
        {
            IniReader iniReader = new IniReader();
            
            if(iniReader.svox_voice_active == 1)
                SpeakVoice.Init_SVOX();
        }
    }
}
