using Core;
using System;
using System.Linq;
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

        //[Fact]
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

        [Theory]
        [InlineData(1, "<D.C. al Coda>")]
        [InlineData(1, "<D.C. al Fine>")]
        [InlineData(1, "<D.C. al 1st End.>")]
        [InlineData(1, "<D.C. al 2nd End.>")]
        [InlineData(1, "<D.C. al 3rd End.>")]
        [InlineData(1, "<D.S. al Coda>")]
        [InlineData(1, "<D.S. al Fine>")]
        [InlineData(1, "<D.S. al 1st End.>")]
        [InlineData(1, "<D.S. al 2nd End.>")]
        [InlineData(1, "<D.S. al 3rd End.>")]
        [InlineData(1, "<Fine>")]
        [InlineData(0, "<Remove this text>")]
        public void Parse_Ignores_StaffText(int expectedTokenCount, string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            Assert.Equal(expectedTokenCount, song.SongChart.Tokens.Length);
        }

        [Fact]
        public void Parse_Ignores_AlternateChords()
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n=(C))";

            var song = sut.Parse(url);

            Assert.Empty(song.SongChart.Tokens);
        }

        [Theory]
        [InlineData(0, "|")] // single bar line
        [InlineData(0, "[")] // opening double bar line
        [InlineData(0, "]")] // closing double bar line
        [InlineData(1, "{")] // opening repeat bar line
        [InlineData(1, "}")] // closing repeat bar line
        [InlineData(0, "Z")] // Final thick double bar line
        public void Parse_Tokenizes_Barlines(int expectedTokenCount, string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            Assert.Equal(expectedTokenCount, song.SongChart.Tokens.Length);
            if (expectedTokenCount == 1)
            {
                Assert.Equal(TokenType.BarLine, song.SongChart.Tokens.Single().Type);
            }
        }

        [Theory]
        [InlineData("T44")]
        [InlineData("T34")]
        [InlineData("T24")]
        [InlineData("T54")]
        [InlineData("T64")]
        [InlineData("T74")]
        [InlineData("T22")]
        [InlineData("T32")]
        [InlineData("T58")]
        [InlineData("T68")]
        [InlineData("T78")]
        [InlineData("T98")]
        [InlineData("T12")]
        public void Parse_Tokenizes_TimeSignature(string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            Assert.Single(song.SongChart.Tokens);
            Assert.Equal(TokenType.TimeSignature, song.SongChart.Tokens.Single().Type);
        }
    }
}
