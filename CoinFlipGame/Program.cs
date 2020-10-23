using System;

namespace CoinFlipGame
{
    class Program
    {
        private static readonly Random _rng = new Random();
        static void Main(string[] args)
        {
            ConsoleKey playAgain = ConsoleKey.Y;
            string playerOne = null, playerTwo = null;
            while (playAgain == ConsoleKey.Y)
            {
                Console.Clear();
                //Check if same players or new players
                if (string.IsNullOrWhiteSpace(playerOne) || string.IsNullOrWhiteSpace(playerOne))
                {
                    //Get Names
                    Console.Write("Please enter the name of Player 1: ");
                    playerOne = Console.ReadLine();
                    Console.Write("Please enter the name of Player 2: ");
                    playerTwo = Console.ReadLine();
                }
                else
                {
                    Console.Write($"Are {playerOne} and {playerTwo} still playing? Y/N: ");
                    if(Console.ReadKey().Key != ConsoleKey.Y)
                    {
                        playerOne = playerTwo = null;
                        continue;
                    }
                }

                //Pick callout player
                bool playerOneChosen = ChoosePlayer(playerOne, playerTwo);

                //Call out side
                bool choseHeads = ChooseCoinSide(playerOneChosen ? playerOne : playerTwo);

                //Flip coin
                if (playerOneChosen)
                {
                    FlipCoin(playerOne, playerTwo, choseHeads);
                }
                else
                {
                    FlipCoin(playerTwo, playerOne, choseHeads);
                }

                //Ask to play again
                Console.WriteLine("\nDo you want to play again? Y/N\n");
                ConsoleKeyInfo pressedKey = Console.ReadKey();
                playAgain = pressedKey.Key;
            }
        }

        static bool ChoosePlayer(string playerOne, string playerTwo)
        {
            bool playerOneChosen;
            Console.Clear();
            Console.WriteLine("Who is choosing a side of the coin?");

            string p1Line, p2Line;
            char p1OptionChar, p2OptionChar;
            (p1Line, p2Line, p1OptionChar, p2OptionChar) = (playerOne[0], playerTwo[0]) switch
            {
                (char p1, char p2) when (p1 != p2) => ($"[{playerOne[0]}]{playerOne[1..]}", $"[{playerTwo[0]}]{playerTwo[1..]}", playerOne[0], playerTwo[0]),
                _ => ($"1: {playerOne}", $"2: {playerTwo}", '1', '2')
            };

            Console.WriteLine(p1Line);
            Console.WriteLine(p2Line);
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            if (char.ToLower(pressedKey.KeyChar) == char.ToLower(p1OptionChar))
                playerOneChosen = true;
            else if (char.ToLower(pressedKey.KeyChar) == char.ToLower(p2OptionChar))
                playerOneChosen = false;
            else
                playerOneChosen = ChoosePlayer(playerOne, playerTwo);

            return playerOneChosen;
        }

        static bool ChooseCoinSide(string playerName)
        {
            bool choseHeads;
            Console.Clear();
            Console.WriteLine($"{playerName} do you pick:");
            Console.WriteLine("[H]eads or [T]ails");
            ConsoleKeyInfo pressedKey = Console.ReadKey();

            choseHeads = pressedKey.Key switch
            {
                ConsoleKey.H => true,
                ConsoleKey.T => false,
                _ => ChooseCoinSide(playerName)
            };

            return choseHeads;
        }

        static void FlipCoin(string calloutPlayer, string otherPlayer, bool choseHeads)
        {
            Console.Clear();
            int flipResult = _rng.Next(1, 1001);
            string resultMessage = "Coin flip resulted in ";
            resultMessage = (flipResult % 2 == 0, choseHeads) switch
            {
                (true, true) => resultMessage + $"Heads up.\n\n{calloutPlayer} you've won!",
                (true, false) => resultMessage + $"Heads up.\n\n{otherPlayer} you've won!",
                (false, true) => resultMessage + $"Tails up.\n\n{otherPlayer} you've won!",
                (false, false) => resultMessage + $"Tails up.\n\n{calloutPlayer} you've won!",
            };
            Console.WriteLine(resultMessage);
        }
    }
}