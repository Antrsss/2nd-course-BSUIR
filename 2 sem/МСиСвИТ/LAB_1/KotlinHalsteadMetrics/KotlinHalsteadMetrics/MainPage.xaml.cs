namespace KotlinHalsteadMetrics
{
    //Программа может запуститься не сразу, а с попытки третьей. Хз почему :(
    public partial class MainPage : ContentPage
    {
         //Путь к файлу с анализируемым кодом
        string code = File.ReadAllText("C:\\Users\\zgdas\\2 курс\\2 сем\\МСиСвИТ\\LAB_1\\example.txt");
        int operatorRowNumber = 2; // Начинаем с 2, так как 0 и 1 заняты заголовками
        int operandRowNumber = 2;  // Начинаем с 2, но будем увеличивать для операндов

        HalsteadMetrics metrics = new HalsteadMetrics();

        public MainPage()
        {
            try
            {
                InitializeComponent();

                var metrics = new HalsteadMetrics();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        private void OnCalculateMetricsBtnClicked(object sender, EventArgs e)
        {
            metrics.CalculateMetrics(code);

            // Определяем количество строк для операторов и операндов
            int operatorRowCount = metrics.OperatorCounts.Count;
            int operandRowCount = metrics.OperandCounts.Count;

            // Добавляем строки в Grid
            for (int i = 0; i < operatorRowCount + operandRowCount; i++)
            {
                MetricsGrid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            }

            // Добавляем операторы
            foreach (var oper in metrics.OperatorCounts)
            {
                AddOperatorInRow(oper.Key, oper.Value, operatorRowNumber);
                operatorRowNumber++; // Увеличиваем номер строки для следующего оператора
            }

            // Добавляем операнды
            foreach (var oper in metrics.OperandCounts)
            {
                AddOperandInRow(oper.Key, oper.Value, operandRowNumber);
                operandRowNumber++; // Увеличиваем номер строки для следующего операнда
            }

            UniqueOperators.Text = metrics.UniqueOperators.ToString();
            UniqueOperands.Text = metrics.UniqueOperands.ToString();
            TotalOperators.Text = metrics.TotalOperators.ToString();
            TotalOperands.Text = metrics.TotalOperands.ToString();
            ProgramVocabulary.Text = metrics.ProgramVocabulary.ToString();
            ProgramLength.Text = metrics.ProgramLength.ToString();
            ProgramVolume.Text = metrics.ProgramVolume.ToString();
        }

        private void AddOperatorInRow(string operatorData, int operatorCount, int rowNumber)
        {
            // Создаем и добавляем элементы в ячейки новой строки
            var operatorLabel = new Label
            {
                Text = operatorData,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var operatorCountLabel = new Label
            {
                Text = operatorCount.ToString(),
                HorizontalTextAlignment = TextAlignment.Center
            };

            // Устанавливаем, что operatorLabel будет в новой строке, в первом столбце
            Grid.SetRow(operatorLabel, rowNumber);
            Grid.SetColumn(operatorLabel, 0);
            Grid.SetRow(operatorCountLabel, rowNumber);
            Grid.SetColumn(operatorCountLabel, 1);

            // Добавляем элементы в Grid
            MetricsGrid.Children.Add(operatorLabel);
            MetricsGrid.Children.Add(operatorCountLabel);
        }

        private void AddOperandInRow(string operandData, int operandCount, int rowNumber)
        {
            var operandLabel = new Label
            {
                Text = operandData,
                HorizontalTextAlignment = TextAlignment.Center
            };
            var operandCountLabel = new Label
            {
                Text = operandCount.ToString(),
                HorizontalTextAlignment = TextAlignment.Center
            };

            // Устанавливаем, что operandLabel будет в новой строке, во втором столбце
            Grid.SetRow(operandLabel, rowNumber);
            Grid.SetColumn(operandLabel, 4);
            Grid.SetRow(operandCountLabel, rowNumber);
            Grid.SetColumn(operandCountLabel, 5);

            // Добавляем элементы в Grid
            MetricsGrid.Children.Add(operandLabel);
            MetricsGrid.Children.Add(operandCountLabel);
        }
    }
}