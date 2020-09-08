using System.IO;
using System.Threading.Tasks;

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

        public static async Task<string> ReadFileAsync(string url)
        {
            string name = url.Replace(':', ' ').Replace('/', ' ').Replace('?', ' ') + ".html";
            using (StreamReader sr = new StreamReader(name))
            {
                return await sr.ReadToEndAsync();
            }
        }

        public static async Task SaveFile(string url, string html)
        {
            string name = url.Replace(':', ' ').Replace('/', ' ').Replace('?', ' ') + ".html";
            using (StreamWriter sw = new StreamWriter(name, false, System.Text.Encoding.Default))
            {
                await sw.WriteLineAsync(html);
            }
        }
    }
}
