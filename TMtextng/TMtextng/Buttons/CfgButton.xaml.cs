using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
using TMtextng;
using TMtextng.Redewendung;

//using TMnote.OneClickScanning;

namespace TMnote
{
    /// <summary>
    /// Interaktionslogik für CfgButton.xaml
    /// </summary>
    public partial class CfgButton : UserControl, INotifyPropertyChanged
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        IniReader iniReader = new IniReader();

        #region Binding

        private string commandParameter; // field
        public string CommandParameter   // property
        {
            get { return commandParameter; }
            set { commandParameter = value; }
        }

        private String _btnTxt;
        public String btnTxt
        {
            get { return _btnTxt; }
            set
            {
                if (value != null)
                {
                    _btnTxt = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _btnFontSize = 20F;
        public double btnFontSize
        {
            get { return _btnFontSize; }
            set
            {
                _btnFontSize = value;
                OnPropertyChanged();
            }
        }

        private double _btnWidth = 20F;
        public double btnWidth
        {
            get { return _btnWidth; }
            set
            {
                _btnWidth = value;
                OnPropertyChanged();
            }
        }

        private double _btnHeight = 20F;
        public double btnHeight
        {
            get { return _btnHeight; }
            set
            {
                _btnHeight = value;
                OnPropertyChanged();

            }
        }

        private double _btnRadius = 20F;
        public double btnRadius
        {
            get { return _btnRadius; }
            set
            {
                _btnRadius = value;
                OnPropertyChanged();

            }
        }

        private ImageSource _btnImage;
        public ImageSource btnImage
        {
            get { return _btnImage; }
            set
            {
                if (value != null)
                {
                    _btnImage = value;
                    OnPropertyChanged("btnImage");
                }
            }
        }

        private Brush _btnBackground;
        public Brush btnBackground
        {
            get { return _btnBackground; }
            set
            {
                if (value != null)
                {
                    _btnBackground = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush _originalBackgroundBrush;
        public Brush originalBackgroundBrush
        {
            get { return _originalBackgroundBrush; }
            set
            {
                if (value != null)
                {
                    _originalBackgroundBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        private Brush _btnBorderBrush;
        public Brush btnBorderBrush
        {
            get { return _btnBorderBrush; }
            set
            {
                if (value != null)
                {
                    _btnBorderBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        private double _btnBorderSize = 20F;
        public double btnBorderSize
        {
            get { return _btnBorderSize; }
            set
            {
                _btnBorderSize = value;
                OnPropertyChanged();

            }
        }

        private Brush _btnTextBrush;
        public Brush btnTextBrush
        {
            get { return _btnTextBrush; }
            set
            {
                if (value != null)
                {
                    _btnTextBrush = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        public enum TYPE
        {
            NONE,
            Cancel, Default, Highlight, Illumination

        };

        public TYPE type;

        public enum CLICKABLE { Yes, No };

        private CLICKABLE _Clickable;
        public CLICKABLE Clickable
        {
            get { return _Clickable; }
            set
            {
                _Clickable = value;
                if (value == CLICKABLE.Yes)
                { ActivateBtn(); }
                else if (value == CLICKABLE.No)
                { DeactivateBtn(); }

            }
        }

        private void DeactivateBtn()
        {
            btnBackground = deactiveBackgroundBrush;
            btnBorderBrush = deactiveBorderBrush;

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
            btnBackground = originalBackgroundBrush;
            btnBorderBrush = originalBorderBrush;

            this.MouseEnter += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseEnter);
            this.MouseLeave += new System.Windows.Input.MouseEventHandler(this.UserControl_MouseLeave);
            this.MouseDown += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseDown);
            this.MouseUp += new System.Windows.Input.MouseButtonEventHandler(this.UserControl_MouseUp);
        }

        private Brush originalBorderBrush;
        private Brush mouseEnterBorderBrush;
        private Brush mouseClickBorderBrush;

        private Brush mouseClickBackgroundBrush;

        private Brush originalTextBrush;
        private Brush mouseClickedTextBrush;

        private Brush deactiveBackgroundBrush;
        private Brush deactiveBorderBrush;

        //public RoutedEventHandler Click;

        public CfgButton(string text, TYPE type, int imageVisibility, int textVisibility)
        {
            DataContext = this;
            InitializeComponent();
            this.btnTxt = text;
            this.type = type;


            InitFromConfig(text);

            SetNonConfigData();
            SetBtnImage(imageVisibility, textVisibility);
        }

        //TODO Probably include this data in config file - ask Rudi
        private void SetNonConfigData()
        {
            //byte r = Convert.ToByte(UserConf.userconfigDic["tmn_menu_config_btn_color_red"]);
            //byte g = Convert.ToByte(UserConf.userconfigDic["tmn_menu_config_btn_color_green"]);
            //byte b = Convert.ToByte(UserConf.userconfigDic["tmn_menu_config_btn_color_blue"]);
            //originalBackground = btnBackground = new SolidColorBrush(Color.FromRgb(100, 100, 100));
            btnBorderBrush = Brushes.Beige;
            btnBorderSize = 5;
            btnRadius = 8;
            mouseEnterBorderBrush = Brushes.White;
            mouseClickBorderBrush = Brushes.DarkOrange;
            originalBorderBrush = Brushes.LightGreen;
            mouseClickBackgroundBrush = Brushes.Black;

            originalTextBrush = btnTextBrush = Brushes.Black;
            mouseClickedTextBrush = Brushes.Yellow;

            deactiveBackgroundBrush = Brushes.Gray;
            deactiveBorderBrush = Brushes.DarkGray;

            this.Clickable = CLICKABLE.No;
        }

        //TODO Finish implementation
        private void SetBtnImage(int imageVisibility, int textVisibility)
        {
            IniReader iniReader = new IniReader();
            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";

            if (btnTxt == "Zurück") { type = TYPE.Cancel; }

            if (textVisibility == 0)
            {
                buttonTextBlock.Visibility = Visibility.Hidden;
            }

            if (imageVisibility == 1)
            {
                if (type == TYPE.Cancel)
                {
                    string path = imageDataPath + ImageHelper.ICON.GLOBAL.CANCEL;
                    btnImage = ImageHelper.LoadBitmapImage(path);
                    buttonTextBlock.Visibility = Visibility.Hidden;
                }
                else if (type == TYPE.Default)
                {
                    string path = imageDataPath + ImageHelper.ICON.PATTERN.GREEN;
                    btnImage = ImageHelper.LoadBitmapImage(path);
                }
                else if (type == TYPE.Highlight)
                {
                    string path = imageDataPath + ImageHelper.ICON.PATTERN.YELLOW;
                    btnImage = ImageHelper.LoadBitmapImage(path);
                }
                else if (type == TYPE.Illumination)
                {
                    string path = imageDataPath + ImageHelper.ICON.PATTERN.BLUE;
                    btnImage = ImageHelper.LoadBitmapImage(path);
                }
            }
        }

        private void InitFromConfig(string text)
        {
            btnFontSize = 40;
            btnWidth = 20;
            btnHeight = 20;
            byte r = Convert.ToByte(130);
            byte g = Convert.ToByte(220);
            byte b = Convert.ToByte(240);
            //originalBackgroundBrush = btnBackground = new SolidColorBrush(Color.FromRgb(r, g, b));
            //originalBackgroundBrush = Brushes.ForestGreen;
            if (text == "Zurück")
            {
                SetBackButtonBackground();
            }
            else
            {
                SetOrginalBackgroundBrush();
            }

        }

        #region Properties Methods

        public event PropertyChangedEventHandler PropertyChanged;
        /// <summary>
        /// Event for manage the DataBindings to XAML
        /// </summary>
        /// <param name="propertyName"></param>
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion

        private void UserControl_MouseEnter(object sender, MouseEventArgs e)
        {
            //IniReader iniReader = new IniReader();
            //string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";
            //btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.BLUE_DARK);
            //btnBorderBrush = mouseEnterBorderBrush;
            //btnBackground = Brushes.Black;
            //btnTextBrush = Brushes.Black;

            btnBorderBrush = mouseEnterBorderBrush;
            btnBackground = Brushes.White;
            btnTextBrush = Brushes.White;
        }

        private void UserControl_MouseLeave(object sender, MouseEventArgs e)
        {//TODO HERE COULD BE A PROBLEM IN THE FUTURE - THIS GETS FOCUSEDE IN WEIRD SITUATIONS
            //IniReader iniReader = new IniReader();
            //string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";
            //btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.GREEN);
            btnBorderBrush = originalBorderBrush;
            btnBackground = originalBackgroundBrush;
            btnTextBrush = originalTextBrush;
        }

        private void UserControl_MouseDown(object sender, MouseButtonEventArgs e)
        {//TODO HERE COULD BE A PROBLEM IN THE FUTURE - THIS GETS FOCUSEDE IN WEIRD SITUATIONS
            btnBorderBrush = mouseClickBorderBrush;
            btnBackground = mouseClickBackgroundBrush;
            btnTextBrush = mouseClickedTextBrush;
        }

        private void UserControl_MouseUp(object sender, MouseButtonEventArgs e)
        {
            btnBorderBrush = mouseEnterBorderBrush;
            btnBackground = originalBackgroundBrush;
            btnTextBrush = originalTextBrush;
            //SetCursorPos(0, 0);
        }

        void SetOrginalBackgroundBrush()
        {
            byte r = Convert.ToByte(iniReader.ButtonColor[3]);
            byte g = Convert.ToByte(iniReader.ButtonColor[4]);
            byte b = Convert.ToByte(iniReader.ButtonColor[5]);
            Brush buttoncolor = new SolidColorBrush(Color.FromRgb(r, g, b));
            originalBackgroundBrush = buttoncolor;
        }
        void SetBackButtonBackground()
        {
            byte r = Convert.ToByte(iniReader.ButtonColor[0]);
            byte g = Convert.ToByte(iniReader.ButtonColor[1]);
            byte b = Convert.ToByte(iniReader.ButtonColor[2]);
            Brush buttoncolor = new SolidColorBrush(Color.FromRgb(r, g, b));
            btnBackground = originalBackgroundBrush = buttoncolor;
            //buttonName.btnBackground = buttonName.originalBackgroundBrush = buttoncolor;
        }
       
    }
}
