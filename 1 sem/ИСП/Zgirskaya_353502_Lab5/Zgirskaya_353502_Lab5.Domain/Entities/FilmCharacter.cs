using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Zgirskaya_353502_Lab5.Domain.Entities
{
    public class FilmCharacter
    {
        public FilmCharacter(string characterName, string film)
        {
            CharacterName = characterName;
            Film = film;
        }
        public FilmCharacter() { }
        public string CharacterName { get; set; }
        [JsonIgnore]
        public string Film { get; set; }
    }
}
