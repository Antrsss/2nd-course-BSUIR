using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_1_MAUI.Lab_3.Entities
{
    [Table("SportTeams")]
    public class SportTeam
    {
        [PrimaryKey, AutoIncrement, Indexed]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Sport { get; set; }
    }
}
