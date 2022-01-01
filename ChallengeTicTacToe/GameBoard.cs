using System;
namespace ChallengeTicTacToe
{
    public class GameBoard
    {
        char[,] boardPosition = { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };

        public string turnOrder = "";
        public int OpenPositions { get; set; }

        public GameBoard()
        {
            this.OpenPositions = 9;
        }

        public void ResetBoard()
        {
            for (int i = 1; i <= 9; i++)
            {
                int[] grid = DecipherPosition(i.ToString()[0]);

                boardPosition[grid[0], grid[1]] = i.ToString()[0];
            }

            turnOrder = "";

            this.OpenPositions = 9;
        }

        public char GetPosition(char position)
        {
            if (position == '0')
            {
                return '0';
            }

            int[] grid = DecipherPosition(position);

            if (grid[0] == -1)
            {
                throw new Exception("Invalid grid position, " + position.ToString() );
            }

            return boardPosition[grid[0], grid[1]];
        }

        public void playPiece(char position, char piece)
        {

            int[] grid = DecipherPosition(position);

            if (grid[0] == -1)
            {
                throw new Exception("Invalid game board position");
            }

            boardPosition[grid[0], grid[1]] = piece;
            this.OpenPositions--;

            turnOrder += ":" + piece.ToString() + "-" + position.ToString() + " ";
        }

        private int[] DecipherPosition(char position)
        {
            int[] grid = new int[2];
            switch (position)
            {
                case '1':
                    grid[0] = 0;
                    grid[1] = 0;
                    break;
                case '2':
                    grid[0] = 0;
                    grid[1] = 1;
                    break;
                case '3':
                    grid[0] = 0;
                    grid[1] = 2;
                    break;
                case '4':
                    grid[0] = 1;
                    grid[1] = 0;
                    break;
                case '5':
                    grid[0] = 1;
                    grid[1] = 1;
                    break;
                case '6':
                    grid[0] = 1;
                    grid[1] = 2;
                    break;
                case '7':
                    grid[0] = 2;
                    grid[1] = 0;
                    break;
                case '8':
                    grid[0] = 2;
                    grid[1] = 1;
                    break;
                case '9':
                    grid[0] = 2;
                    grid[1] = 2;
                    break;
                default:
                    grid[0] = -1;
                    grid[1] = -1;
                    break;
            }

            return grid;
        }

        // See if there are any remaining positions on the game board
        public bool VerifyOpenPositions()
        {
            for (int i = 0; i < boardPosition.GetLength(0); i++)
            {
                if (char.IsNumber(boardPosition[i,0]) || char.IsNumber(boardPosition[i,1]) || char.IsNumber(boardPosition[i,2]))
                {
                    return true;
                }
            }

            return false;
        }

        public void PrintBoard()
        {

            // First clear the screen
            Console.Clear();


            for (int i = 0; i < boardPosition.GetLength(0); i++)
            {
                // Print the first row of padding
                Console.WriteLine("   |   |   ");

                // Print the main row of info
                char display1 = (char.IsNumber(boardPosition[i, 0])) ? ' ' : boardPosition[i, 0];
                char display2 = (char.IsNumber(boardPosition[i, 1])) ? ' ' : boardPosition[i, 1];
                char display3 = (char.IsNumber(boardPosition[i, 2])) ? ' ' : boardPosition[i, 2];
                //Console.WriteLine($" {boardPosition[i,0]} | {boardPosition[i,1]} | {boardPosition[i,2]} ");
                Console.WriteLine($" {display1} | {display2} | {display3} ");

                // Print the final row of padding
                if (i < 2)
                {
                    Console.WriteLine("___|___|___");
                }
                else
                {
                    Console.WriteLine("   |   |   ");
                }

            }

            Console.WriteLine("\n");
        }
    }
}
