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

            // Songs are separated by "==="
            // foreach (var input in songBookUrl.Split(new[] { "1r34LbKcu7" }, StringSplitOptions.RemoveEmptyEntries).Skip(0))
            foreach (var input in songBookUrl.Split(new[] { "irealb://", "===" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var components = Uri.UnescapeDataString(input).Split(new[] { "=", "1r34LbKcu7" }, StringSplitOptions.RemoveEmptyEntries);

                if (components.Length > 4)
                {
                    var unescaped = components[4];
                    var decoded = decoder.Decode(unescaped);
                    var songUrl = $"irealbook://{components[0]}={components[1]}={components[2]}={components[3]}=n={decoded}";
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