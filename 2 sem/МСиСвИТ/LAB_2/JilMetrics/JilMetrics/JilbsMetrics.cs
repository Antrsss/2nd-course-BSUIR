using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace JilMetrics
{
    class JilbsMetrics
    {
        private int TotalOperators { get; set; }
        private int TotalConditionOperators { get; set; }
        private Dictionary<string, int> OperatorCounts { get; set; }

        public int AbsoluteComplexity { get; set; }
        public double RelativeComplexity { get; set; }
        public int MaxNestingLevel { get; set; }

        private string _code;

        public JilbsMetrics()
        {
            OperatorCounts = new Dictionary<string, int>();
        }

        private void AddOperator(string token)
        {
            TotalOperators++;
            if (OperatorCounts.ContainsKey(token))
                OperatorCounts[token]++;
            else
                OperatorCounts[token] = 1;
        }

        private void AddConditionOperator()
        {
            TotalConditionOperators++;
        }

        //Разделение кода из файла на токены (токеном выделяется последовательность символов, отделённая с двух сторон пробелами
        static List<string> ParseTokens(string line)
        {
            var matches = Regex.Matches(line, @"[^\s""']+|""[^""]*""|'[^']*'");
            var tokens = new List<string>();

            foreach (Match match in matches)
            {
                tokens.Add(match.Value);
            }

            return tokens;
        }

        private bool IsOperand(string token)
        {
            return Regex.IsMatch(token, @"^\w+$") ||
                   Regex.IsMatch(token, @"^\d+$") ||
                   Regex.IsMatch(token, @"^""[^""]*""$") ||
                   Regex.IsMatch(token, @"^'[^']*'$");
        }

        public void PrintMetrics()
        {
            Console.WriteLine($"Абсолютная сложность: {AbsoluteComplexity}");
            Console.WriteLine($"Относительная сложность: {RelativeComplexity}");
            Console.WriteLine($"Максимальная вложенность: {MaxNestingLevel}");

        }

        public void CalculateMetrics(string code)
        {
            var whenOperators = new List<string>();
            var openCurlBracketCount = new List<int>();
            var closeCurlBracketCount = new List<int>();

            string compoundOperatorPattern = @"\.\w+";

            code = Regex.Replace(code, @"//.*", "");
            code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
            code = Regex.Replace(code, @"\s*\n\s*", " ");
            code = Regex.Replace(code, @"(?<=[a-zA-Z])\.(?=[a-zA-Z])", " . "); //теоретически, может работать не корректно, т.к. учитываются только буквы английского алфавита, а не все символы
            code = Regex.Replace(code, @"\s*\-\>\s*", " -> ");
            code = Regex.Replace(code, @"\s*\?\.\s*", " ?. ");
            code = Regex.Replace(code, @"\s*\!\!\s*", " !! ");

            code = Regex.Replace(code, @"(\S)\{(\S)", "$1{ $2");
            code = Regex.Replace(code, @"(\S)\}(\S)", "$1 } $2");
            code = Regex.Replace(code, @"(\S)\}", "$1 }");
            code = Regex.Replace(code, @"\}(\S)", "} $1");
            code = Regex.Replace(code, @"(\S)\[(\S)", "$1[ $2");
            code = Regex.Replace(code, @"(\S)\](\S)", "$1 ] $2");
            code = Regex.Replace(code, @"(\S)\]", "$1 ]");
            code = Regex.Replace(code, @"\](\S)", "] $1");
            code = Regex.Replace(code, @"\(", "( ");
            code = Regex.Replace(code, @"(?<=!)\(", " (");
            code = Regex.Replace(code, @"(\S)\)", "$1 )");
            //code = Regex.Replace(code, @"\[", " [");  Вроде не нужна
            code = Regex.Replace(code, @"\@", "@ ");
            code = Regex.Replace(code, @"\s*:\s*", " : ");
            code = Regex.Replace(code, @"(?<=[a-zA-Z\d]),(?=[a-zA-Z\d\s])", " , ");
            code = Regex.Replace(code, @"\s*\.\.\s*", " .. ");
            code = Regex.Replace(code, @"\s*\.\.\<\s*", " ..< ");
            code = Regex.Replace(code, @"\s*\:\:\s*", " :: ");
            code = Regex.Replace(code, @"\s*\+\+\s*", " ++ ");
            code = Regex.Replace(code, @"\s*\-\-\s*", " -- ");

            _code = code;

            string varargPattern = @"\*(?=\w)";

            Console.WriteLine(code);


            var operators = new HashSet<string> {
                  "+", "-", "*", "/", "%",
                  "==", "!=", ">", "<", ">=", "<=",
                  "&&", "||", "!",
                  "=", "+=", "-=", "*=", "/=", "%=",
                  "..", "..<", "is", "!is", "as", "as?",
                  "+", "-", "++", "--", "?:", 
                  ".", "?.", "!!", "[]", "()", 
                  "when", "for", "while", "break", "continue", "return",
                  "in", "!in", "by",
                  "suspend", "async", "await",
                  "@", "->"
            };


            var keywords = new HashSet<string> { "if", "else", "const", "Int", "String", "Double",
                                                 "Byte", "Short", "Long", "UByte", "UShort", "UInt", "Ulong", "Float", "Boolean",
                                                 "Char" };

            var lines = code.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var tokens = ParseTokens(line);

                for (int j = 0; j < tokens.Count; j++)
                {
                    var token = tokens[j];
                    Console.WriteLine("Token:" + token);

                    if (keywords.Contains(token))
                    {
                        if (token == "if" && j > 0 && tokens[j - 1] != "else")
                        {
                            AddOperator(token);
                        }
                        if (token == "if")
                        {
                            AddConditionOperator();
                        }
                        continue;
                    }

                    if (Regex.IsMatch(token, compoundOperatorPattern))
                    {
                        AddOperator(token);
                    }
                    else if (operators.Contains(token))
                    {
                        AddOperator(token);
                        if (token == "while" || token == "for")
                        {
                            AddConditionOperator();
                        }
                        else if (token == "when")
                        {
                            whenOperators.Add("when");
                        }
                        else if (token == "->" && j > 0 && tokens[j - 1] != "else")
                        {
                            if (whenOperators.Count > 0 && openCurlBracketCount[whenOperators.Count - 1] - 1 == closeCurlBracketCount[whenOperators.Count - 1])
                            {
                                AddConditionOperator();
                            }
                        }
                    }
                    else
                    {
                        if (Regex.IsMatch(token, varargPattern))
                        {
                            AddOperator("*.");
                        }
                        else if (token == "(")
                        {
                            if (j > 0 && tokens[j - 1] != "if" && tokens[j - 1] != "while" && tokens[j - 1] != "for" && tokens[j - 1] != "when")
                            {
                                AddOperator("()");
                            }
                        }
                        else if (token == "{")
                        {
                            AddOperator("{}");
                            if (whenOperators.Count > 0)
                            {
                                if (whenOperators.Count > openCurlBracketCount.Count)
                                {
                                    openCurlBracketCount.Add(1);
                                    closeCurlBracketCount.Add(0);
                                }
                                else
                                {
                                    openCurlBracketCount[whenOperators.Count - 1]++;
                                }
                            } 
                        }
                        else if (token == "}")
                        {
                            if (whenOperators.Count > 0)
                            {
                                closeCurlBracketCount[whenOperators.Count - 1]++;
                                if (closeCurlBracketCount[whenOperators.Count - 1] == openCurlBracketCount[whenOperators.Count - 1])
                                {
                                    openCurlBracketCount.RemoveAt(whenOperators.Count - 1);
                                    closeCurlBracketCount.RemoveAt(whenOperators.Count - 1);
                                    whenOperators.RemoveAt(whenOperators.Count - 1);
                                }
                            }
                        }
                        else if (token == "[")
                        {
                            AddOperator("[]");
                        }
                        else
                        {
                            if (token[token.Length - 1] == '(')
                            {
                                AddOperator(token + ')');
                            }
                            else if (token[0] == '\"')
                            {
                                AddOperator("\"\"");

                                // Поиск всех ${...} в token
                                var matches = Regex.Matches(token, @"\$\{[^}]*\}|\$\w+");
                                string newCode = "";

                                for (int k = 0; k < matches.Count; k++)
                                {
                                    newCode += matches[k] + " ";
                                }

                                newCode = Regex.Replace(newCode, @"//.*", "");
                                newCode = Regex.Replace(newCode, @"/\*.*?\*/", "", RegexOptions.Singleline);
                                newCode = Regex.Replace(newCode, @"\s*\n\s*", " ");
                                newCode = Regex.Replace(newCode, @"(?<=[a-zA-Z])\.(?=[a-zA-Z])", " . ");
                                newCode = Regex.Replace(newCode, @"(?<=[a-zA-Z]),(?=[a-zA-Z])", " , ");
                                newCode = Regex.Replace(newCode, @"(\S)\{(\S)", "$1 { $2");
                                newCode = Regex.Replace(newCode, @"(\S)\}(\S)", "$1 } $2");
                                newCode = Regex.Replace(newCode, @"(\S)\}", "$1 }");
                                newCode = Regex.Replace(newCode, @"\}(\S)", "} $1");
                                newCode = Regex.Replace(newCode, @"(?<=[a-zA-Z])\[(?=[a-zA-Z])", " [ ");
                                newCode = Regex.Replace(newCode, @"(?<=[a-zA-Z]);(?=[a-zA-Z])", " ; ");
                                newCode = Regex.Replace(newCode, @"\s*:\s*", " : ");
                                newCode = Regex.Replace(newCode, @"\(", "( ");
                                newCode = Regex.Replace(newCode, @"\@", "@ ");
                                newCode = Regex.Replace(newCode, @"\$", " $ ");
                                newCode = Regex.Replace(newCode, @"(?<=!)\(", " (");
                                newCode = Regex.Replace(newCode, @"(\S)\)", "$1 )");
                                newCode = Regex.Replace(newCode, @"\s*\.\.\s*", " .. ");
                                newCode = Regex.Replace(newCode, @"\s*\:\:\s*", " :: ");
                                newCode = Regex.Replace(newCode, @"\s*\+\+\s*", " ++ ");
                                newCode = Regex.Replace(newCode, @"\s*\-\-\s*", " -- ");
                                newCode = Regex.Replace(newCode, @"\s*\.\.\<\s*", " ..< ");
                                newCode = Regex.Replace(newCode, @"\s*\-\>\s*", " -> ");
                                newCode = Regex.Replace(newCode, @"\s*\?\.\s*", " ?. ");
                                newCode = Regex.Replace(newCode, @"\s*\!\!\s*", " !! ");

                                var newLines = newCode.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

                                for (int l = 0; l < newLines.Length; l++)
                                {
                                    var newLine = newLines[l];
                                    var newTokens = ParseTokens(newLine);

                                    for (int m = 0; m < newTokens.Count; m++)
                                    {
                                        var newToken = newTokens[m];
                                        Console.WriteLine("TOKEN:" + newToken);

                                        if (keywords.Contains(newToken))
                                        {
                                            continue;
                                        }

                                        if (Regex.IsMatch(newToken, compoundOperatorPattern))
                                        {
                                            AddOperator(newToken);
                                        }
                                        else if (operators.Contains(newToken))
                                        {
                                            AddOperator(newToken);
                                            if (newToken == "while" || newToken == "for")
                                            {
                                                AddConditionOperator();
                                            }
                                            else if (newToken == "when")
                                            {
                                                whenOperators.Add("when");
                                            }
                                            else if (newToken == "->" && j > 0 && newTokens[j - 1] != "else")
                                            {
                                                if (whenOperators.Count > 0 && openCurlBracketCount[whenOperators.Count - 1] - 1 == closeCurlBracketCount[whenOperators.Count - 1])
                                                {
                                                    AddConditionOperator();
                                                }
                                            }
                                        }
                                        else
                                        {
                                            if (Regex.IsMatch(newToken, varargPattern))
                                            {
                                                AddOperator("*.");
                                            }
                                            else if (newToken == "(")
                                            {
                                                if (m > 0 && newTokens[m - 1] != "if" && newTokens[m - 1] != "while" && newTokens[m - 1] != "for" && newTokens[m - 1] != "when")
                                                {
                                                    AddOperator("()");
                                                }
                                            }
                                            else if (newToken == "{")
                                            {
                                                AddOperator("{}");
                                                if (whenOperators.Count > 0)
                                                {
                                                    if (whenOperators.Count > openCurlBracketCount.Count)
                                                    {
                                                        openCurlBracketCount.Add(1);
                                                    }
                                                    else
                                                    {
                                                        openCurlBracketCount[whenOperators.Count - 1]++;
                                                    }
                                                }
                                            }
                                            else if (newToken == "}")
                                            {
                                                if (whenOperators.Count > 0)
                                                {
                                                    if (whenOperators.Count > closeCurlBracketCount.Count)
                                                    {
                                                        closeCurlBracketCount.Add(1);
                                                    }
                                                    else
                                                    {
                                                        closeCurlBracketCount[whenOperators.Count - 1]++;
                                                        if (closeCurlBracketCount[whenOperators.Count - 1] == openCurlBracketCount[whenOperators.Count - 1])
                                                        {
                                                            openCurlBracketCount.RemoveAt(whenOperators.Count - 1);
                                                            closeCurlBracketCount.RemoveAt(whenOperators.Count - 1);
                                                            whenOperators.RemoveAt(whenOperators.Count - 1);
                                                        }
                                                    }
                                                }
                                            }
                                            else if (newToken == "[")
                                            {
                                                AddOperator("[]");
                                            }
                                            else
                                            {
                                                if (newToken[newToken.Length - 1] == '(')
                                                {
                                                    AddOperator(newToken + ')');
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            else if (token[0] == '\'')
                            {
                                AddOperator("\'");
                            }
                        }
                    }
                }
            }

            AbsoluteComplexity = TotalConditionOperators;
            RelativeComplexity = (double)TotalConditionOperators / (double)TotalOperators;
        }

        public void CalculateBranchingAndDepth()
        {
            int maxDepth = -1;     // Максимальная глубина вложенности
            int currentDepth = -1; // Текущая глубина вложенности
            var keyList = new List<string>(); // Стек для condition operators, { и }
            var stack = new Stack<string>(); // Стек для отслеживания вложенности

            var conditionOperators = new HashSet<string> {
                "if", "else", "for", "do","while", "when", "->"
            };


            var lines = _code.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i];
                var tokens = ParseTokens(line);

                for (int j = 0; j < tokens.Count; j++)
                {
                    // Увеличиваем уровень вложенности при открытии блока условного оператора
                    if (conditionOperators.Contains(tokens[j]) || tokens[j] == "{" || tokens[j] == "}")
                    {
                        if (tokens[j] == "if" && j < tokens.Count - 1 && tokens[i + 1] == "else")
                        {
                            keyList.Add("if else");
                            j++;
                            Console.WriteLine("if else\n");
                        }
                        else
                        {
                            keyList.Add(tokens[j]);
                            Console.WriteLine($"{tokens[j]}\n");
                        }
                    }
                }
            }

            for (int i = 0; i < keyList.Count; i++)
            {
                if (keyList[i] == "{" && i > 0 && keyList[i - 1] != "->" && conditionOperators.Contains(keyList[i - 1]))
                {
                    currentDepth++;
                    stack.Push(keyList[i]);

                    // Обновляем максимальную глубину
                    if (currentDepth > maxDepth)
                    {
                        maxDepth = currentDepth;
                    }
                }
                // Уменьшаем уровень вложенности при закрытии блока
                else if (keyList[i] == "}")
                {
                    if (stack.Count > 0)
                    {
                        stack.Pop();
                        currentDepth--;
                    }
                }
                // Учитываем ветвление в операторе when
                else if (keyList[i] == "->" && i > 0 && keyList[i - 1] != "else")
                {
                    currentDepth++;
                    if (currentDepth > maxDepth)
                    {
                        maxDepth = currentDepth;
                    }
                }
            }

            MaxNestingLevel = maxDepth;
        }
    }
}
