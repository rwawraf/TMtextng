using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TMnote;

namespace TMtextng.Buttons
{
    /// <summary>
    /// Interaction logic for ImageButton.xaml
    /// </summary>
    public partial class ImageButton : UserControl, INotifyPropertyChanged
    {
        private int _btnWidth;

        public int BtnWidth
        {
            get { return _btnWidth; }
            set
            {
                _btnWidth = value;
                OnPropertyChanged("BtnWidth");
            }
        }


        private string _btnText;

        public string BtnText
        {
            get { return _btnText; }
            set
            {
                _btnText = value;
                OnPropertyChanged("BtnText");
            }
        }

        private Brush _btnBackground;
        public Brush BtnBackground
        {
            get { return _btnBackground; }
            set
            {
                _btnBackground = value;
                OnPropertyChanged("BtnBackground");
            }

        }

        private ImageSource _btnImage;
        public ImageSource BtnImage
        {
            get { return _btnImage; }
            set
            {
                _btnImage = value;
                OnPropertyChanged("BtnImage");
            }
        }

        public ImageButton(string text, string directory)
        {
            InitializeComponent();

            DataContext = this;
            InitializeComponent();

            SetNonConfigDate();
            SetButtonImage(directory, text);
            ActivateBtn();

            BtnText = text;
        }

        private void DeactivateBtn()
        {


            this.MouseEnter -= new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter);
            this.MouseLeave -= new System.Windows.Input.MouseEventHandler(this.UserControl_MouseLeave);
            this.MouseDown -= new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            this.MouseUp -= new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseUp);

            this.MouseEnter -= this.UserControl_MouseEnter;
            this.MouseLeave -= this.UserControl_MouseLeave;
            this.MouseDown -= this.UserControl_MouseDown;
            this.MouseUp -= this.UserControl_MouseUp;
        }

        private void ActivateBtn()
        {

            this.MouseEnter += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter);
            this.MouseLeave += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseLeave);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            this.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseUp);
        }

        private void SetButtonImage(string directoryName, string imageName)
        {
            string imagePath = @"C:\projekte\TMtextng_dir\TMtextng\TMsymlib_German\" + directoryName + "/" + imageName + ".png";

            BtnImage = ImageHelper.LoadBitmapImage(imagePath);
        }


        private void SetNonConfigDate()
        {
            BtnBackground = Brushes.LightGray;
            //BtnWidth = btnWidth;
        }

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            ButtonTextBlock.FontWeight = FontWeights.Bold;
            MouseEnterRectangle.Visibility = Visibility.Visible;
            MouseLeaveRectangle.Visibility = Visibility.Hidden;

        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {
            ButtonTextBlock.FontWeight = FontWeights.Normal;
            MouseLeaveRectangle.Visibility = Visibility.Visible;
            MouseEnterRectangle.Visibility = Visibility.Hidden;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ButtonTextBlock.FontWeight = FontWeights.Bold;

        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ButtonTextBlock.FontWeight = FontWeights.Normal;

        }



        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
