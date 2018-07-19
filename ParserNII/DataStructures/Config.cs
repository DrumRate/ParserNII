using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace ParserNII.DataStructures
{
    public class Config
    {
        public List<ConfigElement> binFileParams { get; set; }
        public List<ConfigElement> datFileParams { get; set; }
        public Dictionary<int, string> trainNames { get; set; }

        private static Config instance;

        public static Config Instance => instance ?? (instance = JsonConvert.DeserializeObject<Config>(File.ReadAllText(Environment.CurrentDirectory + "/config.json")));
    }
}