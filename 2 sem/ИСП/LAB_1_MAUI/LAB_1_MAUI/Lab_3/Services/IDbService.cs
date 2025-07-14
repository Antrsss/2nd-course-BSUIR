using LAB_1_MAUI.Lab_3.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LAB_1_MAUI.Lab_3.Services
{
    public interface IDbService
    {
        IEnumerable<SportTeam> GetSportTeams();
        IEnumerable<Participant> GetTeamParticipants(int id);
        void Init();
    }
}
