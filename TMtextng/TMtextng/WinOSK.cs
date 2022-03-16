using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using TMtextng.KeyboardPages;

namespace TMtextng
{
    class WinOSK
    {
        public void LaunchWindowsOnScreenKeyboard()
        {
            string osk_path = "";

            try
            {
                string[] directiories = System.IO.Directory.GetDirectories(@"C:\Windows\WinSxS", "amd64_microsoft-windows-osk_31bf3856ad364e35_*", System.IO.SearchOption.TopDirectoryOnly);
                var osk_path64 = "";
                var osk_path32 = @"C:\windows\system32\osk.exe";

                if (Environment.Is64BitOperatingSystem)
                {
                    if (directiories.Length > 0)
                    {
                        osk_path64 = directiories[0];
                        osk_path = osk_path64 + @"\osk.exe";
                        Process.Start(osk_path);
                    }
                    else
                        MessageBox.Show("Windows on-screen keyboard missing");
                }
                else
                {
                    if (File.Exists(osk_path32))
                    {
                        osk_path = osk_path32;
                        Process.Start(osk_path);
                    }
                    else
                        MessageBox.Show("Windows on-screen keyboard missing");
                }

                RegistryKey myKey = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Osk", true);


                double onScreenKeyboardHeight = LowerABC.globalLowerABC.LettersGrid.ActualHeight + LowerABC.globalLowerABC.WordSuggestionGrid.ActualHeight;

                double KeyboardTopPosition = ((MainWindow)Application.Current.MainWindow).Top + ((MainWindow)Application.Current.MainWindow).ActualHeight - onScreenKeyboardHeight;

                myKey.SetValue("WindowLeft", ((MainWindow)Application.Current.MainWindow).Left, RegistryValueKind.DWord);
                myKey.SetValue("WindowTop", KeyboardTopPosition, RegistryValueKind.DWord);
                myKey.SetValue("WindowHeight", onScreenKeyboardHeight, RegistryValueKind.DWord);
                myKey.SetValue("WindowWidth", ((MainWindow)Application.Current.MainWindow).Width, RegistryValueKind.DWord);


                if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
                {
                    LowerABC.globalLowerABC.TextBoxContent.Focus();
                }

                else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
                {
                    LowerQWERTZ.globalLowerQWERTZ.TextBoxContent.Focus();
                }

                else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
                {
                    UpperABC.globalUpperABC.TextBoxContent.Focus();
                }

                else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
                {
                    UpperQWERTZ.globalUpperQWERTZ.TextBoxContent.Focus();
                }
            }

            catch (Exception ex)
            {
                MessageBox.Show("Fehlermeldung: " + ex.ToString());
            }
        }
    }
}
