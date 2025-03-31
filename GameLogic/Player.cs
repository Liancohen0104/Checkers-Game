using System.Collections.Generic;
using System.Text;

namespace GameLogic
{
    public class Player
    {
        private int m_PlayerScore;
        private int m_KingsCount;
        private readonly string r_PlayerName;
        private readonly ePlayerType r_PlayerType;
        private ePlayerSymbol m_PlayerSymbol;
        private List<Piece> m_PlayerPiecesList;
        private List<Move> m_PossibleCaptureMoves;
        private List<Move> m_PossibleMoves;

        public Player(string i_PlayerName, ePlayerType i_PlayerType, ePlayerSymbol i_PlayerSymbol)
        {
            r_PlayerName = i_PlayerName;
            r_PlayerType = i_PlayerType;
            m_PlayerSymbol = i_PlayerSymbol;
            m_PlayerPiecesList = new List<Piece>();
            m_PossibleCaptureMoves = new List<Move>();
            m_PossibleMoves = new List<Move>();
            m_KingsCount = 0;
            m_PlayerScore = 0;
        }

        public int PlayerScore
        {
            get
            {
                return m_PlayerScore;
            }
            set
            {
                m_PlayerScore = value;
            }
        }

        public int KingsCount
        {
            get
            {
                return m_KingsCount;
            }
            set
            {
                m_KingsCount = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return r_PlayerName;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return r_PlayerType;
            }
        }

        public ePlayerSymbol PlayerSymbol
        {
            get
            {
                return m_PlayerSymbol;
            }
            set
            {
                m_PlayerSymbol = value;
            }
        }

        public List<Piece> PlayerPiecesList
        {
            get
            {
                return m_PlayerPiecesList;
            }
            set
            {
                m_PlayerPiecesList = value;
            }
        }

        public List<Move> PossibleCaptureMoves
        {
            get
            {
                return m_PossibleCaptureMoves;
            }
            set
            {
                m_PossibleCaptureMoves = value;
            }
        }

        public List<Move> PossibleMoves
        {
            get
            {
                return m_PossibleMoves;
            }
            set
            {
                m_PossibleMoves = value;
            }
        }

        public static bool NameValidation(string i_Player1Name, string i_Player2Name, ref StringBuilder o_InputError)
        {
            bool isValidNames = true;

            if (string.IsNullOrWhiteSpace(i_Player1Name) || i_Player1Name.Length > 10)
            {
                o_InputError.AppendLine("Please enter player 1 name (less than 10 letters)");
                isValidNames = false;
            }

            if (string.IsNullOrWhiteSpace(i_Player2Name) || i_Player2Name.Length > 10)
            {
                o_InputError.AppendLine("Please enter player 2 name (less than 10 letters)");
                isValidNames = false;
            }

            return isValidNames;
        }
    }
}