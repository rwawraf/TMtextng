using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using TMnote;
using TMtextng.KeyboardPages;
using TMtextng.Konfig;
using TMtextng.Redewendung;
using System.Media;
using System.ComponentModel;
using System.Threading;
using TMtextng.Voices;
using System.Speech.Recognition;
using System.Windows.Threading;

namespace TMtextng
{
    class MainWindowViewModel 
    {
        public ICommand writeLetterCommand { get; set; }
        public ICommand writeSuggestedWordCommand { get; set; }

        [DllImport("Win32DLL.dll", EntryPoint = "test")]
        public static extern void test();
        public string osk_path = "";
        public RedewendungDataBinding myNewText { get; set; }

        static CancellationTokenSource cts = new CancellationTokenSource();

        public MainWindowViewModel()
        {
            writeLetterCommand = new RelayCommand(writeLetter, canExecuteWriteLetterCommand);
            writeSuggestedWordCommand = new RelayCommand(ExecuteWriteSuggestedWord, canExecuteWriteSuggestedWordCommand);
        }

        public bool canExecuteWriteLetterCommand(object parameter)
        {
            return true;
        }

        public bool canExecuteWriteSuggestedWordCommand(object parameter)
        {
            return true;
        }

        //char.ToUpper('z')
        readonly SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();
        readonly MainWindow mainWindow = (MainWindow)App.Current.MainWindow;
        readonly BlurEffect blur = new BlurEffect();
        ReadOptions readOptions = new ReadOptions();
        IniReader iniReader = new IniReader();
        IniCreator iniCreator = new IniCreator();
        ScanningOptions scanningOptions = new ScanningOptions();
        bool cursorSelect_Choosen;
        bool run;
        int inf_loop_counter = 0;
        static void recognizer_SpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            Console.WriteLine("Recognized text: " + e.Result.Text);
            switch (e.Result.Text)
            {
                case "minimalisieren":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
                    });
                    
