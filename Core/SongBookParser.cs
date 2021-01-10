using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SongBookParser
    {
        public Song[] Parse(string songBookUrl)
        {
            var result = new List<Song>();
            var decoder = new Decoder();
            var songParser = new SongParser();

            foreach (var input in songBookUrl.Split(new[] { "1r34LbKcu7" }, StringSplitOptions.RemoveEmptyEntries).Skip(1))
            {
                var components = Uri.UnescapeDataString(input).Split('=');
                var unescaped = components[0];
                var decoded = decoder.Decode(unescaped);

                if (components.Length > 10)
                {
                    var songUrl = $"irealbook://{components[6]}={components[7]}={components[9]}={components[10]}=n={decoded}";
                    try
                    {
                        var song = songParser.Parse(songUrl);
                        result.Add(song);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Failed for {components[6]}: {e.Message}");
                    }
                }
            }
            return result.ToArray();
        }
    }
}