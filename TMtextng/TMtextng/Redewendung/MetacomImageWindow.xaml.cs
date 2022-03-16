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
using System.IO;
using TMnote;
using TMtextng.Buttons;
using System.ComponentModel;

namespace TMtextng.Redewendung
{
    /// <summary>
    /// Interaction logic for MetacomImageWindow.xaml
    /// </summary>
    public partial class MetacomImageWindow : Window
    {
        public static MetacomImageWindow globalMetacomImageWindow;


        IniReader iniReader = new IniReader();

        string directoryPath = @"C:\projekte\TMtextng_dir\TMtextng\TMsymlib_German\";
        string directoryName;

        List<string> fileNamesList = new List<string>();
        List<string> imageNamesList = new List<string>();


        MainWindowViewModel buttonMVVM = new MainWindowViewModel();
  

        public MetacomImageWindow()
        {
            InitializeComponent();

            globalMetacomImageWindow = this;
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));

            List<string> metacomImageButtons_Text = new List<string>();
            List<int> metacomImageButtons_ImageVisible = new List<int>();
            List<int> metacomImageButtons_TextVisible = new List<int>();


            LoadFilesNames(fileNamesList);

            foreach (string fileName in fileNamesList)
            {
                DirectoryInfoButton directoryButton = new DirectoryInfoButton(fileName, (int)directoryListSckPanel.Width);
                directoryButton.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(DirectoryInfoButton_Click), true);
                directoryListSckPanel.Children.Add(directoryButton);
            }         


            for (int i = 0; i < 2; i++)
            {
                metacomImageButtons_Text.Add(iniReader.metacomImageButtonsText[i]);
                metacomImageButtons_ImageVisible.Add(iniReader.metacomImageButtonsImageVisible[i]);
                metacomImageButtons_TextVisible.Add(iniReader.metacomImageButtonsTextVisible[i]);


                CfgButton cfgButton = new CfgButton(metacomImageButtons_Text[i], CfgButton.TYPE.Default, metacomImageButtons_ImageVisible[i], metacomImageButtons_TextVisible[i]);
                cfgButton.Clickable = CfgButton.CLICKABLE.Yes;
                cfgButton.Visibility = Visibility.Visible;
                CfgButtonsGrid.Children.Add(cfgButton);
                cfgButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, cfgButton.btnTxt);
            }

        }

        public void LoadFilesNames(List<string> loadedFilesList)
        {

            string[] filePaths = Directory.GetDirectories(directoryPath);
            string fileName;

            int directoryPathLength = directoryPath.Length;

            foreach (string fN in filePaths)
            {
                fileName = fN.Remove(0, directoryPathLength);
                loadedFilesList.Add(fileName);
            }

        }

        public void LoadMetacomImageButtons(List<string> loadedImageList, string directoryName)
        {
            DirectoryInfo di = new DirectoryInfo(directoryPath + directoryName + "/");

            foreach (var fi in di.GetFiles())
            {
                string groupName;
                groupName = fi.FullName;
                groupName = groupName.Remove(groupName.Length - 4);
                groupName = groupName.Remove(0, di.FullName.Length);
                loadedImageList.Add(groupName);
            }
        }

        void buttonAction(object sender, EventArgs e, string commandParameter)
        {
            buttonMVVM.writeLetterCommand.Execute(commandParameter);
        }

        private void DirectoryInfoButton_Click(object sender, RoutedEventArgs e)
        {
            var Button = sender as DirectoryInfoButton;
            directoryName = Button.BtnText;

            LoadMetacomImageButtons(imageNamesList, directoryName);

            foreach (string imageName in imageNamesList)
            {
                ImageButton imageButton = new ImageButton(imageName, directoryName);
                imageButton.AddHandler(FrameworkElement.MouseLeftButtonDownEvent, new MouseButtonEventHandler(ImageButton_Click), true);
                ImageListWrapPanel.Children.Add(imageButton);
            }
            ImageListScrollViewer.Visibility = Visibility.Visible;
            DirectoryListScrollViewer.Visibility = Visibility.Hidden;
        }


        private void ImageButton_Click(object sender, RoutedEventArgs e)
        {
            Window closedWindow = RedewendungKonfigWindowLowerABC.globalRedewendungKonfigWindowLowerABC;
            var Button = sender as ImageButton;
            string imageName = directoryPath + directoryName + "/" + Button.BtnText + ".png";
            RedewendungDataBinding.globalRedewendungImageSource = ImageHelper.CreateBitmapFrameFromString(imageName);
            RedewendungDataBinding.globalImageSet = 1;
            RedewendungKonfigWindowLowerABC newWindow = new RedewendungKonfigWindowLowerABC();
            newWindow.Show();
            closedWindow.Close();
            ImageChoice.globalImageChoiceWindow.Close();
            this.Close();
        }
    }
}
