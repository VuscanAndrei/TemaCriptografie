using System;
using System.Text;

namespace CifrulCezarN
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                Console.WriteLine("\n--- MENIU CAESAR GENERALIZAT (+n) ---");
                Console.WriteLine("1. Criptare (+n)");
                Console.WriteLine("2. Decriptare (-n)");
                Console.WriteLine("3. Criptanaliza (Brute Force)");
                Console.WriteLine("4. Iesire");
                Console.Write("Alege optiunea: ");

                string optiune = Console.ReadLine() ?? "";

                if (optiune == "4") break;

                // Citim textul o singura data, daca nu e criptanaliza
                string text = "";
                if (optiune == "1" || optiune == "2" || optiune == "3")
                {
                    Console.Write("Introdu textul: ");
                    text = Console.ReadLine() ?? "";
                    if (string.IsNullOrWhiteSpace(text)) 
                    {
                        Console.WriteLine("Eroare: Textul nu poate fi gol.");
                        continue;
                    }
                }

                switch (optiune)
                {
                    case "1": // Criptare
                        int nCrypt = CitesteN();
                        Console.WriteLine($"\n[Criptare n={nCrypt}] Rezultat: {CaesarCipher(text, nCrypt)}");
                        break;

                    case "2": // Decriptare
                        int nDecrypt = CitesteN();
                        // Decriptarea este o criptare cu cheia negativa (-n)
                        Console.WriteLine($"\n[Decriptare n={nDecrypt}] Rezultat: {CaesarCipher(text, -nDecrypt)}");
                        break;

                    case "3": // Criptanaliza
                        RunBruteForce(text);
                        break;

                    default:
                        Console.WriteLine("Optiune invalida. Concentreaza-te.");
                        break;
                }
            }
        }

        // Functie auxiliara pentru a valida ca userul introduce un numar, nu prostii
        static int CitesteN()
        {
            Console.Write("Introdu valoarea lui n (cheia): ");
            string input = Console.ReadLine() ?? "0";
            if (int.TryParse(input, out int n))
            {
                return n;
            }
            Console.WriteLine("Valoare invalida. Se foloseste n=0.");
            return 0;
        }

        static string CaesarCipher(string input, int shift)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in input)
            {
                // Orice caracter diferit de litera ramane nemodificat
                if (!char.IsLetter(c))
                {
                    result.Append(c);
                    continue;
                }

                // Determinam baza (A sau a)
                char offset = char.IsUpper(c) ? 'A' : 'a';
                
                // Formula matematica critica:
                // 1. (c - offset) aduce litera in intervalul 0-25
                // 2. + shift aplica deplasarea
                // 3. % 26 asigura rotatia (daca trecem de Z, revenim la A)
                // 4. + 26 si inca un % 26 gestioneaza numerele negative pentru decriptare
                int transformed = (((c - offset) + shift) % 26 + 26) % 26 + offset;

                result.Append((char)transformed);
            }

            return result.ToString();
        }

        static void RunBruteForce(string text)
        {
            Console.WriteLine($"\n--- CRIPTANALIZA PENTRU: {text} ---");
            Console.WriteLine("Generez toate cele 26 posibilitati (Spatiul cheilor 0..25):\n");

            for (int n = 0; n < 26; n++)
            {
                // Incercam decriptarea cu fiecare cheie posibila
                // Daca textul a fost criptat cu +n, il decriptam cu -n
                string attempt = CaesarCipher(text, -n);
                
                // Afisam n si rezultatul pentru ca omul sa identifice textul logic
                Console.WriteLine($"Cheia n={n,2}: {attempt}");
            }
        }
    }
}