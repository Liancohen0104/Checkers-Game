using System;
using System.Collections.Generic;
using System.Text;
namespace Ex05.GameLogic
{
    public class GameBoard
    {
        private bool m_IsPlayerRetire;
        private readonly int r_BoardSize;
        private Piece[,] m_Board;
        private readonly Player r_FirstPlayer;
        private readonly Player r_SecondPlayer;
        private Player m_CurrentPlayerTurn;
        private eGameState m_GameMode;
        private eErrorMessage m_ErrorMessage;
        private Random m_RandomComputerMove;

        public GameBoard(int i_BoardSize, string i_FirstPlayerName, string i_SecondPlayerName)
        {
            this.r_BoardSize = i_BoardSize;
            this.r_FirstPlayer = new Player(i_FirstPlayerName, ePlayerType.Player1, ePlayerSymbol.X);
            ePlayerType player2Type =
                i_SecondPlayerName.ToLower() == "computer" ? ePlayerType.Computer : ePlayerType.Player2;
            this.r_SecondPlayer = new Player(i_SecondPlayerName, player2Type, ePlayerSymbol.O);
            InitializeBoard();
            this.m_GameMode = eGameState.Ongoing;
            this.m_IsPlayerRetire = false;
            this.m_CurrentPlayerTurn = r_FirstPlayer;
        }

        public bool IsPlayerRetire
        {
            get
            {
                return this.m_IsPlayerRetire;
            }
            set
            {
                this.m_IsPlayerRetire = value;
            }
        }

        public int BoardSize
        {
            get
            {
                return this.r_BoardSize;
            }
        }

        public Piece[,] Board
        {
            get
            {
                return this.m_Board;
            }
            set
            {
                this.m_Board = value;
            }
        }

        public Player FirstPlayer
        {
            get
            {
                return this.r_FirstPlayer;
            }
        }

        public Player SecondPlayer
        {
            get
            {
                return this.r_SecondPlayer;
            }
        }

        public Player CurrentPlayerTurn
        {
            get
            {
                return this.m_CurrentPlayerTurn;
            }
            set
            {
                this.m_CurrentPlayerTurn = value;
            }
        }

        public eGameState GameMode
        {
            get
            {
                return this.m_GameMode;
            }
            set
            {
                this.m_GameMode = value;
            }
        }

        public eErrorMessage ErrorMessage
        {
            get
            {
                return this.m_ErrorMessage;
            }
            set
            {
                this.m_ErrorMessage = value;
            }
        }

        public void InitializeBoard()
        {
            int rowsPerPlayer = r_BoardSize / 2 - 1;

            m_Board = new Piece[r_BoardSize, r_BoardSize];
            for(int row = 0; row < rowsPerPlayer; ++row)
            {
                for(int col = 0; col < r_BoardSize; ++col)
                {
                    if((row + col) % 2 != 0)
                    {
                        Piece piece = new Piece(ePlayerType.Player2, ePlayerSymbol.O, new Position(row, col));
                        m_Board[row, col] = piece;
                        r_SecondPlayer.PlayerPiecesList.Add(piece);
                    }
                }
            }

            for(int row = r_BoardSize - rowsPerPlayer; row < r_BoardSize; ++row)
            {
                for(int col = 0; col < r_BoardSize; ++col)
                {
                    if((row + col) % 2 != 0)
                    {
                        Piece piece = new Piece(ePlayerType.Player1, ePlayerSymbol.X, new Position(row, col));
                        m_Board[row, col] = piece;
                        r_FirstPlayer.PlayerPiecesList.Add(piece);
                    }
                }
            }
        }

