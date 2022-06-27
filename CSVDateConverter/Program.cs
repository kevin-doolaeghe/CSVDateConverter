namespace CSVDateConverter
{
    class Program
    {

        static void Main()
        {
            try
            {
                Console.WriteLine("CSV files conversion successfully started!\n");
                string[] files = Directory.GetFiles(GetExecPath(), "*.csv");

                string path = Path.Combine(GetExecPath(), "conv-" + DateTime.UtcNow.ToString("yyyymmdd-hhmmss"));
                if (files.Length > 0) CreateConvDir(path);
                else Console.WriteLine("No CSV files found!");

                foreach (string file in files)
                {
                    string filename = Path.GetFileName(file);
                    Console.WriteLine("- Trying to convert \"" + filename + "\"..");
                    try
                    {
                        string csv = ConvertCSV(file);
                        File.WriteAllText(Path.Combine(path, filename), csv);
                        Console.WriteLine("\tDone.\n");
                    }
                    catch (IOException)
                    {
                        Console.WriteLine("\tCould not convert \"" + filename + "\": file already in use.\n");
                    }
                    catch
                    {
                        Console.WriteLine("\tCould not convert \"" + filename + "\": wrong format.\n");
                    }
                }
            }
            catch (UnauthorizedAccessException)
            {
                Console.WriteLine("Error: permissions are missing..");
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occured: " + e.ToString());
            }
            Console.WriteLine("\nExiting..");
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
            return Directory.GetCurrentDirectory();
        }

        private static void CreateConvDir(string path)
        {
            Directory.CreateDirectory(path);
        }
    }
}