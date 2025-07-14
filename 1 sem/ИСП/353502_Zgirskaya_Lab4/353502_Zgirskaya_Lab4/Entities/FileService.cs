using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using _353502_Zgirskaya_Lab4.Interfaces;

namespace _353502_Zgirskaya_Lab4.Entities
{
    internal class FileService : IFileService<PassengerLuggage>
    {
        public IEnumerable<PassengerLuggage> ReadFile(string fileName)
        {
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException("File not found!");
            }

            using (FileStream fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader binaryReader = new BinaryReader(fileStream))
                {
                    PassengerLuggage item = null;

                    while (fileStream.Position < fileStream.Length)
                    {
                        try
                        {
                            string value = binaryReader.ReadString();
                            item = (PassengerLuggage)Convert.ChangeType(value, typeof(PassengerLuggage));
                        }
                        catch (EndOfStreamException e)
                        {
                            Console.WriteLine($"File reading error: {e.Message}");
                        }
                        catch (ObjectDisposedException e)
                        {
                            Console.WriteLine($"Closed stream error: {e.Message}");
                        }
                        catch (DecoderFallbackException e)
                        {
                            Console.WriteLine($"Decoder error: {e.Message}");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Wrong argument error: {e.Message}");
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine($"Input-output error: {e.Message}");
                        }

                        if (item != null)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        public void SaveData(IEnumerable<PassengerLuggage> data, string fileName)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
                File.Create(fileName).Close();
            }

            using (FileStream fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write))
            {
                using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
                {
                    foreach (var item in data)
                    {
                        try
                        {
                            binaryWriter.Write(item.ToString() + '\n');
                        }
                        catch (ObjectDisposedException e)
                        {
                            Console.WriteLine($"Closed stream error: {e.Message}");
                        }
                        catch (ArgumentException e)
                        {
                            Console.WriteLine($"Wrong argument error: {e.Message}");
                        }
                        catch (IOException e)
                        {
                            Console.WriteLine($"Input-output error: {e.Message}");
                        }
                    }
                }
            }
        }
    }
}
