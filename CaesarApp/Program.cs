using System;
using System.Text;

namespace CaesarApp
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- MENIU CAESAR ---");
                Console.WriteLine("1. Criptare (+3)");
                Console.WriteLine("2. Decriptare (-3)");
                Console.WriteLine("3. Criptanaliza (Brute Force)");
                Console.WriteLine("4. Iesire");
                Console.Write("Alege optiunea: ");

                string optiune = Console.ReadLine();

                if (optiune == "4") break;

                Console.Write("Introdu textul: ");
                string text = Console.ReadLine();

                switch (optiune)
                {
                    case "1":
                        Console.WriteLine($"\nRezultat Criptat: {CaesarCipher(text, 3)}");
                        break;
                    case "2":
                        Console.WriteLine($"\nRezultat Decriptat: {CaesarCipher(text, -3)}");
                        break;
                    case "3":
                        RunBruteForce(text);
                        break;
                    default:
                        Console.WriteLine("Optiune invalida.");
                        break;
                }
            }
        }

        // Functia unica pentru Criptare si Decriptare
        // Logica este: (Caracter + Shift) % 26
        static string CaesarCipher(string input, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                if (!char.IsLetter(c))
                {
                    result.Append(c);
                    continue;
                }

                char offset = char.IsUpper(c) ? 'A' : 'a';
                
                // Formula matematica pentru wrap-around corect (gestioneaza si shift negativ)
                // (c - offset + shift) % 26 poate da rezultate negative in C#, asa ca adaugam 26
                int transformed = (((c - offset) + shift) % 26 + 26) % 26 + offset;

                result.Append((char)transformed);
            }

            return result.ToString();
        }

        // Criptanaliza: Testeaza toate cele 25 de posibilitati de shiftare
        static void RunBruteForce(string text)
        {
            Console.WriteLine("\n--- RAPORT CRIPTANALIZA ---");
            Console.WriteLine("Incercam toate cheile posibile pentru a gasi textul clar:\n");

            for (int s = 1; s < 26; s++)
            {
                // Decriptam presupunand ca 's' a fost cheia (deci aplicam -s)
                string attempt = CaesarCipher(text, -s);
                Console.WriteLine($"Shift {s,2}: {attempt}");
            }
        }
    }
}