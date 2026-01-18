using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisalisModel.CrisalisCore
{
    internal class Parser
    {
        private readonly List<Token> _tokens;
        private int _position;
        public Parser(List<Token> tokens)
        {
            _tokens = tokens;
            _position = 0;
        }
        private Token CurrentToken => _tokens[_position];

        private Token Match(TokenType type)
        {
            if (_position < _tokens.Count && CurrentToken.Type == type)
            {
                return _tokens[_position++];
            }
            throw new Exception($"Expected token of type {type} but found {CurrentToken.Type}");
        }
        private Token Peek()
        {
            if(_position >= _tokens.Count)
            {
                return new Token { Type = TokenType.EOF, Value = ";" };
            }

            return CurrentToken;
        }
        public Token TokenStructure(TokenType type)
        {
            if (_position < _tokens.Count && CurrentToken.Type == type)
            {
                return _tokens[_position++];
            }
            return null;
        }

       

        // Rule: assignment -> Identifier '=' (Number | Identifier)
        public Assignment ParseAssignment() // I should maybe use an interface for different assignment types or for difrent token 
        {
            // Get varName
            Token nameToken = Match(TokenType.Identifier);
            string varName = nameToken.Value;

            // Skipe '='
            Match(TokenType.Assign);

            // Get the value
            int value = ParseExpresion();

            // Execute the code by saving it to memory
            VRMemory.SaveRewriteValueToMemory(varName, value);
            Console.WriteLine($"Assigned {value} to {varName}");
            var assignment = new Assignment(varName, value);

            return assignment;
        }

        // Rule : Expresion -> Identifier '=' number + number+
        public int ParseExpresion()
        {
            // Look for the next tokenType
            int firstValue = ParsePrimary();
            int value = 0;

           while (_position < _tokens.Count() && IsOperator(Peek().Type))  // Recursive Descendant this is called
           {
                Token operatorToken = Match(Peek().Type); // Match either + or -
                int nextNumberValue = ParsePrimary();

                // Check the operator
                firstValue = Operate(operatorToken, firstValue, nextNumberValue);
           }
            return firstValue;
        }
        #region Token structure helpers 
        private int Operate(Token operatorToken, int firstValue, int nextNumberValue)
        {
            if (operatorToken.Type == TokenType.Plus)
                return firstValue += nextNumberValue;

            else if (operatorToken.Type == TokenType.Minus)
                return firstValue -= nextNumberValue;

            else if (operatorToken.Type == TokenType.Time)
                return firstValue *= nextNumberValue;

            else if (operatorToken.Type == TokenType.Divide)
                return firstValue /= nextNumberValue;

            else
                throw new Exception($"Expected Identifier or Number after operator {Peek()} : harachterNumber {_position}");
        }
        public bool IsOperator(TokenType type)
        {
            if (type == TokenType.Plus)
                return true;
            if (type == TokenType.Minus)
                return true;
            if (type == TokenType.Time)
                return true;
            if (type == TokenType.Divide)
                return true;

            throw new Exception($"Not an operator. Found instead {type}");
        }
        public int ParsePrimary()
        {
          
            if(Peek().Type == TokenType.Identifier)
            {
                Token identifierToken = Match(TokenType.Identifier);
                return VRMemory.ReadValueFromMemory(identifierToken.Value);
            }
            else if (Peek().Type == TokenType.Number)
            {
                Token numberToken = Match(TokenType.Number);
                return int.Parse(numberToken.Value);
            }
                throw new Exception($"Expected Identifier or Number but found {Peek().Type}");
        }
        #endregion
        //Helper for complex expresion : check the token type
    }

    public struct Assignment
    {
        public Assignment(string variableName, int value)
        {
            VariableName = variableName;
            Value = value;
        }
        public string VariableName;
        public int Value;
    }
}
