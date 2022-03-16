using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Recognition;
using System.Speech.Synthesis;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Effects;
using System.Windows.Threading;
using TMtextng.KeyboardPages;
using TMtextng.Konfig;

namespace TMtextng
{
    class SpeechRecognitionManager
    {
        public void InitializeSpeechListener()
        {
            SpeechRecognitionEngine sre = new SpeechRecognitionEngine(new System.Globalization.CultureInfo("de-DE"));
            Choices clist = new Choices();
            clist.Add(new string[] { "konfik", "Sprache", "ende",  "scanning einstellungen", "texteingabe", "buchstaben", 
                "lesen einstellungen", "scanning starten", "pause", "abbrechen", "satzweise", "blockweise", "wortweise"});

            GrammarBuilder gb = new GrammarBuilder(clist);
            //gb.Culture = sre.RecognizerInfo.Culture;

            Grammar gr = new Grammar(new GrammarBuilder(clist));

            try
            {
                sre.RequestRecognizerUpdate();
                sre.LoadGrammar(gr);
                sre.SpeechRecognized += sre_SpeechRecognized;
                sre.SetInputToDefaultAudioDevice();
                sre.RecognizeAsync(RecognizeMode.Multiple);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error");
            }
        }

        async void sre_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            var dispatcher = Application.Current.MainWindow.Dispatcher;
            BlurEffect blur = new BlurEffect();
            MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
            ScanningOptions scanningOptions = new ScanningOptions();
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            IniCreator iniCreator;
            ReadOptions readOptions = new ReadOptions();

            switch (e.Result.Text.ToString())
            {
                case "konfik":
                    
                    MenuKonfigWindow menuKonfigWindow = new MenuKonfigWindow();
                    mainWindow.Effect = blur;
                    _ = await Task.Run(() => menuKonfigWindow.ShowDialogAsync());
                    mainWindow.Effect = null;
                    break;

                case "Sprache":
                    VoiceOptionsWindow voiceOptionsWindow = new VoiceOptionsWindow();
                    await Task.Run(() => voiceOptionsWindow.ShowDialogAsync());
                    break;

                case "scanning einstellungen":
                    ScanOptionsWindow window = new ScanOptionsWindow();
                    await Task.Run(() => window.ShowDialogAsync());
                    break;

                case "texteingabe":
                    TextInputWindow textInputWindow = new TextInputWindow();
                    await Task.Run(() => textInputWindow.ShowDialogAsync());
                    break;

                case "buchstaben":
                    FontSettingsWindow fontSettingsWindow = new FontSettingsWindow();
                    await Task.Run(() => fontSettingsWindow.ShowDialogAsync());
                    break;

                case "ende":
                    Application.Current.Shutdown();
                    break;

                case "satzweise":
                    iniCreator = new IniCreator();
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "1");
                    readOptions.DisableReadButton();
                    break;

                case "blockweise":
                    iniCreator = new IniCreator();
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "3");
                    readOptions.EnableReadButton();
                    break;

                case "wortweise":
                    iniCreator = new IniCreator();
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "2");
                    readOptions.DisableReadButton();
                    break;

                case "lesen einstellungen":
                    ReadOptionsWindow readOptionsWindow = new ReadOptionsWindow();
                    await Task.Run(() => readOptionsWindow.ShowDialogAsync());
                    break;

                case "scanning starten":                                     
                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    {
                        if (!Properties.Settings.Default.isScanOn)
                        {
                            _ = scanningOptions.Scanning(LowerABC.globalLowerABC.LettersGrid);
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                    {
                        if (!Properties.Settings.Default.isScanOn)
                        {
                            _ = scanningOptions.Scanning(LowerQWERTZ.globalLowerQWERTZ.LettersGrid);
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                    {
                        if (!Properties.Settings.Default.isScanOn)
                        {
                            _ = scanningOptions.Scanning(UpperABC.globalUpperABC.LettersGrid);
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    {
                        if (!Properties.Settings.Default.isScanOn)
                        {
                            _ = scanningOptions.Scanning(UpperQWERTZ.globalUpperQWERTZ.LettersGrid);
                        }
                    }


                    if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
                    {
                        if (!Properties.Settings.Default.isScanOn)
                        {
                            _ = scanningOptions.Scanning(NumericKeyboard.globalNumericKeyboard.LettersGrid);
                        }
                    }
                    break;

                case "pause":
                    scanningOptions.pauseScan();
                    break;

                case "abbrechen":
                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    {
                        scanningOptions.cancelScan(LowerABC.globalLowerABC.LettersGrid);
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                    {
                        scanningOptions.cancelScan(LowerQWERTZ.globalLowerQWERTZ.LettersGrid);
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                    {
                        scanningOptions.cancelScan(UpperABC.globalUpperABC.LettersGrid);
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    {
                        scanningOptions.cancelScan(UpperQWERTZ.globalUpperQWERTZ.LettersGrid);
                    }


                    if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
                    {
                        scanningOptions.cancelScan(NumericKeyboard.globalNumericKeyboard.LettersGrid);
                    }
                    break;
            }
        }

        private void RecognizedSpeak()
        {
            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();
            //_ = mainWindowViewModel.Speak("Hallo");
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
            speechSynthesizer.Speak(Properties.Settings.Default.TextBoxContent);
            //await mainWindowViewModel.Speak("Hallo");
            //_ = mainWindowViewModel.Speak("Hallo Rafal");
        }
    }
}
