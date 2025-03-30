using System.Collections.Generic;
using System.Text;
namespace Ex05.GameLogic
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
            this.r_PlayerName = i_PlayerName;
            this.r_PlayerType = i_PlayerType;
            this.m_PlayerSymbol = i_PlayerSymbol;
            this.m_PlayerPiecesList = new List<Piece>();
            this.m_PossibleCaptureMoves = new List<Move>();
            this.m_PossibleMoves = new List<Move>();
            this.m_KingsCount = 0;
            this.m_PlayerScore = 0;
        }

        public int PlayerScore
        {
            get
            {
                return this.m_PlayerScore;
            }
            set
            {
                this.m_PlayerScore = value;
            }
        }

        public int KingsCount
        {
            get
            {
                return this.m_KingsCount;
            }
            set
            {
                this.m_KingsCount = value;
            }
        }

        public string PlayerName
        {
            get
            {
                return this.r_PlayerName;
            }
        }

        public ePlayerType PlayerType
        {
            get
            {
                return this.r_PlayerType;
            }
        }

        public ePlayerSymbol PlayerSymbol
        {
            get
            {
                return this.m_PlayerSymbol;
            }
            set
            {
                this.m_PlayerSymbol = value;
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
                return this.m_PossibleCaptureMoves;
            }
            set
            {
                this.m_PossibleCaptureMoves = value;
            }
        }

        public List<Move> PossibleMoves
        {
            get
            {
                return this.m_PossibleMoves;
            }
            set
            {
                this.m_PossibleMoves = value;
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

            if ((string.IsNullOrWhiteSpace(i_Player2Name)) || i_Player2Name.Length > 10)
            {
                o_InputError.AppendLine("Please enter player 2 name (less than 10 letters)");
                isValidNames = false;
            }

            return isValidNames;
        }
    }
}