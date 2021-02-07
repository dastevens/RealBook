using Core;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class PlayerTests
    {
        private ITestOutputHelper logger;

        public PlayerTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

        private static Token ChordToken(string symbol) => new Token { Symbol = symbol, Type = TokenType.Chord };
        private static Token ClosingRepeatBarLine => new Token { Symbol = "}", Type = TokenType.BarLine };
        private static Token OpeningRepeatBarLine => new Token { Symbol = "{", Type = TokenType.BarLine };
 
        [Fact]
        public void Play_LinearChart()
        {
            var song = new Song
            {
                KeySignature = KeySignature.CMajor,
                SongChart = new SongChart
                {
                    Tokens = new[]
                    {
                        ChordToken("C"),
                        ChordToken("F"),
                        ChordToken("G"),
                        ChordToken("C"),
                    },
                }                
            };

            var playedChords = Player.Play(song);

            Assert.Equal(4, playedChords.Length);
            Assert.Equal(Note.C, playedChords[0].Root);
            Assert.Equal(Note.F, playedChords[1].Root);
            Assert.Equal(Note.G, playedChords[2].Root);
            Assert.Equal(Note.C, playedChords[3].Root);
            Assert.All(playedChords, playedChord => Assert.Equal(string.Empty, playedChord.Quality));
        }
 
 
        [Fact]
        public void Play_RepeatedSection()
        {
            var song = new Song
            {
                KeySignature = KeySignature.CMajor,
                SongChart = new SongChart
                {
                    Tokens = new[]
                    {
                        ChordToken("C"),
                        OpeningRepeatBarLine,
                        ChordToken("F"),
                        ChordToken("G"),
                        ClosingRepeatBarLine,
                        ChordToken("C"),
                    },
                }                
            };

            var playedChords = Player.Play(song);

            Assert.Equal(6, playedChords.Length);
            Assert.Equal(Note.C, playedChords[0].Root);
            Assert.Equal(Note.F, playedChords[1].Root);
            Assert.Equal(Note.G, playedChords[2].Root);
            Assert.Equal(Note.F, playedChords[3].Root);
            Assert.Equal(Note.G, playedChords[4].Root);
            Assert.Equal(Note.C, playedChords[5].Root);
            Assert.All(playedChords, playedChord => Assert.Equal(string.Empty, playedChord.Quality));
        }
    }
}