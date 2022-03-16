using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Win32;

namespace TMtextng
{
    class RegistryValue
    {
        private int speed = 100;
        private int pitch = 100;
        private int volume = 100;

        private bool man_voice = true;
        private bool output_MP3 = true;
        private bool create_daisy = true;

        private int convert_type = -1;
        private int minutes = -1;
        private int end_message = -1;

        private int MP3_sample_rate = -1;

        private int setBits(int original, int newValue, int offset, int mask)
        {
            int temp_original = original;
            //return ((original >> offset) & (~mask) | newValue) << offset;
            return ((original >> offset) & (~mask) | newValue) << offset
                | ((~((~0) << offset)) & temp_original);
        }

        //  24  --> MP3 SampleRate
        //  32  --> MP3 SampleRate
        //  40  --> MP3 SampleRate
        //  48  --> MP3 SampleRate
        public int SetMP3SampleRate(int rate)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValOptions");
            if (rate == 24)
                val_opt = setBits(val_opt, 0, 9, 3);   // 00 -> 0
            else if (rate == 32)
                val_opt = setBits(val_opt, 1, 9, 3);   // 01 -> 1
            else if (rate == 40)
                val_opt = setBits(val_opt, 2, 9, 3);   // 10 -> 2
            else if (rate == 48)
                val_opt = setBits(val_opt, 3, 9, 3);   // 11 -> 3
            rk.SetValue("ValOptions", val_opt);

            return val_opt;
        }

        public int ReadMP3SampleRate()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValOptions");

