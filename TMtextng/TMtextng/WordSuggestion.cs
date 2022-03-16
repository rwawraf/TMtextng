using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using TMtextng.KeyboardPages;


namespace TMtextng
{
    class WordSuggestion
    {
        static List<string> temporary_new_words;
        static IniReader iniReader = new IniReader();
        public static string wordsToReadPath_master = iniReader.dataPath + @"ABC\abc_master.txt";
        public static string wordsToReadPath_user = iniReader.dataPath + @"ABC\abc_user.txt";
        public static string wordsToReadPath_user_copy = iniReader.dataPath + @"ABC\abc_user_copy.txt";
        public static string new_words_path = iniReader.dataPath + @"ABC\new_words.txt";


        public static List<string> CreateListFromFile(string path)
        {
            string line;
            StreamReader sr = new StreamReader(path, Encoding.Default);

            List<string> CreatedList = new List<string>();

            while ((line = sr.ReadLine()) != null)
            {
                CreatedList.Add(line);
            }

            sr.Close();
            return CreatedList;
        }

        public static void UpdateUserAbcOnStartup()
        {
            List<string> SuggestionList = CreateListFromFile(wordsToReadPath_user_copy);


            for (int i = 0; i < SuggestionList.Count; i++)
            {
                string[] fields = SuggestionList[i].Split(';');

                int valueToIncrement = int.Parse(fields[2]);
                valueToIncrement++;
                string newValue = valueToIncrement.ToString();

                var dataArray = SuggestionList[i].Split(';');
                dataArray[2] = " " + newValue + " ";
                SuggestionList[i] = String.Join(";", dataArray);

            }

            File.WriteAllLines(wordsToReadPath_user_copy, SuggestionList, Encoding.Default);
        }

        public static void UpdateUserAbcOnWordClick(string checkedWord)
        {
            List<string> SuggestionList = CreateListFromFile(wordsToReadPath_user_copy);


            for (int i = 0; i < SuggestionList.Count; i++)
            {
                string[] fields = SuggestionList[i].Split(';');

                if(fields[0].Equals(checkedWord, StringComparison.OrdinalIgnoreCase))
                {
                    int valueToIncrement = int.Parse(fields[1]);
                    valueToIncrement++;
                    string newValue = valueToIncrement.ToString();

                    var dataArray = SuggestionList[i].Split(';');
                    dataArray[1] = " " + newValue + " ";
                    SuggestionList[i] = String.Join(";", dataArray);
                }

            }

            File.WriteAllLines(wordsToReadPath_user_copy, SuggestionList, Encoding.Default);
        }

        public static void ClearSuggestionWords()
        {
            List<string> SuggestionList = CreateListFromFile(wordsToReadPath_user);

            IniReader iniReader = new IniReader();

            for (int i = 0; i < SuggestionList.Count; i++)
            {
                string[] fields = SuggestionList[i].Split(';');


                if (int.Parse(fields[3]) == 0)
                {
                    if(int.Parse(fields[2]) == iniReader.amount_of_App_Starts)
                    {
                        if(int.Parse(fields[1]) < iniReader.min_Suggested_Word_Uses)
                        {
                            SuggestionList.Remove(SuggestionList[i]);
                            i--;
                        }
                    }
                }             
            }

            if (!File.Exists(wordsToReadPath_user_copy))
            {
                File.WriteAllLines(wordsToReadPath_user_copy, SuggestionList, Encoding.Default);
            }
        }


        public static void ReadSuggestionWords(string[,] words)
        {
            string line;
            int i = 0;
           
            if(File.Exists(wordsToReadPath_user_copy))
            {
                StreamReader sr = new StreamReader(wordsToReadPath_user_copy, Encoding.Default);
                while ((line = sr.ReadLine()) != null)
                {
                    string[] fields = line.Split(';');

                    words[i, 0] = fields[0];
                    words[i, 1] = fields[1];
                    words[i, 2] = fields[2];
                    words[i, 3] = fields[3];
                    i++;
                }

                sr.Close();
            }
        }
  

        public static void SortWordsByNumbersOfUsedTimes(string[,] n)
        {
            for (int i = 0; i < n.GetLength(0) - 1; i++)
            {
                for (int j = i; j < n.GetLength(0); j++)
                {
                    int firstWord_NumberOfUsedTimes = int.Parse(n[i, 1]);
                    int secondWord_NumberOfUsedTimes = int.Parse(n[j, 1]);

                    if (firstWord_NumberOfUsedTimes < secondWord_NumberOfUsedTimes)
                    {
                        for (int k = 0; k < n.GetLength(1); k++)
                        {
                            var temp = n[i, k];
                            n[i, k] = n[j, k];
                            n[j, k] = temp;
                        }
                    }
                }
            }
        }

        public static void CreateAbcUser()
        {
            temporary_new_words = new List<string>();

            if (!File.Exists(wordsToReadPath_user))
            {
                try
                {
                    File.Copy(wordsToReadPath_master, wordsToReadPath_user, true);
                }
                catch (IOException iox)
                {
                    Console.WriteLine(iox.Message);
                }
            }
        }

        public static void DeleteFile(string file)
        {
            try
            {
                // Delete the temp file (if it exists)
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleteing file: " + ex.Message);
            }
        }

        public static void Check_New_Words(string inputText)
        {
            string[] dividedWords = inputText.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries); 
            List<string> new_words = new List<string>(dividedWords);
            List<string> words_to_remove = new List<string>();

            int lineCount;
            string[,] abc_user_words;


            if (File.Exists(wordsToReadPath_user_copy))
            {
                lineCount = File.ReadLines(wordsToReadPath_user_copy).Count();
                abc_user_words = new string[lineCount, 4];

                ReadSuggestionWords(abc_user_words);


                for (int i = 0; i < new_words.Count; i++)
                {
                    for (int j = 0; j < abc_user_words.GetLength(0); j++)
                    {
                        if (new_words[i].Equals(abc_user_words[j, 0].Remove(abc_user_words[j, 0].Length - 1), StringComparison.OrdinalIgnoreCase))
                            words_to_remove.Add(new_words[i]);
                    }
                }

                new_words.RemoveAll(item => words_to_remove.Contains(item));


                if (File.Exists(new_words_path))
                {
                    List<string> saved_new_words = CreateListFromFile(new_words_path);
                    List<string> abc_user_words_list = CreateListFromFile(wordsToReadPath_user_copy);

                    for (int i = 0; i < new_words.Count; i++)
                    {
                        if (saved_new_words.Contains(new_words[i]) && (temporary_new_words.Contains(new_words[i]) == false))
                        {
                            saved_new_words.RemoveAll(item => item == new_words[i]);

                            abc_user_words_list.Add(new_words[i] + " ; 0 ; 0 ; 0");
                            abc_user_words_list.Sort();
                            File.WriteAllLines(wordsToReadPath_user_copy, abc_user_words_list, Encoding.Default);

                            new_words.RemoveAll(item => item == new_words[i]);
                        }

                        else
                        {
                            temporary_new_words.Add(new_words[i]);
                            File.AppendAllLines(new_words_path, temporary_new_words,Encoding.Default);
                        }
                           
                    }
                    File.WriteAllLines(new_words_path, saved_new_words, Encoding.Default);                  
                }

                else
                {
                    for (int i = 0; i < new_words.Count; i++)
                        temporary_new_words.Add(new_words[i]);
                }
                    

                File.AppendAllLines(new_words_path, new_words, Encoding.Default);
            }
        }
    }
}
