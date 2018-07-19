namespace ParserNII.DataStructures
{
    public class ConfigElement
    {
        public int number { get; set; }
        public string name { get; set; }
        public string shortname { get; set; }
        public string measure { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public string type { get; set; }
        public bool display { get; set; }
        public double round { get; set; }
        public int group { get; set; }
    }
}