        public void MakeMove(
            Position i_CurrentPosition,
            Position i_NextPosition,
            Player i_CurrentPlayer,
            Piece i_CapturePiece)
        {
            Piece currentPiece = GetPiece(i_CurrentPosition);
            List<Move> additionalCaptures = new List<Move>();
            bool isHaveAdditionalCapture = false;

            setPieceInBoard(i_NextPosition, currentPiece);
            setPieceInBoard(i_CurrentPosition, null);
            if(i_CapturePiece != null)
            {
                setPieceInBoard(
                    new Position(i_CapturePiece.PiecePosition.BoardRow, i_CapturePiece.PiecePosition.BoardCol),
                    null);
                if(IsCurrentPlayerIsFirstPlayer())
                {
                    if(i_CapturePiece.PieceSymbol == ePlayerSymbol.U)
                    {
                        --r_SecondPlayer.KingsCount;
                    }

                    r_SecondPlayer.PlayerPiecesList.Remove(i_CapturePiece);
                }
                else
                {
                    if(i_CapturePiece.PieceSymbol == ePlayerSymbol.K)
                    {
                        --r_FirstPlayer.KingsCount;
                    }

                    r_FirstPlayer.PlayerPiecesList.Remove(i_CapturePiece);
                }

                checkAndPromoteToKing(currentPiece, i_CurrentPlayer);
                additionalCaptures = findCaptureMovesForPiece(currentPiece);
            }

            if(additionalCaptures.Count > 0)
            {
                i_CurrentPlayer.PossibleCaptureMoves.Clear();
                i_CurrentPlayer.PossibleCaptureMoves.AddRange(additionalCaptures);
                isHaveAdditionalCapture = true;

            }

            if(!isHaveAdditionalCapture)
            {
                checkAndPromoteToKing(currentPiece, i_CurrentPlayer);
                updatePossibleCaptureMovesList(r_FirstPlayer);
                updatePossibleCaptureMovesList(r_SecondPlayer);
                CheckIfGameEnd();
                switchTurn();
            }
        }

        public Piece GetPiece(Position i_NextPosition)
        {
            Piece nextPiece = null;

            if(isWithinBounds(i_NextPosition))
            {
                nextPiece = m_Board[i_NextPosition.BoardRow, i_NextPosition.BoardCol];
            }

            return nextPiece;
        }

        private void setPieceInBoard(Position i_UpdatePosition, Piece i_UpdatePiece)
        {
            m_Board[i_UpdatePosition.BoardRow, i_UpdatePosition.BoardCol] = i_UpdatePiece;
            if(i_UpdatePiece != null)
            {
                i_UpdatePiece.PiecePosition = i_UpdatePosition;
            }
        }

        private List<Move> findCaptureMovesForPiece(Piece i_CurrentPiece)
        {
            List<Move> captureMoves = new List<Move>();
            int[][] directions;

            if(i_CurrentPiece.PieceSymbol == ePlayerSymbol.K || i_CurrentPiece.PieceSymbol == ePlayerSymbol.U)
            {
                directions = new int[][]
                                 {
                                     new int[] { -2, -2 }, new int[] { -2, 2 }, new int[] { 2, -2 }, new int[] { 2, 2 }
                                 };
            }
            else if(i_CurrentPiece.PieceOwner == ePlayerType.Player1)
            {
                directions = new int[][] { new int[] { -2, -2 }, new int[] { -2, 2 } };
            }
            else
            {
                directions = new int[][] { new int[] { 2, -2 }, new int[] { 2, 2 } };
            }

            foreach(int[] direction in directions)
            {
                int targetRow = i_CurrentPiece.PiecePosition.BoardRow + direction[0];
                int targetCol = i_CurrentPiece.PiecePosition.BoardCol + direction[1];
                int middleRow = i_CurrentPiece.PiecePosition.BoardRow + direction[0] / 2;
                int middleCol = i_CurrentPiece.PiecePosition.BoardCol + direction[1] / 2;

                if(targetRow < 0 || targetRow >= r_BoardSize || targetCol < 0 || targetCol >= r_BoardSize)
                {
                    continue;
                }

                if(GetPiece(new Position(targetRow, targetCol)) != null)
                {
                    continue;
                }

                Piece middlePiece = GetPiece(new Position(middleRow, middleCol));
                if(middlePiece == null || middlePiece.PieceOwner == i_CurrentPiece.PieceOwner)
                {
                    continue;
                }

                Position fromPosition = i_CurrentPiece.PiecePosition;
                Position toPosition = new Position(targetRow, targetCol);
                Move potentialMove = new Move(fromPosition, toPosition);
                captureMoves.Add(potentialMove);
            }

            return captureMoves;
        }

        private void updatePossibleCaptureMovesList(Player i_PlayerToUpdate)
        {
            i_PlayerToUpdate.PossibleCaptureMoves.Clear();
            foreach(Piece pieceItem in i_PlayerToUpdate.PlayerPiecesList)
            {
                List<Move> captureMovesForPiece = findCaptureMovesForPiece(pieceItem);
                i_PlayerToUpdate.PossibleCaptureMoves.AddRange(captureMovesForPiece);
            }
        }

