using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrisalisModel.CrisalisCore.bad
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
            if (_position >= _tokens.Count)
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

        #region Token structure helpers 
        public bool IsAssignment(List<Token> allTokensInALin)
        {
            foreach (var token in allTokensInALin)
            {
                if (token.Type == TokenType.Assign)
                {
                    return true;
                }
            }
            return false;
        }

        public bool IsAssignmentVar(List<Token> allTokensInALin)
        {
            foreach (var token in allTokensInALin)
            {
                if (token.Type == TokenType.Assign)
                {
                    int assignIndex = allTokensInALin.IndexOf(token);
                    if (assignIndex + 1 < allTokensInALin.Count &&
                        allTokensInALin[assignIndex + 1].Type == TokenType.Identifier)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

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

            return false;
        }
        #endregion

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
            int firstValue = ParseParantesise();

            while (_position < _tokens.Count() && IsOperator(Peek().Type))  // Recursive Descendant this is called
            {
                Token operatorToken = Match(Peek().Type); // Skip either + or - or / or *
                int nextNumberValue = ParseParantesise();

                int totaleValue = Operate(operatorToken, firstValue, nextNumberValue);
                return totaleValue;
            }
            return firstValue;
        }

        // Rule : Coplex Expresion -> Identifier '=' Identifier || Number + Identifier || Number+
        public int ParseParantesise()
        {
            Console.WriteLine($"Parantesise enter Type : {Peek()} pos : {_position}");

            if (Peek().Type == TokenType.OpenP)
            {

                Token openPToken = Match(TokenType.OpenP); // Skip '('
                Console.WriteLine($"Enter Parantesise : {Peek()} pos : {_position}");
                int firstValue = ParsePrimary();
                Console.WriteLine($"First Primary parsed: {Peek()} pos : {_position}");
                int totalValue = 0;
                while (_position < _tokens.Count && Peek().Type != TokenType.CloseP)
                {

                    Token _operatorToken = Match(Peek().Type); // either + or - or / or * Skip operator
                    int nextNumberValue = ParsePrimary();
                    totalValue += Operate(_operatorToken, firstValue, nextNumberValue);
                    return totalValue;
                }
                Console.WriteLine($"Exiting Parantesise : {Peek()} pos : {_position}");

                Match(Peek().Type); // Skip ')' 
                return firstValue + ParseExpresion();
            }
            Console.WriteLine($"Return primary parse: {Peek()} pos : {_position}");
            return ParsePrimary();
        }

        //Helper for complex expresion : check the token type
        public int ParsePrimary()
        {

            if (Peek().Type == TokenType.Identifier)
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

        #region RULES 

        // Rule: Assignment -> Identifier '=' Identifier
        // To reasign value i you need to rewrite the value from memory
        public Assignment ParseAssignmentVar()  // I can maybe juste check for an '=' sign to know if its an assignment
        {
            // Get varName
            Token tokenName = Match(TokenType.Identifier);
            string varName = tokenName.Value;

            // Skipe '='
            Match(TokenType.Assign);

            // Get the value
            Token tokenValue = Match(TokenType.Identifier);

            int value = VRMemory.ReadValueFromMemory(tokenValue.Value);

            Console.WriteLine($"Assigned identifier : {tokenValue.Value} value ({value}) to {varName}");

            return new Assignment(varName, value);
        }

        // Rule: Assignment -> Identifier '=' Identifier ( '+' | '-' ) Identifier
        public Assignment ParseAssigneSome()
        {
            Token rokenName = Match(TokenType.Identifier);
            string varName = rokenName.Value;

            Match(TokenType.Assign);

            Token tokenValue1 = Match(TokenType.Identifier);

            Token operatorToken = Match(TokenType.Plus); // either + or -

            Token tokenValue2 = Match(TokenType.Identifier);

            int totalValue = VRMemory.ReadValueFromMemory(tokenValue1.Value) + VRMemory.ReadValueFromMemory(tokenValue2.Value);

            return new Assignment(varName, totalValue);
        }
        #endregion

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

