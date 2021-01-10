using Core;
using System;
using Xunit;

namespace Test
{
    public class DecoderTests
    {
        [Fact]
        public void Decode_Skips_Prefix()
        {
            var sut = new Decoder();

            Assert.Equal("irealbook://Null=Stevens Dave==Medium Up Swing=C==Z", sut.Decode("irealbook://Null=Stevens Dave==Medium Up Swing=C==1r34LbKcu7Z"));
        }
    }
}
