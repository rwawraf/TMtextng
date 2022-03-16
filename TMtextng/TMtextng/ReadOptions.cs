using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using TMtextng.KeyboardPages;
using TMtextng.Voices;

namespace TMtextng
{
    class ReadOptions
    {

        public async Task ReadWord(TextBox textBox)
        {
            IniReader iniReader = new IniReader();
            MyVoice myVoice = new MyVoice();

            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

            speechSynthesizer.Rate = iniReader.voiceSpeed_MS - 10;
            speechSynthesizer.Volume = iniReader.voiceVolume;

            if (iniReader.svox_voice_active == 0)
                speechSynthesizer.SelectVoice(iniReader.selectedLanguage);

            if (textBox.Text.EndsWith(" ") && (textBox.Text.Length > Properties.Settings.Default.ReadCursor))
            {
                string writtenText = textBox.Text.Remove(0, Properties.Settings.Default.ReadCursor);
                string[] parts = writtenText.Split(' ');
                string lastWord = parts[parts.Length - 2];

                if (textBox.Text.LastIndexOf(" ") != Properties.Settings.Default.ReadCursor)
                {
                    if (Properties.Settings.Default.MyVoiceActive)
                    {
                        string soundToPlay = myVoice.GetMyVoiceSoundFile(@"TMmyvoice\GER_M", lastWord);
                        if (File.Exists(soundToPlay))
                        {
                            WordSuggestion.Check_New_Words(lastWord);
                            Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(" "); //Cursor setting cannott occur after reading, to prevent misposition when text removed during reading
                            SoundPlayer snd = new SoundPlayer(soundToPlay);
                            await Task.Run(() => snd.PlaySync());
                        }
                        else if (iniReader.svox_voice_active == 1)
                        {
                            WordSuggestion.Check_New_Words(lastWord);
                            Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(" ");
                            await Task.Run(() => SpeakVoice.ReadFromBufferAsync(lastWord));
                        }

                        else
                        {
                            WordSuggestion.Check_New_Words(lastWord);
                            Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(" ");
                            await Task.Run(() => speechSynthesizer.Speak(lastWord));
                        }
                    }

                    else if (iniReader.svox_voice_active == 1)
                    {
                        WordSuggestion.Check_New_Words(lastWord);
                        Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(" ");
                        await Task.Run(() => SpeakVoice.ReadFromBufferAsync(lastWord));
                    }

                    else
                    {
                        WordSuggestion.Check_New_Words(lastWord);
                        Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(" ");
                        await Task.Run(() => speechSynthesizer.Speak(lastWord));
                    }

                    Properties.Settings.Default.LastSpeech = lastWord;
                }
            }
        }

        public async Task ReadSentence(TextBox textBox)
        {
            string text = "";
            string textToCheck = "";
            IniReader iniReader = new IniReader();
            SpeechSynthesizer speechSynthesizer = new SpeechSynthesizer();

            speechSynthesizer.Rate = iniReader.voiceSpeed_MS - 10;
            speechSynthesizer.Volume = iniReader.voiceVolume;

            if (iniReader.svox_voice_active == 0)
                speechSynthesizer.SelectVoice(iniReader.selectedLanguage);

            if (textBox.Text.EndsWith(".") && (textBox.Text.Length > Properties.Settings.Default.ReadCursor))
            {
                int freq = textBox.Text.Split('.').Length - 1;
                int k = 0;

                if (freq == 1)
                {
                    text = textBox.Text.Remove(0, Properties.Settings.Default.ReadCursor);
                }

                else
                {
                    int[] tab = new int[freq];

                    for (int i = 0; i < textBox.Text.Length; i++)
                    {
                        if (textBox.Text[i] == '.')
                        {
                            tab[k] = i;
                            k++;
                        }
                    }

                    int amountOfLetters = tab[freq - 2] + 1;
                    text = textBox.Text.Remove(0, amountOfLetters);
                }

                textToCheck = text.Substring(0, text.Length - 1);
                WordSuggestion.Check_New_Words(textToCheck);
                string[] parts = textToCheck.Split(' ');

                MyVoice myVoice = new MyVoice();

                if (textBox.Text.LastIndexOf(".") != Properties.Settings.Default.ReadCursor)
                {
                    if (Properties.Settings.Default.MyVoiceActive)
                    {
                        foreach (var part in parts)
                        {
                            string soundToPlay = myVoice.GetMyVoiceSoundFile(@"TMmyvoice\GER_M", part);

                            if (File.Exists(soundToPlay))
                            {
                                Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(".");
                                SoundPlayer snd = new SoundPlayer(soundToPlay);
                                await Task.Run(() => snd.PlaySync());
                            }
                            else if (iniReader.svox_voice_active == 1)
                            {
                                Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(".");
                                await Task.Run(() => SpeakVoice.ReadFromBufferAsync(part));
                            }

                            else
                            {
                                Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(".");
                                await Task.Run(() => speechSynthesizer.Speak(part));
                            }
                        }
                    }
                    else if (iniReader.svox_voice_active == 1)
                    {
                        Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(".");
                        await Task.Run(() => SpeakVoice.ReadFromBufferAsync(text));
                    }

                    else
                    {
                        Properties.Settings.Default.ReadCursor = textBox.Text.LastIndexOf(".");
                        await Task.Run(() => speechSynthesizer.Speak(text));
                    }
                }

                Properties.Settings.Default.LastSpeech = text;
            }
        }

