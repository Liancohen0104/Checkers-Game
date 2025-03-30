namespace Ex05.GameLogic
{
    public class Position
    {
        private int m_BoardRow;
        private int m_BoardCol;

        public Position(int i_BoardRow, int i_BoardCol)
        {
            this.m_BoardRow = i_BoardRow;
            this.m_BoardCol = i_BoardCol;
        }

        public int BoardRow
        {
            get
            {
                return this.m_BoardRow;
            }
            set
            {
                this.m_BoardRow = value;
            }
        }

        public int BoardCol
        {
            get
            {
                return this.m_BoardCol;
            }
            set
            {
                this.m_BoardCol = value;
            }
        }
    }
}