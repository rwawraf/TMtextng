using System;
using System.Collections.Generic;
using System.IO;
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

namespace TMtextng.Redewendung
{
    /// <summary>
    /// Interaction logic for RedewendungKonfigWindowUpperABC.xaml
    /// </summary>
    public partial class RedewendungKonfigWindowUpperABC : Window
    {
        public static RedewendungKonfigWindowUpperABC globalRedewendungKonfigWindowUpperABC;

        IniReader iniReader = new IniReader();
        int buttonCount = 0;
        public RedewendungDataBinding myNewText { get; set; }
        public RedewendungDataBinding myImageSource { get; set; }
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        public RedewendungKonfigWindowUpperABC()
        {
            InitializeComponent();

            globalRedewendungKonfigWindowUpperABC = this;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;

            List<string> redewendungKonfigButtons_Text = new List<string>();
            List<int> redewendungKonfigButtons_ImageVisible = new List<int>();
            List<int> redewendungKonfigButtons_TextVisible = new List<int>();


            Button[,] abcButton = new Button[4, 11];

            for (int i = 0; i < 4; i++)
                LettersGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < 10; i++)
                LettersGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    abcButton[i, j] = new Button();
                    abcButton[i, j].Content = iniReader.redewendungABCButtonLetter[buttonCount];
                    abcButton[i, j].FontSize = 40;
                    abcButton[i, j].CommandParameter = iniReader.redewendungABCButtonLetter[buttonCount];

                    Binding myBinding = new Binding("writeLetterCommand");
                    myBinding.Source = buttonMVVM;
                    abcButton[i, j].SetBinding(Button.CommandProperty, myBinding);

                    Grid.SetRow(abcButton[i, j], i);
                    Grid.SetColumn(abcButton[i, j], j);

                    if (i == 3 && j > 3 && j < 5)
                    {
                        buttonCount++;
                        continue;
                    }

                    LettersGrid.Children.Add(abcButton[i, j]);
                    buttonCount++;
                }
            }

            Grid.SetColumnSpan(abcButton[3, 3], 2);



            for (int i = 0; i < 5; i++)
            {
                redewendungKonfigButtons_Text.Add(iniReader.redewendungKonfigButtonsText[i]);
                redewendungKonfigButtons_ImageVisible.Add(iniReader.redewendungKonfigButtonsImageVisible[i]);
                redewendungKonfigButtons_TextVisible.Add(iniReader.redewendungKonfigButtonsTextVisible[i]);

                CfgButton konfigButton = new CfgButton(redewendungKonfigButtons_Text[i], CfgButton.TYPE.Default, redewendungKonfigButtons_ImageVisible[i], redewendungKonfigButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);

                if (redewendungKonfigButtons_Text[i] == "Bild laden")
                {
                    ImageStackPnl.Children.Add(konfigButton);
                    if (RedewendungDataBinding.globalImageSet == 1)
                    {
                        myImageSource = new RedewendungDataBinding { RedewendungImage = RedewendungDataBinding.globalRedewendungImageSource };

                        konfigButton.btnImage = RedewendungDataBinding.globalRedewendungImageSource;
                        konfigButton.btnBackground = Brushes.White;
                        konfigButton.buttonTextBlock.Visibility = Visibility.Hidden;
                    }
                }

                else
                {
                    wrapPanel.Children.Add(konfigButton);
                }
            }

            DataNameTextBox.Text = RedewendungDataBinding.globaldataName;
            
            myNewText = new RedewendungDataBinding { RedewendungText = RedewendungDataBinding.globalRedewendugText };
            this.DataContext = myNewText;

            SetTextBoxAppearance(DataNameTextBox, RedewendungTextTextBox);
            RedewendungTextTextBox.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RedewendungTextTextBox_MouseLeftButtonDown), true);
            DataNameTextBox.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DataNameTextBox_MouseLeftButtonDown), true);
        }

        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }

        private void DataNameTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedewendungDataBinding.globalRedewendungFocus = 0;

            if (RedewendungDataBinding.globalRedewendungFocus == 0)
            {
                SetTextBoxAppearance(DataNameTextBox, RedewendungTextTextBox);
            }
        }

        private void RedewendungTextTextBox_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            RedewendungDataBinding.globalRedewendungFocus = 1;

            if (RedewendungDataBinding.globalRedewendungFocus == 1)
            {
                SetTextBoxAppearance(RedewendungTextTextBox, DataNameTextBox);
            }
        }

        private void SetTextBoxAppearance(TextBox textBox1, TextBox textBox2)
        {
            textBox2.Background = Brushes.White;
            textBox2.BorderThickness = new Thickness(1);

            textBox1.BorderThickness = new Thickness(3);
            textBox1.BorderBrush = SystemColors.MenuHighlightBrush;
            textBox1.Background = Brushes.LightBlue;
        }

    }
}
