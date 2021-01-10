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
        public void Parse_Returns_ChordProgression()
        {
            var sut = new SongParser();

            var url = "irealbook://Song Title=LastName FirstName=Style=Ab=n=T44*A{C^7 |A-7 |D-9 |G7#5 }";

            var song = sut.Parse(url);
            Assert.Equal("T44", song.ChordProgression.Symbols[0]);
            Assert.Equal("*A", song.ChordProgression.Symbols[1]);
            Assert.Equal("{", song.ChordProgression.Symbols[2]);
            Assert.Equal("C", song.ChordProgression.Symbols[3]);
            Assert.Equal("^7", song.ChordProgression.Symbols[4]);
            Assert.Equal(" ", song.ChordProgression.Symbols[5]);
            Assert.Equal("|", song.ChordProgression.Symbols[6]);
            Assert.Equal("A", song.ChordProgression.Symbols[7]);
            Assert.Equal("-7", song.ChordProgression.Symbols[8]);
            Assert.Equal(" ", song.ChordProgression.Symbols[9]);
            Assert.Equal("|", song.ChordProgression.Symbols[10]);
            Assert.Equal("D", song.ChordProgression.Symbols[11]);
            Assert.Equal("-9", song.ChordProgression.Symbols[12]);
            Assert.Equal(" ", song.ChordProgression.Symbols[13]);
            Assert.Equal("|", song.ChordProgression.Symbols[14]);
            Assert.Equal("G", song.ChordProgression.Symbols[15]);
            Assert.Equal("7#5", song.ChordProgression.Symbols[16]);
            Assert.Equal(" ", song.ChordProgression.Symbols[17]);
            Assert.Equal("}", song.ChordProgression.Symbols[18]);
            Assert.Equal(19, song.ChordProgression.Symbols.Length);
        }

        //[Fact]
        public void Parse_26_2()
        {
            var sut = new SongParser();

            var url = "irealbook://26-2=Coltrane John==Medium Up Swing=F==1r34LbKcu7ZL7bD4F^7 ZL7F 7-CZL7C 7A^ZL7E 7^bDZL7bABb^7 4T[A* 7^AZA7LZD^bDZL7bA 7^F[A]* 7C 7-GZL7G 7-7 E7L 7^bGC[B*]-7 F7FZL7C 7^AZL7E ^7bDZL7bA 7^bBZL^7XyQCZL7C7^bD|LZE-7A|QyX7-bE|QyX7b^BZL7F 7^DZL7A b7XyQ7F 7-BZL7F-7 C7L7C 7^AZL7E 7^DbZL7bA 7^F[A*] ZC-7 G|QyXb^7 Ab7LZDb^7 E7LZA^7 C7LZF^7   Z";

            var song = sut.Parse(url);
            Assert.Equal("T44", song.ChordProgression.Symbols[0]);
            Assert.Equal("*A", song.ChordProgression.Symbols[1]);
        }
    }
}
