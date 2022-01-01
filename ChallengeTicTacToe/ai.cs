using System;
namespace ChallengeTicTacToe
{
    public class ai
    {
        private static Random rnd = new Random();

        public ai()
        {
        }

        public char MakeMove(Player aiPlayer, GameBoard gameBoard, Player opponentPlayer)
        {
            char aiMove;

            // Try to pick a winning move
            aiMove = PickObviousMove(aiPlayer, gameBoard);

            if (aiMove != ' ')
            {
                return aiMove;
            }

            // Try to block the opponent
            if (aiPlayer.Difficulty == "Medium" || aiPlayer.Difficulty == "Hard")
            {
                aiMove = PickObviousMove(opponentPlayer, gameBoard);
            }

            if (aiMove != ' ')
            {
                return aiMove;
            }

            // Try to pick the smartest move
            if (aiPlayer.Difficulty == "Hard")
            {
                aiMove = PickBestMove(aiPlayer, gameBoard, opponentPlayer);
            }

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

        public char PickObviousMove(Player aiPlayer, GameBoard gameBoard)
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

        public char PickBestMove(Player aiPlayer, GameBoard gameBoard, Player opponentPlayer)
        {
            if (gameBoard.OpenPositions == 9)
            {
                char[] availablePositions = { '1', '3', '7', '9' };
                return availablePositions[rnd.Next(0, 3)];
            }

            if (gameBoard.OpenPositions == 8)
            {
                // See if the user took the center
                if (gameBoard.GetPosition('5') == opponentPlayer.Piece)
                {
                    // Pick a random corner
                    char[] availablePositions = { '1', '3', '7', '9' };
                    return availablePositions[rnd.Next(0, 3)];
                }

                // See if the user took a corner, then grab the center
                char playersCorner = CheckCorners(gameBoard, opponentPlayer.Piece);
                if (playersCorner != ' ')
                {
                    return '5';
                }

                // See if the user took an edge
                char playersEdge = CheckEdges(gameBoard, opponentPlayer.Piece);
                if (playersEdge != ' ')
                {
                    switch(playersEdge)
                    {
                        case '2':
                            return (rnd.Next(0, 1) == 0) ? '7' : '9';
                        case '4':
                            return (rnd.Next(0, 1) == 0) ? '3' : '9';
                        case '6':
                            return (rnd.Next(0, 1) == 0) ? '7' : '1';
                        case '8':
                            return (rnd.Next(0, 1) == 0) ? '1' : '3';
                        default:
                            return ' ';
                    }
                }
            }

            if (gameBoard.OpenPositions == 7)
            {
                // On our second turn, we only have a pice in a corner, so find it
                char ourCorner = CheckCorners(gameBoard, aiPlayer.Piece);

                // If the other player has the center, then we take an adjacent corner
                if (CheckCenter(gameBoard, opponentPlayer.Piece) != ' ')
                {
                    // Pick a random corner next to ours
                    return PickAdjacentCorner(ourCorner);
                }

                // See if the other player took the corner opposite of ours
                if (gameBoard.GetPosition(PickOppositeCorner(ourCorner)) == opponentPlayer.Piece)
                {
                    return PickAdjacentCorner(ourCorner);
                }

                // See if they other player took either other corner
                char opponentCorner = CheckCorners(gameBoard, opponentPlayer.Piece);
                if (opponentCorner != ' ')
                {
                    return PickOppositeCorner(ourCorner);
                }

                // See if the other player took an edge near us
                char opponentEdge = CheckNearEdgesToCorner(gameBoard, ourCorner, opponentPlayer.Piece);
                if (opponentEdge != ' ')
                {
                    // Pick the open corner (i.e. not blocked by opponent) adjacent to ours
                    if (ourCorner == '1' && opponentEdge == '2') { return '7'; }
                    if (ourCorner == '1' && opponentEdge == '4') { return '3'; }
                    if (ourCorner == '3' && opponentEdge == '2') { return '9'; }
                    if (ourCorner == '3' && opponentEdge == '6') { return '1'; }
                    if (ourCorner == '7' && opponentEdge == '4') { return '9'; }
                    if (ourCorner == '7' && opponentEdge == '8') { return '1'; }
                    if (ourCorner == '9' && opponentEdge == '6') { return '7'; }
                    if (ourCorner == '9' && opponentEdge == '8') { return '3'; }

                    return ' ';
                }

                // Finally, see if the other player took a far edge away from us
                opponentEdge = CheckFarEdgesToCorner(gameBoard, ourCorner, opponentPlayer.Piece);
                if (opponentEdge != ' ')
                {
                    // Pick the adjacent corner that is nearest their edge
                    if (ourCorner == '9' && opponentEdge == '2') { return '3'; }
                    if (ourCorner == '9' && opponentEdge == '4') { return '7'; }
                    if (ourCorner == '7' && opponentEdge == '2') { return '1'; }
                    if (ourCorner == '7' && opponentEdge == '6') { return '9'; }
                    if (ourCorner == '3' && opponentEdge == '4') { return '1'; }
                    if (ourCorner == '3' && opponentEdge == '8') { return '9'; }
                    if (ourCorner == '1' && opponentEdge == '6') { return '3'; }
                    if (ourCorner == '1' && opponentEdge == '8') { return '7'; }
                }

                return ' ';
            }

            if (gameBoard.OpenPositions == 6)
            {
                char ourCorner = CheckCorners(gameBoard, aiPlayer.Piece);
                char ourCenter = CheckCenter(gameBoard, aiPlayer.Piece);

                // Check for the one case where we have the center first
                if (ourCenter != ' ')
                {
                    // Now we must take an edge, take one near the oppoents edge first
                    char opponentsEdge = CheckEdges(gameBoard, opponentPlayer.Piece);

                    if (opponentsEdge != ' ')
                    {
                        return PickEdgeAdjacentToEdge(opponentsEdge);
                    }

                    // If the opponent does not have an edge, then they have the opposite corner, so pick any edge
                    return PickEdge();
                }

                // See if the opponent has the corner opposite of us
                char oppositeCorner = PickOppositeCorner(ourCorner);
                if (gameBoard.GetPosition(oppositeCorner) == opponentPlayer.Piece)
                {
                    // See if the opponent has the center, if so, we take either adjacent corner
                    if (gameBoard.GetPosition('5') == opponentPlayer.Piece)
                    {
                        return PickAdjacentCorner(ourCorner);
                    }

                    // See if the opponent took a middle piece near us
                    char opponentEdge = CheckNearEdgesToCorner(gameBoard, ourCorner, opponentPlayer.Piece);
                    if (opponentEdge != ' ')
                    {
                        if (ourCorner == '1' && opponentEdge == '4') { return '3'; }
                        if (ourCorner == '1' && opponentEdge == '2') { return '7'; }
                        if (ourCorner == '3' && opponentEdge == '6') { return '1'; }
                        if (ourCorner == '3' && opponentEdge == '2') { return '9'; }
                        if (ourCorner == '7' && opponentEdge == '8') { return '1'; }
                        if (ourCorner == '7' && opponentEdge == '4') { return '9'; }
                        if (ourCorner == '9' && opponentEdge == '8') { return '3'; }
                        if (ourCorner == '9' && opponentEdge == '6') { return '7'; }
                    }
                }

                // See if the opponent has an adjacent corner and opposite edge
                char opponentAdjacentCorner = CheckCorners(gameBoard, opponentPlayer.Piece, oppositeCorner);
                char opponentOppositeEdge = CheckFarEdgesToCorner(gameBoard, ourCorner, opponentPlayer.Piece);
                if (opponentAdjacentCorner != ' ' && opponentOppositeEdge != ' ')
                {
                    if (ourCorner == '1' && opponentAdjacentCorner == '3') { return '7'; }
                    if (ourCorner == '1' && opponentAdjacentCorner == '7') { return '3'; }
                    if (ourCorner == '3' && opponentAdjacentCorner == '9') { return '1'; }
                    if (ourCorner == '3' && opponentAdjacentCorner == '1') { return '9'; }
                    if (ourCorner == '7' && opponentAdjacentCorner == '9') { return '1'; }
                    if (ourCorner == '7' && opponentAdjacentCorner == '1') { return '9'; }
                    if (ourCorner == '9' && opponentAdjacentCorner == '7') { return '3'; }
                    if (ourCorner == '9' && opponentAdjacentCorner == '3') { return '7'; }
                }

                // The only way we should be at this point is if the opponent has 2 edges, in which case we take the closest open corner
                char opponentEdge1 = CheckEdges(gameBoard, opponentPlayer.Piece);
                char opponentEdge2 = CheckEdges(gameBoard, opponentPlayer.Piece, opponentEdge1);
                if (opponentEdge1 != ' ' && opponentEdge2 != ' ')
                {
                    char[] opponentEdges = { opponentEdge1, opponentEdge2 };
                    Array.Sort(opponentEdges);

                    if (opponentEdges[0] == '2' && opponentEdges[1] == '4')
                    {
                        if (ourCorner == '3' || ourCorner == '7') { return '9'; }
                        if (ourCorner == '9') { return PickAdjacentCorner('9'); }
                    }

                    if (opponentEdges[0] == '2' && opponentEdges[1] == '6')
                    {
                        if (ourCorner == '1' || ourCorner == '9') { return '7'; }
                        if (ourCorner == '7') { return PickAdjacentCorner('7'); }
                    }

                    if (opponentEdges[0] == '4' && opponentEdges[1] == '8')
                    {
                        if (ourCorner == '1' || ourCorner == '9') { return '3'; }
                        if (ourCorner == '3') { return PickAdjacentCorner('3'); }
                    }

                    if (opponentEdges[0] == '6' && opponentEdges[1] == '8')
                    {
                        if (ourCorner == '3' || ourCorner == '7') { return '1'; }
                        if (ourCorner == '1') { return PickAdjacentCorner('1'); }
                    }
                }
            }

            if (gameBoard.OpenPositions == 5)
            {
                // There is only one move to make at this point. Take the third corner away from everyone
                return GetOpenCornerAwayFromEdges(gameBoard);
            }

            // There are only 2 ways to get to 4 open positions, and the difference is if the center is taken or not
            if (gameBoard.OpenPositions == 4)
            {
                if (char.IsNumber(gameBoard.GetPosition('5')))
                {
                    return '5';
                } else
                {
                    // Find the corner that has an edge open next to it
                    char openEdge = GetOpenEdge(gameBoard);

                    // Find the open corner nearest the edge
                    return GetOpenCornerNearSpecificEdge(gameBoard, openEdge);
                }

            }

            return ' ';
        }

        private char CheckCorners(GameBoard gameBoard, char piece, char ignore = ' ')
        {
            char[] cornerPositions = { '1', '3', '7', '9' };

            foreach (var position in cornerPositions)
            {
                if (position == ignore)
                {
                    continue;
                }

                if (gameBoard.GetPosition(position) == piece)
                {
                    return position;
                }
            }

            return ' ';
        }

        private char CheckEdges(GameBoard gameBoard, char piece, char positionToIgnore = ' ')
        {
            char[] edgePositions = { '2', '4', '6', '8' };

            foreach (var position in edgePositions)
            {
                if (position == positionToIgnore) { continue; }

                if (gameBoard.GetPosition(position) == piece)
                {
                    return position;
                }
            }

            return ' ';
        }

        private char CheckCenter(GameBoard gameBoard, char piece)
        {
            return (gameBoard.GetPosition('5') == piece) ? '5' : ' ';
        }

        private char PickAdjacentCorner(char position)
        {
            switch (position)
            {
                case '1':
                    return (rnd.Next(0, 1) == 0) ? '3' : '7';
                case '3':
                    return (rnd.Next(0, 1) == 0) ? '1' : '9';
                case '7':
                    return (rnd.Next(0, 1) == 0) ? '1' : '9';
                case '9':
                    return (rnd.Next(0, 1) == 0) ? '3' : '7';
            }

            return ' ';
        }

        private char PickOppositeCorner(char position)
        {
            switch (position)
            {
                case '1':
                    return '9';
                case '3':
                    return '7';
                case '7':
                    return '3';
                case '9':
                    return '1';
            }

            return ' ';
        }

        private char PickOppositeEdge(char position)
        {
            switch (position)
            {
                case '1':
                    return (rnd.Next(0, 1) == 0) ? '6' : '8';
                case '3':
                    return (rnd.Next(0, 1) == 0) ? '4' : '8';
                case '7':
                    return (rnd.Next(0, 1) == 0) ? '2' : '6';
                case '9':
                    return (rnd.Next(0, 1) == 0) ? '2' : '4';
            }

            return ' ';
        }

        private char CheckNearEdgesToCorner(GameBoard gameBoard, char position, char piece)
        {
            char[] nearEdges = new char[2];

            switch (position)
            {
                case '1':
                    nearEdges[0] = '2';
                    nearEdges[1] = '4';
                    break;
                case '3':
                    nearEdges[0] = '2';
                    nearEdges[1] = '6';
                    break;
                case '7':
                    nearEdges[0] = '4';
                    nearEdges[1] = '8';
                    break;
                case '9':
                    nearEdges[0] = '6';
                    nearEdges[1] = '8';
                    break;
            }

            foreach (var edge in nearEdges)
            {
                // If piece is not 0, then we're looking for a player's piece
                char thisPiece = gameBoard.GetPosition(edge);
                if ( thisPiece == piece)
                {
                    return edge;
                }

                // If piece is 0, then we're making sure that the edge is not taken
                if (piece == '0')
                {
                    if (!char.IsNumber(thisPiece))
                    {
                        return edge;
                    }
                }
            }

            return ' ';

        }

        private char CheckFarEdgesToCorner(GameBoard gameBoard, char position, char piece)
        {
            char[] farEdges = new char[2];

            switch (position)
            {
                case '9':
                    farEdges[0] = '2';
                    farEdges[1] = '4';
                    break;
                case '7':
                    farEdges[0] = '2';
                    farEdges[1] = '6';
                    break;
                case '3':
                    farEdges[0] = '4';
                    farEdges[1] = '8';
                    break;
                case '1':
                    farEdges[0] = '6';
                    farEdges[1] = '8';
                    break;
            }

            foreach (var edge in farEdges)
            {
                if (gameBoard.GetPosition(edge) == piece)
                {
                    return edge;
                }
            }

            return ' ';
        }

        private char PickEdge(char positionToIgnore = ' ')
        {
            char[] edges = { '2', '4', '6', '8' };
            char[] oppositeEdge = { '8', '6', '4', '2' };

            int choice = rnd.Next(0, 3);

            return (edges[choice] == positionToIgnore) ? oppositeEdge[choice] : edges[choice];
        }

        private char PickEdgeAdjacentToEdge(char position)
        {
            char[] nearEdges = new char[2];

            switch (position)
            {
                case '2':
                    nearEdges[0] = '6';
                    nearEdges[1] = '4';
                    break;
                case '4':
                    nearEdges[0] = '2';
                    nearEdges[1] = '8';
                    break;
                case '6':
                    nearEdges[0] = '2';
                    nearEdges[1] = '8';
                    break;
                case '8':
                    nearEdges[0] = '6';
                    nearEdges[1] = '4';
                    break;
            }

            return nearEdges[rnd.Next(0, 1)];
        }

        public char GetOpenCornerAwayFromEdges(GameBoard gameBoard)
        {
            // Check which corners are empty
            char[] corners = { '1', '3', '7', '9' };

            foreach (var position in corners)
            {
                if (char.IsNumber(gameBoard.GetPosition(position)))
                {
                    // See if this corner is open
                    if (CheckNearEdgesToCorner(gameBoard, position, '0') == ' ')
                    {
                        return position;
                    }
                }

            }

            return ' ';
        }

        public char GetOpenCornerNearSpecificEdge(GameBoard gameBoard, char edgePosition)
        {
            // Check which corners are empty
            char[] corners = new char[2];

            switch (edgePosition)
            {
                case '2':
                    corners[0] = '1';
                    corners[1] = '3';
                    break;
                case '4':
                    corners[0] = '1';
                    corners[1] = '7';
                    break;
                case '6':
                    corners[0] = '3';
                    corners[1] = '9';
                    break;
                case '8':
                    corners[0] = '7';
                    corners[1] = '9';
                    break;
                default:
                    corners[0] = '1';
                    corners[1] = '3';
                    break;
            }

            foreach (var position in corners)
            {
                if (char.IsNumber(gameBoard.GetPosition(position)))
                {
                    return position;
                }
            }

            return ' ';
        }


        private char GetOpenEdge(GameBoard gameBoard)
        {
            // Check which edges are empty (the first found empty edge next to an empty corner)
            char[] edges = { '2', '4', '6', '8' };

            foreach (char position in edges)
            {
                if (char.IsNumber(gameBoard.GetPosition(position)))
                {
                    // See if this corner is open
                    if (GetOpenCornerNearSpecificEdge(gameBoard, position) != ' ')
                    {
                        return position;
                    }
                }
            }

            return ' ';
        }

    }
}
