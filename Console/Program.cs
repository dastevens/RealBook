using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Core;

namespace Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            var songs = ReadCatalog(Path.Combine("..", "Data"))
                .ToArray();

            //System.Console.WriteLine($"{songs.Length} songs in catalog");
            var chordDegrees = Enumerable.Range(0, 12);
            var chordQualities = Count(songs.SelectMany(song => song.GetChords().Select(chord => chord.Quality).ToArray()))
                .OrderByDescending(count => count.Value)
                .Select(count => count.Key)
                .ToArray();

            var headers = new object[]
            {
                "Title", "Composer", "Style", "KeySignature", "TimeSignature"
            }
                .Concat(chordDegrees.Cast<object>())
                .Concat(chordQualities.Cast<object>())
                .ToArray();
            
            PrintCsvLine(headers);
            songs
                .ToList()
                .ForEach(song =>
                {
                    var chords = song.GetChords();
                    var chordDegreeCounts = Count(chords.Select(chord => chord.Degree(song.KeySignature)));
                    var chordQualityCounts = Count(chords.Select(chord => chord.Quality));

                    var fields = new object[]
                    {
                        song.SongTitle,
                        song.Composer,
                        song.Style,
                        song.KeySignature,
                        song.SongChart.Tokens.FirstOrDefault(token => token.Type == TokenType.TimeSignature)?.Symbol,
                    }
                        .Concat(chordDegrees.Select(chordDegree => chordDegreeCounts.ContainsKey(chordDegree) ? chordDegreeCounts[chordDegree] : 0).Cast<object>())
                        .Concat(chordQualities.Select(chordQuality => chordQualityCounts.ContainsKey(chordQuality) ? chordQualityCounts[chordQuality] : 0).Cast<object>())
                        ;

                    PrintCsvLine(fields.ToArray());
                });

            return;

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
            System.Console.WriteLine($"Chord roots");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => Chord.FromSymbol(token.Symbol).Root).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Chord quality counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => Chord.FromSymbol(token.Symbol).Quality).ToArray()))
                .OrderByDescending(count => count.Value)
                .ToList()
                .ForEach(count => System.Console.WriteLine($"{count.Key}: {count.Value}"));

            System.Console.WriteLine();
            System.Console.WriteLine($"Chord degree counts");
            Count(songs.SelectMany(song => song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => Chord.FromSymbol(token.Symbol).Degree(song.KeySignature)).ToArray()))
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

        static Chord[] GetChords(this Song song)
        {
            return song.SongChart.Tokens.Where(token => token.Type == TokenType.Chord).Select(token => Chord.FromSymbol(token.Symbol)).ToArray();
        }

        static void PrintCsvLine(params object[] fields)
        {
            System.Console.WriteLine(string.Join(";", fields.Select(field => field?.ToString()?.Replace(";", " ") ?? string.Empty)));
        }
    }
}
