using IniParser;
using IniParser.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Threading;

namespace TMtextng
{
    /// <summary>
    /// Interaktionslogik für "App.xaml"
    /// </summary>
    /// 

    [ValueConversion(typeof(string), typeof(string))]
    public class RatioConverter : MarkupExtension, IValueConverter
    {
        private static RatioConverter _instance;

        public RatioConverter() { }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        { 
            double size = System.Convert.ToDouble(value) * System.Convert.ToDouble(parameter, CultureInfo.InvariantCulture);
            return size.ToString("G0", CultureInfo.InvariantCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        { // read only converter...
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return _instance ?? (_instance = new RatioConverter());
        }

    }

    public partial class App : Application
    {
        PathChecker pathCheck = new PathChecker();
        private static Mutex _mutex = null;
        
        protected override void OnStartup(StartupEventArgs e)
        {          
            pathCheck.CreateHardLinkToIniFile();

            if(!File.Exists("TMtextng_config.ini"))
            {
                MessageBox.Show("TMtextng_config.ini missing!");
                Environment.Exit(0);
            }

            if(!File.Exists("TMtextng_config_ESP.ini"))
            {
                MessageBox.Show("TMtextng_config_ESP.ini missing!");
                Environment.Exit(0);
            }

            if (!File.Exists("TMtextng_config_user.ini"))
            {
                IniCreator iniCreator = new IniCreator();
                iniCreator.CreateConfigUserIni();
            }     
            WordSuggestion.CreateAbcUser();
            WordSuggestion.ClearSuggestionWords();
            WordSuggestion.UpdateUserAbcOnStartup();

            IniReader iniReader = new IniReader();
            if (iniReader.svox_voice_active == 1)
            {
                SpeakVoice.Init_SVOX();
            }

            const string appName = "TMtextneu";
            bool createdNew;

            _mutex = new Mutex(true, appName, out createdNew);

            if (!createdNew)
            {
                //app is already running! Exiting the application
                Application.Current.Shutdown();
            }

            base.OnStartup(e);
        }
    }
}
