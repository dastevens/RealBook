using System;
using System.Collections.Generic;
using System.Linq;

namespace Core
{
    public class Player
    {
        public static Chord[] Play(Song song)
        {
            var player = new Player(song.SongChart.Tokens);
            return player.Enumerate().ToArray();
        }

        Player(Token[] tokens)
        {
            this.tokens = tokens;
        }

        private bool Completed;
        private Token[] tokens;
        private int tokenIndex = 0;
        private Token CurrentToken => tokens[tokenIndex];
        private TokenType CurrentType => CurrentToken.Type;
        private string CurrentSymbol => CurrentToken.Symbol;
        private bool Forward()
        {
            if (tokenIndex < tokens.Length - 1)
            {
                tokenIndex++;
                return true;
            }
            else if (tokenIndex == tokens.Length - 1)
            {
                Completed = true;
                return false;
            }
            else
            {
                return false;
            }
        }
        private bool Backward()
        {
            if (tokenIndex > 1)
            {
                tokenIndex--;
                return true;
            }
            else
            {
                return false;
            }
        }

        private void RemoveCurrent()
        {
            tokens = tokens.Select((token, index) => index != tokenIndex ? token : new Token{Type = TokenType.Unknown, Symbol = token.Symbol}).ToArray();
        }

        private bool ScanBackward(Func<Token, bool> predicate)
        {
            while (!predicate(CurrentToken))
            {
                if (!Backward())
                {
                    return false;
                }
            }
            return true;
        }

        private bool ScanForward(Func<Token, bool> predicate)
        {
            while (!predicate(CurrentToken))
            {
                if (!Forward())
                {
                    return false;
                }
            }
            return true;
        }
        
        private IEnumerable<Chord> Enumerate()
        {
            while (!Completed)
            {
                switch (CurrentToken.Type)
                {
                    case TokenType.Chord:
                        yield return GetChord(CurrentToken);
                        break;
                    case TokenType.BarLine:
                        if (CurrentSymbol == "}")
                        {
                            RemoveCurrent();
                            ScanBackward(token => token.Type == TokenType.BarLine && token.Symbol == "{");
                            RemoveCurrent();
                        }
                        break;
                    case TokenType.TimeSignature:
                    case TokenType.Unknown:
                        break;
                    default:
                        yield break;
                }
                if (!Forward())
                {
                    yield break;
                }
            }
        }

        static Chord GetChord(Token chordToken)
        {
            return Chord.FromSymbol(chordToken.Symbol);
        }
    }
}