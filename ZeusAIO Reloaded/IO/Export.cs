using System;
using System.IO;

namespace ZeusAIO
{
    internal class Export
    {
        public static void Initialize()
        {
            Directory.CreateDirectory("Results");
            Directory.CreateDirectory("Results/" + date);
        }

        public static void AsResult(string fileName, string content)
        {
            lock (resultLock)
            {
                File.AppendAllText(string.Concat(new string[]
                {
                    "Results/",
                    date,
                    fileName,
                    ".txt"
                }), content + Environment.NewLine);
            }
        }
        static object resultLock = new object();
        public static string date = DateTime.Now.ToString("MM-dd-yyyy H.mm");
    }
}