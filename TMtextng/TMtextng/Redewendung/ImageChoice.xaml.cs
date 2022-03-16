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

namespace TMtextng.Redewendung
{
    /// <summary>
    /// Interaction logic for ImageChoice.xaml
    /// </summary>
    public partial class ImageChoice : Window
    {
        public static ImageChoice globalImageChoiceWindow;
        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
        IniReader iniReader = new IniReader();
        public ImageChoice()
        {

            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            globalImageChoiceWindow = this;

            InitializeComponent();

            List<string> imageButtons_Text = new List<string>();
            List<int> redewendungKonfigButtons_ImageVisible = new List<int>();
            List<int> redewendungKonfigButtons_TextVisible = new List<int>();

            //string[] imageKonfigButtons_Text = { "Text-Symbol", "Bilde Ordner", "Bild aus Datei", "Zurück" }; // change to ini

            for (int i = 0; i < 4; i++)
            {
                imageButtons_Text.Add(iniReader.imageChoiceButtonsText[i]);
                redewendungKonfigButtons_ImageVisible.Add(iniReader.imageChoiceButtonsImageVisible[i]);
                redewendungKonfigButtons_TextVisible.Add(iniReader.imageChoiceButtonsTextVisible[i]);

                CfgButton konfigButton = new CfgButton(imageButtons_Text[i], CfgButton.TYPE.Default, redewendungKonfigButtons_ImageVisible[i], redewendungKonfigButtons_TextVisible[i]);
                konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
                konfigButton.Visibility = Visibility.Visible;
                konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);
                wrapPanel.Children.Add(konfigButton);
            }

        }
        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }
    }
}

