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
            var songs = ReadCatalog(Path.Combine("..", "Data"))
                .Where(song => song.Style.Contains("Country", StringComparison.InvariantCultureIgnoreCase))
                .ToArray();

            System.Console.WriteLine($"{songs.Length} songs in catalog");

            System.Console.WriteLine();
            System.Console.WriteLine($"Key signature counts");
            Count(songs.Select(song => song.KeySignature))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Time signature counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.TimeSignature).Select(token => token.Symbol).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Chord counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => token.Symbol).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Chord quality counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.ChordQuality).Select(token => token.Symbol).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Chord degree counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => ChordDegree(song.KeySignature, token.Symbol)).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));
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

        static int ChordDegree(KeySignature keySignature, string symbol)
        {
            var keySignatureDegree = KeySignatureDegrees[keySignature];
            var chordDegree = ChordDegrees[symbol];
            return (chordDegree - keySignatureDegree + 12) % 12;
        }

        static Dictionary<string, int> ChordDegrees = new Dictionary<string, int>
        {
            { "A", 0 },
            { "A#", 1 },
            { "Bb", 1 },
            { "B", 2 },
            { "C", 3 },
            { "C#", 4 },
            { "Db", 4 },
            { "D", 5 },
            { "D#", 6 },
            { "Eb", 6 },
            { "E", 7 },
            { "F", 8 },
            { "F#", 9 },
            { "Gb", 9 },
            { "G", 10 },
            { "G#", 11 },
            { "Ab", 11 },
        };

        static Dictionary<KeySignature, int> KeySignatureDegrees = new Dictionary<KeySignature, int>
        {
            { KeySignature.AMajor, 0 },
            { KeySignature.AMinor, 0 },
            { KeySignature.BFlatMajor, 1 },
            { KeySignature.BFlatMinor, 1 },
            { KeySignature.BMajor, 2 },
            { KeySignature.BMinor, 2 },
            { KeySignature.CMajor, 3 },
            { KeySignature.CMinor, 3 },
            { KeySignature.CSharpMinor, 4 },
            { KeySignature.DFlatMajor, 4 },
            { KeySignature.DMajor, 5 },
            { KeySignature.DMinor, 5 },
            { KeySignature.EFlatMajor, 6 },
            { KeySignature.EFlatMinor, 6 },
            { KeySignature.EMajor, 7 },
            { KeySignature.EMinor, 7 },
            { KeySignature.FMajor, 8 },
            { KeySignature.FMinor, 8 },
            { KeySignature.GFlatMajor, 9 },
            { KeySignature.FSharpMinor, 9 },
            { KeySignature.GMajor, 10 },
            { KeySignature.GMinor, 10 },
            { KeySignature.AFlatMajor, 11 },
            { KeySignature.GSharpMinor, 11 },
        };

        static Dictionary<CountedType, int> Count<CountedType>(IEnumerable<CountedType> toBeCounted)
        {
            return toBeCounted
                .Aggregate(
                    seed: new Dictionary<CountedType, int>(),
                    func: (counts, item) => 
                    {
                        if (!counts.ContainsKey(item))
                        {
                            counts[item] = 1;
                        }
                        else 
                        {
                            counts[item]++;
                        }
                        return counts;
                    });
        }
    }
}
