using Core;
using System;
using System.Linq;
using Xunit;
using Xunit.Abstractions;

namespace Test
{
    public class SongParserTests
    {
        private ITestOutputHelper logger;

        public SongParserTests(ITestOutputHelper logger)
        {
            this.logger = logger;
        }

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
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "{" }, song.SongChart.Tokens[1]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "C^7" }, song.SongChart.Tokens[2]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "A-7"}, song.SongChart.Tokens[3]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "D-9"}, song.SongChart.Tokens[4]);
            Assert.Equal(new Token{ Type = TokenType.Chord, Symbol = "G7#5"}, song.SongChart.Tokens[5]);
            Assert.Equal(new Token{ Type = TokenType.BarLine, Symbol = "}"}, song.SongChart.Tokens[6]);
            Assert.Equal(7, song.SongChart.Tokens.Length);
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

        [Theory]
        [InlineData(0, "(C)")]
        [InlineData(1, "C(F#)")]
        [InlineData(2, "C(F#)F")]
        [InlineData(2, "C(F#)F(B)")]
        [InlineData(3, "C(F#)F(B)C")]
        public void Parse_Ignores_AlternateChords(int expectedTokenCount, string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            Assert.Equal(expectedTokenCount, song.SongChart.Tokens.Length);
        }

        [Theory]
        [InlineData(TokenType.BarLine, "{")]
        [InlineData(TokenType.BarLine, "}")]
        [InlineData(TokenType.TimeSignature, "T44")]
        [InlineData(TokenType.TimeSignature, "T34")]
        [InlineData(TokenType.TimeSignature, "T24")]
        [InlineData(TokenType.TimeSignature, "T54")]
        [InlineData(TokenType.TimeSignature, "T64")]
        [InlineData(TokenType.TimeSignature, "T74")]
        [InlineData(TokenType.TimeSignature, "T22")]
        [InlineData(TokenType.TimeSignature, "T32")]
        [InlineData(TokenType.TimeSignature, "T58")]
        [InlineData(TokenType.TimeSignature, "T68")]
        [InlineData(TokenType.TimeSignature, "T78")]
        [InlineData(TokenType.TimeSignature, "T98")]
        [InlineData(TokenType.TimeSignature, "T12")]
        [InlineData(TokenType.RehearsalMark, "S")]
        [InlineData(TokenType.RehearsalMark, "Q")]
        [InlineData(TokenType.Ending, "N1")]
        [InlineData(TokenType.Ending, "N2")]
        [InlineData(TokenType.Ending, "N3")]
        [InlineData(TokenType.Ending, "N0")]
        [InlineData(TokenType.StaffText, "<D.C. al Coda>")]
        [InlineData(TokenType.StaffText, "<D.C. al Fine>")]
        [InlineData(TokenType.StaffText, "<D.C. al 1st End.>")]
        [InlineData(TokenType.StaffText, "<D.C. al 2nd End.>")]
        [InlineData(TokenType.StaffText, "<D.C. al 3rd End.>")]
        [InlineData(TokenType.StaffText, "<D.S. al Coda>")]
        [InlineData(TokenType.StaffText, "<D.S. al Fine>")]
        [InlineData(TokenType.StaffText, "<D.S. al 1st End.>")]
        [InlineData(TokenType.StaffText, "<D.S. al 2nd End.>")]
        [InlineData(TokenType.StaffText, "<D.S. al 3rd End.>")]
        [InlineData(TokenType.StaffText, "<Fine>")]
        [InlineData(TokenType.Repeat, "<1x>")]
        [InlineData(TokenType.Repeat, "<2x>")]
        [InlineData(TokenType.Repeat, "<3x>")]
        [InlineData(TokenType.Repeat, "<4x>")]
        [InlineData(TokenType.Repeat, "<5x>")]
        [InlineData(TokenType.Repeat, "<6x>")]
        [InlineData(TokenType.Repeat, "<7x>")]
        [InlineData(TokenType.Repeat, "<8x>")]
        [InlineData(TokenType.RepeatSymbol, "x")]
        [InlineData(TokenType.RepeatSymbol, "r")]
        [InlineData(TokenType.Chord, "C")] 
        [InlineData(TokenType.Chord, "C#")]
        [InlineData(TokenType.Chord, "Db")]
        [InlineData(TokenType.Chord, "D")]
        [InlineData(TokenType.Chord, "D#")]
        [InlineData(TokenType.Chord, "Eb")]
        [InlineData(TokenType.Chord, "E")]
        [InlineData(TokenType.Chord, "F")]
        [InlineData(TokenType.Chord, "F#")]
        [InlineData(TokenType.Chord, "Gb")]
        [InlineData(TokenType.Chord, "G")]
        [InlineData(TokenType.Chord, "G#")]
        [InlineData(TokenType.Chord, "Ab")]
        [InlineData(TokenType.Chord, "A")]
        [InlineData(TokenType.Chord, "A#")]
        [InlineData(TokenType.Chord, "Bb")]
        [InlineData(TokenType.Chord, "B")]
        [InlineData(TokenType.Chord, "C5")]
        [InlineData(TokenType.Chord, "C2")]
        [InlineData(TokenType.Chord, "Cadd9")]
        [InlineData(TokenType.Chord, "C+")]
        [InlineData(TokenType.Chord, "Co")]
        [InlineData(TokenType.Chord, "Ch")]
        [InlineData(TokenType.Chord, "Csus")]
        [InlineData(TokenType.Chord, "C^")]
        [InlineData(TokenType.Chord, "C-")]
        [InlineData(TokenType.Chord, "C^7")]
        [InlineData(TokenType.Chord, "C-7")]
        [InlineData(TokenType.Chord, "C7")]
        [InlineData(TokenType.Chord, "C7sus")]
        [InlineData(TokenType.Chord, "Ch7")]
        [InlineData(TokenType.Chord, "Co7")]
        [InlineData(TokenType.Chord, "C^9")]
        [InlineData(TokenType.Chord, "C^13")]
        [InlineData(TokenType.Chord, "C6")]
        [InlineData(TokenType.Chord, "C69")]
        [InlineData(TokenType.Chord, "C^7#11")]
        [InlineData(TokenType.Chord, "C^9#11")]
        [InlineData(TokenType.Chord, "C^7#5")]
        [InlineData(TokenType.Chord, "C-6")]
        [InlineData(TokenType.Chord, "C-69")]
        [InlineData(TokenType.Chord, "C-^7")]
        [InlineData(TokenType.Chord, "C-^9")]
        [InlineData(TokenType.Chord, "C-9")]
        [InlineData(TokenType.Chord, "C-11")]
        [InlineData(TokenType.Chord, "C-7b5")]
        [InlineData(TokenType.Chord, "Ch9")]
        [InlineData(TokenType.Chord, "C-b6")]
        [InlineData(TokenType.Chord, "C-#5")]
        [InlineData(TokenType.Chord, "C9")]
        [InlineData(TokenType.Chord, "C7b9")]
        [InlineData(TokenType.Chord, "C7#9")]
        [InlineData(TokenType.Chord, "C7#11")]
        [InlineData(TokenType.Chord, "C7b5")]
        [InlineData(TokenType.Chord, "C7#5")]
        [InlineData(TokenType.Chord, "C9#11")]
        [InlineData(TokenType.Chord, "C9b5")]
        [InlineData(TokenType.Chord, "C9#5")]
        [InlineData(TokenType.Chord, "C7b13")]
        [InlineData(TokenType.Chord, "C7#9#5")]
        [InlineData(TokenType.Chord, "C7#9b5")]
        [InlineData(TokenType.Chord, "C7#9#11")]
        [InlineData(TokenType.Chord, "C7b9#11")]
        [InlineData(TokenType.Chord, "C7b9b5")]
        [InlineData(TokenType.Chord, "C7b9#5")]
        [InlineData(TokenType.Chord, "C7b9#9")]
        [InlineData(TokenType.Chord, "C7b9b13")]
        [InlineData(TokenType.Chord, "C7alt")]
        [InlineData(TokenType.Chord, "C13")]
        [InlineData(TokenType.Chord, "C13#11")]
        [InlineData(TokenType.Chord, "C13b9")]
        [InlineData(TokenType.Chord, "C13#9")]
        [InlineData(TokenType.Chord, "C7b9sus")]
        [InlineData(TokenType.Chord, "C7susadd3")]
        [InlineData(TokenType.Chord, "C9sus")]
        [InlineData(TokenType.Chord, "C13sus")]
        [InlineData(TokenType.Chord, "C7b13sus")]
        [InlineData(TokenType.Chord, "C11")]
        [InlineData(TokenType.Chord, "C/E")]
        [InlineData(TokenType.Chord, "C-7/Bb")]
        [InlineData(TokenType.NoChord, "n")]
        public void Parse_Tokenizes_Symbol(TokenType expectedTokenType, string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            logger.WriteLine($"{song.SongChart.Tokens.Length}");
            song.SongChart.Tokens.ToList().ForEach(token => logger.WriteLine($"{token.Type}{token.Symbol}"));

            Assert.Single(song.SongChart.Tokens);
            Assert.Equal(expectedTokenType, song.SongChart.Tokens.Single().Type);
            Assert.Equal(songChartText, song.SongChart.Tokens.Single().Symbol);
        }

        [Theory]
        [InlineData("|")]
        [InlineData("[")]
        [InlineData("]")]
        [InlineData("Z")]
        [InlineData("*A")]
        [InlineData("*B")]
        [InlineData("*C")]
        [InlineData("*D")]
        [InlineData("*V")]
        [InlineData("*i")]
        [InlineData("f")]
        [InlineData("<Remove this text>")]
        [InlineData("Y")]
        [InlineData("YY")]
        [InlineData("YYY")]
        [InlineData("p")]
        [InlineData("s")]
        [InlineData("l")]
        [InlineData(",")]
        [InlineData("(C)")]
        public void Parse_Ignores_Symbol(string songChartText)
        {
            var sut = new SongParser();

            var url = $"irealbook://Song Title=LastName FirstName=Style=Ab=n={songChartText}";

            var song = sut.Parse(url);

            Assert.Empty(song.SongChart.Tokens);
        }
    }
}
