using System;
using System.Text;

namespace VigenereMix
{
    class Program
    {
        // 0. Alfabetul Standard (Input) - pentru a gasi pozitia literei clare
        static string STANDARD = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        // Cele 3 Alfabete de substitutie (Cheile)
        static string[] ALFABETE = new string[]
        {
            "FRQSPYMHNJOELKDVAGXTIWBUZC", // Alfabetul 1 (index 0)
            "SWZCINJTELAFQUMKPXDOVBRGHY", // Alfabetul 2 (index 1)
            "CITYWLNZEVOMQGUPJFXBRAHSKD"  // Alfabetul 3 (index 2)
        };

        static void Main(string[] args)
        {
            // Testul cerut de tine explicit
            string inputDemo = "ABB BCC AB";
            Console.WriteLine("--- DEMONSTRATIE (DATELE TALE) ---");
            Console.WriteLine($"Text Clar: {inputDemo}");
            
            string criptat = Cripteaza(inputDemo);
            Console.WriteLine($"Criptat:   {criptat}");
            
            string decriptat = Decripteaza(criptat);
            Console.WriteLine($"Decriptat: {decriptat}");

            Console.WriteLine("\n--- TEST MANUAL ---");
            Console.Write("Introdu alt text: ");
            string userText = Console.ReadLine() ?? "";
            Console.WriteLine($"Rezultat: {Cripteaza(userText)}");
        }

        static string Cripteaza(string input)
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0; // Contorizam doar literele valide pentru a pastra ciclicitatea 1-2-3

            input = input.ToUpper();

            foreach (char c in input)
            {
                if (!STANDARD.Contains(c))
                {
                    // Pastram spatiile sau semnele, dar NU incrementam counterul?
                    // In exemplul tau (ABB)(BCC), spatiul reseteaza ciclul sau e ignorat?
                    // In criptografie, de obicei spatiile se ignora complet la numaratoare.
                    sb.Append(c); 
                    continue;
                }

                // 1. Aflam pozitia literei in alfabetul standard (ex: A=0, B=1)
                int indexInStandard = STANDARD.IndexOf(c);

                // 2. Selectam alfabetul curent (0, 1 sau 2)
                int indexAlfabet = counter % 3;
                string alfabetCurent = ALFABETE[indexAlfabet];

                // 3. Luam litera de la aceeasi pozitie din alfabetul mixat
                sb.Append(alfabetCurent[indexInStandard]);

                counter++;
            }

            return sb.ToString();
        }

        static string Decripteaza(string input)
        {
            StringBuilder sb = new StringBuilder();
            int counter = 0;

            input = input.ToUpper();

            foreach (char c in input)
            {
                if (!STANDARD.Contains(c)) // Verificam daca e litera valida
                {
                    sb.Append(c);
                    continue;
                }

                // 1. Selectam alfabetul curent (acelasi ca la criptare)
                int indexAlfabet = counter % 3;
                string alfabetCurent = ALFABETE[indexAlfabet];

                // 2. Cautam unde apare litera criptata in acest alfabet mixat
                int indexInMix = alfabetCurent.IndexOf(c);

                // 3. Luam litera de la aceeasi pozitie din alfabetul STANDARD
                sb.Append(STANDARD[indexInMix]);

                counter++;
            }

            return sb.ToString();
        }
    }
}