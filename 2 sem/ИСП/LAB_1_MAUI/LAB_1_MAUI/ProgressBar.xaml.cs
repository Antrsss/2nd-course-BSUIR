using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;
namespace LAB_1_MAUI;

public partial class IntegralProgressBar : ContentPage
{
    private CancellationTokenSource _cts;
    private CancellationToken _token;
    public IntegralProgressBar()
	{
		InitializeComponent();
	}

    private async Task<double> CalculateIntegral(double a, double b, double step, CancellationToken token)
    {
        double sum = 0.0, mult;
        long stepsCount = (long)((b - a) / step);

        for (long i = 0; i < stepsCount; i++)
        {
            await Task.Delay(1, _token);

            sum += Math.Sin(a + i * step) * step;

            for (int j = 0; j < 100000; j++)
            {
                mult = j * j;
            }

            await MainThread.InvokeOnMainThreadAsync(()=> progress_bar.Progress = Convert.ToDouble(i + 1) / stepsCount);
            //percent_label.Text = ((double)(i + 1) / stepsCount * 100).ToString() + "%";
        }

        status_label.Text = $"Result: { sum }";
        cancel_btn.IsEnabled = false;
        start_btn.IsEnabled = true;

        return sum;
    }

    private async void OnStartButtonClicked(object sender, EventArgs e)
    {
        cancel_btn.IsEnabled = true;
        start_btn.IsEnabled = false;

        _cts = new CancellationTokenSource();
        _token = _cts.Token;

        status_label.Text = "Calculating...";
        percent_label.Text = "0%";
        progress_bar.Progress = 0;

        try
        {
            await Task.Run(()=>CalculateIntegral(0, 1, 0.0001, _token));
        }
        catch (OperationCanceledException)
        {
            _cts.Dispose();

            status_label.Text = "Task cancelled";
            cancel_btn.IsEnabled = false;
            start_btn.IsEnabled = true;
        }
    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        _cts.Cancel();
    }
}