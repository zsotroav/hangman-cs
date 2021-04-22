/*
 * Hangman game by zsotroav
 * Copyright 2021 zsotroav
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU Affero General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace HangmanCs
{
    internal class Program
    {
        private static int[][] _lvl = new int[3][]; // Difficulty levels are stored in this

        private static void Main()
        {
            var wordsLoc = "words.txt";  // Default words list
            
            Console.Clear();

            _lvl[0] = new[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };    // Define which frames to use in different difficulty levels.
            _lvl[1] = new[] { 1, 3, 4, 5, 6, 7, 8, 9 };
            _lvl[2] = new[] { 1, 4, 6, 7, 8, 9 };

            if (File.Exists(wordsLoc))
            {
                var i = 0;
                var srChk = new StreamReader(wordsLoc); // Count the number of words in the text file (one word is one line)
                while (!srChk.EndOfStream)
                {
                    srChk.ReadLine();
                    i++;
                }
                srChk.Close();

                var words = new string[i];

                var srLoad = new StreamReader(wordsLoc); // Load the words into a string[]
                for (var f = 0; f < i; f++)
                {
                    words[f] = srLoad.ReadLine().ToLower();
                }
                srLoad.Close();

                GameStart(i, words); // Hand off logic to initialization

            }
            else // Throw errors saying that we can't find the word list
            {

                Console.WriteLine("Hangman game by zsotroav.");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Can't find word list \"{0}\"", wordsLoc);
                Console.ResetColor();

                Console.WriteLine("\nPress Any key to exit...");
                Console.ReadKey();
                Environment.Exit(0);
            }
        }

        private static void GameStart(int number, string[] wordList)
        {
            var ran = new Random();
            var rand = ran.Next(1, number);
            var word = wordList[rand];               // Get a random word from the given list

            var wordChars = new char[word.Length];   // Cut the word into letters (chars)
            for (var i = 0; i < word.Length; i++)
            {
                wordChars[i] = char.Parse(word.Substring(i, 1));
            }

            Console.WriteLine("Hangman game by zsotroav.");
            Console.WriteLine("Before we start... How hard do you want the game to be? (The word is {0} letters long) \n1 Gives you 9 tries \n2 Gives you 7 tries\n3 Gives you 5 tries", word.Length);
            var level = Console.ReadLine();
            if (!int.TryParse(level, out var lvl)) return;

            lvl = int.Parse(level);
            if (lvl > 3)
            {
                Console.WriteLine("That is not a number between 1 and 3...");
                Console.ReadKey();
                Main();
            }
            else
                Game(word, wordChars, lvl);      // Start the game logic
        }

        private static void Game(string word, char[] wordCh, int difficulty)
        {
            Console.Clear();
            // Initialize some important variables
            var wrong = new List<char> { };      // List of wrong letters guessed (shown)
            var guessed = new List<char> { };    // List of all letters guessed (hidden)
            var length = word.Length;   // For convenience
            var guess = 0;              // Count the number off guesses, this is also used for drawing

            var state = true;          // Is the game running?
            var win = true;            // Did you win?
            var guessedRight = 0;       // No. of right letters guessed
            var wordGuessed = "";    // The displayed word

            for (var f = 0; f < length; f++)
            {
                wordGuessed += "_";
            }
            wordGuessed += " (" + length + ")";

            while (state)
            {
                var correct = false;
                Console.SetCursorPosition(0, 0);    // Instead of Console.Clear(); we overwrite the currently displayed items.
                Console.WriteLine("Hangman game by zsotroav.");
                Console.WriteLine("Current word: " + wordGuessed + "\n\n");

                DrawState(guess, difficulty);

                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Wrong letters: " + string.Join(" ", wrong));
                Console.ResetColor();

                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("\n\nNow Guess!");
                Console.ResetColor();

                var input = Console.ReadKey().KeyChar.ToString().ToLower()[0]; // Weird way to read the pressed key and convert it to lower case


                for (var i = 0; i < length; i++)
                {
                    if (input == wordCh[i] && !guessed.Contains(input)) // If it's in the word and is not already guessed
                    {
                        guessedRight++;
                        correct = true;

                        if (i != length)
                            wordGuessed = wordGuessed.Substring(0, i) + input + wordGuessed.Substring(i + 1);
                        else
                            wordGuessed = wordGuessed.Substring(0, i) + input;
                    }
                }

                guessed.Add(input);

                if (!correct && !wrong.Contains(input) && !wordCh.Contains(input)) // We guessed wrong, and we haven't guessed that yet
                {
                    wrong.Add(input);
                    guess++; // Add to the "kill score"...

                    if (guess > _lvl[difficulty-1].Length - 2) // ...so that we can die
                    {
                        state = false; // stop the cycle
                        win = false;
                    }
                }

                if (guessedRight == length) // Did we win?
                    state = false;

            }

            //
            //Last displayed screen
            //
            Console.SetCursorPosition(0, 0);

            Console.WriteLine("Hangman game by zsotroav.");
            Console.WriteLine("Current word: " + wordGuessed + "\n\n");
            DrawState(guess, difficulty);

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("Wrong letters: " + string.Join(" ", wrong));
            Console.ResetColor();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("The correct word was \"{0}\"!", word);
            Console.ResetColor();

            if (win)
            {
                GameWin();
            }
            else
                GameOver();
        }

        private static void GameOver()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nSorry, but you ran out of tries!");
            Console.ResetColor();

            Console.WriteLine("\n\nDo you want to try again? (Y/N)");
            var response = Console.ReadKey().KeyChar;

            Console.Clear();

            if (response == 'y' || response == 'Y')
                Main(); // Try again "restarts" (jumps back to the begging) the program 
        }

        private static void GameWin()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\nCongratulations you won!");
            Console.ResetColor();

            Console.WriteLine("\n\nDo you want to play once more? (Y/N)");
            var response = Console.ReadKey().KeyChar;

            Console.Clear();

            if (response == 'y' || response == 'Y')
                Main();
        }


        private static void DrawState(int missPoint, int difficulty)
        {
            // Here we decide what stage we want dawn.
            var state = _lvl[difficulty-1][missPoint];

            Func<bool>[] states = {DrawState0, DrawState1, DrawState2, DrawState3, DrawState4, DrawState5, DrawState6, DrawState7, DrawState8, DrawState9};

            states[state]();
        }

        #region DrawStates

        /*
         * States Available for drawing
         * Not the best and/or most optimal, but it works.
         */

        private static bool DrawState0()
        {
            for (int i = 0; i < 12; i++)
            {
                Console.Write("\n");
            }
            Console.WriteLine("▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        private static bool DrawState1()
        {
            for (int i = 0; i < 11; i++)
            {
                Console.Write("\n");
            }
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        private static bool DrawState2()
        {
            Console.WriteLine();
            for (int i = 0; i < 10; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        private static bool DrawState3()
        {
            Console.WriteLine("     ═════════╗");
            for (int i = 0; i < 10; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        private static bool DrawState4()
        {
            Console.WriteLine("    ╔═════════╗");
            Console.WriteLine("    ║         ║ \n    ║         ║");
            for (int i = 0; i < 8; i++)
                Console.WriteLine("              ║");
            Console.WriteLine("══════════════╩════ \n▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓▓");
            return true;
        }

        private static bool DrawState5()
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

        private static bool DrawState6()
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

        private static bool DrawState7()
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

        private static bool DrawState8()
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

        private static bool DrawState9()
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
