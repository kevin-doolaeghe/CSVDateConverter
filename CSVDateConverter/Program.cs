using System.Reflection;

namespace CSVDateConverter
{
    class Program
    {

        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("CSV files conversion successfully started!\n");
                string path = Path.Combine(GetExecPath(), "conv-" + DateTime.UtcNow.ToString("yyyymmdd-hhmmss"));
                CreateConvDir(path);
                string[] files = Directory.GetFiles(GetExecPath(), "*.csv");

                foreach (string file in files)
                {
                    string filename = Path.GetFileName(file);
                    Console.WriteLine("- Trying to convert \"" + filename + "\"..");
                    try
                    {
                        string csv = ConvertCSV(file);
                        File.WriteAllText(Path.Combine(path, filename), csv);
                        Console.WriteLine("\tDone.\n");
                    } catch
                    {
                        Console.WriteLine("\tCould not convert \"" + filename + "\": Wrong format.\n");
                    }
                }
            } catch (Exception e)
            {
                Console.WriteLine("An error occured: " + e.Message);
            }
            Console.WriteLine("Exiting..");
            Console.ReadLine();
        }

        private static string ConvertCSV(string filename)
        {
            string csv = "";
            int i = 0;
            foreach (string line in File.ReadLines(filename))
            {
                if (i > 2)
                {
                    string[] tab = line.Split("/");
                    csv += tab[1] + "/" + tab[0] + "/" + tab[2] + "\r\n";
                }
                else
                {
                    csv += line + "\r\n";
                }
                i++;
            }
            return csv;
        }

        private static string GetExecPath()
        {
            string execPath = Assembly.GetExecutingAssembly().Location;
            return Path.GetDirectoryName(execPath) ?? "";
        }

        private static void CreateConvDir(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}