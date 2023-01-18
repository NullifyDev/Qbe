using System;
using System.Text;
using Qbe;

namespace Qbe
{
    public class Lexer
    {
        private int line = 1, curr = 0, start = 0;
        private bool isAtEnd => curr >= src.Length;
        private bool inCommentBlock = false;
        private Dictionary<string, TokenType> reserved = new Dictionary<string, TokenType>();
        private List<Token> tokens = new List<Token>();
        private string currLexeme => src[start..curr];
        private string src;
        Token token = new Token();

        public Lexer(string src)
        {
            this.src = src;
            this.reserved = new Dictionary<string, TokenType>
            {
                ["up"] = Qbe.TokenType.PointerUp,
                ["down"] = Qbe.TokenType.PointerDown,
                ["incr"] = Qbe.TokenType.IncrAddr,
                ["decr"] = Qbe.TokenType.DecrAddr,
                ["getaddrval"] = Qbe.TokenType.GetAddrValue,
                ["getaddrpos"] = Qbe.TokenType.GetAddrPos,
                ["getptrpos"] = Qbe.TokenType.GetPtrPosition,
                ["jmp"] = Qbe.TokenType.JumpToAddr,
                ["out"] = Qbe.TokenType.Output,
                ["in"] = Qbe.TokenType.Input,
                ["func"] = Qbe.TokenType.Function,
            };
        }

        #region Helper functions
        private char Next()
        {
            if (!isAtEnd)
                return src[curr++];
            return '\0';
        }

        private char PeekPrev(int behind = 1)
        {
            if (curr - behind >= src.Length)
                return '\0';
            return src[curr - behind];
        }

        private char Peek()
        {
            if (isAtEnd) return '\0';
            return src[curr];
        }

        private char PeekNext(int ahead = 1)
        {
            if (curr + ahead >= src.Length)
                return '\0';
            return src[curr + ahead];
        }

        private bool Match(char expected)
        {
            if (isAtEnd)
                return false;

            if (src[curr] != expected)
                return false;

            curr++;
            return true;
        }
        #endregion

        private void addToken(TokenType type, object? literal = null)
        {
            tokens.Add(token.Add(type, currLexeme, line, literal));
        }

        #region Scanners
        public List<Token> Scan()
        {
            while (!isAtEnd)
            {
                // We are at the beginning of the next lexeme
                start = curr;
                this.scanToken();
            }

            tokens.Add(token.Add(TokenType.EOF, "", line, null));

            // As a kind of workaround to 01, check if the first token is a Qbe.TokenType.EOL and remove it

            if (tokens[0].Type == Qbe.TokenType.EOL)
            {
                tokens.Remove(tokens[0]);
            }
            return tokens;
        }

        private void GetAddrVal()
        {
            
        }

        private void GetAddrPos()
        {
            
        }

        private void scanNumber()
        {
            while (Char.IsDigit(Peek()))
                Next();

            if (Peek() == '.' && Char.IsDigit(PeekNext()))
            {
                Next();

                while (Char.IsDigit(Peek()))
                    Next();
            }

            var value = src.Substring(start, curr - start);
            addToken(TokenType.NumberLit, Double.Parse(value));
        }
        private void scanChar()
        {
            while (Peek() != '\'' && !isAtEnd)
            {
                if (Peek() == '\n')
                    line++;

                Next();
            }

            if (isAtEnd)
            {
                Utils.Error($"Line {line}: Unterminated character literal");
                return;
            }

            Next();

            string substring = src.Substring(start + 1, curr - start - 2);
            if (substring.Length > 1)
            {
                Utils.Error($"Line {line}: Too many characters in character literal");
                return;
            }

            char value = char.Parse(substring);
            addToken(Qbe.TokenType.CharLit, value);
        }

        private void scanString()
        {
            // TODO: add string interpolation
            while (Peek() != '"' && !isAtEnd)
            {
                if (Peek() == '\n')
                    line++;

                Next();
            }

            if (isAtEnd)
            {
                Utils.Error($"Line {line}: Unterminated string literal");
                return;
            }

            // Closing quote
            Next();

            // Remove the surrounding quotes
            string value = src.Substring(start + 1, curr - start - 2);
            addToken(Qbe.TokenType.StringLit, value);
        }

        private void scanToken()
        {
            char c = Next();
            switch (c)
            {
                case '[': addToken(Qbe.TokenType.LeftBracket); break;
                case ']': addToken(Qbe.TokenType.RightBracket); break;
                case ',': addToken(Qbe.TokenType.Comma); break;
                case '.': addToken(Qbe.TokenType.Dot); break;
                case '+': addToken(Qbe.TokenType.Plus); break;
                case '*': addToken(Qbe.TokenType.Star); break;
                case '=': addToken(Qbe.TokenType.Equals); break;
                case '"': scanString(); break;
                case '\'': scanChar(); break;
                case '\\': // line continuation
                case ' ':
                case '\r':
                case '\t':
                    // Ignore whitespace
                    break;
                case '\n':
                    if (PeekNext(ahead: -2) != '\\')
                    {
                        // FIXME: 01: Kinda works but it still appends Qbe.TokenType.EOL even if the line is actually empty
                        if (Peek() != ' ' && tokens.Count == 0 || Peek() != '\0' || tokens[tokens.Count - 1].Type != Qbe.TokenType.EOL)
                        {
                            addToken(Qbe.TokenType.EOL);
                        }
                        line++;
                    }
                    break;
                case '|':
                    inCommentBlock = false;
                    break;
                case '#':
                    if (Match('>'))
                    {
                        // Multiline comment block
                        inCommentBlock = true;
                    }

                    if (!inCommentBlock)
                    {
                        while (Peek() != '\n' && !isAtEnd) Next();
                        if (Peek() == '\n') Next();
                    }

                    break;
                default:
                    if (!inCommentBlock)
                    {
                        if (Char.IsDigit(c))
                        {
                            scanHex();
                        }
                        else if (Char.IsLetter(c) || c == '_')
                        {
                            scanIdentifier();
                        }
                        else
                        {
                            Utils.Error($"Line {line}: Unexpected character '{c}'");
                        }
                    }
                    else
                    {
                        // Inside a comment block, we ignore everything
                        while (Peek() != '<' && PeekNext() != '#' && !isAtEnd)
                            Next();
                    }
                    break;
            }
        }
        #endregion
    }
}