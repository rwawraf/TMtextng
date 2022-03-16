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

namespace TMtextng.Konfig
{
    /// <summary>
    /// Interaktionslogik für MainKonfigWindow.xaml
    /// </summary>
    public partial class MenuKonfigWindow : Window
    {
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        IniReader iniReader = new IniReader();

        public MenuKonfigWindow()
        {
            InitializeComponent();

            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            List<string> konfigButtons_Text = new List<string>();
            List<int> konfigButtons_ImageVisible = new List<int>();
            List<int> konifgButtons_TextVisible = new List<int>();


            for (int i = 0; i < 5; i++)
            {
                konfigButtons_Text.Add(iniReader.menuKonfigButtonsText[i]);
                konfigButtons_ImageVisible.Add(iniReader.menuKonfigButtonsImageVisible[i]);
                konifgButtons_TextVisible.Add(iniReader.menuKonfigButtonsTextVisible[i]);

                CfgButton konfigButton = new CfgButton(konfigButtons_Text[i], CfgButton.TYPE.Default, konfigButtons_ImageVisible[i], konifgButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                if (konfigButtons_Text[i] != "Zurück")
                {
                    wrapPanel.Children.Add(konfigButton);
                }
                else
                {
                    myCCCplace.Children.Add(konfigButton);
                }
                konfigButton.MouseLeftButtonDown += (sender, e) => OpenWindow(sender, e, konfigButton.btnTxt);
            }
        }

        void OpenWindow(object sender, EventArgs e, string commandParameter)
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
