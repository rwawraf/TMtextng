using Gma.System.MouseKeyHook;
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
using System.Windows.Media.Imaging;
using TMnote;

namespace TMtextng.KeyboardPages
{
    /// <summary>
    /// Interaktionslogik für UpperABC.xaml
    /// </summary>
    public partial class UpperABC : Page
    {
        public static UpperABC globalUpperABC;
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
        public UpperABC()
        {
            globalUpperABC = this;

            InitializeComponent();

            this.DataContext = new MainWindowViewModel();

            MainWindowViewModel buttonMVVM = new MainWindowViewModel();
            
            TextBoxContent.Text = Properties.Settings.Default.SavedText;

            if (iniReader.keyboard_active == 0)
            {
                UpperABCgrid.RowDefinitions.RemoveAt(UpperABCgrid.RowDefinitions.Count - 1);
                UpperABCgrid.Children.RemoveAt(UpperABCgrid.Children.Count - 1);
            }
            TextBoxContent.FontSize = iniReader.fontsizesValues[0] + 30;

            configureButton_TextBlock = new TextBlock[iniReader.configureButton_count];
            configureButton = new Button[iniReader.configureButton_count];
            Button[,] UpperABC_Button = new Button[4, 11];

            switch (iniReader.selectedLanguage)
            {
                case "Spanisch W":
                    buttonLetter = iniReader.upperABCButtonLetter_ESP;
                    break;

                case "Italienisch W":
                    buttonLetter = iniReader.upperABCButtonLetter_ITA;
                    break;

                case "Niederländisch W":
                    buttonLetter = iniReader.upperABCButtonLetter_NLD;
                    break;

                case "English W":
                    buttonLetter = iniReader.upperABCButtonLetter_ENG;
                    break;

                case "Portugiesisch W":
                    buttonLetter = iniReader.upperABCButtonLetter_POR;
                    break;

                case "Französisch W":
                    buttonLetter = iniReader.upperABCButtonLetter_FRA;
                    break;

                default:
                    buttonLetter = iniReader.upperABCButtonLetter;
                    break;
            }

            List<string> suggestedWordsList = new List<string>();

            Viewbox[] configureButton_Viewbox = new Viewbox[iniReader.configureButton_count];

            TextBlock[,] UpperABCButton_TextBlock = new TextBlock[4, 11];
            Viewbox[,] UpperABCButton_Viewbox = new Viewbox[4, 11];


            Viewbox[] wordSuggestionButtons_Viewbox = new Viewbox[4];


            if (File.Exists(WordSuggestion.wordsToReadPath_user_copy))
                lineCount = File.ReadLines(WordSuggestion.wordsToReadPath_user_copy).Count();

            words = new string[lineCount, 4];

            WordSuggestion.ReadSuggestionWords(words);

            WordSuggestion.SortWordsByNumbersOfUsedTimes(words);

            WordSuggestionGrid.Background = (Brush)(new BrushConverter().ConvertFrom("#FF696969"));

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

                if (configureButton_TextBlock[i].Text == "My")
                {

                    if (Properties.Settings.Default.MyVoiceActive)
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
                    else if (valueFromIniReader == ":") { valueFromIniReader = "Doppelpunkt"; }
                    else if (valueFromIniReader == '*'.ToString()) { valueFromIniReader = "Stern"; }

                    string path = iniReader.dataPath + iniReader.keyboardImagesPath + valueFromIniReader + "_.png";

                   UpperABC_Button[i, j] = new Button();
                   UpperABCButton_TextBlock[i, j] = new TextBlock();
                   UpperABCButton_TextBlock[i, j].Text = buttonLetter[buttonCount];
                   UpperABCButton_TextBlock[i, j].FontSize = iniReader.fontsizesValues[3] + 30;
                   UpperABCButton_TextBlock[i, j].MaxWidth = 250;
                   UpperABCButton_TextBlock[i, j].TextWrapping = TextWrapping.Wrap;
                   UpperABCButton_Viewbox[i, j] = new Viewbox();
                   UpperABCButton_Viewbox[i, j].StretchDirection = StretchDirection.DownOnly;
                   UpperABCButton_Viewbox[i, j].Stretch = Stretch.Fill;
                   UpperABCButton_Viewbox[i, j].MaxHeight = 100;
                   UpperABCButton_Viewbox[i, j].MinHeight = 5;
                   UpperABCButton_Viewbox[i, j].MinWidth = 5;
                   UpperABCButton_Viewbox[i, j].MaxWidth = 100;
                   UpperABCButton_Viewbox[i, j].Child =UpperABCButton_TextBlock[i, j];
                   UpperABC_Button[i, j].Content =UpperABCButton_Viewbox[i, j];

                    if (iniReader.upperABCButtonsImages[buttonCount] == 1)
                    {
                        if (!System.IO.File.Exists(path))
                        {
                            path = iniReader.dataPath + @"Tastatur_Bilder\" + valueFromIniReader + ".png";
                        }

                        keyButtonImage = ImageHelper.LoadBitmapImage(path);

                        Image img = new Image();
                        img.Stretch = Stretch.Fill;
                        img.Source = keyButtonImage;

                        WrapPanel wrapPnl = new WrapPanel();
                        wrapPnl.Margin = new Thickness(5);
                        wrapPnl.Children.Add(img);

                        UpperABC_Button[i, j].Content = wrapPnl;
                        UpperABC_Button[i, j].Background = Brushes.DarkGray;
                    }

                    if (valueFromIniReader == "↑")
                        UpperABC_Button[i, j].CommandParameter = "abc";

                    else if (valueFromIniReader == "S-ON")
                    {
                        UpperABC_Button[i, j].CommandParameter = "S-ON UpperABC";
                    }
                    else if (valueFromIniReader == "S-PAU")
                    {
                        UpperABC_Button[i, j].CommandParameter = "S-PAU UpperABC";
                    }
                    else if (valueFromIniReader == "S-OFF")
                    {
                        UpperABC_Button[i, j].CommandParameter = "S-OFF UpperABC";
                    }
                    else
                        UpperABC_Button[i, j].CommandParameter = buttonLetter[buttonCount];

                    Binding myBinding = new Binding("writeLetterCommand");
                    myBinding.Source = buttonMVVM;
                    UpperABC_Button[i, j].SetBinding(Button.CommandProperty, myBinding);


                    Grid.SetRow(UpperABC_Button[i, j], i);
                    Grid.SetColumn(UpperABC_Button[i, j], j);

                    if (i == 3 && j > 3 && j < 5)
                    {
                        buttonCount++;
                        continue;
                    }

                    LettersGrid.Children.Add(UpperABC_Button[i, j]);
                    buttonCount++;
                }
            }