        public bool LegalMoveValidation(
            int i_CurrentPositionRow,
            int i_CurrentPositionCol,
            int i_NextPositionRow,
            int i_NextPositionCol,
            bool i_IsComeFromUi)
        {
            bool isValidMove = true;
            bool isFlag = false;
            int playerDirection = 0;
            int moveRowDifference = i_NextPositionRow - i_CurrentPositionRow;
            int moveColDifference = i_NextPositionCol - i_CurrentPositionCol;
            Position nextPosition = new Position(i_NextPositionRow, i_NextPositionCol);
            Position currentPosition = new Position(i_CurrentPositionRow, i_CurrentPositionCol);
            Piece currentPiece = null;
            Piece capturePiece = null;

            if(IsCurrentPlayerIsFirstPlayer())
            {
                playerDirection = -1;
            }
            else
            {
                playerDirection = 1;
            }

            foreach(Piece pieceItem in m_CurrentPlayerTurn.PlayerPiecesList)
            {
                if(checkIfPositionsEqual(pieceItem.PiecePosition, currentPosition))
                {
                    currentPiece = pieceItem;
                    isFlag = true;
                    break;
                }
            }

            if(!isFlag)
            {
                isValidMove = false;
                m_ErrorMessage = eErrorMessage.NoGamePieceInSourceException;
            }
            else if(!isWithinBounds(nextPosition))
            {
                isValidMove = false;
                m_ErrorMessage = eErrorMessage.OutOfBoundsException;
            }
            else if(GetPiece(nextPosition) != null)
            {
                isValidMove = false;
                m_ErrorMessage = eErrorMessage.TargetCellOccupiedException;
            }
            else if(Math.Abs(moveRowDifference) == 1 && Math.Abs(moveColDifference) == 1)
            {
                if(m_CurrentPlayerTurn.PossibleCaptureMoves.Count > 0 && i_IsComeFromUi)
                {
                    isValidMove = false;
                    m_ErrorMessage = eErrorMessage.MandatoryCaptureMoveException;
                }
                else if(currentPiece.PieceSymbol != ePlayerSymbol.K && currentPiece.PieceSymbol != ePlayerSymbol.U)
                {
                    if(moveRowDifference != playerDirection)
                    {
                        isValidMove = false;
                        m_ErrorMessage = eErrorMessage.InvalidDirectionException;
                    }
                }
            }
            else if(Math.Abs(moveRowDifference) == 2 && Math.Abs(moveColDifference) == 2)
            {
                isFlag = false;
                foreach(Move moveItem in m_CurrentPlayerTurn.PossibleCaptureMoves)
                {
                    if(checkIfPositionsEqual(moveItem.NextPosition, nextPosition)
                       && checkIfPositionsEqual(moveItem.CurrentPosition, currentPosition))
                    {
                        isFlag = true;
                    }
                }

                if(!isFlag)
                {
                    isValidMove = false;
                    m_ErrorMessage = eErrorMessage.MandatoryAdditionalCaptureException;
                }

                int captureRow = (currentPosition.BoardRow + nextPosition.BoardRow) / 2;
                int captureCol = (currentPosition.BoardCol + nextPosition.BoardCol) / 2;

                capturePiece = GetPiece(new Position(captureRow, captureCol));
                if(capturePiece == null || capturePiece.PieceOwner == m_CurrentPlayerTurn.PlayerType)
                {
                    isValidMove = false;
                    m_ErrorMessage = eErrorMessage.NoOpponentPieceToCaptureException;
                }
                else if(currentPiece.PieceSymbol != ePlayerSymbol.K && currentPiece.PieceSymbol != ePlayerSymbol.U)
                {
                    if(moveRowDifference != playerDirection * 2)
                    {
                        isValidMove = false;
                        m_ErrorMessage = eErrorMessage.InvalidDirectionException;
                    }
                }
            }
            else
            {
                isValidMove = false;
                m_ErrorMessage = eErrorMessage.IllegalTargetCellException;
            }

            if(isValidMove && i_IsComeFromUi)
            {
                MakeMove(currentPosition, nextPosition, m_CurrentPlayerTurn, capturePiece);
            }

            return isValidMove;
        }

        private bool isWithinBounds(Position i_NextPosition)
        {
            return i_NextPosition.BoardRow >= 0 && i_NextPosition.BoardRow < r_BoardSize && i_NextPosition.BoardCol >= 0
                   && i_NextPosition.BoardCol < r_BoardSize;
        }

