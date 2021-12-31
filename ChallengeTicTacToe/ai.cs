using System;
namespace ChallengeTicTacToe
{
    public class ai
    {
        private static Random rnd = new Random();

        public ai()
        {
        }

        public char MakeMove(Player aiPlayer, GameBoard gameBoard)
        {
            char aiMove;

            aiMove = PickWinningMove(aiPlayer, gameBoard);

            if (aiMove != ' ')
            {
                return aiMove;
            }

            return PickRandomMove(gameBoard);
        }

        public char PickRandomMove(GameBoard gameBoard)
        {
            string availableFields = "";
            int numberFields = 0;

            // First see what positions are available
            for (int i = 1; i < 10; i++)
            {
                char value = gameBoard.GetPosition(i.ToString()[0]);

                if (value != 'X' && value != 'O')
                {
                    availableFields += value;
                    numberFields++;
                }
            }

            char[] pickFrom = availableFields.ToCharArray();

            int pick = rnd.Next(0, numberFields);

            //System.Diagnostics.Debug.WriteLine($"Randomly picked {pick}");
            //Console.WriteLine($"Picked number {pick}.\npickFrom Length = {pickFrom.Length}");
            //Console.ReadLine();

            return pickFrom[pick];
        }

        public char PickWinningMove(Player aiPlayer, GameBoard gameBoard)
        {
            char winningMove = ' ';
            char[,] rowPositions = { { '1', '2', '3' }, { '4', '5', '6' }, { '7', '8', '9' } };
            char[,] colPositions = { { '1', '4', '7' }, { '2', '5', '8' }, { '3', '6', '9' } };
            char[] diagPositions = { '1', '5', '9' };
            char[] revDiagPositions = { '3', '5', '7' };

            if (aiPlayer.CheckDiagnalScore() == 2)
            {
                foreach (var position in diagPositions)
                {
                    winningMove = (char.IsNumber(gameBoard.GetPosition(position))) ? position : ' ';
                    if (winningMove != ' ')
                    {
                        return winningMove;
                    }
                }
            }

            if (aiPlayer.CheckReverseDiagnalScore() == 2)
            {
                foreach (var position in revDiagPositions)
                {
                    winningMove = (char.IsNumber(gameBoard.GetPosition(position))) ? position : ' ';
                    if (winningMove != ' ')
                    {
                        return winningMove;
                    }
                }
            }


            for (int i = 0; i < 3; i++)
            {
                if (aiPlayer.CheckRowScore(i) == 2)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        winningMove = (char.IsNumber(gameBoard.GetPosition(rowPositions[i,j]))) ? rowPositions[i,j] : ' ';
                        if (winningMove != ' ')
                        {
                            return winningMove;
                        }
                    }
                }

                if (aiPlayer.CheckColumnScore(i) == 2)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        winningMove = (char.IsNumber(gameBoard.GetPosition(colPositions[i, j]))) ? colPositions[i, j] : ' ';
                        if (winningMove != ' ')
                        {
                            return winningMove;
                        }
                    }
                }
            }

            return ' ';
        }
    }
}
