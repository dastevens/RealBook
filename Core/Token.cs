using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class Token
    {
        public string Symbol { get; set; } = string.Empty;
        public TokenType Type { get; set; }

        public override int GetHashCode()
        {
            return Symbol.GetHashCode() & Type.GetHashCode();
        }

        public override bool Equals(object other)
        {
            if (other is Token otherToken)
            {
                return otherToken.Type == Type && otherToken.Symbol == Symbol;
            }
            else
            {
                return false;
            }
        }
    }
}