        private void checkAndPromoteToKing(Piece i_CurrentPiece, Player i_CurrentPlayer)
        {
            if(i_CurrentPiece.PieceSymbol != ePlayerSymbol.K && i_CurrentPiece.PieceSymbol != ePlayerSymbol.U)
            {
                if(i_CurrentPlayer.PlayerType == ePlayerType.Player1)
                {
                    if(i_CurrentPiece.PiecePosition.BoardRow == 0)
                    {
                        i_CurrentPiece.PieceSymbol = ePlayerSymbol.K;
                        ++i_CurrentPlayer.KingsCount;
                    }
                }
                else
                {
                    if(i_CurrentPiece.PiecePosition.BoardRow == r_BoardSize - 1)
                    {
                        i_CurrentPiece.PieceSymbol = ePlayerSymbol.U;
                        ++i_CurrentPlayer.KingsCount;
                    }
                }
            }
        }

        public void CheckIfGameEnd()
        {
            bool currentPlayerHasMoves = false;
            bool nextPlayerHasMoves = false;
            Player nextPlayer;

            if(IsCurrentPlayerIsFirstPlayer())
            {
                nextPlayer = r_SecondPlayer;
            }
            else
            {
                nextPlayer = r_FirstPlayer;
            }

            if(m_IsPlayerRetire)
            {
                if(IsCurrentPlayerIsFirstPlayer())
                {
                    m_GameMode = eGameState.Player2Win;
                    updatePlayerScore(nextPlayer, m_CurrentPlayerTurn);
                }
                else
                {
                    m_GameMode = eGameState.Player1Win;
                    updatePlayerScore(nextPlayer, m_CurrentPlayerTurn);
                }
            }
            else
            {
                updatePossibleMovesForPlayerList(r_FirstPlayer);
                updatePossibleMovesForPlayerList(r_SecondPlayer);
                currentPlayerHasMoves = m_CurrentPlayerTurn.PossibleMoves.Count > 0;
                nextPlayerHasMoves = nextPlayer.PossibleMoves.Count > 0;
                if(!currentPlayerHasMoves && !nextPlayerHasMoves)
                {
                    m_GameMode = eGameState.Draw;
                }
                else if(!nextPlayerHasMoves)
                {
                    if(IsCurrentPlayerIsFirstPlayer())
                    {
                        m_GameMode = eGameState.Player1Win;
                        updatePlayerScore(m_CurrentPlayerTurn, nextPlayer);
                    }
                    else
                    {
                        m_GameMode = eGameState.Player2Win;
                        updatePlayerScore(m_CurrentPlayerTurn, nextPlayer);
                    }
                }
            }
        }

        private void updatePlayerScore(Player i_WinningPlayer, Player i_LoosingPlayer)
        {
            int calcScore = Math.Abs(
                (i_WinningPlayer.PlayerPiecesList.Count + 3 * i_WinningPlayer.KingsCount)
                - (i_LoosingPlayer.PlayerPiecesList.Count + 3 * i_LoosingPlayer.KingsCount));

            i_WinningPlayer.PlayerScore += calcScore;
        }

        private void updatePossibleMovesForPlayerList(Player i_UpdatePlayer)
        {
            bool isNeededToSwitchTurn = i_UpdatePlayer != m_CurrentPlayerTurn;

            if(isNeededToSwitchTurn)
            {
                switchTurn();
            }

            i_UpdatePlayer.PossibleMoves.Clear();
            foreach(Piece pieceItem in i_UpdatePlayer.PlayerPiecesList)
            {
                List<Move> pieceMovesList = findAllMovesForPiece(pieceItem);
                i_UpdatePlayer.PossibleMoves.AddRange(pieceMovesList);
            }

            if(isNeededToSwitchTurn)
            {
                switchTurn();
            }
        }

