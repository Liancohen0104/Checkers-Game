namespace Ex05.GameLogic
{
    public class Move
    {
        private readonly Position r_CurrentPosition;
        private readonly Position r_NextPosition;

        public Move(Position i_CurrentPosition, Position i_NextPosition)
        {
            this.r_CurrentPosition = i_CurrentPosition;
            this.r_NextPosition = i_NextPosition;
        }

        public Position CurrentPosition
        {
            get
            {
                return this.r_CurrentPosition;
            }
        }

        public Position NextPosition
        {
            get
            {
                return this.r_NextPosition;
            }
        }

        public static bool MoveInputValidation(string i_MoveInput)
        {
            return i_MoveInput.Length == 5 && char.IsUpper(i_MoveInput[0]) && char.IsLower(i_MoveInput[1])
                   && i_MoveInput[2] == '>' && char.IsUpper(i_MoveInput[3]) && char.IsLower(i_MoveInput[4])
                   && !i_MoveInput.Equals("Q");
        }
    }
}