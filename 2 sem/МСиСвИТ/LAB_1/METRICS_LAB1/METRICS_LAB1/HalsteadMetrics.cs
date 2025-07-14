using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

class HalsteadMetrics
{
    public int UniqueOperators { get; set; }
    public int UniqueOperands { get; set; }
    public int TotalOperators { get; set; }
    public int TotalOperands { get; set; }
    public Dictionary<string, int> OperatorCounts { get; set; }
    public Dictionary<string, int> OperandCounts { get; set; }

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

    static List<string> ParseTokens(string line)
    {
        var matches = Regex.Matches(line, @"[^\s""']+|""[^""]*""|'[^']*'");
        var tokens = new List<string>();

        foreach (Match match in matches)
        {
            tokens.Add(match.Value); // Добавляем токен без изменений
        }

        return tokens;
    }

    public void CalculateMetrics(string code)
    {
        string compoundOperatorPattern = @"\.\w+";

        code = Regex.Replace(code, @"//.*", "");
        code = Regex.Replace(code, @"/\*.*?\*/", "", RegexOptions.Singleline);
        code = Regex.Replace(code, @"\s*\n\s*", " ");
        code = Regex.Replace(code, @"(?<=[a-zA-Z])\.(?=[a-zA-Z])", " . ");
        code = Regex.Replace(code, @"(?<=[a-zA-Z]),(?=[a-zA-Z])", " , ");
        code = Regex.Replace(code, @"(\S)\{(\S)", "$1{ $2");
        code = Regex.Replace(code, @"(\S)\}(\S)", "$1 } $2");
        code = Regex.Replace(code, @"(\S)\}", "$1 }");
        code = Regex.Replace(code, @"\}(\S)", "} $1");

        code = Regex.Replace(code, @"(?<=[a-zA-Z])\[(?=[a-zA-Z])", " [ ");
        code = Regex.Replace(code, @"(?<=[a-zA-Z]);(?=[a-zA-Z])", " ; ");
        code = Regex.Replace(code, @"\s*:\s*", " : ");
        code = Regex.Replace(code, @"\(", "( ");
        code = Regex.Replace(code, @"\@", "@ ");
        //code = Regex.Replace(code, @"\$", "$ ");
        code = Regex.Replace(code, @"(?<=!)\(", " (");
        code = Regex.Replace(code, @"(\S)\)", "$1 )");
        code = Regex.Replace(code, @"\s*\.\.\s*", " .. ");
        code = Regex.Replace(code, @"\s*\:\:\s*", " :: ");
        code = Regex.Replace(code, @"\s*\+\+\s*", " ++ ");
        code = Regex.Replace(code, @"\s*\-\-\s*", " -- ");
        code = Regex.Replace(code, @"\s*\.\.\<\s*", " ..< ");
        code = Regex.Replace(code, @"\s*\-\>\s*", " -> ");
        code = Regex.Replace(code, @"\s*\?\.\s*", " ?. ");
        code = Regex.Replace(code, @"\s*\!\!\s*", " !! ");
        //code = Regex.Replace(code, @"(['""])", " $1 ");

        string varargPattern = @"\*(?=\w)";
        Console.WriteLine(code);

        var operators = new HashSet<string> { "++", "--", "@", "::", "$",
                          "()", "{}", "[]", ",", ";", ":", ".", "'", "\"",
                          "+", "-", "*", "/", "%", "=", "+=", "-=", "*=",
                          "/=", "%=", "&&", "||", "!", "and", "or", "xor",
                          "shl", "shr", "ushr","==", "!=", "===", "!==",
                          "<", ">", "<=", ">=", "!!", "?.", "?:", "..", "..<",
                          "?", "->", "_", "val", "var", "const", "as", "as?",
                          "break", "class", "continue", "for", "fun", "if",
                          "in", "!in", "interface", "is", "!is", "object",
                          "package", "return", "super", "throw", "try",
                          "typeof", "when", "while", "by", "constructor",
                          "delegate", "dynamic", "field", "file", "get",
                          "import", "init", "param", "property", "reciever",
                          "set", "setparam", "where", "abstract", "companion",
                          "data", "enum", "final", "inner", "lateinit",
                          "internal", "external", "open", "override", "private",
                          "protected", "public", "sealed", "suspend", "vararg",
                          "repeat"};

        var keywords = new HashSet<string> { "else", "do" };

        var lines = code.Split(new[] { '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            var tokens = ParseTokens(line);
            //var tokens = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var token in tokens)
            {
                Console.WriteLine("Token:" + token);

                if (keywords.Contains(token))
                {
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
                        AddOperator("()");
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
                            var fullFunToken = token + ')';
                            AddOperator(fullFunToken);
                            AddOperator("()");
                        }
                        else if (token[0] == '\"')
                        {
                            AddOperator("\"\"");
                            AddOperand(token);
                            // Поиск всех ${...} в token
                            var matches = Regex.Matches(token, @"\$\{[^}]*\}|\$\w+");

                            string newCode = "";
                            // Список для хранения обработанных значений
                            List<string> processedMatches = new List<string>();

                            foreach(var match in matches)
                            {
                                newCode += match + " ";
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
                            
                            foreach (var newLine in newLines)
                            {
                                var newTokens = ParseTokens(newLine);

                                foreach (var newToken in newTokens)
                                {
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
                                    }
                                    else
                                    {
                                        if (Regex.IsMatch(newToken, varargPattern))
                                        {
                                            AddOperator("*.");
                                        }
                                        else if (newToken == "(")
                                        {
                                            AddOperator("()");
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
                                                var fullFunToken = newToken + ')';
                                                AddOperator(fullFunToken);
                                                AddOperator("()");
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

