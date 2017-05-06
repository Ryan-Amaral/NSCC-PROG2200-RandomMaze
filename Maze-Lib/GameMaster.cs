using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// This class contains all objects that have to do with the game.
    /// </summary>
    public class GameMaster
    {
        private Rectangle _drawRect;
        private Maze _maze;

        private int _mazeRows;
        private int _mazeColumns;

        /// <summary>
        /// Handler for winning maze event.
        /// </summary>
        public event MazeWonHandler MazeWon;

        private WinScreen _winScreen;

        /// <summary>
        /// Creates a new GameMaster with the drawing area and maze dimensions.
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="mazeRows"></param>
        /// <param name="mazeColumns"></param>
        public GameMaster(Rectangle drawRect, int mazeRows, int mazeColumns)
        {
            _drawRect = drawRect;
            _maze = new Maze(_drawRect, mazeRows, mazeColumns);
            _mazeRows = mazeRows;
            _mazeColumns = mazeColumns;
            _winScreen = new WinScreen(new Rectangle(_drawRect.Left + (_drawRect.Width / 3), _drawRect.Top + (_drawRect.Height / 3),
                _drawRect.Width / 3, _drawRect.Height / 3));
        }

        /// <summary>
        /// Chooses a random start and end point in addition to creating a random maze.
        /// </summary>
        public void SetupMaze()
        {
            _maze.GenerateRandomMaze();

            // start is in first half rows of maze
            _maze.CurMazeCell = _maze.MazeCells[Utils.Random.Next(_mazeRows / 2), Utils.Random.Next(_mazeColumns)];

            // end is in last half rows of maze
            _maze.MazeCells[Utils.Random.Next(_mazeRows / 2, _mazeRows), Utils.Random.Next(_mazeColumns)].IsEndCell = true;
        }

        /// <summary>
        /// Updates and draws all GameObjects and Sprites. If is win is true draw only win screen
        /// </summary>
        public void UpdateWithGraphics(Graphics graphics, bool isWin)
        {
            // maze draws first since at bottom
            _maze.Draw(graphics);

            if (isWin)
            {
                _winScreen.Draw(graphics);
            }
        }

        /// <summary>
        /// Moves the player in the specified direction if can be.
        /// </summary>
        /// <param name="direction"></param>
        public void MovePlayer(Directions direction)
        {
            // handle the movement

            if (_maze.CurMazeCell.CanGoUp && direction == Directions.Up)
            {
                _maze.CurMazeCell = _maze.MazeCells[_maze.CurMazeCell.CellRow - 1, _maze.CurMazeCell.CellColumn];
            }
            else if (_maze.CurMazeCell.CanGoDown && direction == Directions.Down)
            {
                _maze.CurMazeCell = _maze.MazeCells[_maze.CurMazeCell.CellRow + 1, _maze.CurMazeCell.CellColumn];
            }
            else if (_maze.CurMazeCell.CanGoLeft && direction == Directions.Left)
            {
                _maze.CurMazeCell = _maze.MazeCells[_maze.CurMazeCell.CellRow, _maze.CurMazeCell.CellColumn - 1];
            }
            else if (_maze.CurMazeCell.CanGoRight && direction == Directions.Right)
            {
                _maze.CurMazeCell = _maze.MazeCells[_maze.CurMazeCell.CellRow, _maze.CurMazeCell.CellColumn + 1];
            }

            if (_maze.CurMazeCell.IsEndCell)
            {
                // fire event for winning
                if (MazeWon != null)
                {
                    MazeWon(this, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Congratulate the player for winning.
        /// </summary>
        public void DisplayWin()
        {

        }
    }
}