            Grid.SetColumnSpan(UpperABC_Button[3, 3], 2);
        }


        public async void TextChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            List<string> suggestedWordsList = new List<string>();
            IniReader iniReader = new IniReader();

            if (Properties.Settings.Default.ReadCursor < 0)
                Properties.Settings.Default.ReadCursor = 0;
            //For unknown reason Properties.Settings.Default.ReadCursor value is less than 0 when Text cleared during Blockwesie reading 

            int count = 0;

            for (int i = 0; i < lineCount; i++)
            {
                for (int j = 0; j < 4; j++)
                {
                    if (words[i, j].StartsWith(globalUpperABC.TextBoxContent.Text.Split(' ').Last(), StringComparison.OrdinalIgnoreCase))
                    {
                        suggestedWordsList.Add(words[i, j]);
                    }
                }
            }

            for (int i = 0; i < globalUpperABC.WordSuggestionGrid.Children.Count; i++)
            {
                if (globalUpperABC.TextBoxContent.Text.Split(' ').Last() != "" && count < suggestedWordsList.Count)
                {
                    globalUpperABC.WordSuggestionGrid.Children[i].Visibility = Visibility.Visible;
                    wordSuggestionButtons_TextBlock[i].Text = suggestedWordsList[count];
                    wordSuggestionButtons[i].CommandParameter = wordSuggestionButtons_TextBlock[i].Text;
                }

                else
                {
                    globalUpperABC.WordSuggestionGrid.Children[i].Visibility = Visibility.Hidden;
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

            else if (iniReader.textReadMode == "1")
            {
                ReadOptions readOptions = new ReadOptions();
                await readOptions.ReadSentence(TextBoxContent);
            }
        }

        public static void WriteSuggestedWord(object sender, RoutedEventArgs e)
        {
            string writtenText = globalUpperABC.TextBoxContent.Text;
            string wordToReplace = writtenText.Substring(writtenText.LastIndexOf(" ") + 1);
            string newWord;

            int place = writtenText.LastIndexOf(" ") + 1;
            newWord = char.ToUpper((sender as Button).Content.ToString()[0]) + (sender as Button).Content.ToString().Substring(1);

            string result = writtenText.Remove(place, wordToReplace.Length).Insert(place, newWord);
            globalUpperABC.TextBoxContent.Text = result;


            globalUpperABC.TextBoxContent.CaretIndex = globalUpperABC.TextBoxContent.Text.Length;
            globalUpperABC.TextBoxContent.ScrollToEnd();
            globalUpperABC.TextBoxContent.Focus();

            WordSuggestion.UpdateUserAbcOnWordClick(newWord);
        }
    }
}
