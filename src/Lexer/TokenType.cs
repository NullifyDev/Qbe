namespace Qbe;

public class TokenType
{
    public enum TokenType
    {
        /* StringLits and CharLits will be handled as ASCII which each
        character will be in its own dec val, contained in addresses */
        StringLit, NumberLit, Identifier, CharLit, 
        
        // Pointer
        PointerUp, PointerDown, GetPtrPosition, JumpToAddr,
        
        // Address 
        IncrAddr, DecrAddr, GetAddrValue,
        
        Output, Input, Function,

        EOL, EOF
    }

    public class Token
    {
        public TokenType Type;
        public int Line;
        public object Lexeme = new Object();
        public object? Literal;

        public Token Add(TokenType type, object lexeme, int line, object? literal = null)
        {
            Token token = new();
            token.Type = type;
            token.Lexeme = lexeme;
            token.Line = line;
            token.Literal = literal;
            return token;
        }
    }
}