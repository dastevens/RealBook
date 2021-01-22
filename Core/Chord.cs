using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Core
{
    public class Chord
    {
        private static Dictionary<string, Note> Notes = new Dictionary<string, Note>
        {
            { "A", Note.A },
            { "A#", Note.ASharp },
            { "Bb", Note.BFlat },
            { "B", Note.B },
            { "C", Note.C },
            { "C#", Note.CSharp },
            { "Db", Note.DFlat },
            { "D", Note.D },
            { "D#", Note.DSharp },
            { "Eb", Note.EFlat },
            { "E", Note.E },
            { "F", Note.F },
            { "F#", Note.FSharp },
            { "Gb", Note.GFlat },
            { "G", Note.G },
            { "G#", Note.GSharp },
            { "Ab", Note.AFlat },
        };

        static Dictionary<Note, int> NoteDegrees = new Dictionary<Note, int>
        {
            { Note.A, 0 },
            { Note.ASharp, 1 },
            { Note.BFlat, 1 },
            { Note.B, 2 },
            { Note.C, 3 },
            { Note.CSharp, 4 },
            { Note.DFlat, 4 },
            { Note.D, 5 },
            { Note.DSharp, 6 },
            { Note.EFlat, 6 },
            { Note.E, 7 },
            { Note.F, 8 },
            { Note.FSharp, 9 },
            { Note.GFlat, 9 },
            { Note.G, 10 },
            { Note.GSharp, 11 },
            { Note.AFlat, 11 },
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

        public static Chord FromSymbol(string symbol)
        {
            var match = Regex.Match(symbol, "^([ABCDEFG][b#]?)([^/]*)?(/([ABCDEFG][b#]?))?$");

            switch (match.Groups.Count)
            {
                case 5:
                    return new Chord
                    {
                        Root = Notes[match.Groups[1].Value],
                        Quality = match.Groups[2].Value,
                        Inversion = string.IsNullOrWhiteSpace(match.Groups[4].Value) ? Notes[match.Groups[1].Value] : Notes[match.Groups[4].Value],
                    };
                default:
                    throw new ArgumentException($"Failed to parse symbol {symbol}", nameof(symbol));
            };
        }

        public int Degree(KeySignature keySignature)
        {
            var keySignatureDegree = KeySignatureDegrees[keySignature];
            var chordDegree = NoteDegrees[Root];
            return (chordDegree - keySignatureDegree + 12) % 12;
        }

        public Note Root {get;set;}
        public string Quality { get; set; }
        public Note Inversion { get; set; }
    }
}