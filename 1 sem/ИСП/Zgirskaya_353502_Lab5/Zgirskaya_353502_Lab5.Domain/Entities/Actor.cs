namespace Zgirskaya_353502_Lab5.Domain.Entities
{
    public class Actor : IEquatable<Actor>
    {
        public Actor(string name, int age, FilmCharacter character)
        {
            Name = name;
            Age = age;
            Character = character;
        }
        public Actor() { }

        public string Name { get; set; }
        public int Age { get; set; }

        public FilmCharacter Character { get; set; }

        public bool Equals(Actor? other)
        {
            if (other == null) return false;
            return Name == other.Name && Age == other.Age && Character.CharacterName == other.Character.CharacterName && Character.Film == other.Character.Film;
        }

        public override string ToString()
        {
            return $"Name = {Name}, Age = {Age}, Character = [CharactrName = {Character.CharacterName}, Film = {Character.Film}]";
        }
    }
}
