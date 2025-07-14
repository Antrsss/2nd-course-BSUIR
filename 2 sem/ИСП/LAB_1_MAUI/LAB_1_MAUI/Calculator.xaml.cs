using Microsoft.Maui.Graphics.Text;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Metrics;
using System.Text;

namespace LAB_1_MAUI;

public partial class Calculator : ContentPage
{
	string? memorised_number = null;
    double? persent_number = null;
	public Calculator()
	{
		InitializeComponent();
	}

    private void ActivateMButtons()
    {
        mr_button.IsEnabled = true;
        mc_button.IsEnabled = true;
        mv_button.IsEnabled = true;

        mc_button.TextColor = Colors.Black;
        mr_button.TextColor = Colors.Black;
        mv_button.TextColor = Colors.Black;
    }

    private void CheckTextOnEqualsAndOperations(
        string currentExpr, out bool hasEqual, 
        out int equalsPosition, out bool hasOperation, 
        out int operationsPosition)
    {
        bool has_equal = false;
        int equals_position = -1;

        bool has_operation = false;
        int operations_position = -1;

        for (int i = currentExpr.Length - 1; i >= 0; i--)
        {
            if (currentExpr[i] == '+' || currentExpr[i] == '-' && i != 0 || currentExpr[i] == '*' || currentExpr[i] == '/')
            {
                has_operation = true;
                operations_position = i;
            }
            else if (currentExpr[i] == '=')
            {
                has_equal = true;
                equals_position = i;
            }
        }

        hasEqual = has_equal;
        hasOperation = has_operation;
        equalsPosition = equals_position;
        operationsPosition = operations_position;
    }

    private void CheckTextOnEqualsAndOperations(
    string currentExpr, out bool hasEqual,
    out int equalsPosition, out bool hasOperation,
    out int operationsPosition, out string operation,
    out string firstNumberStr)
    {
        bool has_operation = false;
        int operations_position = -1;
        string operation_str = "";
        string first_number_str = "";

        bool has_equal = false;
        int equals_position = -1;


        for (int i = currentExpr.Length - 1; i >= 0; i--)
        {
            if (currentExpr[i] == '=')
            {
                has_equal = true;
                equals_position = i;
                break;
            }
        }

        for (int i = 0; i < currentExpr.Length; i++)
        {

            if (currentExpr[i] == '+' || currentExpr[i] == '-' && i != 0 || currentExpr[i] == '*' || currentExpr[i] == '/')
            {
                operation_str += currentExpr[i];
                has_operation = true;
                operations_position = i;
                break;
            }
            first_number_str += currentExpr[i];
        }

        hasEqual = has_equal;
        hasOperation = has_operation;
        equalsPosition = equals_position;
        operationsPosition = operations_position;
        operation = operation_str;
        firstNumberStr = first_number_str;
    }

    private void OnNearestIntButtonClicked(object sender, EventArgs e)
    {
        string current_expr = number_label.Text;
        string current_number = "";

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position);

        if (has_equal)
        {
            for (int i = equals_position + 1; i < current_expr.Length; i++)
            {
                current_number += current_expr[i];
            }
        }
        else if (has_operation)
        {
            for (int i = operations_position + 1; i < current_expr.Length; i++)
            {
                current_number += current_expr[i];
            }
        }
        else
        {
            for (int i = 0; i < current_expr.Length; i++)
            {
                current_number += current_expr[i];
            }
        }

