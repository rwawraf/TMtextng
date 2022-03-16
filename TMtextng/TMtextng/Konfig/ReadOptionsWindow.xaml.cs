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
using System.ComponentModel;
using TMnote;

namespace TMtextng.Konfig
{
    /// <summary>
    /// Interaktionslogik für ReadOptionsWindow.xaml
    /// </summary>
    public partial class ReadOptionsWindow : Window
    {

        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        IniReader iniReader = new IniReader();

        public ReadOptionsWindow()
        {
            InitializeComponent();

            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            List<string> readOptionsButtonsText = new List<string>();
            List<int> readOptionsButtons_TextVisible = new List<int>();
            List<int> readOptionsButtons_ImageVisible = new List<int>();


            for (int i = 0; i < 4; i++)
            {
                readOptionsButtonsText.Add(iniReader.readOptionsButtonsText[i]);
                readOptionsButtons_TextVisible.Add(iniReader.readOptionsButtonsTextVisible[i]);
                readOptionsButtons_ImageVisible.Add(iniReader.readOptionsButtonsImageVisible[i]);

                CfgButton konfigButton = new CfgButton(readOptionsButtonsText[i], CfgButton.TYPE.Default, readOptionsButtons_ImageVisible[i], readOptionsButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;

                if (readOptionsButtonsText[i] != "Zurück")
                {
                    wrapPanel.Children.Add(konfigButton);
                }
                else
                {
                    myCCCplace.Children.Add(konfigButton);
                }

                konfigButton.MouseLeftButtonDown += (sender, e) => open_Window(sender, e, konfigButton.btnTxt);

                if ((readOptionsButtonsText[i] == "Satzweise"  && iniReader.textReadMode == "1") ||
                   (readOptionsButtonsText[i] == "Wortweise"  && iniReader.textReadMode == "2") ||
                   (readOptionsButtonsText[i] == "Blockweise" && iniReader.textReadMode == "3"))
                {
                    konfigButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }
            }
        }

        void open_Window(object sender, EventArgs e, string commandParameter)
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
            double heightFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinHeightDiff) * this.ReadOptionsGrid.ActualHeight + maxMinFontSizeDiff / 2;
            double widthFontSizeDouble = (maxMinFontSizeDiff / gridMaxMinWidthDiff) * this.ReadOptionsGrid.ActualWidth + maxMinFontSizeDiff / 2;

            foreach (var children in this.ReadOptionsGrid.Children)
            {
                if (children.GetType() == typeof(Button))
                    (children as Button).FontSize = Math.Min(heightFontSizeDouble, widthFontSizeDouble);
            }
        }
    }
}
