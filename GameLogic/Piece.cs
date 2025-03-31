namespace GameLogic
{
    public class Piece
    {
        private readonly ePlayerType r_PieceOwner;
        private ePlayerSymbol m_PieceSymbol;
        private Position m_PiecePosition;

        public Piece(ePlayerType i_PieceOwner, ePlayerSymbol i_PieceSymbol, Position i_PiecePosition)
        {
            r_PieceOwner = i_PieceOwner;
            m_PieceSymbol = i_PieceSymbol;
            m_PiecePosition = i_PiecePosition;
        }

        public ePlayerType PieceOwner
        {
            get
            {
                return r_PieceOwner;
            }
        }

        public ePlayerSymbol PieceSymbol
        {
            get
            {
                return m_PieceSymbol;
            }
            set
            {
                m_PieceSymbol = value;
            }
        }

        public Position PiecePosition
        {
            get
            {
                return m_PiecePosition;
            }
            set
            {
                m_PiecePosition = value;
            }
        }
    }
}