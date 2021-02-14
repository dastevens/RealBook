using System.Collections.Generic;
using System.IO;
using System.Linq;
using Core;

namespace Console
{
    static class Program
    {
        static void Main(string[] args)
        {
            var songs = ReadCatalog(Path.Combine("..", "Data"))
                .Where(song => args.Length > 0 ? args.Any(arg => song.Style.Contains(arg)) : true)
                .ToArray();

            songs = songs.Where(song => song.SongChart.Tokens.All(token => new[] { TokenType.TimeSignature, TokenType.Chord, TokenType.BarLine }.Contains(token.Type)))
                .ToArray();

            var headers = new object[]
            {
                "Title",
                "Composer",
                "Style",
                "KeySignature",
                "TimeSignature",
            }
                .Append("Chords")
                .ToArray();
            
            PrintCsvLine(headers);
            songs
                .ToList()
                .ForEach(song =>
                {
                    var chords = song.GetChords();
                    var timeSignature = song.SongChart.Tokens.FirstOrDefault(token => token.Type == TokenType.TimeSignature)?.Symbol ?? string.Empty;
                    var fields = new object[]
                    {
                        song.SongTitle,
                        song.Composer,
                        song.Style,
                        song.KeySignature,
                        timeSignature,
                    }
                        .Concat(Player.Play(song).Select(chord => $"[{chord.Degree(song.KeySignature)}]{chord.Quality}").ToArray());

                    PrintCsvLine(fields.ToArray());
                });
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

        static Dictionary<CategoryType, int> CategoryIds<CategoryType>(IEnumerable<CategoryType> categories)
        {
            return categories
                .Distinct()
                .Select((category, index) => new { Category = category, Index = index})
                .ToDictionary(categoryIndex => categoryIndex.Category, styleIndex => styleIndex.Index);
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
