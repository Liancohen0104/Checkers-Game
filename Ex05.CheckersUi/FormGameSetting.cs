using System;
using System.Drawing;
using System.Windows.Forms;
namespace Ex05.CheckersUi
{
    public class FormGameSetting : Form
    {
        private Label m_LabelBoardSize;
        private Label m_LabelPlayers;
        private Label m_LabelPlayer1;
        private Label m_LabelPlayer2;
        private RadioButton m_RadioButtonSmallSize;
        private RadioButton m_RadioButtonMediumSize;
        private RadioButton m_RadioButtonLargeSize;
        private TextBox m_TextBoxPlayer1Name;
        private TextBox m_TextBoxPlayer2Name;
        private CheckBox m_CheckBoxPlayer2;
        private Button m_ButtonDone;
        private bool m_CloseByDone = false;

        public FormGameSetting()
        {
            initializeComponents1();
        }

        public bool CloseByDone
        {
            get
            {
                return this.m_CloseByDone;
            }
        }

        public string Player1Name
        {
            get
            {
                return this.m_TextBoxPlayer1Name.Text;
            }
        }

        public string Player2Name
        {
            get
            {
                return m_CheckBoxPlayer2.Checked ? m_TextBoxPlayer2Name.Text : "computer";
            }
        }

        public int BoardSize
        {
            get
            {
                return m_RadioButtonSmallSize.Checked ? 6 : m_RadioButtonMediumSize.Checked ? 8 : 10;
            }
        }

        private void initializeComponents1()
        {
            initializeWindowGameSettingComponents();
            m_LabelBoardSize = new Label() { Text = "Board Size:", Location = new Point(20, 20), AutoSize = true };
            m_RadioButtonSmallSize = new RadioButton() { Text = "6x6", AutoSize = true, Location = new Point(m_LabelBoardSize.Left + 20, m_LabelBoardSize.Bottom + 5), Checked = true};
            m_RadioButtonMediumSize = new RadioButton() { Text = "8x8", AutoSize = true, Location = new Point(m_RadioButtonSmallSize.Left + 60, m_RadioButtonSmallSize.Top)};
            m_RadioButtonLargeSize = new RadioButton() { Text = "10x10", AutoSize = true, Location = new Point(m_RadioButtonMediumSize.Left + 60, m_RadioButtonMediumSize.Top)};
            m_LabelPlayers = new Label() { Text = "Players:", AutoSize = true, Location = new Point(m_LabelBoardSize.Left, m_RadioButtonSmallSize.Bottom + 5) };
            m_LabelPlayer1 = new Label() { Text = "Player 1:", AutoSize = true, Location = new Point(m_LabelPlayers.Left + 10, m_LabelPlayers.Bottom + 5)};
            m_TextBoxPlayer1Name = new TextBox() { Width = 100, Location = new Point(m_LabelPlayer1.Right + 5, m_LabelPlayer1.Top - 3), AutoSize = true };
            m_CheckBoxPlayer2 = new CheckBox() { Location = new Point(m_LabelPlayer1.Left, m_LabelPlayer1.Bottom + 5), AutoSize = true};
            m_CheckBoxPlayer2.CheckedChanged += new EventHandler(m_CheckBoxPlayer2_CheckedChanged);
            m_LabelPlayer2 = new Label() { Text = "Player 2:", AutoSize = true, Location = new Point(m_CheckBoxPlayer2.Left + 20, m_CheckBoxPlayer2.Top) };
            m_TextBoxPlayer2Name = new TextBox() { Width = 100, Location = new Point(m_TextBoxPlayer1Name.Left, m_LabelPlayer2.Top - 3), Text = "[Computer]", Enabled = false, AutoSize = true };
            m_ButtonDone = new Button() { Text = "Done", Size = new Size(75, 25), Location = new Point(m_TextBoxPlayer2Name.Left + 25, m_TextBoxPlayer2Name.Bottom + 20)};
            m_ButtonDone.Click += m_ButtonDone_Click;
            addComponentsToControls();
        }

        private void initializeWindowGameSettingComponents()
        {
            this.Text = "Game Settings";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            this.ClientSize = new Size(250, 210);
            this.ShowInTaskbar = false;
        }

        private void addComponentsToControls()
        {
            this.Controls.AddRange(new Control[] {
                                                         m_LabelBoardSize,
                                                         m_RadioButtonSmallSize,
                                                         m_RadioButtonMediumSize,
                                                         m_RadioButtonLargeSize,
                                                         m_LabelPlayers,
                                                         m_LabelPlayer1,
                                                         m_TextBoxPlayer1Name,
                                                         m_CheckBoxPlayer2,
                                                         m_LabelPlayer2,
                                                         m_TextBoxPlayer2Name,
                                                         m_ButtonDone
                                                     });
        }

        private void m_ButtonDone_Click(object sender, EventArgs e)
        { 
            m_CloseByDone = true;
            this.Close();
        }

        // $G$ NTT-999 (-3) You should have used string.Empty instead of "".
        private void m_CheckBoxPlayer2_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox player2CheckBox = sender as CheckBox;

            if (player2CheckBox.Checked)
            {
                m_TextBoxPlayer2Name.Enabled = true;
                m_TextBoxPlayer2Name.Text = ""; 
            }
            else
            {
                m_TextBoxPlayer2Name.Enabled = false;
                m_TextBoxPlayer2Name.Text = "[Computer]";
            }
        }
    }
}