using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using TMnote;
using TMtextng.KeyboardPages;
using TMtextng.Konfig;

namespace TMtextng
{
    public class ScanningOptions
    {
        public static ScanningOptions globalScanningOptions;
        CancellationTokenSource cts;
        int cycleScanCount = 0;
 
        public ScanningOptions()
        {
            globalScanningOptions = this;
        }

        public async Task RowScanning(CancellationToken ct, Grid grid)
        {
            cycleScanCount = 0;
            IniReader iniReader = new IniReader();

            if (!Properties.Settings.Default.isScanOn)
            {

                Properties.Settings.Default.isScanOn = true;
                Properties.Settings.Default.globalMouseClickCount = 1;
                Properties.Settings.Default.isScanPaused = false;              

                do
                {

                    for (int i = Properties.Settings.Default.lastActiveRow; i < 4; i++)
                    {

                        if (iniReader.scanningInterval == "1")
                        {
                            if (!Properties.Settings.Default.CycleScanPaused)
                                cycleScanCount++;

                            if (cycleScanCount > iniReader.scan_cycles_amount)
                            {
                                Properties.Settings.Default.CycleScanPaused = true;
                                Properties.Settings.Default.isScanOn = false;
                                Properties.Settings.Default.isScanPaused = true;


                                if (cts != null)
                                {
                                    Properties.Settings.Default.globalMouseClickCount = 0;
                                    cts.Cancel();
                                }

                            }
                        }

                        foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetRow(j) == i))
                        {
                            if (child.GetType() == typeof(Button))
                            {
                                Button button = (Button)child;
                                button.Background = Brushes.Red;

                                Properties.Settings.Default.lastActiveRow = i;
                            }
                        }
                        await Task.Delay(TimeSpan.FromSeconds(0.5), ct);

                        foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetRow(j) == i))
                        {
                            if (child.GetType() == typeof(Button))
                            {
                                Button button = (Button)child;
                                button.Background = Brushes.DarkGray;
                            }
                        }

                        if (Properties.Settings.Default.CycleScanPaused)
                            Properties.Settings.Default.CycleScanPaused = false;

                        if (Properties.Settings.Default.globalMouseClickCount > 1)
                        {
                            do
                            {
                                foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetRow(j) == i))
                                {
                                    if (child.GetType() == typeof(Button))
                                    {
                                        Button button = (Button)child;


                                        button.Background = Brushes.Red;
                                        await Task.Delay(TimeSpan.FromSeconds(0.5), ct);
                                        button.Background = Brushes.DarkGray;
                                    }

                                    if (Properties.Settings.Default.globalMouseClickCount > 2)
                                    {
                                        Properties.Settings.Default.isScanOn = false;
                                        break;
                                    }
                                }

                            } while (Properties.Settings.Default.globalMouseClickCount < 3);

