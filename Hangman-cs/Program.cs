/*
 * Hangman game by zsotroav
 * (c) 2020 zsotroav
 * Licensed under the GNU Affero General Public License v3.0 (GNU AGPL 3.0)
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HangmanCs
{
    class Program
    {
        static int[][] Lvl = new int[3][]; // Difficulty levels are stored in this

        static void Main()
        {
            string WordsLoc = "words.txt";  // Default words list

            if (File.Exists("wordlistloc.hangman.zsotr"))   // Option to save/load wordlist from anywhere (with config file)
            {
                StreamReader srlist = new StreamReader("wordlistloc.hangman.zsotr");
                WordsLoc = srlist.ReadLine();
                srlist.Close();
            }
            Console.Clear();

            Lvl[0] = new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };    // Define which frames to use in different difficulty levels.
            Lvl[1] = new int[] { 1, 3, 4, 5, 6, 7, 8, 9 };
            Lvl[2] = new int[] { 1, 4, 6, 7, 8, 9 };

            if (File.Exists(WordsLoc))
            {
                int i = 0;
                StreamReader SrChk = new StreamReader(WordsLoc); // Count the number of words in the text file (one word is one line)
                while (!SrChk.EndOfStream)
                {
                    SrChk.ReadLine();
                    i++;
                }
                SrChk.Close();

                string[] words = new string[i];

                StreamReader SrLoad = new StreamReader(WordsLoc); // Load the words into a string[]
                for (int f = 0; f < i; f++)
                {
                    words[f] = SrLoad.ReadLine().ToLower();
                }
                SrLoad.Close();

                GameStart(i, words); // Hand off logic to initialization

            }
            else // Throw errors saying that we can't find the word list
            {

                Console.WriteLine("Hangman game by zsotroav.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Can't find word list \"{0}\"", WordsLoc);
                Console.ResetColor();

                Console.WriteLine("\nPress Any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        static void GameStart(int Number, string[] WordList)
        {
            Random Ran = new Random();
            int Rand = Ran.Next(1, Number);
            string Word = WordList[Rand];               // Get a random word from the given list

            char[] WordChars = new char[Word.Length];   // Cut the word into letters (chars)
            for (int i = 0; i < Word.Length; i++)
            {
                WordChars[i] = char.Parse(Word.Substring(i, 1));
            }

            Console.WriteLine("Hangman game by zsotroav.");
            Console.WriteLine("Before we start... How hard do you want the game to be? (The word is {0} letters long) \n1 Gives you 9 tries \n2 Gives you 7 tries\n3 Gives you 5 tries", Word.Length);
            string Level = Console.ReadLine();
            if (int.TryParse(Level, out int Lvl))
            {
                Lvl = int.Parse(Level);
                if (Lvl > 3)
                {
                    Console.WriteLine("That is not a number between 1 and 3...");
                    Console.ReadKey();
                    Main();
                }
                else
                    Game(Word, WordChars, Lvl);      // Start the game logic
            }
        }

        static void Game(string Word, char[] WordCh, int Difficulty)
        {
            Console.Clear();
            // Initialize some important variables
            List<char> Wrong = new List<char> { };      // List of wrong letters guessed (shown)
            List<char> Guessed = new List<char> { };    // List of all letters guessed (hidden)
            int Lenght = Word.Length;   // For convenience
            int Guess = 0;              // Count the number off guesses, this is also used for drawing

            bool state = true;          // Is the game running?
            bool win = true;            // Did you win?
            int GuessedRight = 0;       // No. of right letters guessed
            string WordGuessed = "";    // The displayed word

            for (int f = 0; f < Lenght; f++)
            {
                WordGuessed += "_";
            }
            WordGuessed += " (" + Lenght + ")";

            while (state)
            {
                bool Correct = false;
                Console.SetCursorPosition(0, 0);    // Instead of Console.Clear(); we overwrite the currently displayed items.
                Console.WriteLine("Hangman game by zsotroav.");
                Console.WriteLine("Current word: " + WordGuessed + "\n\n");

                DrawState(Guess, Difficulty);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong letters: " + string.Join(" ", Wrong));
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\nNow Guess!");
                Console.ResetColor();

                char Input = Console.ReadKey().KeyChar.ToString().ToLower()[0]; // Weird way to read the pressed key and convert it to lower case


                for (int i = 0; i < Lenght; i++)
                {
                    if (Input == WordCh[i] && !Guessed.Contains(Input)) // If it's in the word and is not already guessed
                    {
                        GuessedRight++;
                        Correct = true;

                        if (i != Lenght)
                            WordGuessed = WordGuessed.Substring(0, i) + Input + WordGuessed.Substring(i + 1);
                        else
                            WordGuessed = WordGuessed.Substring(0, i) + Input;
                    }
                }

                Guessed.Add(Input);

                if (!Correct && !Wrong.Contains(Input) && !WordCh.Contains(Input)) // We guessed wrong, and we haven't guessed that yet
                {
                    Wrong.Add(Input);
                    Guess++; // Add to the "kill score"...

                    if (Guess > Lvl[Difficulty-1].Length - 2) // ...so that we can die
                    {
                        state = false; // stop the cycle
                        win = false;
                    }
                }

                if (GuessedRight == Lenght) // Did we win?
                    state = false;

            }

            //
            //Last displayed screen
            //
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Hangman game by zsotroav.");
            Console.WriteLine("Current word: " + WordGuessed + "\n\n");
            DrawState(Guess, Difficulty);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong letters: " + string.Join(" ", Wrong));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The correct word was \"{0}\"!", Word);
            Console.ResetColor();

            if (win)
            {
                GameWin();
            }
            else
                GameOver();
        }

        static void GameOver()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nSorry, but you ran out of tries!");
            Console.ResetColor();

            Console.WriteLine("\n\nDo you want to try again? (Y/N)");
            char response = Console.ReadKey().KeyChar;

            Console.Clear();

            if (response == 'y' || response == 'Y')
                Main(); // Try again "restarts" (jumps back to the beggining) the program 
        }

        static void GameWin()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nCongratulations you won!");
            Console.ResetColor();

            Console.WriteLine("\n\nDo you want to play once more? (Y/N)");
            char response = Console.ReadKey().KeyChar;

            Console.Clear();

            if (response == 'y' || response == 'Y')
                Main();
        }


        static void DrawState(int MissPoint, int Difficulty)
        {
            // Here we decide what stage we want dawn.
            int State = Lvl[Difficulty-1][MissPoint];

            Func<bool>[] States = {DrawState0, DrawState1, DrawState2, DrawState3, DrawState4, DrawState5, DrawState6, DrawState7, DrawState8, DrawState9};

            States[State]();
        }

        #region DrawStates

        /*
         * States Available for drawing
         * Not the best and/or most optimal, but it works.
         */

        static bool DrawState0()
        {
            for (int i = 0; i < 12; i++)
            {
                Console.Write("\n");
            }
            Console.WriteLine("▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState1()
        {
            for (int i = 0; i < 11; i++)
            {
                Console.Write("\n");
            }
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState2()
        {
            Console.WriteLine();
            for (int i = 0; i < 10; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState3()
        {
            Console.WriteLine("     ═════════╗");
            for (int i = 0; i < 10; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState4()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            for (int i = 0; i < 8; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState5()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            Console.WriteLine("  ╔═══╗       ║");
            Console.WriteLine("  ║   ║       ║");
            Console.WriteLine("  ╚═══╝       ║");
            for (int i = 0; i < 5; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState6()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            Console.WriteLine("  ╔═══╗       ║");
            Console.WriteLine("  ║   ║       ║");
            Console.WriteLine("  ╚═╦═╝       ║");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            for (int i = 0; i < 3; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState7()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            Console.WriteLine("  ╔═══╗       ║");
            Console.WriteLine("  ║   ║       ║");
            Console.WriteLine("  ╚═╦═╝       ║");
            Console.WriteLine("  ══╬══       ║ \n    ║         ║");
            for (int i = 0; i < 3; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState8()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            Console.WriteLine("  ╔═══╗       ║");
            Console.WriteLine("  ║   ║       ║");
            Console.WriteLine("  ╚═╦═╝       ║");
            Console.WriteLine("  ══╬══       ║ \n   ╔╩╗        ║ \n   ║ ║        ║");
            for (int i = 0; i < 2; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        static bool DrawState9()
        {
            Console.WriteLine("    ╔═════════╗"); 
            Console.WriteLine("    ║         ║ \n    ║         ║");
            Console.WriteLine("  ╔═══╗       ║");
            Console.WriteLine("  ║■ ■║       ║");
            Console.WriteLine("  ╚═╦═╝       ║");
            Console.WriteLine("  ══╬══       ║ \n   ╔╩╗        ║ \n   ║ ║        ║");
            for (int i = 0; i < 2; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }
        #endregion
    }
}