        double.TryParse(current_number, out double number);
        number = Math.Round(number, MidpointRounding.AwayFromZero);
        number_label.Text = number.ToString();
    }

    public void OnMSButtonClicked(object sender, EventArgs e)
    {
        string current_expr = number_label.Text;

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position);
        memorised_number = "";

        if (has_equal)
        {
            for (int i = equals_position + 1; i < current_expr.Length; i++)
            {
                memorised_number += current_expr[i];
            }
        }
        else if (has_operation)
        {
            for (int i = operations_position + 1; i < current_expr.Length; i++)
            {
                memorised_number += current_expr[i];
            }
        }
        else
        {
            for (int i = 0; i < current_expr.Length; i++)
            {
                memorised_number += current_expr[i];
            }
        }

        ActivateMButtons();
    }

    private void OnMCButtonClicked(object sender, EventArgs e)
    {
        memorised_number = null;
        persent_number = null;

        memory_label.Text = "";

        mc_button.IsEnabled = false;
        mr_button.IsEnabled = false;
        mv_button.IsEnabled = false;

        mc_button.TextColor = Colors.DarkGray;
        mr_button.TextColor = Colors.DarkGray;
        mv_button.TextColor = Colors.DarkGray;
    }

    private void OnMRButtonClicked(object sender, EventArgs e)
    {
        number_label.Text = memorised_number;
    }

    public void OnMVButtonClicked(object sender, EventArgs e)
    {
        if (memory_label.IsVisible)
        {
            memory_label.IsVisible = false;
        }
        else
        {
            memory_label.Text = memorised_number;
            memory_label.IsVisible = true;
        }
    }

    private void OnMPlusButtonClicked(object sender, EventArgs e)
    {
        double current_number, memorised_number_double;
        double.TryParse(number_label.Text, out current_number);

        if (memorised_number != null)
        {
            double.TryParse(memorised_number, out memorised_number_double);
        }
        else
        {
            memorised_number_double = 0;

            ActivateMButtons();
        }

        memorised_number_double += current_number;
        memorised_number = memorised_number_double.ToString();
    }

    private void OnMMinusButtonClicked(object sender, EventArgs e)
    {
        double current_number, memorised_number_double;
        double.TryParse(number_label.Text, out current_number);

        if (memorised_number != null)
        {
            double.TryParse(memorised_number, out memorised_number_double);
        }
        else
        {
            memorised_number_double = 0;

            ActivateMButtons();
        }

        memorised_number_double -= current_number;
        memorised_number = memorised_number_double.ToString();
    }

    private void OnCancelButtonClicked(object sender, EventArgs e)
    {
        string new_number_text = "";
        string current_number_text = number_label.Text.ToString();

        for (int i = 0; i < number_label.Text.Length - 1; i++) 
        {
            new_number_text += current_number_text[i];
        }
        if (new_number_text.Length == 0)
        {
            new_number_text = "0";
        }

        number_label.Text = new_number_text;
    }

    private void OnCButtonClicked(object sender, EventArgs e)
    {
        number_label.Text = "0";
        persent_number = null;
    }

    private void OnCEButtonClicked(object sender, EventArgs e)
    {
        string current_expr = number_label.Text;
        string new_expr = "";

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position);

        if (has_equal)
        {
            if (has_operation)
            {
                new_expr = "0";
            }
            else
            {
                for (int i = 0; i <= equals_position; i++)
                {
                    new_expr += current_expr[i];
                }
                new_expr += "0";
            }
        }
        else
        {
            if (has_operation)
            {
                for (int i = 0; i <= operations_position; i++)
                {
                    new_expr += current_expr[i];
                }
                new_expr += "0";
            }
            else
            {
                new_expr = "0";
            }
        }

        number_label.Text = new_expr;
    }

    private void OnDigitButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;

        string digit = button.Text;

        string current_expr = number_label.Text;
        int lenght = current_expr.Length;
        bool expr_has_equals = false;

        for (int i = current_expr.Length - 1; i >= 0; i--)
        {
            if (current_expr[i] == '=')
            {
                expr_has_equals = true;
                break;
            }
        }

        if (expr_has_equals)
        {
            number_label.Text = digit;
        }
        else
        {
            if (number_label.Text != "0")
            {
                if (current_expr[lenght - 1] == '0' && (current_expr[lenght - 2] == '+' || current_expr[lenght - 2] == '-' || current_expr[lenght - 2] == '*' || current_expr[lenght - 2] == '/'))
                {

                    StringBuilder sb = new StringBuilder(current_expr);
                    sb[lenght - 1] = digit.ToString()[0];
                    number_label.Text = sb.ToString();
                }
                else
                {
                    number_label.Text += digit;
                }
            }
            else if (digit != "0")
            {
                number_label.Text = digit;
            }
        }
    }

    private void OnOperationButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string current_operation = button.Text;

        string current_expr = number_label.Text;
        string second_number_str = "";

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position, out string operation, out string first_number_str);


        if (has_equal)
        {
            if (has_operation)
            {
                string last_number = "";
                for (int i = equals_position + 1; i < current_expr.Length; i++)
                {
                    last_number += current_expr[i];
                }

                current_expr = last_number + current_operation;

                if (current_operation == "=")
                {
                    number_label.Text = current_expr + last_number;
                }
                else
                {
                    number_label.Text = current_expr;
                }
            }
            else
            {
                string new_expr = "";
                for (int i = 0; i < current_expr.Length; i++)
                {
                    if (current_expr[i] == '=')
                    {
                        new_expr += current_operation;
                        i++;
                    }
                    new_expr += current_expr[i];
                }
                number_label.Text = new_expr;
            }
        }
        else
        {
            if (has_operation)
            {
                for (int i = operations_position + 1; i < current_expr.Length; i++)
                {
                    second_number_str += current_expr[i];
                }

                double first_number, second_number, result = 0;
                double.TryParse(first_number_str, out first_number);
                double.TryParse(second_number_str, out second_number);

                switch (operation)
                {
                    case "+":
                        result = first_number + second_number;
                        break;
                    case "-":
                        result = first_number - second_number;
                        break;
                    case "*":
                        result = first_number * second_number;
                        break;
                    case "/":
                        result = first_number / second_number;
                        break;
                }

                if (current_operation == "=")
                {
                    number_label.Text += "=" + result.ToString();
                }
                else
                {
                    number_label.Text = result.ToString();
                }
            }
            else
            {
                if (current_operation == "=")
                {
                    number_label.Text += current_operation + first_number_str;
                }
            }


            if (current_operation != "=")
            {
                number_label.Text += current_operation;
            }
        }
    }

    private void OnCommaButtonClicked(object sender, EventArgs e)
    {
        int length = number_label.Text.Length;
        if (number_label.Text[length - 1] == '+' || number_label.Text[length - 1] == '-' || number_label.Text[length - 1] == '*' || number_label.Text[length - 1] == '/')
        {
            number_label.Text += "0";
        }
        if (number_label.Text != ",")
        {
            number_label.Text += ",";
        }
    }

    private void OnSignButtonClicked(object sender, EventArgs e)
    {
        string current_expr = number_label.Text;

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position);

        if (!has_operation)
        {
            if (number_label.Text[0] == '-')
            {
                string new_number_label = "";
                for (int i = 1; i < number_label.Text.Length; i++)
                {
                    new_number_label += number_label.Text[i];
                }
                number_label.Text = new_number_label;
            }
            else
            {
                number_label.Text = "-" + number_label.Text;
            }
        }
        else
        {
            string new_number_label = "";

            if (has_equal)
            {
                new_number_label += "-";
                for (int i = equals_position + 1;  i < number_label.Text.Length; i++)
                {
                    new_number_label += number_label.Text[i];
                }
            }
            else
            {
                for (int i = 0, j = 0; i < number_label.Text.Length; i++, j++)
                {
                    if (j == operations_position + 1)
                    {
                        new_number_label += "-";
                        j++;
                    }
                    new_number_label += number_label.Text[i];
                }
            }


            number_label.Text = new_number_label;
        }
    }

    private void OnSpecialOperationButtonClicked(object sender, EventArgs e)
    {
        Button button = (Button)sender;
        string current_operation = button.Text;
        string current_expr = number_label.Text;

        string second_number_str = "";
        double first_number = 0;
        double second_number = 0;
        double result = 0;

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position, out string operation, out string first_number_str);


        if (has_equal)
        {
            for (int i = equals_position + 1; i < current_expr.Length; i++)
            {
                second_number_str += current_expr[i];
            }
            double.TryParse(second_number_str, out second_number);

            switch (current_operation)
            {
                case "1/x":
                    if (second_number != 0)
                    {
                        result = 1 / second_number;
                    }
                    else
                    {
                        number_label.Text = "Cannot divide by zero";
                    }
                    break;
                case "x²":
                    result = second_number * second_number;
                    break;
                case "²√x":
                    if (second_number >= 0)
                    {
                        result = Math.Sqrt(second_number);
                    }
                    else
                    {
                        number_label.Text = "Invalid input";
                    }
                    break;
            }
        }
        else
        {
            if (has_operation)
            {
                for (int i = operations_position + 1; i < current_expr.Length; ++i)
                {
                    second_number_str += current_expr[i];
                }
                double.TryParse(first_number_str, out first_number);
                double.TryParse(second_number_str, out second_number);

                switch (current_operation)
                {
                    case "1/x":
                        if (second_number != 0)
                        {
                            second_number = 1 / second_number;
                        }
                        else
                        {
                            number_label.Text = "Cannot divide by zero";
                        }
                        break;
                    case "x²":
                        second_number *= second_number;
                        break;
                    case "²√x":
                        if (second_number >= 0)
                        {
                            second_number = Math.Sqrt(second_number);
                        }
                        else
                        {
                            number_label.Text = "Invalid input";
                        }
                        break;
                }
            }
            else
            {
                double.TryParse(first_number_str, out first_number);

                switch (current_operation)
                {
                    case "1/x":
                        if (first_number != 0)
                        {
                            result = 1 / first_number;
                        }
                        else
                        {
                            number_label.Text = "Cannot divide by zero";
                        }
                        break;
                    case "x²":
                        result = first_number * first_number;
                        break;
                    case "²√x":
                        if (first_number >= 0)
                        {
                            result = Math.Sqrt(first_number);
                        }
                        else
                        {
                            number_label.Text = "Invalid input";
                        }
                        break;
                }
            }
        }

        if (number_label.Text != "Invalid input")
        {
            if (!has_equal && has_operation)
            {
                string new_expr = "";
                for (int i = 0; i <= operations_position; i++)
                {
                    new_expr += current_expr[i];
                }
                number_label.Text = new_expr + second_number.ToString();
            }
            else
            {
                number_label.Text = result.ToString();
            }
        }
    }

    private void OnPercentButtonClicked(object sender, EventArgs e)
    {
        string current_expr = number_label.Text;

        string second_number_str = "";
        double first_number;
        double second_number;
        double result = 0;

        CheckTextOnEqualsAndOperations(current_expr, out bool has_equal, out int equals_position, out bool has_operation, out int operations_position, out string operation, out string first_number_str);

        if (has_equal)
        {
            for (int i = equals_position + 1; i < current_expr.Length; i++)
            {
                second_number_str += current_expr[i];
            }
            double.TryParse(second_number_str, out second_number);

            if (persent_number == null)
            {
                persent_number = second_number;
            }

            result = (double)second_number * (double)persent_number / 100;
        }
        else
        {
            if (has_operation)
            {
                for (int i = operations_position + 1; i < current_expr.Length; ++i)
                {
                    second_number_str += current_expr[i];
                }
                double.TryParse(first_number_str, out first_number);
                double.TryParse(second_number_str, out second_number);

                switch (operation)
                {
                    case "+":
                        result = first_number + first_number * second_number / 100;
                        break;
                    case "-":
                        result = first_number - first_number * second_number / 100;
                        break;
                    case "*":
                        result = first_number * (first_number * second_number / 100);
                        break;
                    case "/":
                        result = first_number / (first_number * second_number / 100);
                        break;
                }
                persent_number = result;
            }
            else
            {
                double.TryParse(first_number_str, out first_number);
                if (persent_number != null)
                {
                    result = (double)first_number * (double)persent_number / 100;
                }
            }
        }

        number_label.Text = result.ToString();
    }
}