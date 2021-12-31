using System;

namespace ChallengeTicTacToe
{
    class Program
    {
        // Setup the primary objects
        private static GameBoard gameBoard = new GameBoard();
        private static Player player1 = new Player("Player 1", 'X');
        private static Player player2 = new Player("Player 2", 'O');

        // Add the players to an array so we can access them by turn order
        private static Player[] players = { player1, player2 };

        // Setup some tracking vars
        private static bool gameOver = false;
        private static int playersTurn = 0;

        static void Main(string[] args)
        {

            // Ask the players for their names
            player1.Name = AskTheUser("What is Player 1's name?");
            player2.Name = AskTheUser("What is Player 2's name?");

            // Start the game loop
            while (true)
            {
                // If the game is over, ask the player if they want to play again
                if (gameOver)
                {
                    if (PlayersWantToQuit())
                    {
                        break;
                    }
                }

                // Set some tracking variables
                char field = ' ';

                // Print the game board
                gameBoard.PrintBoard();

                // Get the position the player wants to play in
                while (true)
                {
                    field = ChooseYourField(players[playersTurn].Name);

                    // Make sure the input was valid, which means it is not a player piece and a valid number
                    if (ValidFieldPlacement(field))
                    {
                        break;
                    }

                    Console.WriteLine("Invalid entry. Please try again. Use 0 to exit.");
                }

                // Exit the game if the user wanted to exit
                if (field == '0')
                {
                    Console.WriteLine("Quiting game. That was probalby a good idea. You likely would have lost.");
                    gameOver = true;
                    continue; // We have the game over conditions at the top of the loop, so go back to the top
                }

                // Play the piece for the player and add it to their personal score
                gameBoard.playPiece(field, players[playersTurn].Piece);
                players[playersTurn].TrackScore(field);

                // See if the player won
                if (players[playersTurn].Won())
                {
                    CongratulateWinner();
                    continue;
                }

                // See if there are still open positions
                if (!gameBoard.OpenPositions())
                {
                    Console.WriteLine("Cat's game. Better luck next time.");
                    gameOver = true;
                }

                TogglePlayersTurn();
            }

            Console.WriteLine("Thank you for playing with us!");

        }

        public static string AskTheUser(string questionToAsk)
        {
            Console.Write(questionToAsk + " ");
            return Console.ReadLine();
        }

        public static char ChooseYourField(string playerName)
        {
            string field = AskTheUser($"{playerName}: Choose your field!");
            return field[0];
        }

        public static void ResetGame()
        {
            gameBoard.ResetBoard();
            player1.ResetScore();
            player2.ResetScore();
        }

        public static void TogglePlayersTurn()
        {
            playersTurn = (playersTurn == 1) ? 0 : 1;
        }

        public static void CongratulateWinner()
        {
            // Redraw the game board so the player sees that he won
            gameBoard.PrintBoard();

            Console.WriteLine($"Congratulations, {players[playersTurn].Name}! You won!");
            gameOver = true;

            // Just in case they want to play again, let the loser start first next time
            TogglePlayersTurn();
        }

        public static bool PlayersWantToQuit()
        {
            string again = AskTheUser("\n\nWant to play again? (yes or no)");

            if (again.ToLower() == "yes" || again.ToLower() == "y")
            {
                ResetGame();
                gameOver = false;

                return false;
            }

            return true;
        }

        public static bool ValidFieldPlacement(char field)
        {
            if (char.IsNumber(field) && char.IsNumber(gameBoard.GetPosition(field)))
            {
                return true;
            }

            return false;
        }
    }
}
