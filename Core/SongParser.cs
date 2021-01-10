using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class SongParser
    {
        public Song Parse(string songUrl)
        {
            var components = SplitSongUrl(songUrl);

            if (components.Length != 6)
            {
                throw new Exception($"Song url has {components.Length} components - expecting 6");
            }

            return new Song
            {
                SongTitle = components[0],
                Composer = components[1],
                Style = components[2],
                KeySignature = ToKeySignature(components[3]),
                // components[4] is ignored
                ChordProgression = ToChordProgression(components[5]),
            };
        }

        private string[] SplitSongUrl(string songUrl)
        {
            if (!songUrl.StartsWith("irealbook://"))
            {
                throw new Exception($"Song Uri has invalid scheme: {songUrl}");
            }

            return songUrl
                .Substring("irealbook://".Length)
                .Split("=".ToCharArray());
        }

        private KeySignature ToKeySignature(string component)
        {
            switch (component)
            {
                case "C": return KeySignature.CMajor;
                case "C#": return KeySignature.DFlatMajor;
                case "Db": return KeySignature.DFlatMajor;
                case "D": return KeySignature.DMajor;
                case "D#": return KeySignature.EFlatMajor;
                case "Eb": return KeySignature.EFlatMajor;
                case "E": return KeySignature.EMajor;
                case "F": return KeySignature.FMajor;
                case "F#": return KeySignature.GFlatMajor;
                case "Gb": return KeySignature.GFlatMajor;
                case "G": return KeySignature.GMajor;
                case "G#": return KeySignature.AFlatMajor;
                case "Ab": return KeySignature.AFlatMajor;
                case "A": return KeySignature.AMajor;
                case "A#": return KeySignature.BFlatMajor;
                case "Bb": return KeySignature.BFlatMajor;
                case "B": return KeySignature.BMajor;
                case "A-": return KeySignature.AMinor;
                case "A#-": return KeySignature.BFlatMinor;
                case "Bb-": return KeySignature.BFlatMinor;
                case "B-": return KeySignature.BMinor;
                case "C-": return KeySignature.CMinor;
                case "C#-": return KeySignature.CSharpMinor;
                case "Db-": return KeySignature.CSharpMinor;
                case "D-": return KeySignature.DMinor;
                case "D#-": return KeySignature.EFlatMinor;
                case "Eb-": return KeySignature.EFlatMinor;
                case "E-": return KeySignature.EMinor;
                case "F-": return KeySignature.FMinor;
                case "F#-": return KeySignature.FSharpMinor;
                case "Gb-": return KeySignature.FSharpMinor;
                case "G-": return KeySignature.GMinor;
                case "G#-": return KeySignature.GSharpMinor;
                case "Ab-": return KeySignature.GSharpMinor;
                default: throw new Exception($"Unknown key signature {component}");
            };
        }

        private ChordProgression ToChordProgression(string component)
        {
            return new ChordProgression
            {
                Symbols = ToSymbols(component).ToArray()
            };
        }

        private IEnumerable<string> ToSymbols(string component)
        {
            if (component.Length == 0)
            {
                return new string[0];
            }

            var longestMatchingSymbol = Symbols
                .Where(symbol => component.StartsWith(symbol))
                .OrderByDescending(symbol => symbol.Length)
                .FirstOrDefault();

            if (longestMatchingSymbol != null)
            {
                return ToSymbols(component.Substring(longestMatchingSymbol.Length))
                    .Prepend(longestMatchingSymbol);
            }
            else
            {
                return ToSymbols(component.Substring(1));
            }
        }

        private string[] Symbols = new[]
        {
            // Bar Lines
            "|", // single bar line
            "[", // opening double bar line
            "]", // closing double bar line
            "{", // opening repeat bar line
            "}", // closing repeat bar line
            "Z", // Final thick double bar line

            // Time signatures: to be placed before a bar line
            "T44", // 4/4
            "T34", // 3/4
            "T24", // 2/4
            "T54", // 5/4
            "T64", // 6/4
            "T74", // 7/4
            "T22", // 2/2
            "T32", // 3/2
            "T58", // 5/8
            "T68", // 6/8
            "T78", // 7/8
            "T98", // 9/8
            "T12", // 12/8

            // Rehearsal Marks
            "*A", // A section
            "*B", // B section
            "*C", // C Section
            "*D", // D Section
            "*V", // Verse
            "*i", // Intro
            "S", // Segno
            "Q", // Coda
            "f", // Fermata

            // Endings
            "N1", // First ending
            "N2", // Second Ending
            "N3", // Third Ending
            "N0", // No text Ending

            // Staff Text
            // Staff text appears under the current chords and needs to be enclosed in angle brackets
            // <Some staff text>
            // You can move the text upwards relative to the current chord by adding a * followed by two digit number between 00 (below the system) and 74 (above the system):
            // <*36Some raised staff text>
            // There are a number of specific staff text phrases that are recognized by the player in iReal Pro:
            "<D.C. al Coda>",
            "<D.C. al Fine>",
            "<D.C. al 1st End.>",
            "<D.C. al 2nd End.>",
            "<D.C. al 3rd End.>",
            "<D.S. al Coda>",
            "<D.S. al Fine>",
            "<D.S. al 1st End.>",
            "<D.S. al 2nd End.>",
            "<D.S. al 3rd End.>",
            "<Fine>",

            // TODO: Optimistically just ignore anything else,
            // hopefully no collisions with other symbols
            "<",
            ">",

            // If you have a section of the song that is enclosed in repeat bar lines { } you can add in the staff text a number followed by ‘x’ to indicate that the section should repeat that number of times instead of the default 2 times:
            // "<8x>",
            // TODO: Just enumerate?
            "<1x>",
            "<2x>",
            "<3x>",
            "<4x>",
            "<5x>",
            "<6x>",
            "<7x>",
            "<8x>",

            // Vertical Space
            // You can add a small amount of vertical space between staves by adding between 1 and 3 ‘Y’ at the beginning of a system
            "Y",
            "YY",
            "YYY",

            // Chords
            //
            // Chord symbol format: Root + an optional chord quality + an optional inversion

            // For example just a root:
            // C
            // or a root plus a chord quality
            // C-7
            // or a root plus in inversion inversion
            // C/E
            // or a root plus a quality plus an inversion
            // C-7/Bb

            // All valid roots and inversions:
            "C",
            "C#",
            "Db",
            "D",
            "D#",
            "Eb",
            "E",
            "F",
            "F#",
            "Gb",
            "G",
            "G#",
            "Ab",
            "A",
            "A#",
            "Bb",
            "B",

            // All valid qualities:
            "5",
            "2",
            "add9", 
            "+", 
            "o", 
            "h", 
            "sus",
            "^",
            "-",
            "^7",
            "-7",
            "7",
            "7sus",
            "h7",
            "o7",
            "^9",
            "^13",
            "6",
            "69",
            "^7#11",
            "^9#11",
            "^7#5",
            "-6",
            "-69",
            "-^7",
            "-^9",
            "-9",
            "-11",
            "-7b5",
            "h9",
            "-b6",
            "-#5",
            "9",
            "7b9",
            "7#9",
            "7#11",
            "7b5",
            "7#5",
            "9#11",
            "9b5",
            "9#5",
            "7b13",
            "7#9#5",
            "7#9b5",
            "7#9#11",
            "7b9#11",
            "7b9b5",
            "7b9#5",
            "7b9#9",
            "7b9b13",
            "7alt",
            "13",
            "13#11",
            "13b9",
            "13#9",
            "7b9sus",
            "7susadd3",
            "9sus",
            "13sus",
            "7b13sus",
            "11",

            // Alternate Chords:
            // iReal Pro can also display smaller “alternate” chords above the regular chords. All the same rules apply for the format of the chord and to mark them as alternate chords you enclose them in round parenthesis:
            // (Db^7/F)
            "(",
            ")",

            // No Chord: n
            // Adds a N.C. symbol in the chart which makes the player skip harmony and bass for that measure or beats
            "n",

            // Repeat Symbols:
            "x", // This is the “Repeat one measure” % symbol and is usually inserted in the middle of an empty measure:
            "r", // This is the “Repeat the previous two measures” symbol and is usually inserted across two empty measures:

            // Slash:
            // Sometimes we might want to add slash symbol to indicate that we want to repeat the preceding chord:
            // |C7ppF7|
            "p",

            // Chord size:
            // When trying to squeeze many chords in one measure you might want to make them narrower.
            // To do this insert an s in the chord progression and all the following chord symbols will be narrower until a l symbol is encountered that restores the normal size.
            "s",
            "l",

            // Dividers:
            // One or more space characters are usually used to separate chords but sometimes we want to pack many chords in one measure in which case we use the comma , to separate the chords without adding empty cells to the chord progression.
            ",",

            // Empty cell
            //
            " ",
        };
    }
}
