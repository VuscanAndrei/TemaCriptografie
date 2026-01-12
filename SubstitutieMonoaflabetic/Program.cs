using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SubstitutieGenerala
{
    class Program
    {
        // Alfabetul standard (sursa)
        static string ALFABET = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        
        // Frecventa standard in limba Engleza (de la cel mai des la cel mai rar)
        // Pentru Romana, ordinea ar fi aprox: E, I, A, R, N, T, C, U, L, S...
        static string FRECVENTA_ENGLEZA = "ETAOINSHRDLCUMWFGYPBVKJXQZ";

        static void Main(string[] args)
        {
            // Generam o cheie random la pornire
            string key = GenerateRandomKey();

            while (true)
            {
                Console.WriteLine($"\n--- SUBSTITUTIE MONOALFABETICA ---");
                Console.WriteLine($"Cheia curenta: {key}");
                Console.WriteLine($"Alfabet ref:   {ALFABET}");
                Console.WriteLine("1. Cripteaza");
                Console.WriteLine("2. Decripteaza");
                Console.WriteLine("3. Criptanaliza (Frecventa)");
                Console.WriteLine("4. Genereaza alta cheie");
                Console.WriteLine("5. Iesire");
                Console.Write("Optiune: ");

                string opt = Console.ReadLine() ?? "";

                if (opt == "5") break;
                if (opt == "4") 
                {
                    key = GenerateRandomKey();
                    continue;
                }

                Console.Write("Introdu textul: ");
                string text = (Console.ReadLine() ?? "").ToUpper();

                switch (opt)
                {
                    case "1":
                        Console.WriteLine($"\nCriptat: {Substitution(text, ALFABET, key)}");
                        break;
                    case "2":
                        // La decriptare, inversam rolul cheii cu alfabetul
                        Console.WriteLine($"\nDecriptat: {Substitution(text, key, ALFABET)}");
                        break;
                    case "3":
                        RunFrequencyAnalysis(text);
                        break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }
            }
        }

        // Algoritm Fisher-Yates pentru amestecare (Shuffle)
        static string GenerateRandomKey()
        {
            char[] chars = ALFABET.ToCharArray();
            Random rng = new Random();
            int n = chars.Length;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                var value = chars[k];
                chars[k] = chars[n];
                chars[n] = value;
            }
            return new string(chars);
        }

        // Functie universala de substitutie
        // inputMap = alfabetul sursa, outputMap = alfabetul destinatie
        static string Substitution(string input, string inputMap, string outputMap)
        {
            StringBuilder sb = new StringBuilder();
            foreach (char c in input)
            {
                int index = inputMap.IndexOf(c);
                if (index != -1)
                {
                    sb.Append(outputMap[index]);
                }
                else
                {
                    // Caracterele necunoscute (spatii, numere) raman la fel
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

        static void RunFrequencyAnalysis(string cipherText)
        {
            Console.WriteLine("\n--- RAPORT CRIPTANALIZA ---");
            
            // 1. Numaram aparitiile fiecarei litere in textul criptat
            var stats = new Dictionary<char, int>();
            foreach(char c in ALFABET) stats[c] = 0;
            
            int totalLetters = 0;
            foreach(char c in cipherText)
            {
                if (ALFABET.Contains(c))
                {
                    stats[c]++;
                    totalLetters++;
                }
            }

            // 2. Sortam literele din textul criptat dupa frecventa (Descrescator)
            var sortedStats = stats.OrderByDescending(x => x.Value).ToList();

            Console.WriteLine("Top frecvente in textul criptat:");
            foreach(var pair in sortedStats.Take(5))
            {
                double proc = totalLetters > 0 ? (pair.Value * 100.0 / totalLetters) : 0;
                Console.WriteLine($"'{pair.Key}': {pair.Value} ({proc:F1}%)");
            }

            // 3. Incercare automata de decriptare
            // Mapam cea mai frecventa litera din Criptotext cu cea mai frecventa din Engleza ('E')
            // A doua cu 'T', etc.
            
            char[] guessedKey = new char[26];
            // Initializam cu '?' pentru ce nu putem ghici
            for(int i=0; i<26; i++) guessedKey[i] = '?';

            Console.WriteLine("\nTentativa de mapare statistica (Naive):");
            
            // Construim o cheie inversa temporara
            // sortedStats[0].Key (ex: 'X') devine FRECVENTA_ENGLEZA[0] (ex: 'E')
            
            Dictionary<char, char> mapping = new Dictionary<char, char>();
            
            for(int i = 0; i < 26; i++)
            {
                char cipherChar = sortedStats[i].Key;
                char assumedPlain = FRECVENTA_ENGLEZA[i];
                mapping[cipherChar] = assumedPlain;
            }

            StringBuilder attempt = new StringBuilder();
            foreach(char c in cipherText)
            {
                if(mapping.ContainsKey(c))
                    attempt.Append(mapping[c]);
                else
                    attempt.Append(c);
            }

            Console.WriteLine($"\nText original (presupus): {attempt}");
            Console.WriteLine("NOTA: Aceasta metoda functioneaza bine doar pe texte LUNGI (>500 caractere).");
            Console.WriteLine("Pe texte scurte, frecventa literelor nu respecta statistica standard.");
        }
    }
}