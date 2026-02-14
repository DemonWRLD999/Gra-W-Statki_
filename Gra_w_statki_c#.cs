using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    const int rozmiar = 10;

    static char[,] planszaGracza = new char[rozmiar, rozmiar];
    static char[,] strzalyGracza = new char[rozmiar, rozmiar];

    static char[,] planszaBota = new char[rozmiar, rozmiar];
    static char[,] strzalyBota = new char[rozmiar, rozmiar];

    static List<int> statki = new List<int> { 4, 3, 3, 2, 2, 2, 1, 1, 1, 1 };

    static List<List<(int, int)>> statkiGracza = new();
    static List<List<(int, int)>> statkiBota = new();

    static Random los = new Random();

    static void Main()
    {
        zrobPlansze();

        Console.WriteLine("ustaw statki gracza");
        ustawStatkiGracza();

        Console.Clear();
        Console.WriteLine("bot ustawia statki");
        ustawStatkiBota();

        gra();
        koniec();
    }

    static void zrobPlansze()
    {
        for (int i = 0; i < rozmiar; i++)
        {
            for (int j = 0; j < rozmiar; j++)
            {
                planszaGracza[i, j] = '~';
                strzalyGracza[i, j] = '~';
                planszaBota[i, j] = '~';
                strzalyBota[i, j] = '~';
            }
        }
    }

    static void ustawStatkiGracza()
    {
        foreach (int dlugosc in statki)
        {
            bool postawiony = false;

            while (!postawiony)
            {
                pokazPlansze(planszaGracza);
                Console.WriteLine("ustaw statek o dlugosci " + dlugosc);

                int wiersz = podajWiersz("podaj wiersz a-j: ");
                int kolumna = podajKolumne("podaj kolumne 0-9: ");

                Console.Write("kierunek h poziomo v pionowo: ");
                string kierunek = Console.ReadLine().ToLower();

                bool poziomo = kierunek == "h";

                if (kierunek != "h" && kierunek != "v")
                {
                    Console.WriteLine("niepoprawny kierunek");
                    Console.ReadKey();
                    continue;
                }

                if (czyMoznaPostawic(planszaGracza, wiersz, kolumna, dlugosc, poziomo))
                {
                    List<(int, int)> nowy = new List<(int, int)>();

                    for (int i = 0; i < dlugosc; i++)
                    {
                        int r = wiersz + (poziomo ? 0 : i);
                        int c = kolumna + (poziomo ? i : 0);
                        planszaGracza[r, c] = 'S';
                        nowy.Add((r, c));
                    }

                    statkiGracza.Add(nowy);
                    postawiony = true;
                }
                else
                {
                    Console.WriteLine("nie mozna tu postawic statku");
                    Console.ReadKey();
                }
            }
        }
    }

    static void ustawStatkiBota()
    {
        foreach (int dlugosc in statki)
        {
            bool postawiony = false;

            while (!postawiony)
            {
                int wiersz = los.Next(rozmiar);
                int kolumna = los.Next(rozmiar);
                bool poziomo = los.Next(2) == 0;

                if (czyMoznaPostawic(planszaBota, wiersz, kolumna, dlugosc, poziomo))
                {
                    List<(int, int)> nowy = new List<(int, int)>();

                    for (int i = 0; i < dlugosc; i++)
                    {
                        int r = wiersz + (poziomo ? 0 : i);
                        int c = kolumna + (poziomo ? i : 0);
                        planszaBota[r, c] = 'S';
                        nowy.Add((r, c));
                    }

                    statkiBota.Add(nowy);
                    postawiony = true;
                }
            }
        }
    }

    static bool czyMoznaPostawic(char[,] plansza, int wiersz, int kolumna, int dlugosc, bool poziomo)
    {
        for (int i = 0; i < dlugosc; i++)
        {
            int r = wiersz + (poziomo ? 0 : i);
            int c = kolumna + (poziomo ? i : 0);

            if (r >= rozmiar || c >= rozmiar)
                return false;

            for (int dr = -1; dr <= 1; dr++)
            {
                for (int dc = -1; dc <= 1; dc++)
                {
                    int nr = r + dr;
                    int nc = c + dc;

                    if (nr >= 0 && nr < rozmiar && nc >= 0 && nc < rozmiar)
                        if (plansza[nr, nc] == 'S')
                            return false;
                }
            }
        }
        return true;
    }

    static void gra()
    {
        while (statkiGracza.Count > 0 && statkiBota.Count > 0)
        {
            turaGracza();
            if (statkiBota.Count == 0) break;
            turaBota();
        }
    }

    static void turaGracza()
    {
        Console.Clear();
        pokazDwiePlansze();

        int wiersz = podajWiersz("podaj wiersz a-j: ");
        int kolumna = podajKolumne("podaj kolumne 0-9: ");

        if (strzalyGracza[wiersz, kolumna] != '~')
        {
            Console.WriteLine("tu juz strzelales");
            Console.ReadKey();
            return;
        }

        if (planszaBota[wiersz, kolumna] == 'S')
        {
            Console.WriteLine("trafiony");
            strzalyGracza[wiersz, kolumna] = 'X';
            sprawdzZatopiony(statkiBota, strzalyGracza, wiersz, kolumna);
        }
        else
        {
            Console.WriteLine("pudlo");
            strzalyGracza[wiersz, kolumna] = 'O';
        }

        Console.ReadKey();
    }

    static void turaBota()
    {
        int wiersz, kolumna;

        do
        {
            wiersz = los.Next(rozmiar);
            kolumna = los.Next(rozmiar);
        } while (strzalyBota[wiersz, kolumna] != '~');

        Console.Clear();
        pokazDwiePlansze();

        if (planszaGracza[wiersz, kolumna] == 'S')
        {
            strzalyBota[wiersz, kolumna] = 'X';
            planszaGracza[wiersz, kolumna] = 'X';
            Console.WriteLine("bot trafia w " + (char)('A' + wiersz) + kolumna);
            sprawdzZatopiony(statkiGracza, strzalyBota, wiersz, kolumna);
        }
        else
        {
            strzalyBota[wiersz, kolumna] = 'O';
            Console.WriteLine("bot pudlo w " + (char)('A' + wiersz) + kolumna);
        }

        Console.ReadKey();
    }

    static void sprawdzZatopiony(List<List<(int, int)>> listaStatkow, char[,] strzaly, int wiersz, int kolumna)
    {
        foreach (var statek in listaStatkow.ToList())
        {
            if (statek.Contains((wiersz, kolumna)))
            {
                bool zatopiony = statek.All(p => strzaly[p.Item1, p.Item2] == 'X');
                if (zatopiony)
                {
                    Console.WriteLine("statek zatopiony");
                    listaStatkow.Remove(statek);
                    Console.WriteLine("pozostalo statkow " + listaStatkow.Count);
                }
                break;
            }
        }
    }

    static void pokazPlansze(char[,] plansza)
    {
        Console.WriteLine("   0 1 2 3 4 5 6 7 8 9");
        for (int i = 0; i < rozmiar; i++)
        {
            Console.Write((char)('A' + i) + "  ");
            for (int j = 0; j < rozmiar; j++)
                Console.Write(plansza[i, j] + " ");
            Console.WriteLine();
        }
    }

    static void pokazDwiePlansze()
    {
        Console.WriteLine("twoje strzaly".PadRight(25) + "strzaly bota");
        Console.WriteLine("   0 1 2 3 4 5 6 7 8 9".PadRight(25) + "   0 1 2 3 4 5 6 7 8 9");

        for (int i = 0; i < rozmiar; i++)
        {
            char litera = (char)('A' + i);

            Console.Write(litera + "  ");
            for (int j = 0; j < rozmiar; j++)
                Console.Write(strzalyGracza[i, j] + " ");

            Console.Write("      ");

            Console.Write(litera + "  ");
            for (int j = 0; j < rozmiar; j++)
                Console.Write(strzalyBota[i, j] + " ");

            Console.WriteLine();
        }

        Console.WriteLine();
    }

    static int podajWiersz(string tekst)
    {
        while (true)
        {
            Console.Write(tekst);
            string wejscie = Console.ReadLine().ToLower();
            if (wejscie.Length == 1 && wejscie[0] >= 'a' && wejscie[0] <= 'j')
                return wejscie[0] - 'a';
            Console.WriteLine("podaj litere a-j");
        }
    }

    static int podajKolumne(string tekst)
    {
        while (true)
        {
            Console.Write(tekst);
            if (int.TryParse(Console.ReadLine(), out int liczba))
                if (liczba >= 0 && liczba < rozmiar)
                    return liczba;
            Console.WriteLine("podaj liczbe 0-9");
        }
    }

    static void koniec()
    {
        Console.Clear();
        if (statkiBota.Count == 0)
            Console.WriteLine("wygral gracz");
        else
            Console.WriteLine("wygral bot");

        Console.ReadKey();
    }
}
