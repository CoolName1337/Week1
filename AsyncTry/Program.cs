using System.Diagnostics;

MyFile[] files = {
    new("file1", 1000),
    new("file2", 3000),
    new("file3", 2000),
    new("file4", 1000),
};

Stopwatch stopwatch = Stopwatch.StartNew();
stopwatch.Start();

//////////////// Синхронно /////////////////
Console.WriteLine("Начало синхронной \"обработки\" файлов");
foreach (var file in files)
{
    Console.WriteLine(ProcessData(file));
}
Console.WriteLine("Конец синхронной \"обработки\" файлов"); // Все четверо.... в потоке...
////////////////////////////////////////////

stopwatch.Stop();
Console.WriteLine("На синхронную обработку было затрачено:" + stopwatch.ElapsedMilliseconds / 1000f + "с");


stopwatch.Restart();

//////////////// Асинхронно ////////////////
Console.WriteLine("Начало асинхронной \"обработки\" файлов");
List<Task<string>> tasks = new List<Task<string>>();
foreach (var file in files)
{
    tasks.Add(ProcessDataAsync(file));
}
// Еще какая либо работа в основном потоке пока выполняются задачи во "внешних" потоках
MainThreadWork(2500);

foreach (var task in tasks)
    Console.WriteLine(await task);
Console.WriteLine("Конец асинхронной \"обработки\" файлов");
///////////////////////////////////////////

stopwatch.Stop();
Console.WriteLine("На асинхронную обработку было затрачено:" + stopwatch.ElapsedMilliseconds / 1000f + "с");

void MainThreadWork(int TimeToWork)
{
    for (int i = 0; i < 5; i++)
    {
        Thread.Sleep(TimeToWork / 5);
        Console.WriteLine($"Еще какая-то работа в {Thread.CurrentThread.ManagedThreadId} потоке");
    }
}

string ProcessData(MyFile file)
{
    Thread.Sleep(file.TimeToProcess);
    Console.WriteLine($"{file.Name} обработан");
    return $"{file.Name} обработан за {file.TimeToProcess} в {Thread.CurrentThread.ManagedThreadId} потоке";
}

async Task<string> ProcessDataAsync(MyFile file)
{
    await Task.Delay(file.TimeToProcess);
    Console.WriteLine($"{file.Name} обработан");
    return $"{file.Name} обработан за {file.TimeToProcess} в {Thread.CurrentThread.ManagedThreadId} потоке";
}


public record MyFile(string Name, int TimeToProcess);
