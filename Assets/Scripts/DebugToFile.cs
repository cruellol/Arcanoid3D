using System.IO;
using UnityEngine;

namespace Arcanoid
{
    public class DebugToFile
    {
        private static string _path = "Log.txt";
        public static void SetPath(string path)
        {
            _path = path;
        }

        public static void Log(object message)
        {
#if PLATFORM_STANDALONE_WIN
            using (StreamWriter sw = File.AppendText(_path))
            {
                sw.WriteLine(message);
            }
#endif
            Debug.Log(message);
        }

    }
}
