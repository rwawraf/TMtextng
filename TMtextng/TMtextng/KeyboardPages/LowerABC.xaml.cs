using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using TMtextng.Konfig;
using TMnote;
using TMtextng.Redewendung;

namespace TMtextng.KeyboardPages
{
    /// <summary>
    /// Interaktionslogik für LowerABC.xaml
    /// </summary>
    public partial class LowerABC : Page
    {
        public static LowerABC globalLowerABC;

        int buttonCount = 0;
        int lineCount;
        string[,] words;       
        ImageSource keyButtonImage;
        IniReader iniReader = new IniReader();
        public Button[] configureButton;
        public TextBlock[] configureButton_TextBlock;
        public TextBlock[] wordSuggestionButtons_TextBlock = new TextBlock[4];
        public Button[] wordSuggestionButtons = new Button[4];
        List<string> buttonLetter;
        public LowerABC()
        {
            globalLowerABC = this;

            InitializeComponent();

            if(iniReader.keyboard_active == 0)
            {
                LowerABCgrid.RowDefinitions.RemoveAt(LowerABCgrid.RowDefinitions.Count - 1);
                LowerABCgrid.Children.RemoveAt(LowerABCgrid.Children.Count - 1);
            }
            TextBoxContent.FontSize = iniReader.fontsizesValues[0] + 30;

            DataContext = new MainWindowViewModel();            
            MainWindowViewModel buttonMVVM = new MainWindowViewModel();          

            TextBoxContent.Text = Properties.Settings.Default.SavedText;

            configureButton_TextBlock = new TextBlock[iniReader.configureButton_count];
            configureButton = new Button[iniReader.configureButton_count];
            Button[,] LowerABC_Button = new Button[4, 11];


            switch (iniReader.selectedLanguage)
            {
                case "Spanisch W":
                    buttonLetter = iniReader.abcButtonLetter_ESP;
                    break;

                case "Italienisch W":
                    buttonLetter = iniReader.abcButtonLetter_ITA;
                    break;

                case "Niederländisch W":
                    buttonLetter = iniReader.abcButtonLetter_NLD;
                    break;

                case "English W":
                    buttonLetter = iniReader.abcButtonLetter_ENG;
                    break;

                case "Portugiesisch W":
                    buttonLetter = iniReader.abcButtonLetter_POR;
                    break;

                case "Französisch W":
                    buttonLetter = iniReader.abcButtonLetter_FRA;
                    break;

                default:
                    buttonLetter = iniReader.abcButtonLetter;
                    break;
            }


            List<string> suggestedWordsList = new List<string>();
            
            Viewbox[] configureButton_Viewbox = new Viewbox[iniReader.configureButton_count];

            TextBlock[,] LowerABC_TextBlock = new TextBlock[4, 11];
            Viewbox[,] LowerABC_Viewbox = new Viewbox[4, 11];


            Viewbox[] wordSuggestionButtons_Viewbox = new Viewbox[4];


            if (File.Exists(WordSuggestion.wordsToReadPath_user_copy))
                lineCount = File.ReadLines(WordSuggestion.wordsToReadPath_user_copy).Count();

            words = new string[lineCount, 4];        

            WordSuggestion.ReadSuggestionWords(words);
            WordSuggestion.SortWordsByNumbersOfUsedTimes(words);

            WordSuggestionGrid.Background = (Brush)new BrushConverter().ConvertFrom("#FF696969");

            

            for (int i = 0; i < 4; i++)
            {
                wordSuggestionButtons[i] = new Button();              
                wordSuggestionButtons[i].Visibility = Visibility.Hidden;
                wordSuggestionButtons[i].Background = (Brush)new BrushConverter().ConvertFrom("#FFDCDCDC");

                wordSuggestionButtons_TextBlock[i] = new TextBlock();
                wordSuggestionButtons_TextBlock[i].FontSize = iniReader.fontsizesValues[2] + 30;
                wordSuggestionButtons_TextBlock[i].MaxWidth = 550;
                wordSuggestionButtons_TextBlock[i].TextWrapping = TextWrapping.Wrap;


                wordSuggestionButtons_Viewbox[i] = new Viewbox();
                wordSuggestionButtons_Viewbox[i].StretchDirection = StretchDirection.DownOnly;
                wordSuggestionButtons_Viewbox[i].Stretch = Stretch.Fill;
                wordSuggestionButtons_Viewbox[i].MaxHeight = 300;
                wordSuggestionButtons_Viewbox[i].MinHeight = 10;
                wordSuggestionButtons_Viewbox[i].MinWidth = 50;
                wordSuggestionButtons_Viewbox[i].MaxWidth = 300;
                wordSuggestionButtons_Viewbox[i].Child = wordSuggestionButtons_TextBlock[i];
               
                Binding myBinding = new Binding("writeSuggestedWordCommand");
                myBinding.Source = buttonMVVM;
                wordSuggestionButtons[i].SetBinding(Button.CommandProperty, myBinding);
                wordSuggestionButtons[i].Content = wordSuggestionButtons_Viewbox[i];
   
                WordSuggestionGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(wordSuggestionButtons[i], i);

                WordSuggestionGrid.Children.Add(wordSuggestionButtons[i]);
            }

            for (int i = 0; i < configureButton.Length; i++)
            {
                configureButton[i] = new Button();
                configureButton_TextBlock[i] = new TextBlock();

                if (i == 1)
                {
                    if (iniReader.textReadMode == "1")
                        configureButton_TextBlock[i].Text = "de/s";

                    if (iniReader.textReadMode == "2")
                        configureButton_TextBlock[i].Text = "de/w";

                    if (iniReader.textReadMode == "3")
                        configureButton_TextBlock[i].Text = "de/b";
                }

                else
                configureButton_TextBlock[i].Text = iniReader.configureButtonLetter[i];


                configureButton_TextBlock[i].FontSize = iniReader.fontsizesValues[1] + 30;
                configureButton_TextBlock[i].MaxWidth = 250;
                configureButton_TextBlock[i].TextWrapping = TextWrapping.Wrap;
                configureButton_Viewbox[i] = new Viewbox();
                configureButton_Viewbox[i].StretchDirection = StretchDirection.DownOnly;
                configureButton_Viewbox[i].Stretch = Stretch.Fill;
                configureButton_Viewbox[i].MaxHeight = 300;
                configureButton_Viewbox[i].MinHeight = 10;
                configureButton_Viewbox[i].MinWidth = 50;
                configureButton_Viewbox[i].MaxWidth = 300;
                configureButton_Viewbox[i].Child = configureButton_TextBlock[i];
                configureButton[i].Content = configureButton_Viewbox[i];

                if(configureButton_TextBlock[i].Text == "My")
                {

                    if(Properties.Settings.Default.MyVoiceActive)
                    configureButton[i].Background = new SolidColorBrush(Color.FromRgb(iniReader.configureButtonRed[i], iniReader.configureButtonGreen[i], iniReader.configureButtonBlue[i]));

                    else
                        configureButton[i].Background = Brushes.AliceBlue;
                }
                else
                    configureButton[i].Background = new SolidColorBrush(Color.FromRgb(iniReader.configureButtonRed[i], iniReader.configureButtonGreen[i], iniReader.configureButtonBlue[i]));

                if (configureButton_TextBlock[i].Text == "Vorl")
                {
                    if (iniReader.textReadMode == "3")
                        configureButton[i].IsEnabled = true;
                    else
                        configureButton[i].IsEnabled = false;
                }

                else
                    configureButton[i].IsEnabled = iniReader.configureButtonEnabled[i];


                if (iniReader.configureButtonVisible[i] == "hidden")
                    configureButton[i].Visibility = Visibility.Hidden;


                configureButton[i].CommandParameter = configureButton_TextBlock[i].Text;
                Binding myBinding = new Binding("writeLetterCommand");
                myBinding.Source = buttonMVVM;
                configureButton[i].SetBinding(Button.CommandProperty, myBinding);


                ControlGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
                Grid.SetColumn(configureButton[i], i);
                ControlGrid.Children.Add(configureButton[i]);
            }


            for (int i = 0; i < 4; i++)
                LettersGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < 11; i++)
                LettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    string valueFromIniReader = buttonLetter[buttonCount];

