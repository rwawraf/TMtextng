using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for TextSymbolKeyboard.xaml
    /// </summary>
    public partial class TextSymbolKeyboard : Window
    {
        public static TextSymbolKeyboard globalTextSymbolKeyboardWindow;
        public static string globalTextSymbolKeyboardText;
        //public static ImageSource globalRedewendungImageSource;
        public RedewendungDataBinding myImageText { get; set; }


        IniReader iniReader = new IniReader();
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        int buttonCount = 0;

        public TextSymbolKeyboard()
        {
            InitializeComponent();

            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            globalTextSymbolKeyboardWindow = this;
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;

            List<string> textSymbolKonfigButtons_Text = new List<string>();
            List<int> textSymbolKonfigButtons_ImageVisible = new List<int>();
            List<int> textSymbolKonfigButtons_TextVisible = new List<int>();

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
                    abcButton[i, j].Content = iniReader.redewendungabcButtonLetter[buttonCount]; ;
                    abcButton[i, j].FontSize = 40;
                    abcButton[i, j].CommandParameter = iniReader.redewendungabcButtonLetter[buttonCount]; ;

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

            string[] konfigButtons_Text = { "Zurück", "Das Bild speichern", "Vorschau" };

            for (int i = 0; i < 3; i++)
            {
                textSymbolKonfigButtons_Text.Add(iniReader.textSymbolKeyboardButtonsText[i]);
                textSymbolKonfigButtons_TextVisible.Add(iniReader.textSymbolKeyboardButtonsTextVisible[i]);
                textSymbolKonfigButtons_ImageVisible.Add(iniReader.textSymbolKeyboardButtonsImageVisible[i]);


                CfgButton konfigButton = new CfgButton(textSymbolKonfigButtons_Text[i], CfgButton.TYPE.Default, textSymbolKonfigButtons_ImageVisible[i], textSymbolKonfigButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;

                if (textSymbolKonfigButtons_Text[i] == "Vorschau")
                {
                    RefreshImage.Children.Add(konfigButton);
                    konfigButton.MouseLeftButtonDown += new MouseButtonEventHandler(RefreshRedewendungImage_Click);

                }
                else if (textSymbolKonfigButtons_Text[i] == "Das Bild speichern")
                {
                    wrapPanel.Children.Add(konfigButton);
                    konfigButton.MouseLeftButtonDown += new MouseButtonEventHandler(SaveRedewendungImage_Click);

                }
                else
                {
                    wrapPanel.Children.Add(konfigButton);
                    konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);
                }
            }
        }

        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }

        private void RefreshRedewendungImage_Click(object sender, RoutedEventArgs e)
        {
            CfgButton myButton = sender as CfgButton;

            myImageText = new RedewendungDataBinding { ImageText = ImageDataTextBox.Text };
            this.DataContext = myImageText;

        }

        private void SaveRedewendungImage_Click(object sender, RoutedEventArgs e)
        {

            CfgButton myButton = sender as CfgButton;
            RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC.Close();
            RedewendungDataBinding.globalRedewendungImageSource = GetImageWithText();
            RedewendungDataBinding.globalImageSet = 1;
            RedewendungKonfigWindowLowerABC newWindow = new RedewendungKonfigWindowLowerABC();
            newWindow.Show();
            ImageChoice.globalImageChoiceWindow.Close();
            this.Close();

        }
        public BitmapFrame GetImageWithText()
        {
            //On the "TextSymbolKeyboard.xaml" class I created a "ResizeImageGrid" grid. This grid is unvisible on the Window. I need this only to resize Image and get it with a perfect quality

            //Draw Image with Text
            Size size = ResizeImageGrid.DesiredSize;
            if (size.IsEmpty)
                return null;

            RenderTargetBitmap result = new RenderTargetBitmap((int)size.Width, (int)size.Height, 96, 96, PixelFormats.Pbgra32);
            DrawingVisual drawingvisual = new DrawingVisual();
            using (DrawingContext context = drawingvisual.RenderOpen())
            {
                context.DrawRectangle(new VisualBrush(ResizeImageGrid), null, new Rect(new Point(), size));
                context.Close();
            }

            result.Render(drawingvisual);


            return BitmapFrame.Create(result);
        }

    }
}

