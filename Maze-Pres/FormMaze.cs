using Maze_Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Maze_Pres
{
    /// <summary>
    /// The form that the maze game is built on.
    /// </summary>
    public partial class FormMaze : Form
    {
        private GameMaster _gameMaster;

        /// <summary>
        /// Create a new form.
        /// </summary>
        public FormMaze()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Maximized;
        }

        private void timerGameTick_Tick(object sender, EventArgs e)
        {
            _gameMaster.UpdateWithGraphics(this.CreateGraphics(), false);
            timerGameTick.Stop();
            timerGameTick.Enabled = false;
        }

        private void FormMaze_Load(object sender, EventArgs e)
        {
            // change the last 2 numbers of this line to change game size
            _gameMaster = new GameMaster(this.DisplayRectangle, 20, 20);
            _gameMaster.SetupMaze();
            _gameMaster.MazeWon += new MazeWonHandler(MazeWon);
        }

        private void MazeWon(Object sender, EventArgs e)
        {
            // the the timer to start so win screen can show for 5 seconds
            timerMazeWinner.Enabled = true;
            timerMazeWinner.Start();
            timerGameTick.Stop();
            timerGameTick.Enabled = false;

            // tell to show with win screen
            _gameMaster.UpdateWithGraphics(this.CreateGraphics(), true);
        }

        private void FormMaze_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void FormMaze_KeyDown(object sender, KeyEventArgs e)
        {
            // only move if not end
            if (timerMazeWinner.Enabled == false) {
                // move the current location in the maze
                if (e.KeyCode == Keys.Up)
                {
                    _gameMaster.MovePlayer(Directions.Up);
                }
                else if (e.KeyCode == Keys.Down)
                {
                    _gameMaster.MovePlayer(Directions.Down);
                }
                else if (e.KeyCode == Keys.Left)
                {
                    _gameMaster.MovePlayer(Directions.Left);
                }
                else if (e.KeyCode == Keys.Right)
                {
                    _gameMaster.MovePlayer(Directions.Right);
                }

                // the state of this timer can change within this if statement so check again
                if (timerMazeWinner.Enabled == false)
                {
                    _gameMaster.UpdateWithGraphics(this.CreateGraphics(), false);
                }
            }
        }

        private void FormMaze_Paint(object sender, PaintEventArgs e)
        {

        }

        private void FormMaze_Shown(object sender, EventArgs e)
        {

        }

        private void timerMazeWinner_Tick(object sender, EventArgs e)
        {
            // win screen was shown for 5 seconds, reset maze
            _gameMaster.SetupMaze();
            timerMazeWinner.Stop();
            timerMazeWinner.Enabled = false;
            timerGameTick.Enabled = true;
            timerGameTick.Start();
        }
    }
}
