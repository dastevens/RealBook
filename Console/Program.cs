using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;

namespace Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var songs = ReadCatalog(Path.Combine("..", "Data"));

            System.Console.WriteLine($"{songs.Length} songs in catalog");
            songs
                .Select(song => song.Style)
                .Distinct()
                .OrderBy(style => style)
                .ToList()
                .ForEach(style => System.Console.WriteLine(style));
        }

        static Song[] ReadCatalog(string folder)        
        {
            var result = new List<Song>();
            var songBookParser = new SongBookParser();
            foreach (var irealb in Directory.EnumerateFiles(folder, "*.irealb"))
            {
                var songBookUrl = File.ReadAllText(irealb);

                var songs = songBookParser.Parse(songBookUrl);
                result.AddRange(songs);
            }
            return result.ToArray();
        }
    }
}
