namespace CrisalisModel.CrisalisCore.program
{


    internal class Programe()
    {
        public void InitMemory()
        {
        }
        public static void Main()
        {
            string fileName = "secondCrFile.cr";
            string[] sourceCode;
            Console.WriteLine("Starting the Crisalis Interpreter...");
            try
            {
                if (File.Exists(fileName))
                {
                    Console.WriteLine($"Reading file: {fileName}"); // If the file is inside a folder , you need to provide the relative or absolute path
                    sourceCode = File.ReadAllLines(fileName); // The programe source code not will not work if ther isan empty line at the beginin of the source code

                    Console.WriteLine($"Number of line: {sourceCode.Length}");
                    ProcessCode(sourceCode);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
            }
        }
        private static void ProcessCode(string[] myLine)
        {
            foreach(string line in myLine)
            {
                Lexer lexer = new Lexer(line); // Out of bounds exception here
                var tokens = lexer.Tokenize();
                //ProcessLineStructure(tokens);
                ProcessExpression(tokens);
                VRMemoryDebug.PrintMemory();
            }
        }
        // TO Check the structure of each current line
        /*public static void ProcessLineStructure(List<Token> tokens)  // Better put this in a foreach loop
        {
            TokenType tokenType = TokenType.Identifier;
            int tokensPosition = 0;

            if (tokens[tokensPosition].Type == TokenType.Identifier)
            {

                if (tokens[tokensPosition + 1].Type == TokenType.Assign)
                {

                    if (tokens[tokensPosition + 2].Type == TokenType.Number)
                    {
                        Parser parser = new Parser(tokens);
                        var assignment = parser.ParseAssignment();
                        VRMemory.AddToMemory(assignment);
                    }
                    else if (tokens[tokensPosition + 2].Type == TokenType.Identifier)
                    {
                        Parser parser = new Parser(tokens);
                        var assignmentVar = parser.ParseAssignmentVar();

                        // Check whether to rewrite or add to memory
                        if (VRMemory.memory.ContainsKey(tokens[tokensPosition].Value))
                        {
                            VRMemory.RewriteInMemory(assignmentVar);

                        }
                        else  // I think it have to be in the memory i think,s but i am not sure. 
                        {
                            Console.WriteLine($"Value is NOT difine in memory: Variable '{tokens[tokensPosition + 2].Value}' .");
                            VRMemory.AddToMemory(assignmentVar);
                        }
                    }
                }
            }

        }*/

        public static void ProcessExpression(List<Token> tokens)
        {
            Parser parser = new Parser(tokens);
            parser.ParseAssignment();
        }
    }

    internal struct TokenOrder {
        public Token firtIdentifier;
        public Token secondIdentifier;
        public Token thirdIdentifier;
    }
}