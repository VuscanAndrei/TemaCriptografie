using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PlayfairCipher
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- CIFRUL PLAYFAIR (5x5) ---");
                Console.WriteLine("1. Cripteaza");
                Console.WriteLine("2. Decripteaza");
                Console.WriteLine("3. Iesire");
                Console.Write("Optiune: ");

                string opt = Console.ReadLine() ?? "";
                if (opt == "3") break;

                Console.Write("Introdu cheia (ex: MONARCHY): ");
                string key = Console.ReadLine() ?? "";

                Console.Write("Introdu textul: ");
                string text = Console.ReadLine() ?? "";

                Playfair pf = new Playfair(key);
                pf.PrintMatrix(); // Afisam matricea pentru verificare vizuala

                if (opt == "1")
                {
                    string result = pf.Encrypt(text);
                    Console.WriteLine($"\nREZULTAT CRIPTAT: {result}");
                    // Afisam perechile pentru claritate
                    Console.WriteLine($"Perechi procesate: {FormatPairs(text, true)}");
                }
                else if (opt == "2")
                {
                    string result = pf.Decrypt(text);
                    Console.WriteLine($"\nREZULTAT DECRIPTAT: {result}");
                }
            }
        }

        static string FormatPairs(string text, bool encrypting)
        {
            // Doar o functie vizuala, logica reala e in clasa Playfair
            return "(Vezi logica interna pentru perechile exacte 'X' padding)";
        }
    }

    class Playfair
    {
        private char[,] _matrix = new char[5, 5];
        // Dictionar pentru lookup rapid O(1) al coordonatelor
        private Dictionary<char, (int row, int col)> _coords = new Dictionary<char, (int, int)>();

        public Playfair(string key)
        {
            GenerateMatrix(key);
        }

        private void GenerateMatrix(string key)
        {
            // 1. Curatam cheia: Doar litere, Upper, J->I
            string cleanKey = string.Join("", key.ToUpper().Where(char.IsLetter)).Replace('J', 'I');
            string alphabet = "ABCDEFGHIKLMNOPQRSTUVWXYZ"; // Fara J
            
            // 2. Construim sirul complet fara duplicate
            string fullKey = new string((cleanKey + alphabet).Distinct().ToArray());

            // 3. Umplem matricea
            int k = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _matrix[i, j] = fullKey[k];
                    _coords[fullKey[k]] = (i, j); // Salvam coordonatele
                    k++;
                }
            }
        }

        public void PrintMatrix()
        {
            Console.WriteLine("\nMatricea generata:");
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    Console.Write(_matrix[i, j] + " ");
                }
                Console.WriteLine();
            }
        }

        // Pregatirea textului (digrafe + padding X)
        private List<string> PrepareText(string input)
        {
            string clean = string.Join("", input.ToUpper().Where(char.IsLetter)).Replace('J', 'I');
            List<string> pairs = new List<string>();

            int i = 0;
            while (i < clean.Length)
            {
                char c1 = clean[i];
                char c2 = 'X'; // Default padding

                if (i + 1 < clean.Length)
                {
                    c2 = clean[i + 1];
                    
                    if (c1 == c2)
                    {
                        // Regula: Litere identice -> inseram X si nu avansam indexul pentru a doua litera
                        c2 = 'X';
                        i++; // Avansam doar 1 (am consumat c1)
                    }
                    else
                    {
                        i += 2; // Am consumat perechea c1, c2
                    }
                }
                else
                {
                    // Ultima litera singura -> adaugam X
                    i++;
                }

                pairs.Add($"{c1}{c2}");
            }
            return pairs;
        }

        public string Encrypt(string text) => Process(text, 1);
        public string Decrypt(string text) => Process(text, -1);

        private string Process(string input, int direction)
        {
            // La decriptare nu mai inseram X intre litere identice, 
            // lucram direct pe perechile din criptotext
            List<string> pairs;
            
            if (direction == 1) // Criptare
            {
                pairs = PrepareText(input);
            }
            else // Decriptare
            {
                // Pentru decriptare doar impartim in bucati de 2
                pairs = new List<string>();
                string clean = string.Join("", input.ToUpper().Where(char.IsLetter));
                for (int k = 0; k < clean.Length; k += 2)
                    if (k+1 < clean.Length) pairs.Add($"{clean[k]}{clean[k+1]}");
            }

            StringBuilder result = new StringBuilder();

            foreach (var pair in pairs)
            {
                char a = pair[0];
                char b = pair[1];

                (int r1, int c1) = _coords[a];
                (int r2, int c2) = _coords[b];

                if (r1 == r2) // Aceeasi linie: Shift orizontal
                {
                    // Modulo logic: (val + shift + 5) % 5 handles negative numbers correctly
                    int newC1 = (c1 + direction + 5) % 5;
                    int newC2 = (c2 + direction + 5) % 5;
                    result.Append(_matrix[r1, newC1]);
                    result.Append(_matrix[r2, newC2]);
                }
                else if (c1 == c2) // Aceeasi coloana: Shift vertical
                {
                    int newR1 = (r1 + direction + 5) % 5;
                    int newR2 = (r2 + direction + 5) % 5;
                    result.Append(_matrix[newR1, c1]);
                    result.Append(_matrix[newR2, c2]);
                }
                else // Dreptunghi: Swap coloane
                {
                    // Rindurile raman la fel, coloanele se schimba intre ele
                    result.Append(_matrix[r1, c2]);
                    result.Append(_matrix[r2, c1]);
                }
            }
            return result.ToString();
        }
    }
}