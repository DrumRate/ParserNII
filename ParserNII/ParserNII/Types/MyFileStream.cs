using System.IO;

namespace ParserNII.Types
{
    public static class MyFileStream
    {
        private static FileStream _fs;

        public static FileStream GetFileStreamInstance(string path = "log.dat")
        {
            return _fs ?? (_fs = new FileStream(path, FileMode.Open));
        }
    }
}