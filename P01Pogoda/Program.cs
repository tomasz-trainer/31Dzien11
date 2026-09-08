
using P06ZadaniePogoda;


Console.WriteLine("Hello, World!");


ManagerPogody mz = new ManagerPogody();
double temp = mz.PodajTemperature("warszawa", "C");
Console.WriteLine(temp);