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

            //System.Console.WriteLine($"{songs.Length} songs in catalog");
            var styleIds = CategoryIds(songs.Select(song => song.Style));
            var keySignatureIds = CategoryIds(songs.Select(song => song.KeySignature));
            var timeSignatureIds = CategoryIds(songs.Select(song => song.SongChart.Tokens.FirstOrDefault(token => token.Type == TokenType.TimeSignature)?.Symbol ?? string.Empty));
            var chordDegrees = Enumerable.Range(0, 12);
            var chordQualities = Count(songs.SelectMany(song => song.GetChords().Select(chord => chord.Quality).ToArray()))
                .OrderByDescending(count => count.Value)
                .Select(count => count.Key)
                .ToArray();

            var headers = new object[]
            {
                "Title",
                "Composer",
                "Style",
                "StyleId",
                "KeySignature",
                "KeySignatureId",
                "TimeSignature",
                "TimeSignatureId",
            }
                .Concat(chordDegrees.Select(chordDegree => $"[{chordDegree}]").Cast<object>())
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
                    var timeSignature = song.SongChart.Tokens.FirstOrDefault(token => token.Type == TokenType.TimeSignature)?.Symbol ?? string.Empty;
                    var fields = new object[]
                    {
                        song.SongTitle,
                        song.Composer,
                        song.Style,
                        styleIds[song.Style],
                        song.KeySignature,
                        keySignatureIds[song.KeySignature],
                        timeSignature,
                        timeSignatureIds[timeSignature],
                    }
                        .Concat(chordDegrees.Select(chordDegree => chordDegreeCounts.ContainsKey(chordDegree) ? chordDegreeCounts[chordDegree] : 0).Cast<object>())
                        .Concat(chordQualities.Select(chordQuality => chordQualityCounts.ContainsKey(chordQuality) ? chordQualityCounts[chordQuality] : 0).Cast<object>())
                        ;

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
