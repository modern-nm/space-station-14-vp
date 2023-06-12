using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TerraFX.Interop.Windows;

#pragma warning disable RA0003

namespace Content.Client.UserInterface.Systems.Chat
{
    internal class ChatAutocompleteHandler
    {
        public List<Word> Dict { get; set; }

        private string JsonString { get; set; }

        readonly string _dictpath = Path.Combine(Environment.GetFolderPath(
    Environment.SpecialFolder.ApplicationData), "Space Station 14/data/autocomplete_dict.json");

        public ChatAutocompleteHandler()
        {
            Dict = GetDictList();
            JsonString = "";
        }

        public List<Word> GetDictList()
        {
            if (OperatingSystem.IsWindows())
            {
                if (!File.Exists(_dictpath))
                {
                    File.WriteAllText(_dictpath, JsonSerializer.Serialize(Dict));
                    //File.WriteAllText(_dictpath, JsonSerializer.Serialize<List<Word>>(new List<Word>()));
                    //StreamWriter streamWriter = new StreamWriter(_dictpath, false);
                    //streamWriter.Write(JsonSerializer.Serialize(Dict));
                    //streamWriter.Close();
                    return new List<Word>();
                }
                else
                {
                    JsonString = File.ReadAllText(_dictpath);
                    var data = (JsonSerializer.Deserialize<List<Word>>(JsonString));
                    return data != null ? data : new List<Word>();
                }
            }
            else return new List<Word>();
        }
        public void SaveDictionary()
        {
            File.WriteAllText(_dictpath, JsonSerializer.Serialize(Dict));
        }

        //Try to add word to lexinon [ List<Word> Dict ]
        public void AddWord(string item)
        {
            Word word = new Word(item);
            if (!Dict.Contains(word))
            {
                Dict.Add(word);
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
                strings.Add(word.Value);
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
                foreach (var item in Dict)
                {
                    if (item.Text.Contains(match.Value))
                    {
                        //int lastIndex = item.Text.LastIndexOf(match.Value);
                        //string toAppend = item.Text.Substring(0,lastIndex);
                        int length = item.Text.Length - match.Value.Length;
                        string toAppend = item.Text.Substring(match.Value.Length, length);
                        return toAppend;
                    }
                }
                return "";
            }
            return "";
        }

    }
    internal class Word
    {
        public string Text { get; set; }
        public Word(string text) { Text = text; }
    }
}
