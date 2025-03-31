namespace GameLogic
{
    public class Position
    {
        private int m_BoardRow;
        private int m_BoardCol;

        public Position(int i_BoardRow, int i_BoardCol)
        {
            m_BoardRow = i_BoardRow;
            m_BoardCol = i_BoardCol;
        }

        public int BoardRow
        {
            get
            {
                return m_BoardRow;
            }
            set
            {
                m_BoardRow = value;
            }
        }

        public int BoardCol
        {
            get
            {
                return m_BoardCol;
            }
            set
            {
                m_BoardCol = value;
            }
        }
    }
}