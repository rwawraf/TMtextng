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
    /// Interaction logic for TextSettings.xaml
    /// </summary>
    public partial class FontSettingsWindow : Window
    {
        Label[] optionLabels = new Label[5];
        IniReader iniReader = new IniReader();
        static TextBox[] textBoxes = new TextBox[4];
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        public FontSettingsWindow()
        {
            InitializeComponent();

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            string[] fontSizeKeys = { "Input_fontsize", "Konfig_fontsize", "WordSuggestion_fontsize", "Keyboard_fontsize"};

            for(int i = 0; i < 5; i++)
            {
                optionLabels[i] = new Label();
                optionLabels[i].FontSize = 20;
                optionLabels[i].Content = iniReader.fontSettingsText[i];
                optionLabels[i].VerticalContentAlignment = VerticalAlignment.Center;
                Grid.SetRow(optionLabels[i], i);
                Grid.SetColumn(optionLabels[i], 0);
                TextSettingsGrid.Children.Add(optionLabels[i]);
            }

            for (int i = 0; i < 4; i++)
            {
                textBoxes[i] = new TextBox();
                textBoxes[i].PreviewTextInput += TextBoxInput;
                textBoxes[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                textBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;
                textBoxes[i].FontSize = 40;
                textBoxes[i].MaxLength = 2;

                textBoxes[i].Text = iniReader.fontsizesValues[i].ToString();

                if (i == 0)
                    textBoxes[i].TextChanged += SetInputFontsize;

                else if (i == 1)
                    textBoxes[i].TextChanged += SetKonfigFontsize;

                else if (i == 2)
                    textBoxes[i].TextChanged += SetWordSuggestionFontsize;

                else if (i == 3)
                    textBoxes[i].TextChanged += SetKeyboardFontsize;      

                Grid.SetRow(textBoxes[i], i);
                Grid.SetColumn(textBoxes[i], 1);
                TextSettingsGrid.Children.Add(textBoxes[i]);
            }  

            CheckBox checkBox = new CheckBox();
            
            checkBox.Checked += TurnOnKeyboard;
            checkBox.Unchecked += TurnOffKeyboard;

            if (iniReader.keyboard_active == 1)
                checkBox.IsChecked = true;

            else
                checkBox.IsChecked = false;

            Viewbox viewbox = new Viewbox();
            viewbox.Child = checkBox;

            Grid.SetRow(viewbox, 4);
            Grid.SetColumn(viewbox, 1);
            TextSettingsGrid.Children.Add(viewbox);


            CfgButton konfigButton = new CfgButton("Zurück", CfgButton.TYPE.Default, 1, 1);
            konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
            konfigButton.Visibility = Visibility.Visible;
            konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);
            myCCCplace1.Children.Add(konfigButton);
        }
        

        private void TurnOnKeyboard(object sender, RoutedEventArgs e)
        {
            IniReader iniReader = new IniReader();
            IniCreator iniCreator = new IniCreator();

            if(iniReader.keyboard_active == 0)
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "Keyboard_active", "1");
        }

        private void TurnOffKeyboard(object sender, RoutedEventArgs e)
        {
            IniReader iniReader = new IniReader();
            IniCreator iniCreator = new IniCreator();

            if (iniReader.keyboard_active == 1)
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "Keyboard_active", "0");
        }

        public static void SetKonfigFontsize(object sender, RoutedEventArgs e)
        {
            int fontsize;

            if (String.IsNullOrEmpty(textBoxes[1].Text))
            {
                fontsize = 30;
            }
            else
            {
                if (int.Parse(textBoxes[1].Text) > 50)
                {
                    fontsize = 50;
                    textBoxes[1].Text = fontsize.ToString();
                }

                else if (int.Parse(textBoxes[1].Text) <= 0)
                {
                    fontsize = 1;
                    textBoxes[1].Text = fontsize.ToString();
                }

                else
                    fontsize = int.Parse(textBoxes[1].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "Konfig_fontsize", fontsize.ToString());
        }

        public static void SetInputFontsize(object sender, RoutedEventArgs e)
        {
            int fontsize;

            if (String.IsNullOrEmpty(textBoxes[0].Text))
            {
                fontsize = 30;
            }
            else
            {
                if (int.Parse(textBoxes[0].Text) > 50)
                {
                    fontsize = 50;
                    textBoxes[0].Text = fontsize.ToString();
                }

                else if (int.Parse(textBoxes[0].Text) <= 0)
                {
                    fontsize = 1;
                    textBoxes[0].Text = fontsize.ToString();
                }

                else
                    fontsize = int.Parse(textBoxes[0].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "Input_fontsize", fontsize.ToString());
        }

        public static void SetKeyboardFontsize(object sender, RoutedEventArgs e)
        {
            int fontsize;

            if (String.IsNullOrEmpty(textBoxes[3].Text))
            {
                fontsize = 30;
            }
            else
            {
                if (int.Parse(textBoxes[3].Text) > 50)
                {
                    fontsize = 50;
                    textBoxes[3].Text = fontsize.ToString();
                }

                else if (int.Parse(textBoxes[3].Text) <= 0)
                {
                    fontsize = 1;
                    textBoxes[3].Text = fontsize.ToString();
                }

                else
                    fontsize = int.Parse(textBoxes[3].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "Keyboard_fontsize", fontsize.ToString());
        }

        public static void SetWordSuggestionFontsize(object sender, RoutedEventArgs e)
        {
            int fontsize;

            if (String.IsNullOrEmpty(textBoxes[2].Text))
            {
                fontsize = 30;
            }
            else
            {
                if (int.Parse(textBoxes[2].Text) > 50)
                {
                    fontsize = 50;
                    textBoxes[2].Text = fontsize.ToString();
                }

                else if (int.Parse(textBoxes[2].Text) <= 0)
                {
                    fontsize = 1;
                    textBoxes[2].Text = fontsize.ToString();
                }

                else
                    fontsize = int.Parse(textBoxes[2].Text);
            }
            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "WordSuggestion_fontsize", fontsize.ToString());
        }


        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }

        public void TextBoxInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
    }
}
