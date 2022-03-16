using System;
using IniParser;
using IniParser.Model;
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

namespace TMtextng.Redewendung
{
    /// <summary>
    /// Interaction logic for GroupViewWindow.xaml
    /// </summary>
    public partial class GroupViewWindow : Window
    {
        public static GroupViewWindow globalGroupViweWindow;
        ImageSource groupButtonImage;
        IniReader iniReader = new IniReader();

        public GroupViewWindow()
        {
            InitializeComponent();

            MainWindowViewModel buttonMVVM = new MainWindowViewModel();
            this.WindowStyle = (WindowStyle)iniReader.windowStyle;
            this.Background = new SolidColorBrush(Color.FromRgb(130, 220, 240));
            globalGroupViweWindow = this;

            List<string> paths = new List<string>();
            DirectoryInfo di = new DirectoryInfo(@"C:\Program Files\TMND-GMBH\TMpicto\Lang_German\");

            List<string> buttonsName = new List<string>();

            foreach (var fi in di.GetFiles())
            {
                if (fi.FullName != di + "60_Sport.png" && fi.FullName != di + "61_Orte.png")
                {
                    paths.Add(fi.FullName);
                    string groupName;
                    groupName = fi.FullName;
                    groupName = groupName.Remove(groupName.Length - 4);
                    groupName = groupName.Remove(0, di.FullName.Length);
                    // I need this, because a Button name cannot have a number and special character in it 
                    groupName = groupName.Replace("1", "aaa")
                                        .Replace("2", "bbb")
                                        .Replace("3", "ccc")
                                        .Replace("4", "ddd")
                                        .Replace("5", "eee")
                                        .Replace("6", "fff")
                                        .Replace("7", "ggg")
                                        .Replace("8", "hhh")
                                        .Replace("9", "iii")
                                        .Replace("0", "jjj")
                                        .Replace("_", "kkk")
                                        .Replace("-", "lll");
                    buttonsName.Add(groupName);
                }
            }

            for (int i = 0; i < 4; i++)
                GroupButtonsGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            for (int i = 0; i < 9; i++)
                GroupButtonsGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            Button[,] groupButton = new Button[4, 10];
            int count = 0;

            for (int i = 0; i < 4; i++)
            {
                for (int j = 0; j < 9; j++)
                {
                    if (count == paths.Count)
                    {
                        goto end;
                    }

                    groupButtonImage = ImageHelper.LoadBitmapImage(paths[count]);
                    groupButton[i, j] = new Button();
                    //groupButton[i, j].Margin = new Thickness(2);

                    Image img = new Image();
                    img.Stretch = Stretch.Fill;
                    img.Source = groupButtonImage;

                    WrapPanel wrapPnl = new WrapPanel();
                    wrapPnl.Margin = new Thickness(5);
                    wrapPnl.Children.Add(img);

                    groupButton[i, j].Name = buttonsName[count];
                    groupButton[i, j].Content = wrapPnl;

                    Grid.SetRow(groupButton[i, j], i);
                    Grid.SetColumn(groupButton[i, j], j);
                    groupButton[i, j].Click += new RoutedEventHandler(Button_Click);

                    GroupButtonsGrid.Children.Add(groupButton[i, j]);
                    count++;
                }
            }
        end:;

            CfgButton konfigButton = new CfgButton("Zurück", CfgButton.TYPE.Default, 1, 0);
            konfigButton.Clickable = CfgButton.CLICKABLE.Yes;
            konfigButton.Visibility = Visibility.Visible;
            BackButtonGrid.Children.Add(konfigButton);
            konfigButton.MouseLeftButtonDown += (sender, e) => buttonAction(sender, e, konfigButton.btnTxt);

            void buttonAction(object sender, EventArgs e, string commandParameter)
            {
                buttonMVVM.writeLetterCommand.Execute(commandParameter);
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
           RedewendungDataBinding.globalGroupName = button.Name;
            // Convert the Buttons name (without numbers and special characters) to the Group name
           RedewendungDataBinding.globalGroupName =RedewendungDataBinding.globalGroupName.Replace("aaa", "1")
                                        .Replace("bbb", "2")
                                        .Replace("ccc", "3")
                                        .Replace("ddd", "4")
                                        .Replace("eee", "5")
                                        .Replace("fff", "6")
                                        .Replace("ggg", "7")
                                        .Replace("hhh", "8")
                                        .Replace("iii", "9")
                                        .Replace("jjj", "0")
                                        .Replace("kkk", "_")
                                        .Replace("lll", "-");

            RedewendungKonfigWindowLowerABC window1 = new RedewendungKonfigWindowLowerABC();
            window1.Show();
            this.Visibility = Visibility.Hidden;
        }


    }
}

