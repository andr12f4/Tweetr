using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.IO;


namespace Tweetr.Data
{
    public class JsonFile<T>
    {
        public Dictionary<int, T> ReadJsonFile(string filePath)
        {
            string input = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Dictionary<int, T>>(input);
        }
        public List<T> ReadJsonFileList(string filePath)
        {
            string input = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<List<T>>(input);
        }

        public void WriteJsonFile(Dictionary<int, T> dic, string filePath)
        {
            string output = JsonConvert.SerializeObject(dic, Formatting.Indented);
            File.WriteAllText(filePath, output);
        }
    }
}

