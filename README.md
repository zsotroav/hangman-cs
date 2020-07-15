# Hangman game by zsotroav

This game was made by me as a public school project, so I've decided to share it on GitHub too, because why not.

## First run / After Update
Just make sure to copy `words.txt` next to the executable (on Windows).

If you are running it on Linux copy `words.txt` into the folder where you are running the game from.

> **Important:** There is no word list included in this project. For English words, I recommend [Xethron's](https://github.com/Xethron/Hangman/blob/master/words.txt) Hangman's word list.

## How to play?

Simple:
1. Open Hangman.exe (Compile it using Visual Studio 2019 or download the latest release)
2. Choose a difficulty
    - 1 gives you 9 options to fail
    - 2 gives 7
    - 3 gives 5
    _Hint: you can see how long the word is at the end of the question_
3. Guess! You are in the game, so you just need to press one of the buttons on your keyboard
4. If you guessed right, you'll see all occurrences of that letter on the top of the Console.
5. If you guessed wrong, you'll see that letter in the **Wrong Letters** Line.
6. Have fun! If you managed to guess the word right or you lost, you can start a new round with pressing the `Y` key.
    - Once a new game starts, jump back to step 2.

## I'm a nerd, where's the code?

I guess you've already found it, since you are reading the GitHub readme, so it's available here, under the `AGPL 3.0` license. 

## Some stats
Built on C# .NET Framework 4.7.2 (Windows version, available on GitHub)
Ported to C# .NET Core 3.1 (For Linux and macOS, Soon-to-be on GitHub)
~366 lines of code