            int temp = (int)(val_opt << 20);
            temp = (int)(temp >> 29);
            return temp;
        }

        public int SaveMP3SampleRate()
        {
            this.MP3_sample_rate = ReadMP3SampleRate();

            if (MP3_sample_rate == -4)
                this.MP3_sample_rate = 24;
            if (MP3_sample_rate == -3)
                this.MP3_sample_rate = 32;
            if (MP3_sample_rate == -2)
                this.MP3_sample_rate = 40;
            if (MP3_sample_rate == -1)
                this.MP3_sample_rate = 48;
            return this.MP3_sample_rate;
        }

        public int RestoreMP3SampleRate()
        {
            SetMP3SampleRate(this.MP3_sample_rate);
            return this.MP3_sample_rate;
        }



        //  true    --> Daisy erzeugen
        //  false   --> Kein Daisy erzeugen
        public bool SetDaisy(bool c_daisy)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValOptions");
            if (c_daisy)
                val_opt = setBits(val_opt, 1, 27, 1);
            else
                val_opt = setBits(val_opt, 0, 27, 1);
            rk.SetValue("ValOptions", val_opt);

            return Convert.ToBoolean(val_opt);
        }

        public bool ReadDaisy()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValOptions");

            int temp = (int)(val_opt << 4);
            temp = (int)(temp >> 31);
            return Convert.ToBoolean(temp);
        }

        public bool SaveDaisy()
        {
            this.create_daisy = (bool)ReadDaisy();
            return this.create_daisy;
        }

        public bool RestoreDaisy()
        {
            SetDaisy(create_daisy);
            return this.create_daisy;
        }



        //  1       --> Text nicht in mehrere Sounddateien aufteilen
        //  2       --> Text in 1 - 100 Minuten lange Sounddateien trennen
        //  3       --> Text nach TM - Steuerzeichen trennen
        public int SetConvert(int type)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValOptions");
            if (type == 1)
                val_opt = setBits(val_opt, 0, 24, 7);
            else if (type == 2)
                val_opt = setBits(val_opt, 2, 24, 7);
            else if (type == 3)
                val_opt = setBits(val_opt, 4, 24, 7);
            rk.SetValue("ValOptions", val_opt);

            return val_opt;
        }

        public int ReadConvert()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValOptions");

            //return val_opt >> 24;
            int temp = (int)(val_opt << 5);
            temp = (int)(temp >> 29);
            return (int)temp;
        }

        public int SaveConvert()
        {
            int convert = ReadConvert();
            if (convert == 0)
                this.convert_type = 1;
            if (convert == 2)
                this.convert_type = 2;
            if (convert == -4)
                this.convert_type = 3;
            return this.convert_type;
        }

        public int RestoreConvert()
        {
            SetConvert(this.convert_type);
            return this.convert_type;
        }



        //  1-100   --> Text in 1 - 100 Minuten lange Sounddateien trennen
        public int SetMinutes(int minutes)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValSplitTime");
            val_opt = setBits(val_opt, minutes, 0, 127);
            rk.SetValue("ValSplitTime", val_opt);

            return val_opt;
        }

        public int ReadMinutes()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValSplitTime");

            //int temp = (int)(val_opt << 25);
            //temp = (int)(temp >> 25);
            int temp = val_opt;
            return temp;
        }

        public int SaveMinutes()
        {
            this.minutes = ReadMinutes();
            return this.minutes;
        }

        public int RestoreMinutes()
        {
            SetMinutes(this.minutes);
            return this.minutes;
        }



        //  1 -> Keine Meldung
        //  2 -> Piepton
        //  3 -> Text ansagen
        public int SetEndMessage(int message)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValOptions");
            if (message == 1)
                val_opt = setBits(val_opt, 0, 6, 7);
            else if (message == 2)
                val_opt = setBits(val_opt, 2, 6, 7);
            else if (message == 3)
                val_opt = setBits(val_opt, 4, 6, 7);
            rk.SetValue("ValOptions", val_opt);

            return val_opt;
        }

        public int ReadEndMessage()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValOptions");

            int temp = (int)(val_opt << 22);
            temp = (int)(temp >> 28);
            return temp;
        }

        public int SaveEndMessage()
        {
            if (ReadEndMessage() == 0)
                this.end_message = 1;
            if (ReadEndMessage() == 2)
                this.end_message = 2;
            if (ReadEndMessage() == 4)
                this.end_message = 3;

            return this.end_message;
        }

        public int RestoreEndMessage()
        {
            SetEndMessage(this.end_message);
            return this.end_message;
        }




        public int SetSpeed(int speed)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValSpeak");
            val_opt = setBits(val_opt, speed, 5, 511);
            rk.SetValue("ValSpeak", val_opt);

            return val_opt;
        }

        public int ReadSpeed()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValSpeak");

            int temp = (int)(val_opt << 17);
            temp = (int)(temp >> 22);
            return temp;
        }

        public int SaveSpeed()
        {
            this.speed = ReadSpeed();
            return this.speed;
        }

        public int RestoreSpeed()
        {
            SetSpeed(this.speed);
            return this.speed;
        }



        public int SetPitch(int pitch)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValSpeak");
            val_opt = setBits(val_opt, pitch, 16, 255);
            rk.SetValue("ValSpeak", val_opt);

            return val_opt;
        }

        public int ReadPitch()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValSpeak");

            int temp = (int)(val_opt << 8);
            temp = (int)(temp >> 24);
            return temp;
        }

        public int SavePitch()
        {
            this.pitch = ReadPitch();
            return this.pitch;
        }

        public int RestorePitch()
        {
            SetPitch(this.pitch);
            return this.pitch;
        }



        public int SetVolume(int volume)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValSpeak");
            val_opt = setBits(val_opt, volume, 24, 127);
            rk.SetValue("ValSpeak", val_opt);

            return val_opt;
        }

        public int ReadVolume()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValSpeak");

            int temp = (int)(val_opt << 0);
            temp = (int)(temp >> 24);
            return temp;
        }

        public int SaveVolume()
        {
            this.volume = ReadVolume();
            return this.volume;
        }

        public int RestoreVolume()
        {
            SetVolume(this.volume);
            return this.volume;
        }



        public bool SetVoice(bool man_voice)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValSpeak");
            val_opt = setBits(val_opt, Convert.ToInt32(man_voice), 31, 1);
            rk.SetValue("ValSpeak", val_opt);

            return Convert.ToBoolean(val_opt);
        }

        public bool ReadVoice()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValSpeak");

            int temp = (int)(val_opt << 0);
            temp = (int)(temp >> 31);
            return Convert.ToBoolean(temp);
        }

        public bool SaveVoice()
        {
            this.man_voice = (bool)ReadVoice();
            return this.man_voice;
        }

        public bool RestoreVoice()
        {
            SetVoice(this.man_voice);
            return this.man_voice;
        }



        public bool SetMP3(bool output_MP3)
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32", true);

            int val_opt = (int)rk.GetValue("ValOptions");
            val_opt = setBits(val_opt, Convert.ToInt32(output_MP3), 11, 1);
            rk.SetValue("ValOptions", val_opt);

            return Convert.ToBoolean(val_opt);
        }

        public bool ReadMP3()
        {
            RegistryKey rk = Registry.LocalMachine;
            rk = rk.OpenSubKey("SOFTWARE\\TMND-GMBH\\TMdocu32");
            int val_opt = (int)rk.GetValue("ValOptions");

            int temp = (int)(val_opt << 20);
            temp = (int)(temp >> 31);
            return Convert.ToBoolean(temp);
        }

        public bool SaveMP3()
        {
            this.output_MP3 = (bool)ReadMP3();
            return this.output_MP3;
        }

        public bool RestoreMP3()
        {
            SetMP3(this.output_MP3);
            return this.output_MP3;
        }
    }
}
