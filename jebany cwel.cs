using System;

class Program
{
	static string[,] plansza = new string[10, 10];

	static void Main()
	{
		InicjalizujPlansze();
		RysujPlansze();

		while (true)
		{
			Console.Write("Podaj strzał (np. A5): ");
			string input = Console.ReadLine().ToUpper();

			if (input.Length < 2) continue;

			int kolumna = input[0] - 'A';
			int wiersz = int.Parse(input.Substring(1)) - 1;

			if (kolumna >= 0 && kolumna < 10 && wiersz >= 0 && wiersz < 10)
			{
				plansza[wiersz, kolumna] = " X ";
			}

			Console.Clear();
			RysujPlansze();
		}
	}

	static void InicjalizujPlansze()
	{
		for (int i = 0; i < 10; i++)
			for (int j = 0; j < 10; j++)
				plansza[i, j] = "I~I";
	}

	static void RysujPlansze()
	{
		Console.Write("   ");
		for (char c = 'A'; c <= 'J'; c++)
			Console.Write($" {c}  ");
		Console.WriteLine();

		for (int i = 0; i < 10; i++)
		{
			Console.Write((i + 1).ToString().PadLeft(2) + " ");

			for (int j = 0; j < 10; j++)
			{
				Console.Write(plansza[i, j] + " ");
			}

			Console.WriteLine();
		}
	}
}
