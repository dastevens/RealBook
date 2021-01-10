using Core;
using System;
using Xunit;

namespace Test
{
    public class SongParserTests
    {
        [Fact]
        public void Parse_Throws_WhenSchemeIsInvalid()
        {
            var sut = new SongParser();

            var url = "xxx://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            Assert.Throws<Exception>(() => sut.Parse(url));
        }

        [Fact]
        public void Parse_Returns_SongTitle()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal("Song Title", song.SongTitle);
        }

        [Fact]
        public void Parse_Returns_Composer()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal("LastName FirstName", song.Composer);
        }

        [Fact]
        public void Parse_Returns_Style()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal("Style", song.Style);
        }

        [Fact]
        public void Parse_Returns_KeySignature()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal(KeySignature.AFlatMajor, song.KeySignature);
        }

        [Fact]
        public void Parse_Returns_SongChart()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal(new Token{ Type = TokenType.TimeSignature, Symbol = "T44" }, song.SongChart.Tokens[0]);
            Assert.Equal(new Token{ Type = TokenType.RehearsalMark, Symbol = "*A" }, song.SongChart.Tokens[1]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "{" }, song.SongChart.Tokens[2]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "C" }, song.SongChart.Tokens[3]);
            Assert.Equal(new Token{ Type = TokenType.ChordQuality, Symbol = "^7" }, song.SongChart.Tokens[4]);
            Assert.Equal(new Token{ Type = TokenType.EmptyCell, Symbol = " "}, song.SongChart.Tokens[5]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "|"}, song.SongChart.Tokens[6]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "A"}, song.SongChart.Tokens[7]);
            Assert.Equal(new Token{ Type = TokenType.ChordQuality, Symbol = "-7"}, song.SongChart.Tokens[8]);
            Assert.Equal(new Token{ Type = TokenType.EmptyCell, Symbol = " "}, song.SongChart.Tokens[9]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "|"}, song.SongChart.Tokens[10]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "D"}, song.SongChart.Tokens[11]);
            Assert.Equal(new Token{ Type = TokenType.ChordQuality, Symbol = "-9"}, song.SongChart.Tokens[12]);
            Assert.Equal(new Token{ Type = TokenType.EmptyCell, Symbol = " "}, song.SongChart.Tokens[13]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "|"}, song.SongChart.Tokens[14]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "G"}, song.SongChart.Tokens[15]);
            Assert.Equal(new Token{ Type = TokenType.ChordQuality, Symbol = "7#5"}, song.SongChart.Tokens[16]);
            Assert.Equal(new Token{ Type = TokenType.EmptyCell, Symbol = " "}, song.SongChart.Tokens[17]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "}"}, song.SongChart.Tokens[18]);
            Assert.Equal(19, song.SongChart.Tokens.Length);
        }
    }
}
