using System;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Ex05.GameLogic;
namespace Ex05.CheckersUi
{
    public class FormCheckersGame : Form
    {
        private readonly GameBoard r_GameBoard;
        private Button[,] m_ButtonsBoard;
        private Label m_LabelPlayer1;
        private Label m_LabelPlayer2;
        private Label m_LabelPlayer1Score;
        private Label m_LabelPlayer2Score;
        private Button m_FromButton = null;
        private Button m_ToButton = null;
        private Timer m_ComputerTimer;
        private const int k_ButtonSize = 50;
        private const int k_StartLeft = 20;
        private const int k_StartTop = 70;

        public FormCheckersGame(GameBoard i_GameBoard)
        {
            r_GameBoard = new GameBoard(i_GameBoard.BoardSize, i_GameBoard.FirstPlayer.PlayerName, i_GameBoard.SecondPlayer.PlayerName);
            initializeComponents();
        }
        
        private void initializeComponents()
        {
            initializeWindowGameBoardComponents();
            initializeLabels();
            m_ButtonsBoard = new Button[r_GameBoard.BoardSize, r_GameBoard.BoardSize];
            buildBoard();
        }

        private void initializeWindowGameBoardComponents()
        {
            this.Text = "Damka";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
        }

        private void initializeLabels()
        {
            int totalWidth = k_StartLeft + (r_GameBoard.BoardSize * k_ButtonSize) + 20;
            int totalHeight = k_StartTop + (r_GameBoard.BoardSize * k_ButtonSize) + 20;
            int totalLabelsWidth;

            this.ClientSize = new Size(totalWidth, totalHeight);
            m_LabelPlayer1 = new Label() { Text = $"{r_GameBoard.FirstPlayer.PlayerName}:", AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(m_LabelPlayer1);
            m_LabelPlayer1Score = new Label() { Text = "0", AutoSize = true, Enabled = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(m_LabelPlayer1Score);
            m_LabelPlayer2 = new Label() { Text = $"{r_GameBoard.SecondPlayer.PlayerName}:", AutoSize = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(m_LabelPlayer2);
            m_LabelPlayer2Score = new Label() { Text = "0", AutoSize = true, Enabled = true, Font = new Font("Arial", 14, FontStyle.Bold) };
            this.Controls.Add(m_LabelPlayer2Score);
            totalLabelsWidth = m_LabelPlayer1.PreferredSize.Width + 5 +
                               m_LabelPlayer1Score.PreferredSize.Width + 50 +
                               m_LabelPlayer2.PreferredSize.Width + 5 +
                               m_LabelPlayer2Score.PreferredSize.Width;
            m_LabelPlayer1.Location = new Point(ClientSize.Width / 2 - totalLabelsWidth / 2, 20);
            m_LabelPlayer1Score.Location = new Point(m_LabelPlayer1.Right + 5, m_LabelPlayer1.Top);
            m_LabelPlayer2.Location = new Point(m_LabelPlayer1Score.Right + 50, m_LabelPlayer1.Top);
            m_LabelPlayer2Score.Location = new Point(m_LabelPlayer2.Right + 5, m_LabelPlayer2.Top);
        }

        private void buildBoard()
        {
            int playerRows = r_GameBoard.BoardSize / 2 - 1; 

            for (int row = 0; row < r_GameBoard.BoardSize; ++row)
            {
                for (int col = 0; col < r_GameBoard.BoardSize; ++col)
                {
                    Button cellButton = new Button
                                            {
                                                Size = new Size(k_ButtonSize, k_ButtonSize),
                                                Location = new Point(k_StartLeft + col * k_ButtonSize, k_StartTop + row * k_ButtonSize),
                                                BackColor = (row + col) % 2 == 0 ? Color.DimGray : Color.White,
                                                FlatStyle = FlatStyle.Flat,
                                                Enabled = (row + col) % 2 != 0
                                            };
                    cellButton.FlatAppearance.BorderColor = Color.DarkGray;
                    cellButton.FlatAppearance.BorderSize = 3;
                    if (row < playerRows && cellButton.Enabled)
                    {
                        cellButton.Image = Resource.whitepiece;
                    }
                    else if (row >= r_GameBoard.BoardSize - playerRows && cellButton.Enabled)
                    {
                        cellButton.Image = Resource.blackpiece;

                    }

                    cellButton.Tag = new Point(row, col);
                    cellButton.Click += cellButton_Click;
                    m_ButtonsBoard[row, col] = cellButton;
                    this.Controls.Add(cellButton);
                }
            }
        }

        private void cellButton_Click(object sender, EventArgs e)
        {
            Button clickedButton;
            const bool v_IsComeFromUi = true;

            if (!r_GameBoard.IsCurrentPlayerIsFirstPlayer() && r_GameBoard.SecondPlayer.PlayerType == ePlayerType.Computer)
            {
                clickedButton = null;
            }
            else
            {
                clickedButton = sender as Button;
            }

            if (clickedButton != null)
            {
                if (m_FromButton == null)
                {
                    m_FromButton = clickedButton;
                    clickedButton.BackColor = Color.Gray;
                }
                else if (m_FromButton == clickedButton)
                {
                    clickedButton.BackColor = Color.White;
                    m_FromButton = null;
                }
                else
                {
                    m_ToButton = clickedButton;
                    if(!r_GameBoard.LegalMoveValidation(
                           ((Point)m_FromButton.Tag).X,
                           ((Point)m_FromButton.Tag).Y,
                           ((Point)m_ToButton.Tag).X,
                           ((Point)m_ToButton.Tag).Y,
                           v_IsComeFromUi)) 
                    {
                        showErrorMessage();
                    }
                    else
                    {
                        updateBoardUi();
                        if (r_GameBoard.GameMode == eGameState.Ongoing)
                        {
                            doComputerMoveIfNeeded();
                        }

                        if(r_GameBoard.GameMode != eGameState.Ongoing)
                        {
                            showEndGameMessageBox();
                        }

                        m_ToButton.BackColor = Color.White;
                    }

                    m_FromButton.BackColor = Color.White;
                    m_FromButton = null;
                    m_ToButton = null;
                }
            }
        }

        private void showEndGameMessageBox()
        {
            StringBuilder messageBuilder = new StringBuilder();

            switch (r_GameBoard.GameMode)
            {
                case eGameState.Player1Win:
                    messageBuilder.AppendLine($"{r_GameBoard.FirstPlayer.PlayerName} Won!");
                    break;
                case eGameState.Player2Win:
                    messageBuilder.AppendLine($"{r_GameBoard.SecondPlayer.PlayerName} Won!");
                    break;
                case eGameState.Draw:
                    messageBuilder.AppendLine("Tie!");
                    break;
            }

            messageBuilder.AppendLine("Another Round?");
            DialogResult result = MessageBox.Show(
                messageBuilder.ToString(),
                "Damka",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);
            anotherRoundInitializationIfUserWant(result);
        }

        private void anotherRoundInitializationIfUserWant(DialogResult i_UserButtonChoice)
        {
            if (i_UserButtonChoice == DialogResult.Yes)
            {
                r_GameBoard.InitializeRematchGame();
                m_LabelPlayer1Score.Text = r_GameBoard.FirstPlayer.PlayerScore.ToString();
                m_LabelPlayer2Score.Text = r_GameBoard.SecondPlayer.PlayerScore.ToString();
                updateBoardUi();
            }
            else
            {
                Application.Exit();
            }
        }

        private void doComputerMoveIfNeeded()
        {
            if(!r_GameBoard.IsCurrentPlayerIsFirstPlayer() && r_GameBoard.SecondPlayer.PlayerType == ePlayerType.Computer )
            {
                if(m_ComputerTimer == null)
                {
                    m_ComputerTimer = new Timer();
                    m_ComputerTimer.Interval = 1000;
                    m_ComputerTimer.Tick += m_ComputerTimer_Tick;
                }

                m_ComputerTimer.Start();
            }
        }

        private void m_ComputerTimer_Tick(object sender, EventArgs e)
        {
            if(r_GameBoard.GameMode != eGameState.Ongoing ||
               !(!r_GameBoard.IsCurrentPlayerIsFirstPlayer() && r_GameBoard.SecondPlayer.PlayerType == ePlayerType.Computer)) 
            {
                m_ComputerTimer.Stop();
            }
            else
            {
                r_GameBoard.MakeComputerMove();
                updateBoardUi();
                if(r_GameBoard.GameMode != eGameState.Ongoing)
                {
                    m_ComputerTimer.Stop();
                    showEndGameMessageBox();

                }
                else if( r_GameBoard.IsCurrentPlayerIsFirstPlayer())
                {
                    m_ComputerTimer.Stop();
                }
            }
        }

        private void updateBoardUi()
        {
            for (int row = 0; row < r_GameBoard.BoardSize; ++row)
            {
                for (int col = 0; col < r_GameBoard.BoardSize; ++col)
                {
                    Piece piece = r_GameBoard.Board[row, col];

                    if (piece == null)
                    {
                        m_ButtonsBoard[row, col].Image = null;
                    }
                    else
                    {
                        if (piece.PieceSymbol == ePlayerSymbol.X)
                        {
                            m_ButtonsBoard[row, col].Image = Resource.blackpiece;
                        }
                        else if (piece.PieceSymbol == ePlayerSymbol.O)
                        {
                            m_ButtonsBoard[row, col].Image = Resource.whitepiece;
                        }
                        else if (piece.PieceSymbol == ePlayerSymbol.K)
                        {
                            m_ButtonsBoard[row, col].Image = Resource.blackkingpiece;
                        }
                        else if (piece.PieceSymbol == ePlayerSymbol.U)
                        {
                            m_ButtonsBoard[row, col].Image = Resource.whitekingpiece;
                        }
                    }
                }
            }
        }
        
        private void showErrorMessage()
        {
            string errorMessage = null;

            switch (r_GameBoard.ErrorMessage)
            {
                case eErrorMessage.NoGamePieceInSourceException:
                    errorMessage = "There is no a game piece belonging to you in the source cell, please enter legal move";
                    break;
                case eErrorMessage.OutOfBoundsException:
                    errorMessage = "Out of bounds, please enter legal move";
                    break;
                case eErrorMessage.TargetCellOccupiedException:
                    errorMessage = "There is a game piece in target cell, please enter legal move";
                    break;
                case eErrorMessage.MandatoryCaptureMoveException:
                    errorMessage = "There is a possible capture move you must do, please enter legal move";
                    break;
                case eErrorMessage.InvalidDirectionException:
                    errorMessage = "Wrong direction, please enter legal move";
                    break;
                case eErrorMessage.MandatoryAdditionalCaptureException:
                    errorMessage = "There is an additional capture move to your previous piece you must do, please enter legal move";
                    break;
                case eErrorMessage.NoOpponentPieceToCaptureException:
                    errorMessage = "There is no opponent's piece in the cell you tried to capture, please enter legal move";
                    break;
                case eErrorMessage.IllegalTargetCellException:
                    errorMessage = "The target cell is illegal, please enter legal move";
                    break;
                default:
                    errorMessage = "An unknown error occurred, please enter legal move";
                    break;
            }

            MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}