        public void DisableReadButton()
        {

            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {             
                for(int i = 0; i < LowerABC.globalLowerABC.configureButton_TextBlock.Length; i++)
                {
                    if (LowerABC.globalLowerABC.configureButton_TextBlock[i].Text == "Vorl")
                        LowerABC.globalLowerABC.configureButton[i].IsEnabled = false;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                for (int i = 0; i < LowerQWERTZ.globalLowerQWERTZ.configureButton_TextBlock.Length; i++)
                {
                    if (LowerQWERTZ.globalLowerQWERTZ.configureButton_TextBlock[i].Text == "Vorl")
                        LowerQWERTZ.globalLowerQWERTZ.configureButton[i].IsEnabled = false;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                for (int i = 0; i < UpperABC.globalUpperABC.configureButton_TextBlock.Length; i++)
                {
                    if (UpperABC.globalUpperABC.configureButton_TextBlock[i].Text == "Vorl")
                        UpperABC.globalUpperABC.configureButton[i].IsEnabled = false;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                for (int i = 0; i < UpperQWERTZ.globalUpperQWERTZ.configureButton_TextBlock.Length; i++)
                {
                    if (UpperQWERTZ.globalUpperQWERTZ.configureButton_TextBlock[i].Text == "Vorl")
                        UpperQWERTZ.globalUpperQWERTZ.configureButton[i].IsEnabled = false;
                }
            }
        }

        public void EnableReadButton()
        {

            if (((MainWindow)Application.Current.MainWindow).Content == LowerABC.globalLowerABC)
            {
                for (int i = 0; i < LowerABC.globalLowerABC.configureButton_TextBlock.Length; i++)
                {
                    if (LowerABC.globalLowerABC.configureButton_TextBlock[i].Text == "Vorl")
                        LowerABC.globalLowerABC.configureButton[i].IsEnabled = true;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == LowerQWERTZ.globalLowerQWERTZ)
            {
                for (int i = 0; i < LowerQWERTZ.globalLowerQWERTZ.configureButton_TextBlock.Length; i++)
                {
                    if (LowerQWERTZ.globalLowerQWERTZ.configureButton_TextBlock[i].Text == "Vorl")
                        LowerQWERTZ.globalLowerQWERTZ.configureButton[i].IsEnabled = true;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperABC.globalUpperABC)
            {
                for (int i = 0; i < UpperABC.globalUpperABC.configureButton_TextBlock.Length; i++)
                {
                    if (UpperABC.globalUpperABC.configureButton_TextBlock[i].Text == "Vorl")
                        UpperABC.globalUpperABC.configureButton[i].IsEnabled = true;
                }
            }

            else if (((MainWindow)Application.Current.MainWindow).Content == UpperQWERTZ.globalUpperQWERTZ)
            {
                for (int i = 0; i < UpperQWERTZ.globalUpperQWERTZ.configureButton_TextBlock.Length; i++)
                {
                    if (UpperQWERTZ.globalUpperQWERTZ.configureButton_TextBlock[i].Text == "Vorl")
                        UpperQWERTZ.globalUpperQWERTZ.configureButton[i].IsEnabled = true;
                }
            }
        }
    }
}
