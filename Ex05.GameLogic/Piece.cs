namespace Ex05.GameLogic
{
    public class Piece
    {
        private readonly ePlayerType r_PieceOwner;
        private ePlayerSymbol m_PieceSymbol;
        private Position m_PiecePosition;

        public Piece(ePlayerType i_PieceOwner, ePlayerSymbol i_PieceSymbol, Position i_PiecePosition)
        {
            this.r_PieceOwner = i_PieceOwner;
            this.m_PieceSymbol = i_PieceSymbol;
            this.m_PiecePosition = i_PiecePosition;
        }

        public ePlayerType PieceOwner
        {
            get
            {
                return this.r_PieceOwner;
            }
        }

        public ePlayerSymbol PieceSymbol
        {
            get
            {
                return this.m_PieceSymbol;
            }
            set
            {
                this.m_PieceSymbol = value;
            }
        }

        public Position PiecePosition
        {
            get
            {
                return this.m_PiecePosition;
            }
            set
            {
                this.m_PiecePosition = value;
            }
        }
    }
}