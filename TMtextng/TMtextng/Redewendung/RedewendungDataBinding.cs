using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace TMtextng.Redewendung
{
    public class RedewendungDataBinding : INotifyPropertyChanged
    { 

        public static BitmapFrame globalRedewendungImageSource;
        public static string globalGroupName;
        public static string globaldataName;
        public static string globalRedewendugText;
        public static int globalRedewendungFocus = 0;
        public static int globalImageSet = 0;

        private string _myText;

        public string RedewendungText
        {
            get { return _myText; }
            set
            {
                if (value != _myText)
                {
                    _myText = value;
                    OnPropertyChanged("RedewendungText");
                }
            }
        }

        private ImageSource _SetImageSource;

        public ImageSource RedewendungImage
        {
            get { return _SetImageSource; }
            set
            {
                if (value != _SetImageSource)
                {
                    _SetImageSource = value;
                    OnPropertyChanged("RedewendungImage");
                }
            }
        }

        private string _myImageText;

        public string ImageText
        {
            get { return _myImageText; }
            set
            {
                if (value != _myImageText)
                {
                    _myImageText = value;
                    OnPropertyChanged("ImageText");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }
    }
}
