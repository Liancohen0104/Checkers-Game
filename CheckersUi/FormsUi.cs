using GameLogic;
using System.Text;
using System.Windows.Forms;

namespace CheckersUi
{
    public class FormsUi
    {
        public static void StartGame()
        {
            StringBuilder errorStringBuilder = new StringBuilder();
            FormGameSetting settingForm = new FormGameSetting();

            settingForm.ShowDialog();
            if (settingForm.CloseByDone)
            {
                if (UiValidation.UiValidationSettings(settingForm.Player1Name, settingForm.Player2Name, ref errorStringBuilder))
                {
                    GameBoard gameBoard = new GameBoard(settingForm.BoardSize, settingForm.Player1Name, settingForm.Player2Name);
                    FormCheckersGame checkersGameForm = new FormCheckersGame(gameBoard);
                    checkersGameForm.ShowDialog();
                }
                else
                {
                    if (MessageBox.Show(errorStringBuilder.ToString(), "Error", MessageBoxButtons.RetryCancel) == DialogResult.Retry)
                    {
                        StartGame();
                    }
                }
            }
        }
    }
}