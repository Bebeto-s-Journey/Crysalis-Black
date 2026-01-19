namespace CrisalisModel.CrisalisCore
{
    internal static  class VRMemory
    {
        public static Dictionary<string, int> memory = new Dictionary<string, int>();

        public static void AddToMemory(Assignment values)
        {
            memory[values.VariableName] = values.Value;
        }

        public static int GetMemory(string key)
        {
            int value;  
            try
            {
                memory.TryGetValue(key, out value);
                return value;
            }catch(Exception ex)
            {
                throw new Exception("Assignment not found in memory", ex);
            }
        }

        public static void RewriteInMemory(Assignment values)
        {
            try
            {
                memory[values.VariableName] = values.Value;
            }
            catch (Exception ex)
            {
                throw new Exception("Assignment not found in memory", ex);
            }
        }
        public static int ReadValueFromMemory(string key)
        {
            if (ExistsInMemory(key) == false)
            {
                throw new Exception($"Variable '{key}' not found in memory.");
            }
            int value = VRMemory.GetMemory(key);
            Console.WriteLine($"Read from memory: {key} = {value}");
            return value;
        }
        public static void SaveRewriteValueToMemory(Assignment assignment)
        {
            if (ExistsInMemory(assignment.VariableName))
            {
                RewriteInMemory(new Assignment(assignment.VariableName, assignment.Value));
                Console.WriteLine($"Rewrite to memory: adreasse {assignment.VariableName} =  value : {assignment.Value}");
            }
            else
            {
                AddToMemory(new Assignment(assignment.VariableName, assignment.Value));
                Console.WriteLine($"Saved to memory: {assignment.VariableName} = {assignment.Value}");
            }
        }


        public static bool ExistsInMemory(string key)
        {
            return memory.ContainsKey(key);
        }
        public static void ClearMemory()
        {
            memory.Clear();
        }
    }


    // DEBUG PURPOSES ONLY
    
    public static class VRMemoryDebug
    {
        public static void PrintMemory()
        {
            Console.WriteLine("Current Memory State:");
            foreach (var kvp in VRMemory.memory)
            {
                Console.WriteLine($"{kvp.Key} = {kvp.Value}");
            }
        }
    }


}