        private List<Move> findAllMovesForPiece(Piece i_CurrentPiece)
        {
            const bool v_IsComeFromUi = true;
            int targetRow;
            int targetCol;
            string currentPosition = null;
            string nextPosition = null;
            int[][] directions;
            List<Move> allMoves = new List<Move>();

            if(i_CurrentPiece.PieceSymbol == ePlayerSymbol.K || i_CurrentPiece.PieceSymbol == ePlayerSymbol.U)
            {
                directions = new int[][]
                                 {
                                     new int[] { -1, -1 }, new int[] { -1, 1 }, new int[] { 1, -1 }, new int[] { 1, 1 },
                                     new int[] { -2, -2 }, new int[] { -2, 2 }, new int[] { 2, -2 }, new int[] { 2, 2 }
                                 };
            }
            else if(i_CurrentPiece.PieceOwner == ePlayerType.Player1)
            {
                directions = new int[][]
                                 {
                                     new int[] { -1, -1 }, new int[] { -1, 1 }, new int[] { -2, -2 },
                                     new int[] { -2, 2 }
                                 };
            }
            else
            {
                directions = new int[][]
                                 {
                                     new int[] { 1, -1 }, new int[] { 1, 1 }, new int[] { 2, -2 }, new int[] { 2, 2 }
                                 };
            }

            foreach(int[] direction in directions)
            {
                targetRow = i_CurrentPiece.PiecePosition.BoardRow + direction[0];
                targetCol = i_CurrentPiece.PiecePosition.BoardCol + direction[1];
                Move potentialMove = new Move(i_CurrentPiece.PiecePosition, new Position(targetRow, targetCol));
                if(LegalMoveValidation(
                       i_CurrentPiece.PiecePosition.BoardRow,
                       i_CurrentPiece.PiecePosition.BoardCol,
                       potentialMove.NextPosition.BoardRow,
                       potentialMove.NextPosition.BoardCol,
                       !v_IsComeFromUi))
                {
                    allMoves.Add(potentialMove);
                }
            }

            return allMoves;
        }

        private string convertPositionToString(Position i_ConvertPosition)
        {
            char rowChar = (char)('A' + i_ConvertPosition.BoardRow);
            char colChar = (char)('a' + i_ConvertPosition.BoardCol);

            return $"{rowChar}{colChar}";
        }

        private bool checkIfPositionsEqual(Position i_Position1, Position i_Position2)
        {
            return i_Position1.BoardCol == i_Position2.BoardCol && i_Position1.BoardRow == i_Position2.BoardRow;
        }

        public void InitializeRematchGame()
        {
            r_FirstPlayer.PlayerPiecesList.Clear();
            r_SecondPlayer.PlayerPiecesList.Clear();
            r_FirstPlayer.PossibleCaptureMoves.Clear();
            r_SecondPlayer.PossibleCaptureMoves.Clear();
            r_FirstPlayer.PossibleMoves.Clear();
            r_SecondPlayer.PossibleMoves.Clear();
            m_GameMode = eGameState.Ongoing;
            m_IsPlayerRetire = false;
            InitializeBoard();
            m_CurrentPlayerTurn = r_FirstPlayer;
            r_FirstPlayer.KingsCount = 0;
            r_SecondPlayer.KingsCount = 0;
        }

        public string MakeComputerMove()
        {
            int randomIndexInList;
            int captureRow;
            int captureCol;
            Piece capturePiece = null;
            Move currentComputerMove;
            StringBuilder computerStringMove = new StringBuilder();

            if(m_RandomComputerMove == null)
            {
                m_RandomComputerMove = new Random();
            }

            if(r_SecondPlayer.PossibleCaptureMoves.Count > 0)
            {
                randomIndexInList = m_RandomComputerMove.Next(r_SecondPlayer.PossibleCaptureMoves.Count);
                currentComputerMove = r_SecondPlayer.PossibleCaptureMoves[randomIndexInList];
                captureRow = (currentComputerMove.CurrentPosition.BoardRow + currentComputerMove.NextPosition.BoardRow)
                             / 2;
                captureCol = (currentComputerMove.CurrentPosition.BoardCol + currentComputerMove.NextPosition.BoardCol)
                             / 2;
                capturePiece = GetPiece(new Position(captureRow, captureCol));
            }
            else
            {
                randomIndexInList = m_RandomComputerMove.Next(r_SecondPlayer.PossibleMoves.Count);
                currentComputerMove = r_SecondPlayer.PossibleMoves[randomIndexInList];
            }

            MakeMove(
                currentComputerMove.CurrentPosition,
                currentComputerMove.NextPosition,
                r_SecondPlayer,
                capturePiece);
            computerStringMove.Append(convertPositionToString(currentComputerMove.CurrentPosition));
            computerStringMove.Append(">");
            computerStringMove.Append(convertPositionToString(currentComputerMove.NextPosition));

            return computerStringMove.ToString();
        }

        public bool IsCurrentPlayerIsFirstPlayer()
        {
            return m_CurrentPlayerTurn.PlayerType == r_FirstPlayer.PlayerType;
        }

        private void switchTurn()
        {
            if(IsCurrentPlayerIsFirstPlayer())
            {
                m_CurrentPlayerTurn = r_SecondPlayer;
            }
            else
            {
                m_CurrentPlayerTurn = r_FirstPlayer;
            }
        }
    }
}