                            break;
                        }

                    }
                    Properties.Settings.Default.lastActiveRow = 0;
                } while (Properties.Settings.Default.globalMouseClickCount < 2);
            }
        }

        public async Task ColumnScanning(CancellationToken ct, Grid grid)
        {
            cycleScanCount = 0;
            IniReader iniReader = new IniReader();

            if (!Properties.Settings.Default.isScanOn)
            {

                Properties.Settings.Default.isScanOn = true;
                Properties.Settings.Default.globalMouseClickCount = 1;
                Properties.Settings.Default.isScanPaused = false;

                do
                {

                    for (int i = Properties.Settings.Default.lastActivecColumn; i < 11; i++)
                    {
                        if (iniReader.scanningInterval == "1")
                        {
                            if (!Properties.Settings.Default.CycleScanPaused)
                                cycleScanCount++;

                            if (cycleScanCount > iniReader.scan_cycles_amount)
                            {
                                Properties.Settings.Default.CycleScanPaused = true;
                                Properties.Settings.Default.isScanOn = false;
                                Properties.Settings.Default.isScanPaused = true;


                                if (cts != null)
                                {
                                    Properties.Settings.Default.globalMouseClickCount = 0;
                                    cts.Cancel();
                                }

                            }
                        }


                        foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetColumn(j) == i))
                        {
                            if (child.GetType() == typeof(Button))
                            {
                                Button button = (Button)child;
                                button.Background = Brushes.Red;

                                Properties.Settings.Default.lastActivecColumn = i;
                            }
                        }
                        await Task.Delay(TimeSpan.FromSeconds(0.5), ct);

                        foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetColumn(j) == i))
                        {
                            if (child.GetType() == typeof(Button))
                            {
                                Button button = (Button)child;
                                button.Background = Brushes.DarkGray;
                            }
                        }

                        if (Properties.Settings.Default.CycleScanPaused)
                            Properties.Settings.Default.CycleScanPaused = false;

                        if (Properties.Settings.Default.globalMouseClickCount > 1)
                        {
                            do
                            {
                                foreach (object child in grid.Children.Cast<UIElement>().Where(j => Grid.GetColumn(j) == i))
                                {
                                    if (child.GetType() == typeof(Button))
                                    {
                                        Button button = (Button)child;


                                        button.Background = Brushes.Red;
                                        await Task.Delay(TimeSpan.FromSeconds(0.5), ct);
                                        button.Background = Brushes.DarkGray;
                                    }

                                    if (Properties.Settings.Default.globalMouseClickCount > 2)
                                    {
                                        Properties.Settings.Default.isScanOn = false;
                                        break;
                                    }
                                }

                            } while (Properties.Settings.Default.globalMouseClickCount < 3);

                            break;
                        }

                    }
                    Properties.Settings.Default.lastActivecColumn = 0;
                } while (Properties.Settings.Default.globalMouseClickCount < 2);
            }
        }

        public void cancelScan(Grid grid)
        {
            Properties.Settings.Default.isScanOn = false;

            if (cts != null)
            {
                Properties.Settings.Default.globalMouseClickCount = 0;
                cts.Cancel();
            }

            foreach (object child in grid.Children.Cast<UIElement>())
            {
                if (child.GetType() == typeof(Button))
                {
                    Button button = (Button)child;
                    button.Background = Brushes.DarkGray;
                }
            }
            Properties.Settings.Default.lastActiveRow = 0;
            Properties.Settings.Default.lastActivecColumn = 0;
        }


        public void pauseScan()
        {
            if (cts != null && Properties.Settings.Default.isScanOn)
            {
                Properties.Settings.Default.isScanOn = false;
                Properties.Settings.Default.isScanPaused = true;
                cts.Cancel();
            }
        }


        public async Task Scanning(Grid grid)
        {
            cts = new CancellationTokenSource();
            IniReader iniReader = new IniReader();
            switch (iniReader.scanningType)
            {
                case "1":
                    try
                    {
                        if (iniReader.scanningInterval == "2")
                            cts.CancelAfter(iniReader.scan_duration_seconds * 1000);

                        await RowScanning(cts.Token, grid);
                    }
                    catch (OperationCanceledException)
                    {
                        if (iniReader.scanningInterval == "2")
                            Properties.Settings.Default.CycleScanPaused = true;

                        if (Properties.Settings.Default.isScanOn)
                        {
                            Properties.Settings.Default.isScanOn = false;
                            Properties.Settings.Default.isScanPaused = true;
                        }

                        Properties.Settings.Default.globalMouseClickCount = 0;
                    }
                    break;

                case "2":
                    try
                    {
                        if (iniReader.scanningInterval == "2")
                            cts.CancelAfter(iniReader.scan_duration_seconds * 1000);

                        await ColumnScanning(cts.Token, grid);
                    }
                    catch (OperationCanceledException)
                    {
                        if (iniReader.scanningInterval == "2")
                            Properties.Settings.Default.CycleScanPaused = true;

                        if (Properties.Settings.Default.isScanOn)
                        {
                            Properties.Settings.Default.isScanOn = false;
                            Properties.Settings.Default.isScanPaused = true;
                        }

                        Properties.Settings.Default.globalMouseClickCount = 0;
                    }
                    break;
            }

            cts = null;
        }

        public void ScanWrite(Grid grid, TextBox textBox)
        {
            foreach (object child in grid.Children)
            {
                if (child.GetType() == typeof(Button))
                {
                    Button button = (Button)child;

                    if (button.Background == Brushes.Red)
                    {
                        if (button.CommandParameter.ToString() == "qwertz")
                        {
                            Properties.Settings.Default.SavedText = textBox.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new LowerQWERTZ();
                        }

                        else if (button.CommandParameter.ToString() == "ABC")
                        {
                            Properties.Settings.Default.SavedText = textBox.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new UpperABC();
                        }

                        else if (button.CommandParameter.ToString() == "QWERTZ")
                        {
                            Properties.Settings.Default.SavedText = textBox.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new UpperQWERTZ();
                        }

                        else if (button.CommandParameter.ToString() == "abc")
                        {
                            Properties.Settings.Default.SavedText = textBox.Text;
                            ((MainWindow)Application.Current.MainWindow).Content = new LowerABC();
                        }

                        else if (button.CommandParameter.ToString() == "Leer")
                            textBox.Text += " ";

                        else if (button.CommandParameter.ToString() == "S-ON")
                            _ = Scanning(grid);

                        else if (button.CommandParameter.ToString() == "S-PAU LowerABC")
                            pauseScan();

                        else if (button.CommandParameter.ToString() == "S-OFF")
                            cancelScan(grid);

                        else if(button.CommandParameter.ToString() == "Win")
                        {
                            WinOSK winOSK = new WinOSK();
                            winOSK.LaunchWindowsOnScreenKeyboard();
                        }

                        else if (button.CommandParameter.ToString() == "Lö e")
                        {
                            if (textBox.Text.Length > 0)
                                textBox.Text = textBox.Text.Remove(textBox.Text.Length - 1);
                        }

                        else
                            textBox.Text += button.CommandParameter;
                    }
                }
            }
        }

        public void SetScanningType(string pressedButtonText, string otherButtonText, string oldScanningType, string newScanningType)
        {
            IniCreator iniCreator = new IniCreator();
            IniReader iniReader = new IniReader();
            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";

            foreach (CfgButton scanButton in ScanOptionsWindow.globalScanOptions.wrapPanel.Children)
            {
                if (scanButton.btnTxt == pressedButtonText && iniReader.scanningType == oldScanningType)
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }

                else if (scanButton.btnTxt == otherButtonText && iniReader.scanningType == oldScanningType)
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.GREEN);
                }
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "scan_type", newScanningType);
            }
        }

        public void SetScanningInterval(string pressedButtonText, string otherButtonText, string oldScanningInterval, string newScanningInterval)
        {
            IniCreator iniCreator = new IniCreator();
            IniReader iniReader = new IniReader();
            string imageDataPath = iniReader.dataPath + @"TMglobal-pictures\";

            foreach (CfgButton scanButton in ScanOptionsWindow.globalScanOptions.wrapPanel.Children)
            {
                if (scanButton.btnTxt == pressedButtonText && iniReader.scanningInterval == oldScanningInterval)
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.YELLOW);
                }

                else if (scanButton.btnTxt == otherButtonText && iniReader.scanningInterval == oldScanningInterval)
                {
                    scanButton.btnImage = ImageHelper.LoadBitmapImage(imageDataPath + ImageHelper.ICON.PATTERN.GREEN);
                }
                iniCreator.SaveValue("TMtextng_config_user.ini", "User", "scan_interval", newScanningInterval);
            }
        }
    }
}