                    if (valueFromIniReader == "?") { valueFromIniReader = "fragezeichen"; }
                    else if (valueFromIniReader == "!?#") { valueFromIniReader = "!fragezeichen#"; }
                    else if (valueFromIniReader == '"'.ToString()) { valueFromIniReader = "Hochkomma"; }
                    else if (valueFromIniReader == '*'.ToString()) { valueFromIniReader = "Stern"; }

                    string path = iniReader.dataPath + iniReader.keyboardImagesPath + valueFromIniReader + ".png";

                    LowerABC_Button[i, j] = new Button();
                    LowerABC_TextBlock[i, j] = new TextBlock();
                    LowerABC_TextBlock[i, j].Text = buttonLetter[buttonCount];
                    LowerABC_TextBlock[i, j].FontSize = iniReader.fontsizesValues[3] + 30;
                    LowerABC_TextBlock[i, j].MaxWidth = 250;
                    LowerABC_TextBlock[i, j].TextWrapping = TextWrapping.Wrap;
                    LowerABC_Viewbox[i, j] = new Viewbox();
                    LowerABC_Viewbox[i, j].StretchDirection = StretchDirection.DownOnly;
                    LowerABC_Viewbox[i, j].Stretch = Stretch.Fill;
                    LowerABC_Viewbox[i, j].MaxHeight = 100;
                    LowerABC_Viewbox[i, j].MinHeight = 5;
                    LowerABC_Viewbox[i, j].MinWidth = 5;
                    LowerABC_Viewbox[i, j].MaxWidth = 100;
                    LowerABC_Viewbox[i, j].Child = LowerABC_TextBlock[i, j];
                    LowerABC_Button[i, j].Content = LowerABC_Viewbox[i, j];

