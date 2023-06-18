using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Text.Unicode;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;

#pragma warning disable RA0003

namespace Content.Client.UserInterface.Systems.Chat
{
    internal class ChatAutocompleteHandler
    {
        public List<Word> Lexicon { get; set; }
        public JsonSerializerOptions Options { get; set; }
        private string JsonString { get; set; }

        public string PrevInput { get; set; }

        readonly string _dictpath = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), @"Space Station 14/data/autocomplete_dict.json");

        public ChatAutocompleteHandler()
        {
            Lexicon = GetDictList();
            JsonString = "";
            Options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic), WriteIndented = true};
            PrevInput = "";
            SortLexicon();
            SaveDictionary();
        }

        public List<Word> GetDictList()
        {
            if (OperatingSystem.IsWindows())
            {
                if (!File.Exists(_dictpath))
                {
                    File.WriteAllText(_dictpath, JsonSerializer.Serialize(Lexicon, Options));
                    //File.WriteAllText(_dictpath, JsonSerializer.Serialize<List<Word>>(new List<Word>()));
                    //StreamWriter streamWriter = new StreamWriter(_dictpath, false);
                    //streamWriter.Write(JsonSerializer.Serialize(Dict));
                    //streamWriter.Close();
                    return new List<Word>();
                }
                else
                {
                    JsonString = File.ReadAllText(_dictpath);
                    var data = (JsonSerializer.Deserialize<List<Word>>(JsonString, Options));
                    return data != null ? data : new List<Word>();
                }
            }
            else return new List<Word>();
        }
        public void SaveDictionary()
        {
            var str = JsonSerializer.Serialize(Lexicon,Options);
            File.WriteAllText(_dictpath, str);
        }

        //Try to add word to lexinon [ List<Word> Dict ]
        public void AddWord(string item)
        {
            Word word = new Word(item);
            if (!Lexicon.Contains(word))
            {
                Lexicon.Add(word);
            }
        }
        //Parse chat input. прикрутить на Chatbox.OnTextEntered
        public List<string> ParseInput(string input)
        {
            Regex regex = new Regex(@"\w+");
            MatchCollection words = regex.Matches(input);
            List<string> strings = new List<string>();
            foreach (Match word in words)
            {
                strings.Add(word.Value.ToLower());
            }
            return strings;
        }
        //прикрутить на chatbox.onTextChanged
        public string GetWordPartToAppend(string input)
        {
            Regex regex = new Regex(@"\w+$");
            Match match = regex.Match(input);
            if (match.Value != "")
            {
                foreach (var item in Lexicon)
                {
                    if (item.Text.Contains(match.Value.ToLower()))
                    {
                        //int lastIndex = item.Text.LastIndexOf(match.Value);
                        //string toAppend = item.Text.Substring(0,lastIndex);
                        int length = item.Text.Length - match.Value.Length;
                        string toAppend = item.Text.Substring(match.Value.Length, length);
                        if (PrevInput.Length >= input.Length)
                        {
                            PrevInput = input;
                            return "";
                        }
                        PrevInput = input;
                        return toAppend;
                    }
                }
                PrevInput = input;
                return "";
            }
            PrevInput = input;
            return "";
        }
        public void SortLexicon()
        {
            Lexicon.Sort(delegate (Word x, Word y)
            {
                if (x.Text == null && y.Text == null) return 0;
                else if (x.Text == null) return -1;
                else if (y.Text == null) return 1;
                else return x.Text.CompareTo(y.Text);
            });
        }

    }
    internal class Word
    {
        public string Text { get; set; }
        public Word(string text) { Text = text; }
    }
}
