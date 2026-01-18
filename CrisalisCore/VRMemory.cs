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
            if (VRMemory.ExistsInMemory(key) == false)
            {
                throw new Exception($"Variable '{key}' not found in memory.");
            }
            int value = VRMemory.GetMemory(key);
            Console.WriteLine($"Read from memory: {key} = {value}");
            return value;
        }
        public static void SaveRewriteValueToMemory(string key, int value)
        {
            if (VRMemory.ExistsInMemory(key))
            {
                VRMemory.RewriteInMemory(new Assignment(key, value));
                Console.WriteLine($"Rewrite to memory: adreasse {key} =  value : {value}");
            }
            else
            {
                VRMemory.AddToMemory(new Assignment(key, value));
                Console.WriteLine($"Saved to memory: {key} = {value}");
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
