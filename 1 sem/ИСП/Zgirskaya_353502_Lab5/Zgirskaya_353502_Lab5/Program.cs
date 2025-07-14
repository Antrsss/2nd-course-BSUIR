using Zgirskaya_353502_Lab5.Domain;
using SerializerLib;
using Microsoft.Extensions.Configuration;
using Zgirskaya_353502_Lab5.Domain.Entities;

namespace Zgirskaya_353502_Lab5
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<Actor> actors = new List<Actor>() 
            {
                new Actor("Alan", 18, new FilmCharacter("Tom", "Tom Sawyer")),
                new Actor("Jerry", 34, new FilmCharacter("Andrey Bolkonsky", "War and piece")),
                new Actor("Ann", 25, new FilmCharacter("Jane Eyre", "Jane Eyre")),
                new Actor("Helga", 41, new FilmCharacter("Anna Karenina", "Anna Karenina")),
                new Actor("Jack", 29, new FilmCharacter("Rodion Raskolnicov", "Crime and punishment"))
            };

            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            string xmlFilePath = configuration["FilePaths:XmlFilePath"];
            string jsonFilePath = configuration["FilePaths:JsonFilePath"];
            string linqXmlFilePath = configuration["FilePaths:LinqXmlFilePath"];

            Serializer serializer = new Serializer();
            serializer.SerializeXML(actors, xmlFilePath);
            serializer.SerializeJSON(actors, jsonFilePath);
            serializer.SerializeByLINQ(actors, linqXmlFilePath);

            IEnumerable<Actor> deserializedFromXml = serializer.DeSerializeXML(xmlFilePath);
            IEnumerable<Actor> deserializedFromJson = serializer.DeSerializeJSON(jsonFilePath);
            IEnumerable<Actor> deserializedFromLinqXml = serializer.DeSerializeByLINQ(linqXmlFilePath);

            for (int i = 0; i < deserializedFromJson.ToList().Count; i++)
            {
                Console.WriteLine($"{actors[i]}");
                Console.WriteLine($"{deserializedFromXml.ToList()[i]}");
                Console.WriteLine($"{deserializedFromJson.ToList()[i]}");
                Console.WriteLine($"{deserializedFromLinqXml.ToList()[i]}");
            }

            Console.WriteLine($"XML: {actors.SequenceEqual(deserializedFromXml)}");
            Console.WriteLine($"JSON: {actors.SequenceEqual(deserializedFromJson)}");
            Console.WriteLine($"LINQ XML: {actors.SequenceEqual(deserializedFromLinqXml)}");
        }
    }
}
