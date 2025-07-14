using LAB_1_MAUI.Lab_3.Entities;
using LAB_1_MAUI.Lab_3.Services;

namespace LAB_1_MAUI;

public partial class TeamList : ContentPage
{
	private readonly IDbService _dbService;
    private IEnumerable<SportTeam> _teams;

	Label header = new Label { Text = "Выберите спортивную команду:", FontSize = 18 };
    Picker teamPicker = new Picker { Title = "Команда" };
    CollectionView participantCollection = new CollectionView { Margin = 4 };

    public TeamList(IDbService dbService)
	{
		InitializeComponent();
		_dbService = dbService;

        teamPicker.SelectedIndexChanged += PickerSelectedIndexChanged;

        var stackLayout = new VerticalStackLayout
        {
            Children = { header, teamPicker, participantCollection }
        };

        Content = stackLayout;
    }

    private void LoadSportTeams(object sender, EventArgs e)
    {
        _teams = _dbService.GetSportTeams();
        foreach(var team in _teams)
        {
            teamPicker.Items.Add(team.Name);
        }
    }

    private void PickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedTeam = teamPicker.Items[teamPicker.SelectedIndex];
        header.Text = $"Вы выбрали {selectedTeam}";

        var participants = _dbService.GetTeamParticipants(teamPicker.SelectedIndex + 1);

        var names = new List<String>();
        foreach (var particpant in participants)
        {
            names.Add(particpant.Name);
        }

        participantCollection.ItemsSource = names;
    }
}