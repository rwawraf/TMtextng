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
using TMtextng.KeyboardPages;

namespace TMtextng.Konfig
{
    /// <summary>
    /// Interaktionslogik für KonfigWindow.xaml
    /// </summary>
    public partial class ScanOptionsWindow : Window
    {
        public static ScanOptionsWindow globalScanOptions;
        static TextBox[] optionTextBoxes = new TextBox[2];

        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        public ScanOptionsWindow()
        {
            globalScanOptions = this;
            InitializeComponent();

            IniReader iniReader = new IniReader();
            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            Label[] optionLabels = new Label[2];
            

            List<string> scanOptionsButtonsText = new List<string>();
            List<int> scanOptionsButtons_TextVisible = new List<int>();
            List<int> scanOptionsButtons_ImageVisible = new List<int>();

            for(int i = 0; i < 2; i++)
            {
                optionLabels[i] = new Label();
                optionLabels[i].FontSize = 30;
                optionLabels[i].Content = iniReader.scanOpitonsButtonsText[i + 5];
                optionLabels[i].VerticalContentAlignment = VerticalAlignment.Center;

                Grid.SetColumn(optionLabels[i], 0);
                Grid.SetRow(optionLabels[i], i);
                griddy.Children.Add(optionLabels[i]);

                optionTextBoxes[i] = new TextBox();
                optionTextBoxes[i].FontSize = 30;
                optionTextBoxes[i].PreviewTextInput += TextBoxInput;
                optionTextBoxes[i].MaxLength = 2;
                optionTextBoxes[i].HorizontalContentAlignment = HorizontalAlignment.Center;
                optionTextBoxes[i].VerticalContentAlignment = VerticalAlignment.Center;

                if (i == 0)
                {
                    optionTextBoxes[i].Text = iniReader.scan_cycles_amount.ToString();
                    optionTextBoxes[i].TextChanged += SetCyclesAmount;
                }    
                   

                else
                {
                    optionTextBoxes[i].Text = iniReader.scan_duration_seconds.ToString();
                    optionTextBoxes[i].TextChanged += SetScanDuration;
                }
                    

                Grid.SetColumn(optionTextBoxes[i], 1);
                Grid.SetRow(optionTextBoxes[i], i);
                griddy.Children.Add(optionTextBoxes[i]);              
            }

            if (iniReader.scanningInterval == "1")
            {
                optionLabels[1].IsEnabled = false;
                optionTextBoxes[1].IsEnabled = false;
            }
            else
            {
                optionLabels[0].IsEnabled = false;
                optionTextBoxes[0].IsEnabled = false;
            }

            for (int i = 0; i < 5; i++)
            {
                scanOptionsButtonsText.Add(iniReader.scanOpitonsButtonsText[i]);
                scanOptionsButtons_TextVisible.Add(iniReader.scanOpitonsButtonsTextVisible[i]);
                scanOptionsButtons_ImageVisible.Add(iniReader.scanOpitonsButtonsImageVisible[i]);
                

                CfgButton scanButton = new CfgButton(scanOptionsButtonsText[i], CfgButton.TYPE.Default, scanOptionsButtons_ImageVisible[i], scanOptionsButtons_TextVisible[i]);

                if(iniReader.scanningType == "1" && scanButton.btnTxt == "Reihenscanning")
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }
                else if(iniReader.scanningType == "2" && scanButton.btnTxt == "Spaltenscanning")
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }

                if (iniReader.scanningInterval == "1" && scanButton.btnTxt == "Ziklusinterval")
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }
                else if (iniReader.scanningInterval == "2" && scanButton.btnTxt == "Timeout-Interval")
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }

                scanButton.Clickable = CfgButton.CLICKABLE.Yes;
                scanButton.Visibility = Visibility.Visible;
                scanButton.MouseLeftButtonDown += (sender, e) => SetScanOptionClick(sender, e, scanButton.btnTxt, optionLabels[0], optionLabels[1], optionTextBoxes[0], optionTextBoxes[1]);

                if(scanButton.btnTxt == "Zurück")
                {
                    myCCCplace.Children.Add(scanButton);
                }
                else
                    wrapPanel.Children.Add(scanButton);
            }
        }

        void SetScanOptionClick(object sender, EventArgs e, string commandParameter, Label label1, Label label2, TextBox textBox1, TextBox textBox2)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);


            if (commandParameter == "Ziklusinterval")
            {
                label1.IsEnabled = true;
                textBox1.IsEnabled = true;

                label2.IsEnabled = false;
                textBox2.IsEnabled = false;

            }
            else if (commandParameter == "Timeout-Interval")
            {
                label1.IsEnabled = false;
                textBox1.IsEnabled = false;

                label2.IsEnabled = true;
                textBox2.IsEnabled = true;
            }
        }

        public void TextBoxInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        public static void SetCyclesAmount(object sender, RoutedEventArgs e)
        {
            int cyclesAmount = 0;

            if (String.IsNullOrEmpty(optionTextBoxes[0].Text))
            {
                cyclesAmount = 5;
            }
            else if(int.Parse(optionTextBoxes[0].Text) <= 0)
            {
                cyclesAmount = 5;
                optionTextBoxes[0].Text = cyclesAmount.ToString();               
            }
            else
                cyclesAmount = int.Parse(optionTextBoxes[0].Text);

            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "scan_cycles_amount", cyclesAmount.ToString());
        }

        public static void SetScanDuration(object sender, RoutedEventArgs e)
        {
            int scanDuration = 0;

            if (String.IsNullOrEmpty(optionTextBoxes[1].Text))
            {
                scanDuration = 8;
            }
            else if (int.Parse(optionTextBoxes[1].Text) <= 0)
            {
                scanDuration = 8;
                optionTextBoxes[1].Text = scanDuration.ToString();
            }
            else
                scanDuration = int.Parse(optionTextBoxes[1].Text);

            IniCreator iniCreator = new IniCreator();
            iniCreator.SaveValue("TMtextng_config_user.ini", "User", "scan_cycles_amount", scanDuration.ToString());
        }
    }
}
