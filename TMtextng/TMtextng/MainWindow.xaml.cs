using System;
using System.Text;
using System.Windows;
using TMtextng.KeyboardPages;
using Gma.System.MouseKeyHook;
using System.Diagnostics;
using System.IO;

namespace TMtextng
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SpeechRecognitionManager speechManager = new SpeechRecognitionManager();
            speechManager.InitializeSpeechListener();

            IniReader iniReader = new IniReader();
            ScanningOptions scanningOptions = new ScanningOptions();

            var desktopWorkingArea = System.Windows.SystemParameters.WorkArea;
            this.Left = desktopWorkingArea.Left;
            this.Top = desktopWorkingArea.Top;

            ((MainWindow)Application.Current.MainWindow).Content = new LowerABC();

            this.Width = iniReader.windowWidth;
            
            if(iniReader.keyboard_active == 0)
                this.Height = iniReader.windowHeight - LowerABC.globalLowerABC.LettersGrid.Height;

            else
                this.Height = iniReader.windowHeight;


            Hook.GlobalEvents().MouseDown += (sender, e) =>
            {
                if (!Properties.Settings.Default.isScanPaused)
                {
                    Properties.Settings.Default.globalMouseClickCount++;
                }


                else if (Properties.Settings.Default.isScanPaused)
                {
                    if (Content == LowerABC.globalLowerABC)
                        _ = ScanningOptions.globalScanningOptions.Scanning(LowerABC.globalLowerABC.LettersGrid); 

                    if (Content == LowerQWERTZ.globalLowerQWERTZ)
                        _ = ScanningOptions.globalScanningOptions.Scanning(LowerQWERTZ.globalLowerQWERTZ.LettersGrid);

                    if (Content == UpperABC.globalUpperABC)
                        _ = ScanningOptions.globalScanningOptions.Scanning(UpperABC.globalUpperABC.LettersGrid);

                    if (Content == UpperQWERTZ.globalUpperQWERTZ)
                        _ = ScanningOptions.globalScanningOptions.Scanning(UpperQWERTZ.globalUpperQWERTZ.LettersGrid);

                    if (Content == NumericKeyboard.globalNumericKeyboard)
                        _ = ScanningOptions.globalScanningOptions.Scanning(NumericKeyboard.globalNumericKeyboard.LettersGrid);
                }


                if (this.Content == LowerABC.globalLowerABC)
                {
                    if (Properties.Settings.Default.globalMouseClickCount == 3)
                    {
                        scanningOptions.ScanWrite(LowerABC.globalLowerABC.LettersGrid, LowerABC.globalLowerABC.TextBoxContent);
                    }
                }


                if (this.Content == LowerQWERTZ.globalLowerQWERTZ)
                {
                    if (Properties.Settings.Default.globalMouseClickCount == 3)
                    {
                        scanningOptions.ScanWrite(LowerQWERTZ.globalLowerQWERTZ.LettersGrid, LowerQWERTZ.globalLowerQWERTZ.TextBoxContent);
                    }
                }

                if (this.Content == UpperABC.globalUpperABC)
                {
                    if (Properties.Settings.Default.globalMouseClickCount == 3)
                    {
                        scanningOptions.ScanWrite(UpperABC.globalUpperABC.LettersGrid, UpperABC.globalUpperABC.TextBoxContent);
                    }
                }


                if (this.Content == UpperQWERTZ.globalUpperQWERTZ)
                {
                    if (Properties.Settings.Default.globalMouseClickCount == 3)
                    {
                        scanningOptions.ScanWrite(UpperQWERTZ.globalUpperQWERTZ.LettersGrid, UpperQWERTZ.globalUpperQWERTZ.TextBoxContent);
                    }
                }

                if (this.Content == NumericKeyboard.globalNumericKeyboard)
                {
                    if (Properties.Settings.Default.globalMouseClickCount == 3)
                    {
                        scanningOptions.ScanWrite(NumericKeyboard.globalNumericKeyboard.LettersGrid, NumericKeyboard.globalNumericKeyboard.TextBoxContent);
                    }
                }
            };
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            string abc_user_content = "";

            MainWindowViewModel mainWindowViewModel = new MainWindowViewModel();

            Process[] DesktopKeyboardProcess = Process.GetProcessesByName(mainWindowViewModel.osk_path);
            if (DesktopKeyboardProcess.Length > 0)
                DesktopKeyboardProcess[0].Kill();

            if(File.Exists(WordSuggestion.wordsToReadPath_user_copy))
                abc_user_content = File.ReadAllText(WordSuggestion.wordsToReadPath_user_copy, Encoding.Default);

            WordSuggestion.DeleteFile(WordSuggestion.wordsToReadPath_user);
            WordSuggestion.DeleteFile(WordSuggestion.wordsToReadPath_user_copy);

            if (!Properties.Settings.Default.Is_abc_user_removed)
                File.WriteAllText(WordSuggestion.wordsToReadPath_user, abc_user_content, Encoding.Default);

            else
                WordSuggestion.CreateAbcUser();

            Process.GetCurrentProcess().Kill();
            Application.Current.Shutdown();
        }
    }
}
