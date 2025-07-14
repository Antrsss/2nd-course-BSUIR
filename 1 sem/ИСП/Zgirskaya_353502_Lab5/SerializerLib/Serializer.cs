using System.Text.Json;
using System.Xml.Linq;
using System.Xml.Serialization;
using Zgirskaya_353502_Lab5.Domain.Entities;
using Zgirskaya_353502_Lab5.Domain.Interfaces;

namespace SerializerLib
{
    public class Serializer : ISerializer
    {
        public IEnumerable<Actor> DeSerializeByLINQ(string fileName)
        {
            XDocument document = XDocument.Load(fileName);
            IEnumerable<Actor> items = document.Root.Elements("Actor")
                .Select(item =>
                {
                    var actor = new Actor();

                    actor.Name = item.Element("Name")?.Value;
                    actor.Age = int.Parse(item.Element("Age")?.Value ?? "0");

                    var filmCharacterElement = item.Element("Character");
                    actor.Character = new FilmCharacter
                    {
                        CharacterName = filmCharacterElement.Element("CharacterName")?.Value,
                        Film = filmCharacterElement.Element("Film")?.Value
                    };
                    return actor;

                });
            return items;
        }

        public IEnumerable<Actor> DeSerializeJSON(string fileName)
        {
            string jsonString = File.ReadAllText(fileName);
            return JsonSerializer.Deserialize<IEnumerable<Actor>>(jsonString);
        }

        public IEnumerable<Actor> DeSerializeXML(string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Actor>));
            using (StreamReader reader = new StreamReader(fileName))
            {
                return (IEnumerable<Actor>)xmlSerializer.Deserialize(reader);
            }
        }

        public void SerializeByLINQ(IEnumerable<Actor> xxx, string fileName)
        {
            XElement xml = new XElement("Root", from x in xxx select new XElement(
                "Actor", new XElement("Name", x.Name),
                        new XElement("Age", x.Age),
                        new XElement("Character",
                            new XElement("CharacterName", x.Character.CharacterName),
                            new XElement("Film", x.Character.Film))
                        ));
            xml.Save(fileName);
        }

        public void SerializeJSON(IEnumerable<Actor> xxx, string fileName)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(xxx, options);
            File.WriteAllText(fileName, jsonString);
        }

        public void SerializeXML(IEnumerable<Actor> xxx, string fileName)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(List<Actor>));
            using (StreamWriter writer = new StreamWriter(fileName))
            {
                xmlSerializer.Serialize(writer, xxx.ToList());
            }
        }
    }
}