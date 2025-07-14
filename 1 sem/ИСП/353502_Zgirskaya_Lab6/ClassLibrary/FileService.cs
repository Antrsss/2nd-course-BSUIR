using System.Text;
using System.Text.Json;
using _353502_Zgirskaya_Lab6;

namespace ClassLibrary
{
    public class FileService<T> : IFileService<T> where T : class
    {
        public IEnumerable<T> ReadFile(string fileName)
        {
            try
            {
                string jsonString = File.ReadAllText(fileName);
                var data = JsonSerializer.Deserialize<IEnumerable<T>>(jsonString);
                return data ?? new List<T>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading file: {ex.Message}");
                return new List<T>();
            }
        }

        public void SaveData(IEnumerable<T> data, string fileName)
        {
            try
            {
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(data, options);
                File.WriteAllText(fileName, jsonString);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to file: {ex.Message}");
            }
        }
    }
}
