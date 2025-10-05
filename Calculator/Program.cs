
// Есть возможность добавить другие операторы но требуется реализация в методе DoOperation
char[] operators = ['*', '/', '+', '-'];

bool WantToContinue = false;

double operand1, operand2;
char op;
double result;

do
{
    operand1 = GetNumberFromConsole("Введите первое число");
    CleanOutput($"{operand1}");

    op = GetOperatorFromConsole($"Выберите операцию:\n| {string.Join(" || ", operators)} |");
    CleanOutput($"{operand1}{op}");

    operand2 = GetNumberFromConsole("Введите второе число");
    CleanOutput($"{operand1}{op}{operand2}");

    // Реализация try catch блока (как пример) для обработки DivideByZeroException
    try
    {
        result = DoOperation();
        Console.WriteLine($"Ваш ответ:{result}");
    }
    catch (DivideByZeroException)
    {
        Console.WriteLine("Ошибка: Нельзя делить на ноль");
    }

    // Повторное использование
    Console.WriteLine("Для продолжения нажмите любую клавишу. Чтобы закончить нажмите Escape");
    WantToContinue = Console.ReadKey().Key != ConsoleKey.Escape;
    Console.Clear();

} while (WantToContinue);

void CleanOutput(string output = "")
{
    Console.Clear();
    Console.WriteLine(output);
}

double GetNumberFromConsole(string comment)
{
    Console.WriteLine(comment);
    do
    {
        // Чтобы не работать в данном случае с Nullable type использую оператор ??
        string input = Console.ReadLine() ?? "";

        if (double.TryParse(input, out double digitalInput))
            return digitalInput;

        Console.WriteLine("Неверный ввод. Пример ввода: | -6,7 | 13 | 37,00 |");
    } while (true);
}

char GetOperatorFromConsole(string comment)
{
    Console.WriteLine(comment);
    do
    {
        string input = Console.ReadLine() ?? "";

        if (input.Length == 1 && operators.Contains(input[0]))
            return input[0];

        Console.WriteLine("Неверный ввод");
    } while (true);
}

double DoOperation() => op switch
{
    '*' => operand1 * operand2,
    '/' => operand2 == 0 ? throw new DivideByZeroException() : operand1 / operand2,
    '+' => operand1 + operand2,
    '-' => operand1 - operand2,
    // Ошибка вызывается при использовании оператора но отсутствии его реализации
    _ => throw new NotImplementedException($"Not implemented operator: {op}")
};