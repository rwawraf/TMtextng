using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
using System.Speech.Recognition;
using System.Windows.Threading;
using System.Threading;
using System.Diagnostics;

namespace TMtextng.Konfig
{
    /// <summary>
    /// Interaktionslogik für MainKonfigWindow.xaml
    /// </summary>
    public partial class MenuKonfigWindow : Window
    {
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        IniReader iniReader = new IniReader();
        public static CancellationTokenSource cts = new CancellationTokenSource();

        public MenuKonfigWindow()
        {
            InitializeComponent();
            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            
            List<string> konfigButtons_Text = new List<string>();
            List<int> konfigButtons_ImageVisible = new List<int>();
            List<int> konifgButtons_TextVisible = new List<int>();


            for (int i = 0; i < 6; i++)
            {
                konfigButtons_Text.Add(iniReader.menuKonfigButtonsText[i]);
                konfigButtons_ImageVisible.Add(iniReader.menuKonfigButtonsImageVisible[i]);
                konifgButtons_TextVisible.Add(iniReader.menuKonfigButtonsTextVisible[i]);

                CfgButton konfigButton = new CfgButton(konfigButtons_Text[i], CfgButton.TYPE.Default, konfigButtons_ImageVisible[i], konifgButtons_TextVisible[i]);
                
                
                

                if ((konfigButtons_Text[i] == "Sprache Erkennung" && iniReader.speechRecMode == 1))
                {
                    konfigButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                    
                }
                

                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                konfigButton.MouseLeftButtonDown += (sender, e) => OpenWindow(sender, e, konfigButton.btnTxt);

                if (konfigButtons_Text[i] != "Zurück")
                {
                    wrapPanel.Children.Add(konfigButton);
                }
                else
                {
                    myCCCplace.Children.Add(konfigButton);
                }



            }
        }

        void OpenWindow(object sender, EventArgs e, string commandParameter)
        {
            if (commandParameter == "Sprache Erkennung")
            {
                IniCreator iniCreator = new IniCreator();

                if (iniReader.speechRecMode == 0)
                {
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "speechRecMode", "1");
                    infLoopSpeechRecAsync(cts.Token);
                }
                else if (iniReader.speechRecMode == 1)
                {
                    iniCreator.SaveValue("TMtextng_config_user.ini", "User", "speechRecMode", "0");
                    cts.Cancel();
                    cts.Dispose();
                    cts = new CancellationTokenSource();
                }

            }
            buttonMVVM.writeLetterCommand.Execute(commandParameter);

        }

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

        public async static void infLoopSpeechRecAsync(CancellationToken cancellationToken)
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

        private void FontSizeChange(object sender, SizeChangedEventArgs e)
        {
            double minFontSize = 20.0;
            double maxFontSize = 45.0;
            double maxMinFontSizeDiff = maxFontSize - minFontSize;

            double gridMinHeight = 294;
            double gridMaxHeight = 707.3;
            double gridMaxMinHeightDiff = gridMaxHeight - gridMinHeight;

            double gridMinWidth = 624;
            double gridMaxWidth = 1924;
            double gridMaxMinWidthDiff = gridMaxWidth - gridMinWidth;

            //Linear equation considering "max/min FontSize" and "max/min GridHeight/GridWidth"
            double heightFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinHeightDiff) * this.MenuGrid.ActualHeight + maxMinFontSizeDiff / 2;
            double widthFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinWidthDiff) * this.MenuGrid.ActualWidth + maxMinFontSizeDiff / 2;

            foreach (var children in this.MenuGrid.Children)
            {
                if (children.GetType() == typeof(Button))
                    (children as Button).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);
            }
        }
    }
}
