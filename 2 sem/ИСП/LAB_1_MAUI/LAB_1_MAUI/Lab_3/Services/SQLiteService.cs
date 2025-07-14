using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using Microsoft.Maui.Storage;
using LAB_1_MAUI.Lab_3.Entities;

namespace LAB_1_MAUI.Lab_3.Services
{
    public class SQLiteService : IDbService
    {
        private SQLiteConnection _connection;

        public SQLiteService()
        {
            var dbPath = Path.Combine(FileSystem.AppDataDirectory, "sport.db");
            System.Diagnostics.Debug.WriteLine($"Путь к БД: {dbPath}");

            _connection = new SQLiteConnection(dbPath);

            _connection.CreateTable<SportTeam>();
            _connection.CreateTable<Participant>();

            Init();
        }

        public IEnumerable<SportTeam> GetSportTeams()
        {
            return _connection.Table<SportTeam>().ToList();
        }

        public IEnumerable<Participant> GetTeamParticipants(int id)
        {
            return _connection.Table<Participant>().Where(p => p.TeamId == id).ToList();
        }

        public void Init()
        {
            if (!_connection.Table<SportTeam>().Any())
            {
                var teams = new List<SportTeam>
                {
                    new SportTeam { Name = "Локомотив", Sport = "Футбол" },
                    new SportTeam { Name = "ЦСКА", Sport = "Хоккей" },
                    new SportTeam { Name = "Спартак", Sport = "Баскетбол" }
                };

                // Вставляем команды и получаем их ID
                foreach (var team in teams)
                {
                    _connection.Insert(team);
                }

                // 2. Создаем участников для каждой команды
                var participants = new List<Participant>
                {
                    // Участники для Локомотива (TeamId = 1)
                    new Participant { Name = "Иван Петров", Age = 22, TeamId = 1 },
                    new Participant { Name = "Алексей Смирнов", Age = 24, TeamId = 1 },
                    new Participant { Name = "Дмитрий Иванов", Age = 23, TeamId = 1 },
                    new Participant { Name = "Сергей Кузнецов", Age = 21, TeamId = 1 },
                    new Participant { Name = "Михаил Попов", Age = 25, TeamId = 1 },
        
                    // Участники для ЦСКА (TeamId = 2)
                    new Participant { Name = "Анна Сидорова", Age = 20, TeamId = 2 },
                    new Participant { Name = "Елена Михайлова", Age = 22, TeamId = 2 },
                    new Participant { Name = "Ольга Новикова", Age = 21, TeamId = 2 },
                    new Participant { Name = "Наталья Волкова", Age = 23, TeamId = 2 },
                    new Participant { Name = "Мария Соколова", Age = 20, TeamId = 2 },
                    new Participant { Name = "Ирина Петрова", Age = 24, TeamId = 2 },
        
                    // Участники для Спартака (TeamId = 3)
                    new Participant { Name = "Артем Васильев", Age = 19, TeamId = 3 },
                    new Participant { Name = "Кирилл Федоров", Age = 20, TeamId = 3 },
                    new Participant { Name = "Никита Морозов", Age = 21, TeamId = 3 },
                    new Participant { Name = "Павел Семенов", Age = 22, TeamId = 3 },
                    new Participant { Name = "Глеб Павлов", Age = 20, TeamId = 3 }
                };

                // Вставляем всех участников одной транзакцией для производительности
                _connection.RunInTransaction(() =>
                {
                    foreach (var participant in participants)
                    {
                        _connection.Insert(participant);
                    }
                });
            }
        }
    }
}