                    break;
                case "maximieren":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Maximized;
                        ((MainWindow)Application.Current.MainWindow).Activate();
                    });

                    break;
                case "schließen":
                    Application.Current.Dispatcher.Invoke(() =>
                    {
                        Process[] DesktopKeyboardProcess = Process.GetProcessesByName("osk");
                        if (DesktopKeyboardProcess.Length > 0)
                            DesktopKeyboardProcess[0].Kill();

                        Application.Current.Shutdown();
                    });
                    
                    break;
                case "zurück":
                    break;
            }

        }

        static void recognizerAsync()
        {
            // Create an in-process speech recognizer for the de-DE locale.
            using (
            SpeechRecognitionEngine recognizer =
             new SpeechRecognitionEngine(
               new System.Globalization.CultureInfo("de-DE")))
            {
                
                string[] choices = new string[] { "schließen", "maximieren", "minimalisieren", "zurück" };
                Choices myChoices = new Choices(choices);

                // Create and load a dictation grammar.  
                Grammar myComannds = new Grammar(myChoices);
                recognizer.LoadGrammarAsync(myComannds);
                
                // Add a handler for the speech recognized event.  
                recognizer.SpeechRecognized +=
                  new EventHandler<SpeechRecognizedEventArgs>(recognizer_SpeechRecognized);

                // Configure input to the speech recognizer.  
                recognizer.SetInputToDefaultAudioDevice();
                recognizer.InitialSilenceTimeout = TimeSpan.FromSeconds(0.5);

                // Start synchronousa speech recognition.
                recognizer.Recognize();
           
            }
            
        }


        async static void infLoopSpeechRecAsync(CancellationToken cancellationToken)
        {
            try
            {
                while (true)
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await Task.Run(() => recognizerAsync());               

                }
            }
            catch (OperationCanceledException)
            {
                Console.WriteLine("Operation cancelled");

            }
        }

        public void ExecuteWriteSuggestedWord(object parameter)
        {
            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {
                WriteSuggestedWord(parameter.ToString(), LowerABC.globalLowerABC.TextBoxContent);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                WriteSuggestedWord(parameter.ToString(), LowerQWERTZ.globalLowerQWERTZ.TextBoxContent);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                WriteSuggestedWord(parameter.ToString(), UpperABC.globalUpperABC.TextBoxContent);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                WriteSuggestedWord(parameter.ToString(), UpperQWERTZ.globalUpperQWERTZ.TextBoxContent);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
            {
                WriteSuggestedWord(parameter.ToString(), NumericKeyboard.globalNumericKeyboard.TextBoxContent);
            }
        }

        public void WriteSuggestedWord(string word, TextBox textBox)
        {
            string writtenText = textBox.Text;
            string wordToReplace = writtenText.Substring(writtenText.LastIndexOf(" ") + 1);
            string newWord;

            int place = writtenText.LastIndexOf(" ") + 1;
            newWord = word;

            string result = writtenText.Remove(place, wordToReplace.Length).Insert(place, newWord);
            textBox.Text = result;
            RedewendungDataBinding.globalRedewendugText = result;

            textBox.CaretIndex = textBox.Text.Length;
            textBox.ScrollToEnd();
            textBox.Focus();

            WordSuggestion.UpdateUserAbcOnWordClick(newWord);
        }

        public void writeLetter(object parameter)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            switch (parameter.ToString())
            {
                case "Lö":
                    RemoveTextBoxContent(parameter.ToString());
                    Properties.Settings.Default.ReadCursor = 0;
                    break;

                case "de/w":
                    ShowReadMenu();
                    ////RELOAD KEYBOARDPAGE////////////////////////////////////////////

                    string mainWindowContentName = ((MainWindow)Application.Current.MainWindow).Content.ToString();

                    switch (mainWindowContentName)
                    {
                        case "TMtextng.KeyboardPages.LowerABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerABC.globalLowerABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerABC();

                            break;

                        case "TMtextng.KeyboardPages.UpperABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperABC.globalUpperABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperABC();

                            break;

                        case "TMtextng.KeyboardPages.LowerQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.UpperQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.NumericKeyboard":
                            Properties.Settings.Default.SavedText = KeyboardPages.NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.NumericKeyboard();

                            break;
                    }

                    ////UPDATE FONT SIZE//////////////////////////////////////////////////////////////////////////////////////////
                    break;

                case "de/b":
                    ShowReadMenu();
                    ////RELOAD KEYBOARDPAGE////////////////////////////////////////////

                     mainWindowContentName = ((MainWindow)Application.Current.MainWindow).Content.ToString();

                    switch (mainWindowContentName)
                    {
                        case "TMtextng.KeyboardPages.LowerABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerABC.globalLowerABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerABC();

                            break;

                        case "TMtextng.KeyboardPages.UpperABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperABC.globalUpperABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperABC();

                            break;

                        case "TMtextng.KeyboardPages.LowerQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.UpperQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.NumericKeyboard":
                            Properties.Settings.Default.SavedText = KeyboardPages.NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.NumericKeyboard();

                            break;
                    }

                    ////UPDATE FONT SIZE//////////////////////////////////////////////////////////////////////////////////////////
                    break;

                case "de/s":
                    ShowReadMenu(); 
                    ////RELOAD KEYBOARDPAGE////////////////////////////////////////////

                     mainWindowContentName = ((MainWindow)Application.Current.MainWindow).Content.ToString();

                    switch (mainWindowContentName)
                    {
                        case "TMtextng.KeyboardPages.LowerABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerABC.globalLowerABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerABC();

                            break;

                        case "TMtextng.KeyboardPages.UpperABC":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperABC.globalUpperABC.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperABC();

                            break;

                        case "TMtextng.KeyboardPages.LowerQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.LowerQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.UpperQWERTZ":
                            Properties.Settings.Default.SavedText = KeyboardPages.UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.UpperQWERTZ();

                            break;

                        case "TMtextng.KeyboardPages.NumericKeyboard":
                            Properties.Settings.Default.SavedText = KeyboardPages.NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new KeyboardPages.NumericKeyboard();

                            break;
                    }

                    ////UPDATE FONT SIZE//////////////////////////////////////////////////////////////////////////////////////////
                    break;

                case "Min":
                    ((MainWindow)Application.Current.MainWindow).WindowState = WindowState.Minimized;
                    break;

                case "SprachErke":
                    if (inf_loop_counter == 0)
                    {
                        infLoopSpeechRecAsync(cts.Token);
                        MessageBox.Show("Spracherkennung ein");
                        inf_loop_counter = 1;
                        
                    }
                    else
                    {
                        MessageBox.Show("Spracherkennung aus");
                        inf_loop_counter = 0;
                        cts.Cancel();
                        cts.Dispose();
                        cts = new CancellationTokenSource();
                        
                    }
                    break;

                case "My":
                    if(!Properties.Settings.Default.MyVoiceActive)
                    {
                        Properties.Settings.Default.MyVoiceActive = true;
                        SetMyButtonBackground(0, 128, 255);
                    }
                    else if(Properties.Settings.Default.MyVoiceActive)
                    {
                        Properties.Settings.Default.MyVoiceActive = false;
                        SetMyButtonBackground(240, 248, 255);
                    }
                    break;

                case "Satzweise":
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "1");
                    readOptions.DisableReadButton();
                    ReturToPreviousWindow();
                    break;

                case "Blockweise":
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "3");
                    readOptions.EnableReadButton();
                    ReturToPreviousWindow();
                    break;

                case "Wortweise":
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "textReadMode", "2");
                    readOptions.DisableReadButton();
                    ReturToPreviousWindow();
                    break;

                case "Zurück":
                    ReturToPreviousWindow();
                    break;

                case "Wdh":
                    _ = Repeat();
                    break;

                case "Male voice":

                    break;

                case "Vorl":
                    _ = ReadText();
                    break;

                case "Spei":
                    _ = ShowGroupViewWindow();
                    break;

                case "Win":
                    WinOSK winOSK = new WinOSK();
                    winOSK.LaunchWindowsOnScreenKeyboard();
                    break;

                case "SelectVorl":
                    _ = SpeakSelected();
                    break;

                case "CursorVorl":
                    _ = CursorTextSpeak();
                    break;

                case "S-PAU LowerABC":
                    scanningOptions.pauseScan();
                    break;

                case "S-PAU LowerQWERTZ":
                    scanningOptions.pauseScan();
                    break;

                case "S-PAU UpperABC":
                    scanningOptions.pauseScan();
                    break;

                case "S-PAU UpperQWERTZ":
                    scanningOptions.pauseScan();
                    break;

                case "S-PAU NumericKeyboard":
                    scanningOptions.pauseScan();
                    break;

                case "S-OFF LowerABC":
                    scanningOptions.cancelScan(LowerABC.globalLowerABC.LettersGrid);
                    break;

                case "S-OFF LowerQWERTZ":
                    scanningOptions.cancelScan(LowerQWERTZ.globalLowerQWERTZ.LettersGrid);
                    break;

                case "S-OFF UpperABC":
                    scanningOptions.cancelScan(UpperABC.globalUpperABC.LettersGrid);
                    break;

                case "S-OFF UpperQWERTZ":
                    scanningOptions.cancelScan(UpperQWERTZ.globalUpperQWERTZ.LettersGrid);
                    break;

                case "S-OFF NumericKeyboard":
                    scanningOptions.cancelScan(NumericKeyboard.globalNumericKeyboard.LettersGrid);
                    break;

                case "Ende":
                    Process[] DesktopKeyboardProcess = Process.GetProcessesByName("osk");
                    if (DesktopKeyboardProcess.Length > 0)
                        DesktopKeyboardProcess[0].Kill();

                    Application.Current.Shutdown();
                    break;

                case "S-ON LowerABC":
                    if (!Properties.Settings.Default.isScanOn)
                    {
                        _ = scanningOptions.Scanning(LowerABC.globalLowerABC.LettersGrid);
                    }
                    break;

                case "S-ON UpperABC":
                    if (!Properties.Settings.Default.isScanOn)
                        _ = scanningOptions.Scanning(UpperABC.globalUpperABC.LettersGrid);
                    break;

                case "S-ON LowerQWERTZ":
                    if (!Properties.Settings.Default.isScanOn)
                        _ = scanningOptions.Scanning(LowerQWERTZ.globalLowerQWERTZ.LettersGrid);
                    break;

                case "S-ON UpperQWERTZ":
                    if (!Properties.Settings.Default.isScanOn)
                        _ = scanningOptions.Scanning(UpperQWERTZ.globalUpperQWERTZ.LettersGrid);
                    break;

                case "S-ON NumericKeyboard":
                    if (!Properties.Settings.Default.isScanOn)
                        _ = scanningOptions.Scanning(NumericKeyboard.globalNumericKeyboard.LettersGrid);
                    break;


                case "Reihenscanning":
                    scanningOptions.SetScanningType("Reihenscanning", "Spaltenscanning" , "2", "1");
                    break;

                case "Spaltenscanning":
                    scanningOptions.SetScanningType("Spaltenscanning", "Reihenscanning", "1", "2");
                    break;

                case "Ziklusinterval":
                    scanningOptions.SetScanningInterval("Ziklusinterval", "Timeout-Interval", "2", "1");
                    break;

                case "Timeout-Interval":
                    scanningOptions.SetScanningInterval("Timeout-Interval", "Ziklusinterval", "1", "2");
                    break;

                case "Konfg":
                    _ = ShowKonfigMenu();
                    break;

                case "Leer":
                    WriteButtonContentToTextbox(" ");
                    break;

                case "Lö e":
                    RemoveTextBoxContent(parameter.ToString());
                    if (Properties.Settings.Default.ReadCursor > 0)
                        Properties.Settings.Default.ReadCursor--;
                    break;

                case "Scanning":
                    _ = ShowScanMenu();
                    break;

                case "Sprache":
                    _ = ShowSpeechMenu();
                    break;

                case "Texteingabe":
                    _ = ShowTextInputMenu();
                    break;

                case "Buchstaben Einstellungen":
                    _ = ShowTextSettingsMenu();
                    break;

                case "Benutzer - Wortschatz löschen":
                    WordSuggestion.DeleteFile(WordSuggestion.wordsToReadPath_user);
                    Properties.Settings.Default.Is_abc_user_removed = true;
                    WordSuggestion.DeleteFile(WordSuggestion.new_words_path);
                    break;

                case "qwertz":
                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                        Properties.Settings.Default.SavedText = LowerABC.globalLowerABC.TextBoxContent.Text;

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                        Properties.Settings.Default.SavedText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;

                    ((MainWindow)Application.Current.MainWindow).Content = new LowerQWERTZ();
                    break;

                case "abc":
                    switch (window.Title)
                    {
                        // "abc" parameter for the RedewendungKonfigWindow keyboard
                        case "RedewendungKonfigWindowUpperABC":
                            var newWindow = new RedewendungKonfigWindowLowerABC();
                            newWindow.Show();
                            RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.Close();
                            break;
                        // "abc" parameter for the main keyboard
                        default:
                            if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                                Properties.Settings.Default.SavedText = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;

                            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                                Properties.Settings.Default.SavedText = UpperABC.globalUpperABC.TextBoxContent.Text;

                            ((MainWindow)Application.Current.MainWindow).Content = new LowerABC();
                            break;
                    }
                    break;

                case "QWERTZ":
                    if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                        Properties.Settings.Default.SavedText = UpperABC.globalUpperABC.TextBoxContent.Text;

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                        Properties.Settings.Default.SavedText = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;

                    ((MainWindow)Application.Current.MainWindow).Content = new UpperQWERTZ();
                    break;

                case "ABC":

                    switch (window.Title)
                    {
                        // "ABC" parameter for the RedewendungKonfigWindow keyboard
                        case "RedewendungKonfigWindowLowerABC":
                            var newWindow = new RedewendungKonfigWindowUpperABC();
                            newWindow.Show();
                            RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.Close();
                            break;

                        // "ABC" parameter for the main keyboard
                        default:
                            if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                                Properties.Settings.Default.SavedText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;

                            else if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                                Properties.Settings.Default.SavedText = LowerABC.globalLowerABC.TextBoxContent.Text;

                            ((MainWindow)Application.Current.MainWindow).Content = new UpperABC();
                            break;
                    }
                    break;

                case "&123":
                    //if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    //    Properties.Settings.Default.SavedText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;

                    //else if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    //    Properties.Settings.Default.SavedText = LowerABC.globalLowerABC.TextBoxContent.Text;

                    ((MainWindow)Application.Current.MainWindow).Content = new NumericKeyboard();
                    break;

                case "!?#":
                    ((MainWindow)Application.Current.MainWindow).Content = new NumericKeyboard();
                    break;

                case "Bild laden":
                    _ = ShowImageChoiceWindow();
                    break;

                case "Gruppe ändern":
                    GroupViewWindow.globalGroupViweWindow.Visibility = Visibility.Visible;
                    RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.Visibility = Visibility.Hidden;
                    break;

                case "Text-Symbol":
                    _ = ShowTextSymbolKeyboardWindow();
                    break;

                case "Bilde Ordner":
                    _ = ShowMetacomImageWindow();
                    break;

                case "Speichern":
                    SaveRedewendungVariables();
                        break;

                case "Ordner ändern":
                   BackToDirectoryChoose();
                    break;

                case "Bild aus Datei":
                    OpenFileDialogForImages();
                    break;


                default:
                    IniReader iniReader = new IniReader();
                    if (iniReader.readPressedButtonTextMode_active == 1)
                    {
                        Properties.Settings.Default.PressedButtonLetter = parameter.ToString();

                        if (Properties.Settings.Default.PressedButton_once == false)
                        {
                            Properties.Settings.Default.PressedButton_once = true;

                            _ = Speak(parameter.ToString());                           
                        }
                        else if (parameter.ToString() == Properties.Settings.Default.PressedButtonLetter)
                        {
                            WriteButtonContentToTextbox(parameter.ToString());
                            Properties.Settings.Default.PressedButton_once = false;
                        }
                    }

                    else
                        WriteButtonContentToTextbox(parameter.ToString());
                    break;
            }
        }

        private void RemoveTextBoxContent(string buttonParameter)
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);

            switch (window.Title)
            {
                case "RedewendungKonfigWindowLowerABC":
                    if (RedewendungDataBinding.globalRedewendungFocus == 1)
                    {
                        if (RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Length > 0)
                        {
                            RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Remove(RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Length - 1);
                            RedewendungDataBinding.globalRedewendugText = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text;
                        }
                    }

                    else if (RedewendungDataBinding.globalRedewendungFocus == 0)
                    {
                        if (RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text.Length > 0)
                        {
                            RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text.Remove(RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text.Length - 1);
                            RedewendungDataBinding.globaldataName = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text;
                        }
                    }
                    break;

                case "RedewendungKonfigWindowUpperABC":
                    if (RedewendungDataBinding.globalRedewendungFocus == 1)
                    {
                        if (RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text.Length > 0)
                        {
                            RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text.Remove(RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text.Length - 1);
                            RedewendungDataBinding.globalRedewendugText = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text;
                        }
                    }

                    else if (RedewendungDataBinding.globalRedewendungFocus == 0)
                    {
                        if (RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text.Length > 0)
                        {
                            RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text.Remove(RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text.Length - 1);
                            RedewendungDataBinding.globaldataName = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text;
                        }
                    }
                    break;

                case "TextSymbolKeyboard":
                    if (TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text.Length > 0)
                    {
                        TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text = TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text.Remove(TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text.Length - 1);
                    }
               
                    break;

                default:

                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    {
                        if (LowerABC.globalLowerABC.TextBoxContent.Text.Length > 0)
                        {
                            if (buttonParameter == "Lö")
                            {
                                LowerABC.globalLowerABC.TextBoxContent.Clear();
                                RedewendungDataBinding.globalRedewendugText = LowerABC.globalLowerABC.TextBoxContent.Text;
                            }

                            else if (buttonParameter == "Lö e")
                            {
                                LowerABC.globalLowerABC.TextBoxContent.Text = LowerABC.globalLowerABC.TextBoxContent.Text.Remove(LowerABC.globalLowerABC.TextBoxContent.Text.Length - 1);
                                RedewendungDataBinding.globalRedewendugText = LowerABC.globalLowerABC.TextBoxContent.Text;
                            }
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                    {
                        if (LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Length > 0)
                        {
                            if (buttonParameter == "Lö")
                            {
                                LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Clear();
                                RedewendungDataBinding.globalRedewendugText = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                            }

                            else if (buttonParameter == "Lö e")
                            {
                                LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Remove(LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Length - 1);
                                RedewendungDataBinding.globalRedewendugText = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                            }
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                    {
                        if (UpperABC.globalUpperABC.TextBoxContent.Text.Length > 0)
                        {
                            if (buttonParameter == "Lö")
                            {
                                UpperABC.globalUpperABC.TextBoxContent.Clear();
                                RedewendungDataBinding.globalRedewendugText = UpperABC.globalUpperABC.TextBoxContent.Text;
                            }

                            else if (buttonParameter == "Lö e")
                            {
                                UpperABC.globalUpperABC.TextBoxContent.Text = UpperABC.globalUpperABC.TextBoxContent.Text.Remove(UpperABC.globalUpperABC.TextBoxContent.Text.Length - 1);
                                RedewendungDataBinding.globalRedewendugText = UpperABC.globalUpperABC.TextBoxContent.Text;
                            }
                        }
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    {
                        if (UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Length > 0)
                        {
                            if (buttonParameter == "Lö")
                            {
                                UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Clear();
                                RedewendungDataBinding.globalRedewendugText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                            }


                            else if (buttonParameter == "Lö e")
                            {
                                UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Remove(UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Length - 1);
                                RedewendungDataBinding.globalRedewendugText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                            }
                        }
                    }
                    break;
            }
        }

        private void RemoveRedewendungGlobalText()
        {
            if (LowerABC.globalLowerABC != null && LowerABC.globalLowerABC.TextBoxContent.Text.Length == 0)
            {
                RedewendungDataBinding.globalRedewendugText = "";
                RedewendungDataBinding.globaldataName = "";
            }

            else if (LowerQWERTZ.globalLowerQWERTZ != null && LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Length == 0)
            {
                RedewendungDataBinding.globalRedewendugText = "";
                RedewendungDataBinding.globaldataName = "";
            }

            else if (UpperABC.globalUpperABC != null && UpperABC.globalUpperABC.TextBoxContent.Text.Length == 0)
            {
                RedewendungDataBinding.globalRedewendugText = "";
                RedewendungDataBinding.globaldataName = "";
            }

            else if (UpperQWERTZ.globalUpperQWERTZ != null && UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Length == 0)
            {
                RedewendungDataBinding.globalRedewendugText = "";
                RedewendungDataBinding.globaldataName = "";
            }
        }

        public async Task ReadText()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            switch (window.Title)
            {
                case "RedewendungKonfigWindowLowerABC":

                    WordSuggestion.Check_New_Words(RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                    await Speak(RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text);
                    Properties.Settings.Default.ReadCursor = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Length - 1;
                    break;

                case "RedewendungKonfigWindowUpperABC":
                    WordSuggestion.Check_New_Words(RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                    await Speak(RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text);
                    Properties.Settings.Default.ReadCursor = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text.Length - 1;
                    break;

                default:

                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    {
                        WordSuggestion.Check_New_Words(LowerABC.globalLowerABC.TextBoxContent.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                        await Speak(LowerABC.globalLowerABC.TextBoxContent.Text);
                        Properties.Settings.Default.ReadCursor = LowerABC.globalLowerABC.TextBoxContent.Text.Length - 1;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                    {
                        WordSuggestion.Check_New_Words(LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                        await Speak(LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text);
                        Properties.Settings.Default.ReadCursor = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Length - 1;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                    {
                        WordSuggestion.Check_New_Words(UpperABC.globalUpperABC.TextBoxContent.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                        await Speak(UpperABC.globalUpperABC.TextBoxContent.Text);
                        Properties.Settings.Default.ReadCursor = UpperABC.globalUpperABC.TextBoxContent.Text.Length - 1;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    {
                        WordSuggestion.Check_New_Words(UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Remove(0, Properties.Settings.Default.ReadCursor));
                        await Speak(UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text);
                        Properties.Settings.Default.ReadCursor = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Length - 1;
                    }
                    break;
            }
        }

        private void WriteButtonContentToTextbox(string letter)
        {

            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            switch (window.Title)
            {
                case "RedewendungKonfigWindowLowerABC":
                    if (RedewendungDataBinding.globalRedewendungFocus == 1)
                    {
                        RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text;
                    }

                    else if (RedewendungDataBinding.globalRedewendungFocus == 0 && RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text.Length <= 20)
                    {
                        RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text += letter;
                        RedewendungDataBinding.globaldataName = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text;
                    }
                    break;

                case "RedewendungKonfigWindowUpperABC":
                    if (RedewendungDataBinding.globalRedewendungFocus == 1)
                    {
                        RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.RedewendungTextTextBox.Text;
                    }

                    else if (RedewendungDataBinding.globalRedewendungFocus == 0 && RedewendungDataBinding.globaldataName.Length <= 20)
                    {
                        RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text += letter;
                        RedewendungDataBinding.globaldataName = RedewendungKonfigWindowUpperABC.globalRedewendungKonfigWindowUpperABC.DataNameTextBox.Text;
                    }
                    break;

                case "TextSymbolKeyboard":
                    if (TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text.Length <= 26)
                    {
                        TextSymbolKeyboard.globalTextSymbolKeyboardWindow.ImageDataTextBox.Text += letter;
                    }

                    break;

                default:
                    if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                    {
                        LowerABC.globalLowerABC.TextBoxContent.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = LowerABC.globalLowerABC.TextBoxContent.Text;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                    {
                        LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                    {
                        UpperABC.globalUpperABC.TextBoxContent.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = UpperABC.globalUpperABC.TextBoxContent.Text;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                    {
                        UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text;
                    }

                    else if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
                    {
                        NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text += letter;
                        RedewendungDataBinding.globalRedewendugText = NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text;
                    }
                    break;
            }
        }

        private void SetMyButtonBackground(byte r, byte g, byte b)
        {
            int count = 0;

            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {
                foreach (Button button in LowerABC.globalLowerABC.ControlGrid.Children)
                {
                    if (LowerABC.globalLowerABC.configureButton_TextBlock[count].Text == "My")
                        button.Background = new SolidColorBrush(Color.FromRgb(r, g, b));

                    count++;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                foreach (Button button in LowerQWERTZ.globalLowerQWERTZ.ControlGrid.Children)
                {
                    if (LowerQWERTZ.globalLowerQWERTZ.configureButton_TextBlock[count].Text == "My")
                        button.Background = new SolidColorBrush(Color.FromRgb(r, g, b));

                    count++;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                foreach (Button button in UpperABC.globalUpperABC.ControlGrid.Children)
                {
                    if (UpperABC.globalUpperABC.configureButton_TextBlock[count].Text == "My")
                        button.Background = new SolidColorBrush(Color.FromRgb(r, g, b));

                    count++;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                foreach (Button button in UpperQWERTZ.globalUpperQWERTZ.ControlGrid.Children)
                {
                    if (UpperQWERTZ.globalUpperQWERTZ.configureButton_TextBlock[count].Text == "My")
                        button.Background = new SolidColorBrush(Color.FromRgb(r, g, b));

                    count++;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
            {
                foreach (Button button in NumericKeyboard.globalNumericKeyboard.ControlGrid.Children)
                {
                    if (NumericKeyboard.globalNumericKeyboard.configureButton_TextBlock[count].Text == "My")
                        button.Background = new SolidColorBrush(Color.FromRgb(r, g, b));

                    count++;
                }
            }
        }
        private async Task ShowKonfigMenu()
        {   
            MenuKonfigWindow window = new MenuKonfigWindow();
            mainWindow.Effect = blur;
            await Task.Run(() => window.ShowDialogAsync());
            mainWindow.Effect = null;
        }

        private async Task ShowScanMenu()
        {
            ScanOptionsWindow window = new ScanOptionsWindow();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowTextSettingsMenu()
        {
            FontSettingsWindow window = new FontSettingsWindow();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowSpeechMenu()
        {
            VoiceOptionsWindow window = new VoiceOptionsWindow();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowTextInputMenu()
        {
            TextInputWindow window = new TextInputWindow();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private void ShowReadMenu()
        {
            ReadOptionsWindow window = new ReadOptionsWindow();
            window.Top = mainWindow.Top;
            window.Left = mainWindow.Left;
            mainWindow.Effect = blur;
            //await Task.Run(() => window.ShowDialogAsync());
            window.ShowDialog();
            
            mainWindow.Effect = null;
        }

        private async Task ShowGroupViewWindow()
        {
            GroupViewWindow window = new GroupViewWindow();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowImageChoiceWindow()
        {
            ImageChoice window = new ImageChoice();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowTextSymbolKeyboardWindow()
        {
            TextSymbolKeyboard window = new TextSymbolKeyboard();
            await Task.Run(() => window.ShowDialogAsync());
        }

        private async Task ShowMetacomImageWindow()
        {
            MetacomImageWindow window = new MetacomImageWindow();
            await Task.Run(() => window.ShowDialogAsync());
           
        }

        private void BackToDirectoryChoose()
        {
            Window closedWindow = MetacomImageWindow.globalMetacomImageWindow;
            MetacomImageWindow window = new MetacomImageWindow();
            window.Show();
            closedWindow.Close();

        }

        private void OpenFileDialogForImages()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.InitialDirectory = @"C:\";
            openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.tif;...";

            if (openFileDialog.ShowDialog() == true)
            {
                Window closedWindow = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC;

                var filePath = openFileDialog.FileName;
                var redewendungImageSource = ImageHelper.CreateBitmapFrameFromString(filePath);
                RedewendungDataBinding.globalRedewendungImageSource = redewendungImageSource;

                RedewendungDataBinding.globalImageSet = 1;
                RedewendungKonfigWindowLowerABC newWindow = new RedewendungKonfigWindowLowerABC();
                newWindow.Show();
                closedWindow.Close();
                ImageChoice.globalImageChoiceWindow.Close();

            }


            //Process.Start("explorer.exe", @"C:\");
        }


        private void SaveRedewendungVariables()
        {
            if  (RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.RedewendungTextTextBox.Text.Length == 0)
            { MessageBox.Show("Sie müssen den Text eingeben"); }

            else if (RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text.Length == 0)
            { MessageBox.Show("Sie müssen den Dateinamen eingeben"); }

            else if (RedewendungDataBinding.globalImageSet == 0)
            { MessageBox.Show("Sie müssen ein Bild auswählen"); }

            else
            {
                string textToWavFile = RedewendungDataBinding.globalRedewendugText;

                // Save text
                TextWriter txt = new StreamWriter(@"C:\\Program Files\\TMND-GMBH\\TMpicto\\Lang_German\\" +RedewendungDataBinding.globalGroupName + "\\" +
                    RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text + ".txt");

                txt.Write(RedewendungDataBinding.globalRedewendugText);
                txt.Close();

                // Save Image
                ImageHelper.SaveImage(RedewendungDataBinding.globalRedewendungImageSource, @"C:\\Program Files\\TMND-GMBH\\TMpicto\\Lang_German\\" + 
                   RedewendungDataBinding.globalGroupName + "\\" + RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text + ".png");

                // Save Wav
                //Svox Voice
                SpeakVoice.GenerateWave(textToWavFile, @"C:\\Program Files\\TMND-GMBH\\TMpicto\\Lang_German\\" +RedewendungDataBinding.globalGroupName + "\\" +
                    RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.DataNameTextBox.Text + ".wav");

                MessageBox.Show("Speichern komplett");

                GroupViewWindow.globalGroupViweWindow.Close();
                RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.Close();

            }
        }

        public async Task Speak(string writtenText)
        {
            IniReader iniReader = new IniReader();
            MyVoice myVoice = new MyVoice();
            speechSynthesizer.Rate = iniReader.voiceSpeed_MS - 10;
            speechSynthesizer.Volume = iniReader.voiceVolume;
            string textToSpeak;

            if (iniReader.svox_voice_active == 0)
                speechSynthesizer.SelectVoice(iniReader.selectedLanguage);

            if (!Properties.Settings.Default.PressedButton_once && !cursorSelect_Choosen)
                textToSpeak = writtenText.Remove(0, Properties.Settings.Default.ReadCursor);

            else
                textToSpeak = writtenText;


            if (Properties.Settings.Default.MyVoiceActive)
            {
                string[] parts = textToSpeak.Split(' ');               
                foreach (var part in parts)
                {
                    //string soundDataPath = iniReader.dataPath + @"TMmyvoice\GER_M\__juergen_sicher\";
                    //string soundToPlay = soundDataPath + part + ".wav";
                    string soundToPlay = myVoice.GetMyVoiceSoundFile(@"TMmyvoice\GER_M", part);

                    if (File.Exists(soundToPlay))
                    {
                        SoundPlayer snd = new SoundPlayer(soundToPlay);

                        await Task.Run(() => snd.PlaySync()); //PlaySync vermeidet, dass MyVoice und Svox/MS Stimmen gleichzeitig abgespielt werden
                    }

                    else if(iniReader.svox_voice_active == 1)
                    {
                        await Task.Run(() => SpeakVoice.ReadFromBuffer(part, ""));
                    }

                    else
                    {                   
                        await Task.Run(() => speechSynthesizer.Speak(part));
                    }                     
                }                
            }

            else if(iniReader.svox_voice_active == 1)
            {
                await Task.Run(() => SpeakVoice.ReadFromBufferAsync(textToSpeak));
            }

            else
            {
                await Task.Run(() => speechSynthesizer.Speak(textToSpeak));
            }

            Properties.Settings.Default.LastSpeech = textToSpeak;
            cursorSelect_Choosen = false;
        }

        private async Task Repeat()
        {
            IniReader iniReader = new IniReader();
            speechSynthesizer.Rate = iniReader.voiceSpeed_MS - 10;
            speechSynthesizer.Volume = iniReader.voiceVolume;

            if (iniReader.svox_voice_active == 1 && Properties.Settings.Default.MyVoiceActive == false)
                await Task.Run(() => SpeakVoice.ReadFromBufferAsync(Properties.Settings.Default.LastSpeech));

            else if(iniReader.svox_voice_active == 0 && Properties.Settings.Default.MyVoiceActive == false)
            {
                speechSynthesizer.SelectVoice(iniReader.selectedLanguage);
                await Task.Run(() => speechSynthesizer.Speak(Properties.Settings.Default.LastSpeech));
            }
               

            else if(Properties.Settings.Default.MyVoiceActive)
            {
                string[] words = Properties.Settings.Default.LastSpeech.Split(' ');
                MyVoice myVoice = new MyVoice();

                foreach(string word in words)
                {
                    string soundToPlay = myVoice.GetMyVoiceSoundFile(@"TMmyvoice\GER_M", word);

                    if (File.Exists(soundToPlay))
                    {                        
                        SoundPlayer snd = new SoundPlayer(soundToPlay);
                        await Task.Run(() => snd.PlaySync());
                    }

                   else  if (iniReader.svox_voice_active == 1)
                        await Task.Run(() => SpeakVoice.ReadFromBuffer(word, ""));

                    else
                        await Task.Run(() => speechSynthesizer.Speak(word));
                }
            }         
        }

        private void ReturToPreviousWindow()
        {
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            
            switch (window.Title)
            {
                case "ReadOptionsWindow":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "Scanningeinstellungen":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "Spracheinstellungen":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "TextInputWindow":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "Konfigurationen":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "GroupViewWindow":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "RedewendungKonfigWindowLowerABC":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    RedewendungDataBinding.globalImageSet = 0;
                    RemoveRedewendungGlobalText();
                    break;

                case "RedewendungKonfigWindowUpperABC":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    RedewendungDataBinding.globalImageSet = 0;
                    RemoveRedewendungGlobalText();
                    break;

                case "ImageChoice":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "TextSymbolKeyboard":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "MetacomImageWindow":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;

                case "FontSettngsWindow":
                    window.Visibility = System.Windows.Visibility.Hidden;
                    break;
            }
        }


        private async Task SpeakSelected()
        {
            cursorSelect_Choosen = true;

            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {
                await Task.Run(() => Speak(LowerABC.globalLowerABC.TextBoxContent.SelectedText));
                Properties.Settings.Default.LastSpeech = LowerABC.globalLowerABC.TextBoxContent.SelectedText;
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                await Task.Run(() => Speak(LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.SelectedText));
                Properties.Settings.Default.LastSpeech = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.SelectedText;
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                await Task.Run(() => Speak(UpperABC.globalUpperABC.TextBoxContent.SelectedText));
                Properties.Settings.Default.LastSpeech = UpperABC.globalUpperABC.TextBoxContent.SelectedText;
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                await Task.Run(() => Speak(UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.SelectedText));
                Properties.Settings.Default.LastSpeech = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.SelectedText;
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
            {
                await Task.Run(() => Speak(NumericKeyboard.globalNumericKeyboard.TextBoxContent.SelectedText));
                Properties.Settings.Default.LastSpeech = NumericKeyboard.globalNumericKeyboard.TextBoxContent.SelectedText;
            }
        }

        private async Task CursorTextSpeak()
        {
            string text = "aaa";
            cursorSelect_Choosen = true;

            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {
                text = LowerABC.globalLowerABC.TextBoxContent.Text.Remove(0, LowerABC.globalLowerABC.TextBoxContent.SelectionStart);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                text = LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Text.Remove(0, LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.SelectionStart);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                text = UpperABC.globalUpperABC.TextBoxContent.Text.Remove(0, UpperABC.globalUpperABC.TextBoxContent.SelectionStart);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                text = UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Text.Remove(0, UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.SelectionStart);
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == NumericKeyboard.globalNumericKeyboard)
            {
                text = NumericKeyboard.globalNumericKeyboard.TextBoxContent.Text.Remove(0, NumericKeyboard.globalNumericKeyboard.TextBoxContent.SelectionStart);
            }

            await Task.Run(() => Speak(text));
            Properties.Settings.Default.LastSpeech = text;
        } 
        
        private async Task<string> bleble()
        {
            string lol = "";
            await Task.Run(() => lol = SpeakVoice.IsSpeaking().ToString());
            return lol;
        }
    }
}
