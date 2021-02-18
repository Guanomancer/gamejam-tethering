using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Tethering.Net
{
    public class ProfanityFilter
    {
        public const string FILE_NAME = "ProfanityList.txt";
        const string WORD_SECTION_MARKER = "-----";

        private List<string> _words = new List<string>();

        public IReadOnlyList<string> Words { get => _words; }

        public ProfanityFilter()
        {
            if (File.Exists(FILE_NAME))
                LoadFile();
        }

        public bool ContainsProfanity(string text, out string word)
        {
            text = text.ToLower();
            for(int i = 0; i < _words.Count; i++)
            {
                if (text.Contains(_words[i]))
                {
                    word = _words[i];
                    return true;
                }
            }
            word = null;
            return false;
        }

        public void LoadString(string wordString)
        {
            using (var ms = new MemoryStream(Encoding.UTF8.GetBytes(wordString)))
            {
                LoadStream(ms);
            }
        }

        public void LoadFile(string file = FILE_NAME)
        {
            using (var fs = File.OpenRead(file))
            {
                LoadStream(fs);
            }
        }

        private void LoadStream(Stream stream)
        {
            using (TextReader reader = new StreamReader(stream))
            {
                bool isReading = false;
                while (reader.Peek() != -1)
                {
                    var line = reader.ReadLine().ToLower();
                    var isMarkerLine = line.StartsWith(WORD_SECTION_MARKER);
                    if (!isReading && isMarkerLine)
                        isReading = true;
                    else if (isReading && !isMarkerLine)
                        _words.Add(line);
                    else if (isReading && isMarkerLine)
                        break;
                }
            }
        }
    }
}
