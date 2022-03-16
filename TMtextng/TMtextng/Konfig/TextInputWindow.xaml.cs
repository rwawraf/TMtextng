using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Linq;
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
    /// Interaction logic for TextInputWindow.xaml
    /// </summary>
    public partial class TextInputWindow : Window
    {
        Label[] optionLabels = new Label[2];
        static TextBox[] textBoxes = new TextBox[2];
        IniReader iniReader = new IniReader();
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();

        public TextInputWindow()
        {
            InitializeComponent();
            MainWindowViewModel buttonMVVM = new MainWindowViewModel();

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            List<string> textInputButtons_Text = new List<string>();
            List<int> textInputButtons_ImageVisible = new List<int>();
            List<int> textInputButtons_TextVisible = new List<int>();
            string[] userWordsOptions = { "Anzahl der starts von TMTextng (1-40)", "Mindestanzahl der Wortbenutzung (1-10)" };


            for (int i = 0; i < 2; i++)
            {
                optionLabels[i] = new Label();
                optionLabels[i].FontSize = 20;

                optionLabels[i].Content = userWordsOptions[i];
                optionLabels[i].VerticalContentAlignment = VerticalAlignment.Center;


                Grid.SetRow(optionLabels[i], i);
                Grid.SetColumn(optionLabels[i], 1);
                TextInputMenuGrid.Children.Add(optionLabels[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                textBoxes[i] = new TextBox();
                textBoxes[i].PreviewTextInput += TextBoxInput;
                textBoxes[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                textBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;
                textBoxes[i].FontSize = 40;
                textBoxes[i].MaxLength = 2;

                if (i == 0)
                {
                    textBoxes[i].Text = iniReader.amount_of_App_Starts.ToString();
                    textBoxes[i].TextChanged += Set_Amount_Of_App_Starts;
                }

                if (i == 1)
                {
                    textBoxes[i].Text = iniReader.min_Suggested_Word_Uses.ToString();
                    textBoxes[i].TextChanged += Set_Min_Suggested_Word_Uses;
                }


                Grid.SetRow(textBoxes[i], i);
                Grid.SetColumn(textBoxes[i], 0);
                TextInputMenuGrid.Children.Add(textBoxes[i]);
            }

            for (int i = 0; i < 2; i++)
            {
                textInputButtons_Text.Add(iniReader.textInputButtonsText[i]);
                textInputButtons_ImageVisible.Add(iniReader.textInputButtonsTextVisible[i]);
                textInputButtons_TextVisible.Add(iniReader.textInputButtonsTextVisible[i]);


                CfgButton konfigButton = new CfgButton(textInputButtons_Text[i], CfgButton.TYPE.Default, textInputButtons_ImageVisible[i], textInputButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                if (textInputButtons_Text[i] != "Zurück")
                {
                    myCCCplace1.Children.Add(konfigButton);
                }
                else
                {
                    myCCCplace2.Children.Add(konfigButton);
                }
                konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);

            }


            /*Button deleteUserAbcButton = new Button();
             deleteUserAbcButton.CommandParameter = "deleteUserAbcButton";
             deleteUserAbcButton.Content = "Benutzer - Wortschatz löschen";
             deleteUserAbcButton.FontSize = 40;
             Binding myBinding = new Binding("writeLetterCommand");
             myBinding.Source = buttonMVVM;
             deleteUserAbcButton.SetBinding(Button.CommandProperty, myBinding);


             Grid.SetRow(deleteUserAbcButton, 2);
             Grid.SetColumn(deleteUserAbcButton, 0);

             Grid.SetColumnSpan(deleteUserAbcButton, 2);

             TextInputMenuGrid.Children.Add(deleteUserAbcButton);*/
        }

        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
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
            double heightFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinHeightDiff) * this.TextInputMenuGrid.ActualHeight + maxMinFontSizeDiff / 2;
            double widthFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinWidthDiff) * this.TextInputMenuGrid.ActualWidth + maxMinFontSizeDiff / 2;

            foreach (var children in this.TextInputMenuGrid.Children)
            {
                if (children.GetType() == typeof(Button))
                    (children as Button).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);
            }
        }

        public static void Set_Amount_Of_App_Starts(object sender, RoutedEventArgs e)
        {
            int amount_of_app_starts = 0;

            if (String.IsNullOrEmpty(textBoxes[0].Text))
            {
                amount_of_app_starts = 4;
            }
            else
            {
                if (int.Parse(textBoxes[0].Text) > 40)
                    amount_of_app_starts = 40;

                else if (int.Parse(textBoxes[0].Text) < 1)
                    amount_of_app_starts = 1;

                else
                    amount_of_app_starts = int.Parse(textBoxes[0].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "TMTextng_Amount_Of_Starts", amount_of_app_starts.ToString());
        }

        public static void Set_Min_Suggested_Word_Uses(object sender, RoutedEventArgs e)
        {
            int amount_of_suggested_word_uses = 0;

            if (String.IsNullOrEmpty(textBoxes[1].Text))
            {
                amount_of_suggested_word_uses = 3;
            }
            else
            {
                if (int.Parse(textBoxes[1].Text) > 10)
                    amount_of_suggested_word_uses = 10;

                else if (int.Parse(textBoxes[1].Text) < 1)
                    amount_of_suggested_word_uses = 1;

                else
                    amount_of_suggested_word_uses = int.Parse(textBoxes[1].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "TMTextng_Minimum_Amount_Of_Suggestested_Word_Uses", amount_of_suggested_word_uses.ToString());
        }
        public void TextBoxInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}