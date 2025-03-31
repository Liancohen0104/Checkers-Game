using System.Text;

namespace CheckersUi
{
    public class UiValidation
    {
        public static bool UiValidationSettings(string i_Player1Name, string i_Player2Name, ref StringBuilder io_InputError)
        {
            return GameLogic.Player.NameValidation(i_Player1Name, i_Player2Name, ref io_InputError);
        }
    }
}
