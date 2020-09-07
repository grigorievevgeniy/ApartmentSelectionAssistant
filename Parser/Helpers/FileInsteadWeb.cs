using System.IO;

namespace Parser.Helpers
{
    /// <summary>
    /// Чтобы лишний раз не обращаться к сайту, сохраняем html файл на жестком диске
    /// </summary>
    public class FileInsteadWeb
    {
        public static bool CheckExistFile(string url)
        {
            url = url.Replace(':', ' ').Replace('/', ' ').Replace('?', ' ') + ".html";
            var files = Directory.GetFiles("./");

            for (int i = 0; i < files.Length; i++)
            {
                if (files[i] == "./" + url)
                {
                    return true;
                }
            }
            return false;
        }

        public static string ReadFile(string url)
        {
            string name = url.Replace(':', ' ').Replace('/', ' ').Replace('?', ' ') + ".html";
            using (StreamReader sr = new StreamReader(name))
            {
                return sr.ReadToEnd();
            }
        }

        public static void SaveFile(string url, string html)
        {
            string name = url.Replace(':', ' ').Replace('/', ' ').Replace('?', ' ') + ".html";
            using (StreamWriter sw = new StreamWriter(name, false, System.Text.Encoding.Default))
            {
                sw.WriteLine(html);
            }
        }
    }
}
