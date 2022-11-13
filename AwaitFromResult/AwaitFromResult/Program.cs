using AwaitFromResult;
using System.Threading.Tasks;

var watch = System.Diagnostics.Stopwatch.StartNew();

var isAwait = false;

ITasks tasks = isAwait ? new TasksAwait() : new TasksFromResult();

var paoTask = tasks.Make("Pão", 3000);
var presuntoTask = tasks.Make("Presunto", 2000);
var queijoTask = tasks.Make("Queijo", 1000);

var pao = await paoTask;
var presunto = await presuntoTask;
var queijo = await queijoTask;

var suco = "Pronto";

if (pao == "Pronto" && presunto == "Pronto" && queijo == "Pronto")
{
    Console.WriteLine("Sandubinha pronto!");
}

if (suco == "Pronto")
{
    Console.WriteLine("Suco pronto!");
}

watch.Stop();
var elapsedMs = watch.ElapsedMilliseconds;

Console.WriteLine(elapsedMs + " Milliseconds");