using LAB_1_MAUI.Lab_3.Services;
using LAB_1_MAUI.Lab_3.Entities;

namespace LAB_1_MAUI;

public partial class CurrencyConverter : ContentPage
{
    private readonly IRateService _rateService;
    private List<Rate> _rates = new();

    public CurrencyConverter(IRateService rateService)
    {
        InitializeComponent();
        _rateService = rateService;
        datePicker.Date = DateTime.Today;
        _ = LoadData();
    }

    private async Task LoadData()
    {
        try
        {
            var selectedDate = datePicker.Date;
            var rates = await _rateService.GetRates(selectedDate);

            _rates = rates?
                .Where(r => new[] { "RUB", "EUR", "USD", "CHF", "CNY", "GBP" }
                    .Contains(r.Cur_Abbreviation))
                .ToList() ?? new List<Rate>();

            var ratesCollection = new List<string>();
            foreach(var rate in _rates)
            {
                ratesCollection.Add(rate.Cur_Name);
            }

            currencyCollection.ItemsSource = ratesCollection;
            currencyPicker.ItemsSource = _rates;

            if (_rates.Any()) currencyPicker.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Ошибка", ex.Message, "OK");
        }
    }

    private void ConvertCurrency()
    {
        if (currencyPicker.SelectedItem is not Rate selectedRate ||
            !decimal.TryParse(amountEntry.Text, out var amount))
        {
            resultLabel.Text = "0.00";
            return;
        }

        var result = toBYN.IsChecked
                ? amount * selectedRate.Cur_OfficialRate / selectedRate.Cur_Scale
                : amount / selectedRate.Cur_OfficialRate * selectedRate.Cur_Scale;

        if (result.HasValue)
        {
            resultLabel.Text = Math.Round(result.Value, 2).ToString();
        }
        else
        {
            resultLabel.Text = "0.00";
        }
    }

    private void OnAmountChanged(object sender, TextChangedEventArgs e)
    {
        ConvertCurrency();
    }

    private void OnCurrencyChanged(object sender, EventArgs e)
    {
        ConvertCurrency();
    }

    private async void OnDateChanged(object sender, DateChangedEventArgs e)
    {
        await LoadData();
    }

    private void OnDirectionChanged(object sender, CheckedChangedEventArgs e)
    {
        ConvertCurrency();
    }
}