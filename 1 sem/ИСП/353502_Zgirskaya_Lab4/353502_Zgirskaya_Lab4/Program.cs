using _353502_Zgirskaya_Lab4.Entities;

namespace _353502_Zgirskaya_Lab4
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string MY_DIR_PATH = "C:\\Users\\zgdas\\OneDrive\\Documents\\2 курс\\ИСП\\Zgirskaya_Lab4";

            if (Directory.Exists(MY_DIR_PATH))
            {
                Directory.Delete(MY_DIR_PATH, recursive: true);
                Console.WriteLine("Directory '" + MY_DIR_PATH + "' was deleted!");
            }
            var myDir = Directory.CreateDirectory(MY_DIR_PATH);


            String? currentFileName = null;
            for (int i = 1; i <= 10; ++i)
            {
                if (i % 4 == 0)
                {
                    currentFileName = Path.Combine(MY_DIR_PATH, $"{i.ToString()}.txt");
                }
                else if (i % 4 == 1)
                {
                    currentFileName = Path.Combine(MY_DIR_PATH, $"{i.ToString()}.rtf");
                }
                else if (i % 4 == 2)
                {
                    currentFileName = Path.Combine(MY_DIR_PATH, $"{i.ToString()}.dat");
                }
                else
                {
                    currentFileName = Path.Combine(MY_DIR_PATH, $"{i.ToString()}.inf");
                }
                
                File.Create(currentFileName).Close();
            }
            Console.WriteLine();


            var fileNamesList = Directory.GetFiles(MY_DIR_PATH);
            FileInfo file = null;
            foreach (var fileName in fileNamesList)
            {
                file = new FileInfo(fileName);
                Console.WriteLine($"File: {file.Name} has an extension '{file.Extension}'");
            }
            Console.WriteLine();


            List<PassengerLuggage> lagguageList = new List<PassengerLuggage>();
            lagguageList.Add(new PassengerLuggage("bag", 10, true));
            lagguageList.Add(new PassengerLuggage("case", 5, false));
            lagguageList.Add(new PassengerLuggage("suitcase", 3, true));
            lagguageList.Add(new PassengerLuggage("packet", 1, true));
            lagguageList.Add(new PassengerLuggage("bag", 4, false));


            FileService fileService = new FileService();
            fileService.SaveData(lagguageList, MY_DIR_PATH + "\\4.txt");


            File.Move(MY_DIR_PATH + "\\4.txt", MY_DIR_PATH + "\\lagguage.txt");
            Console.WriteLine($"File renamed from '4.txt' to 'lagguage.txt'\n");


            List<PassengerLuggage> newLagguageList = new List<PassengerLuggage>();
            newLagguageList.AddRange(fileService.ReadFile(MY_DIR_PATH + "\\lagguage.txt"));
            Console.WriteLine("Data loaded successfully from the file.\n");


            Console.WriteLine("Source collection:\n");
            foreach (var laguage in lagguageList)
            {
                Console.WriteLine(laguage.ToString());
            }
            Console.WriteLine("File collection:\n");
            foreach (var laguage in newLagguageList)
            {
                Console.WriteLine(laguage.ToString());
            }
        }
    }
}