                    if (iniReader.abcButtonsImages[buttonCount] == 1 && iniReader.keyButtonImageVisibility == 1)
                    {
                        keyButtonImage = ImageHelper.LoadBitmapImage(path);
                        Image img = new Image();
                        img.Stretch = Stretch.Fill;
                        img.Source = keyButtonImage;

                        WrapPanel wrapPnl = new WrapPanel();
                        wrapPnl.Margin = new Thickness(5);
                        wrapPnl.Children.Add(img);

                        LowerABC_Button[i, j].Content = wrapPnl;
                        LowerABC_Button[i, j].Background = Brushes.DarkGray;
                    }

                    if (valueFromIniReader == "↑")
                        LowerABC_Button[i, j].CommandParameter = "ABC";

                    else if (valueFromIniReader == "S-ON")
                    {
                        LowerABC_Button[i, j].CommandParameter = "S-ON LowerABC";
                    }
                    else if (valueFromIniReader == "S-PAU")
                    {
                        LowerABC_Button[i, j].CommandParameter = "S-PAU LowerABC";
                    }
                    else if (valueFromIniReader == "S-OFF")
                    {
                        LowerABC_Button[i, j].CommandParameter = "S-OFF LowerABC";
                    }
                    else
                        LowerABC_Button[i, j].CommandParameter = buttonLetter[buttonCount];

                    Binding myBinding = new Binding("writeLetterCommand");
                    myBinding.Source = buttonMVVM;
                    LowerABC_Button[i, j].SetBinding(Button.CommandProperty, myBinding);

                    Grid.SetRow(LowerABC_Button[i, j], i);
                    Grid.SetColumn(LowerABC_Button[i, j], j);

                    if (i == 3 && j > 3 && j < 5)
                    {
                        buttonCount++;
                        continue;                        
                    }     

                    LettersGrid.Children.Add(LowerABC_Button[i, j]);
                    buttonCount++;
                }

            }

            Grid.SetColumnSpan(LowerABC_Button[3, 3], 2);
        }

        public async void TextChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            List<string> suggestedWordsList = new List<string>();
            Properties.Settings.Default.TextBoxContent = TextBoxContent.Text;
            int count = 0;
            IniReader iniReader = new IniReader();

            if (Properties.Settings.Default.ReadCursor < 0)
                Properties.Settings.Default.ReadCursor = 0;
            //For unknown reason Properties.Settings.Default.ReadCursor value is less than 0 when Text cleared during Blockwesie reading 

            for (int i = 0; i < lineCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (words[i, j].StartsWith(globalLowerABC.TextBoxContent.Text.Split(' ').Last(), StringComparison.OrdinalIgnoreCase))
                    {
                        suggestedWordsList.Add(words[i, j]);                     
                    }                
                }
            }

            for(int i = 0; i < globalLowerABC.WordSuggestionGrid.Children.Count; i++)
            {
                if (globalLowerABC.TextBoxContent.Text.Split(' ').Last() != "" && count < suggestedWordsList.Count)
                {
                    globalLowerABC.WordSuggestionGrid.Children[i].Visibility = Visibility.Visible;
                    wordSuggestionButtons_TextBlock[i].Text = suggestedWordsList[count];
                    wordSuggestionButtons[i].CommandParameter = wordSuggestionButtons_TextBlock[i].Text;
                }

                else
                {
                    globalLowerABC.WordSuggestionGrid.Children[i].Visibility = Visibility.Hidden;
                    wordSuggestionButtons_TextBlock[i].Text = "";
                }


                if (count < suggestedWordsList.Count)
                {
                    count++;
                }
            }

            if (iniReader.textReadMode == "2")
            {
                ReadOptions readOptions = new ReadOptions();
                await readOptions.ReadWord(TextBoxContent);
            }
            
            else if(iniReader.textReadMode == "1")
            {
                ReadOptions readOptions = new ReadOptions();
                await readOptions.ReadSentence(TextBoxContent);
            }
        }
    }
}
