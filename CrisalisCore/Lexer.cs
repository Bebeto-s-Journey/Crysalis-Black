using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisalisModel.CrisalisCore
{
    internal class Lexer
    {
        private readonly string _input;  // Sit on the Heap 
        private int _position; // Sit on the Stack
        private Dictionary<string, TokenType> _keyWord = new()
        {
            { "Wanema", TokenType.PrintStmt }

        };
        public Lexer(string input)
        {
            _input = input;
            _position = 0;
        }


        public List<Token> Tokenize()   /// The list Sit on the Heap
        {
            var tokens = new List<Token>();
            while (_position < _input.Length)
            {
                char current = _input[_position];
                if (char.IsWhiteSpace(current))
                {
                    _position++;
                    continue;
                }
                else if (char.IsLetter(current))
                {
                    string letter = ReadWhile(char.IsLetter);
                    if (_keyWord.TryGetValue(letter, out TokenType type))
                    {
                        tokens.Add(new Token { Type = type, Value = letter });
                        continue;
                    }
                    else 
                        tokens.Add(new Token { Type = TokenType.Identifier, Value =  letter});
                    continue;
                }
                else if (char.IsDigit(current))
                {
                    tokens.Add(new Token { Type = TokenType.Number, Value = ReadWhile(char.IsDigit) });
                    continue;

                }
                else
                {
                    switch (current)
                    {
                        case '=':
                            tokens.Add(new Token { Type = TokenType.Assign, Value = "=" });
                            _position++;
                            break;
                        case '+':
                            tokens.Add(new Token { Type = TokenType.Plus, Value = "+" });
                            _position++;
                            break;
                        case '-':
                            tokens.Add(new Token { Type = TokenType.Minus, Value = "-" });
                            _position++;
                            break;
                        case '/':
                            tokens.Add(new Token { Type = TokenType.Divide, Value = "/" });
                            _position++;
                            break;
                        case '*':
                            tokens.Add(new Token { Type = TokenType.Time, Value = "*" });
                            _position++;
                            break;
                        case ';':
                            tokens.Add(new Token { Type = TokenType.EOF, Value = ";" });
                            _position++;
                            break;
                        case ',':
                            tokens.Add(new Token { Type = TokenType.Coma, Value = "," });
                            _position++;
                            break;
                        case '(':
                            tokens.Add(new Token { Type = TokenType.OpenP, Value = "(" });
                            _position++;
                            break;
                        case ')':
                            tokens.Add(new Token { Type = TokenType.CloseP, Value = ")" });
                            _position++;
                            break;
                        default:
                            throw new Exception($"Unexpected character: {current}");
                    }
                }
            }
            return tokens;
        }
        private string ReadWhile(Func<char, bool> condition)
        {
            int start = _position;
            while (_position < _input.Length && condition(_input[_position]))
            {
                _position++;
            }

            return _input.Substring(start, _position - start);
        }
    }
}
