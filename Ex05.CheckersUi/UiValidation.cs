using System.Text;
namespace Ex05.CheckersUi
{
    // $G$ SFN-013 (+5) Bonus: UI features.
    public class UiValidation
    {
        // $G$ CSS-015 (-3) Bad variable name (should be in the form of: ref io_CamelCase).
        public static bool UiValidationSettings(string i_Player1Name, string i_Player2Name, ref StringBuilder o_InputError)
        {
            return GameLogic.Player.NameValidation(i_Player1Name, i_Player2Name, ref o_InputError);
        }
    }
}
