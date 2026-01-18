namespace CrisalisModel.CrisalisCore
{
    internal enum TokenType
    {
        Identifier, Number, Assign, Plus, Minus, Time, Divide, Coma, OpenP, CloseP, EOF
    }

    internal class Token
    {
        public TokenType Type { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"Token(Type: {Type}, '{Value}')";
        }
    }
}
