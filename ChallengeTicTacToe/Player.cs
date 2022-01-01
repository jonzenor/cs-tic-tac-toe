using System;
namespace ChallengeTicTacToe
{
    public class Player
    {
        public string Name { get; set; }
        public char Piece { get; }
        public string Difficulty { get; set; }
        private int[,] points = { { 0, 0, 0 },{ 0, 0, 0 },{ 0, 0, 0 } };


        public Player(string name, char piece)
        {
            this.Name = name;
            this.Piece = piece;
            this.Difficulty = "Player";
        }

        public void SetAILevel(int aiLevel)
        {
            switch (aiLevel)
            {
                case 0:
                    this.Difficulty = "Player";
                    this.Name = "Player 2";
                    break;
                case 1:
                    this.Difficulty = "Easy";
                    this.Name = "Sleepy";
                    break;
                case 2:
                    this.Difficulty = "Medium";
                    this.Name = "Arty";
                    break;
                case 3:
                    this.Difficulty = "Hard";
                    this.Name = "Einstein";
                    break;
                default:
                    this.Difficulty = "Player";
                    this.Name = "Player 2";
                    break;
            }
        }

        public void ResetScore()
        {
            for (int i = 0; i < 3; i++)
            {
                points[i, 0] = 0;
                points[i, 1] = 0;
                points[i, 2] = 0;
            }
        }

        public void TrackScore(char position)
        {
            switch (position)
            {
                case '1':
                    points[0, 0] = 1;
                    break;
                case '2':
                    points[0, 1] = 1;
                    break;
                case '3':
                    points[0, 2] = 1;
                    break;
                case '4':
                    points[1, 0] = 1;
                    break;
                case '5':
                    points[1, 1] = 1;
                    break;
                case '6':
                    points[1, 2] = 1;
                    break;
                case '7':
                    points[2, 0] = 1;
                    break;
                case '8':
                    points[2, 1] = 1;
                    break;
                case '9':
                    points[2, 2] = 1;
                    break;
                default:
                    throw new Exception("Invalid game board position");
            }

        }

        public bool Won()
        {
            for (int i = 0; i < points.GetLength(0); i++)
            {
                // Check if the user won on this row
                if (points[i,0] + points[i,1] + points[i,2] == 3)
                {
                    return true;
                }

                // Check if the user won down this column
                if (points[0,i] + points[1,i] + points[2,i] == 3)
                {
                    return true;
                }

            }

            // Check if the user won down the diagnal
            if(points[0,0] + points[1,1] + points[2,2] == 3)
            {
                return true;
            }

            // Check if the user won down the opposite diagnal
            if (points[0,2] + points[1,1] + points[2,0] == 3)
            {
                return true;
            }

            return false;
        }

        public int CheckRowScore(int row)
        {
            return points[row, 0] + points[row, 1] + points[row, 2];
        }

        public int CheckColumnScore(int col)
        {
            return points[0, col] + points[1, col] + points[2, col];
        }

        public int CheckDiagnalScore()
        {
            return points[0, 0] + points[1, 1] + points[2, 2];
        }

        public int CheckReverseDiagnalScore()
        {
            return points[0, 2] + points[1, 1] + points[2, 0];
        }
    }
}
