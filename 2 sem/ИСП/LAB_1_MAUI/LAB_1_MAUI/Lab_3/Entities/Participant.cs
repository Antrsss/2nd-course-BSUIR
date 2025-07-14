using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_1_MAUI.Lab_3.Entities
{
    [Table("Participants")]
    public class Participant
    {
        [PrimaryKey, AutoIncrement, Indexed]
        [Column("Id")]
        public int ParticipantId { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        [Indexed]
        public int TeamId { get; set; }
    }
}
