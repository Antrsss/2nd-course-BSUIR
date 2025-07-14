using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class HalsteadMetrics
{
    public int UniqueOperators { get; set; } // η₁
    public int UniqueOperands { get; set; } // η₂
    public int TotalOperators { get; set; }  // N₁
    public int TotalOperands { get; set; }  // N₂
    public Dictionary<string, int> OperatorCounts { get; set; } // f₁ⱼ
    public Dictionary<string, int> OperandCounts { get; set; }  // f₂ᵢ

    public int ProgramVocabulary { get; set; }
    public int ProgramLength { get; set; }
    public double ProgramVolume { get; set; }

    public HalsteadMetrics()
    {
        OperatorCounts = new Dictionary<string, int>();
        OperandCounts = new Dictionary<string, int>();
    }

    private void AddOperator(string token)
    {
        TotalOperators++;
        if (OperatorCounts.ContainsKey(token))
            OperatorCounts[token]++;
        else
            OperatorCounts[token] = 1;
    }

    private void AddOperand(string token)
    {
        TotalOperands++;
        if (OperandCounts.ContainsKey(token))
            OperandCounts[token]++;
        else
            OperandCounts[token] = 1;
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

    public void CalculateMetrics(string code)
    {
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

        string varargPattern = @"\*(?=\w)";

        Console.WriteLine(code);


        var operators = new HashSet<string> { "++", "--", "@", "::", "$",
              "()", "{}", "[]", ",", ";", ":", ".", "'", "\"",
              "+", "-", "*", "/", "%", "=", "+=", "-=", "*=",
              "/=", "%=", "&&", "||", "!", "and", "or", "xor",
              "shl", "shr", "ushr","==", "!=", "===", "!==",
              "<", ">", "<=", ">=", "!!", "?.", "?:", "..", "..<",
              "?", "->", "_", "as", "as?",
              "break", "class", "continue", "for", "forEach", "fun",
              "in", "!in", "interface", "is", "!is", "object",
              "package", "return", "super", "throw", "try",
              "typeof", "when", "while", "by", "constructor",
              "delegate", "dynamic", "field", "file", "get",
              "import", "init", "param", "property", "reciever",
              "set", "setparam", "where", "abstract", "companion",
              "data", "enum", "final", "inner", "lateinit",
              "internal", "external", "open", "override", "private",
              "protected", "public", "sealed", "suspend", "vararg",
              "repeat", "find"};


        var keywords = new HashSet<string> { "if", "else", "do", "var", "val", "const", "Int", "String", "Double",
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
                //Console.WriteLine("Token:" + token);

                if (keywords.Contains(token))
                {
                    if (token == "if" && j > 0 && tokens[j - 1] != "else")
                    {
                        AddOperator(token);
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
                }
                else
                {
                    if (Regex.IsMatch(token, varargPattern))
                    {
                        AddOperator("*.");
                    }
                    else if (token == "(")
                    {
                        if (j > 0 && tokens[j-1] != "if" && tokens[j-1] != "while" && tokens[j-1] != "for" && tokens[j-1] != "when")
                        {
                            AddOperator("()");
                        }
                    }
                    else if (token == "{")
                    {
                        AddOperator("{}");
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
                            AddOperand(token);

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
                                    //Console.WriteLine("TOKEN:" + newToken);

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
                                            else if (IsOperand(newToken))
                                            {
                                                AddOperand(newToken);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (token[0] == '\'')
                        {
                            AddOperator("\'");
                            AddOperand(token);
                        }
                        else if (IsOperand(token))
                        {
                            AddOperand(token);
                        }
                    }
                }
            }
        }

        UniqueOperators = OperatorCounts.Count;
        UniqueOperands = OperandCounts.Count;

        ProgramVocabulary = UniqueOperators + UniqueOperands;
        ProgramLength = TotalOperators + TotalOperands;
        ProgramVolume = Math.Round(ProgramLength * Math.Log2(ProgramVocabulary), 2);
    }

    private bool IsOperand(string token)
    {
        return Regex.IsMatch(token, @"^\w+$") ||
               Regex.IsMatch(token, @"^\d+$") ||
               Regex.IsMatch(token, @"^""[^""]*""$") ||
               Regex.IsMatch(token, @"^'[^']*'$");
    }


    //Метод для вывода в консоль (в .net MAUI не нужен)
    public void PrintMetrics()
    {
        Console.WriteLine("Метрики Холстеда:");
        Console.WriteLine($"η₁ (уникальные операторы): {UniqueOperators}");
        Console.WriteLine($"η₂ (уникальные операнды): {UniqueOperands}");
        Console.WriteLine($"N₁ (общее число операторов): {TotalOperators}");
        Console.WriteLine($"N₂ (общее число операндов): {TotalOperands}");
        Console.WriteLine($"Словарь программы (η): {UniqueOperators + UniqueOperands}");
        Console.WriteLine($"Длина программы (N): {TotalOperators + TotalOperands}");
        Console.WriteLine($"Объем программы (V): {(TotalOperators + TotalOperands) * Math.Log(UniqueOperators + UniqueOperands, 2)}");
        Console.WriteLine("\nКоличество вхождений каждого оператора:");
        foreach (var op in OperatorCounts)
        {
            if (op.Key == "\"")
            {
                Console.WriteLine($"{op.Key}: {op.Value / 2}");
            }
            else if (op.Key == "'")
            {
                Console.WriteLine($"{op.Key}: {op.Value / 2}");
            }
            else
            {
                Console.WriteLine($"{op.Key}: {op.Value}");
            }
        }

        Console.WriteLine("\nКоличество вхождений каждого операнда:");
        foreach (var operand in OperandCounts)
        {
            Console.WriteLine($"{operand.Key}: {operand.Value}");
        }
    }
}
