using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// Class for the maze, can create random maze.
    /// </summary>
    public class Maze : Sprite
    {
        private Rectangle _drawRect;
        private int _rows;
        private int _columns;

        /// <summary>
        /// The cells that make up this maze.
        /// </summary>
        public MazeCell[,] MazeCells;

        private MazeCell _curMazeCell;
        /// <summary>
        /// The maze cell that the player currently is in.
        /// </summary>
        public MazeCell CurMazeCell {
            get
            {
                return _curMazeCell;
            }
            set
            {
                if (_curMazeCell != null)
                {
                    _curMazeCell.IsCurrentCell = false;
                }
                _curMazeCell = value;
                _curMazeCell.IsCurrentCell = true;
                _curMazeCell.HasBeenVisited = true;
            }
        }

        private MazeCell_Wall[,] _mazeCellWalls;
        private List<MazeCell_Mergy> _mergyMazeCells; // needs to be list so can easily reduce size

        private int _mazeWidth;
        private int _mazeHeight;
        private int _cellWidth;
        private int _cellHeight;

        /// <summary>
        /// Creates a maze.
        /// </summary>
        /// <param name="drawRect"></param>
        /// <param name="rows"></param>
        /// <param name="columns"></param>
        public Maze(Rectangle drawRect, int rows, int columns)
        {
            _drawRect = drawRect;
            _rows = rows;
            _columns = columns;
            MazeCells = new MazeCell[_rows, _columns];
            _mazeCellWalls = new MazeCell_Wall[rows, columns];
            _mergyMazeCells = new List<MazeCell_Mergy>(); // list so can delete elements later

            _mazeWidth = _drawRect.Width;
            _mazeHeight = _drawRect.Height;
            _cellWidth = _mazeWidth / _columns;
            _cellHeight = _mazeHeight / _rows;
        }

        /// <summary>
        /// Generates a new random maze.
        /// </summary>
        public void GenerateRandomMaze()
        {
            // reset lists
            _mergyMazeCells = new List<MazeCell_Mergy>();

            // initialize the maze.
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    // create a new cell in this position
                    MazeCells[row, col] = new MazeCell(new Rectangle(col * _cellWidth, row * _cellHeight, _cellWidth, _cellHeight), row, col, _cellWidth, _cellHeight);
                    _mergyMazeCells.Add(new MazeCell_Mergy(new List<MazeCell_Wall>()));

                    // add wall on up or left if appropriate
                    if(row > 0)
                    {
                        // add up wall
                        _mazeCellWalls[row - 1, col] = new MazeCell_Wall(WallConnectability.ConnectsVertically, 
                            MazeCells[row - 1, col], _mergyMazeCells[(((row - 1) * _columns) + col)],
                            MazeCells[row, col], _mergyMazeCells[((row * _columns) + col)]);
                        _mergyMazeCells[(((row - 1) * _columns) + col)].Walls.Add(_mazeCellWalls[row - 1, col]);
                        _mergyMazeCells[((row * _columns) + col)].Walls.Add(_mazeCellWalls[row - 1, col]);
                    }
                    if (col > 0)
                    {
                        // add left wall
                        _mazeCellWalls[row, col - 1] = new MazeCell_Wall(WallConnectability.ConnectsHorizontally, 
                            MazeCells[row, col - 1], _mergyMazeCells[((row * _columns) + (col - 1))],
                            MazeCells[row, col], _mergyMazeCells[((row * _columns) + col)]);
                        _mergyMazeCells[((row * _columns) + (col - 1))].Walls.Add(_mazeCellWalls[row, col - 1]);
                        _mergyMazeCells[((row * _columns) + col)].Walls.Add(_mazeCellWalls[row, col - 1]);
                    }
                }
            }

            // variables needed for merging
            MazeCell_Mergy mergy1;
            MazeCell_Mergy mergy2;
            MazeCell_Wall wall;

            // merge untill all connected
            int mergeCount = (_rows * _columns) - 1;
            for (int i = 0; i < mergeCount; i++)
            {
                // choose random element in _mergyMazeCells (mergy cell 1)
                mergy1 = _mergyMazeCells[Utils.Random.Next(0, _mergyMazeCells.Count)];

                // choose random wall in that mergy maze cell (other side is mergy cell 2)
                wall = mergy1.Walls[Utils.Random.Next(0, mergy1.Walls.Count)];
                if (wall.MergyCell1 == mergy1)
                {
                    mergy2 = wall.MergyCell2;
                }
                else
                {
                    mergy2 = wall.MergyCell1;
                }

                // tell cells on both sides of wall which direction they can move in
                if(wall.WallConnectability == WallConnectability.ConnectsHorizontally)
                {
                    wall.Cell1.CanGoRight = true;
                    wall.Cell2.CanGoLeft = true;
                }
                else
                {
                    wall.Cell1.CanGoDown = true;
                    wall.Cell2.CanGoUp = true;
                }

                // delete that wall on both mergy cells
                mergy1.Walls.Remove(wall);
                mergy2.Walls.Remove(wall);


                // check all walls
                for (int j = 0; j < mergy2.Walls.Count; j++)
                {
                    // get the current wall
                    wall = mergy2.Walls[j];

                    // if wall is between mergy cell 1 and mergy cell 2, delete it
                    if ((wall.MergyCell1 == mergy1 && wall.MergyCell2 == mergy2) || (wall.MergyCell1 == mergy2 && wall.MergyCell2 == mergy1))
                    {
                        mergy1.Walls.Remove(wall);
                        mergy2.Walls.Remove(wall);
                    }
                    // if wall is between mergy cell 2 and other mergy cell, make mergy cell 2 side into mergy cell 1
                    else
                    {
                        // mergy 1 takes over mergy 2's walls
                        if (wall.MergyCell1 == mergy2)
                        {
                            wall.MergyCell1 = mergy1;
                        }
                        else
                        {
                            wall.MergyCell2 = mergy1;
                        }

                        // remove the wall from mergy 2
                        mergy2.Walls.Remove(wall);

                        // add the wall to mergy 1
                        mergy1.Walls.Add(wall);
                    }
                    j--;
                }
                // delete mergy 2
                _mergyMazeCells.Remove(mergy2);
            }
        }

        /// <summary>
        /// Draws all of the cells.
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            for (int row = 0; row < _rows; row++)
            {
                for (int col = 0; col < _columns; col++)
                {
                    MazeCells[row, col].Draw(graphics);
                }
            }
        }
    }

    /// <summary>
    /// An individual part of the maze that has defined which way out of the maze you can go.
    /// </summary>
    public class MazeCell : Sprite
    {
        private Rectangle _drawRect;

        /// <summary>
        /// Determines weather the player can move up from this piece.
        /// </summary>
        public bool CanGoUp { get; set; }

        /// <summary>
        /// Determines weather the player can move down from this piece.
        /// </summary>
        public bool CanGoDown { get; set; }

        /// <summary>
        /// Determines weather the player can move left from this piece.
        /// </summary>
        public bool CanGoLeft { get; set; }

        /// <summary>
        /// Determines weather the player can move right from this piece.
        /// </summary>
        public bool CanGoRight { get; set; }

        private int _width;
        private int _height;

        /// <summary>
        /// The row coordinate of the cell.
        /// </summary>
        public int CellRow { get; set; }
        /// <summary>
        /// The column of the call.
        /// </summary>
        public int CellColumn { get; set; }

        /// <summary>
        /// If this is the cell that the player is in.
        /// </summary>
        public bool IsCurrentCell { get; set; }

        /// <summary>
        /// Whether this cell has been visited by the player yet.
        /// </summary>
        public bool HasBeenVisited { get; set; }

        /// <summary>
        /// If this cell is the ending cell.
        /// </summary>
        public bool IsEndCell { get; set; }

        /// <summary>
        /// Creates a new maze cell with all directions disabled.
        /// </summary>
        public MazeCell(Rectangle drawRect, int row, int column, int width, int height)
        {
            _drawRect = drawRect;

            CanGoUp = false;
            CanGoDown = false;
            CanGoLeft = false;
            CanGoRight = false;

            _width = width;
            _height = height;

            CellRow = row;
            CellColumn = column;

            IsCurrentCell = false;
            HasBeenVisited = false;
            IsEndCell = false;
        }

        /// <summary>
        /// Draws this cell.
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            // draw all black
            graphics.FillRectangle(Brushes.Brown, _drawRect);

            // then fill in wall if can't go that direction

            if (!CanGoUp)
            {
                graphics.FillRectangle(Brushes.Gray, _drawRect.Left, _drawRect.Top, _drawRect.Width, _drawRect.Height / 10);
            }

            if (!CanGoDown)
            {
                graphics.FillRectangle(Brushes.Gray, _drawRect.Left, _drawRect.Bottom - (_drawRect.Height / 10), _drawRect.Width, _drawRect.Height / 10);
            }

            if (!CanGoLeft)
            {
                graphics.FillRectangle(Brushes.Gray, _drawRect.Left, _drawRect.Top, _drawRect.Width / 10, _drawRect.Height);
            }

            if (!CanGoRight)
            {
                graphics.FillRectangle(Brushes.Gray, _drawRect.Right - (_drawRect.Width / 10), _drawRect.Top, _drawRect.Width / 10, _drawRect.Height);
            }

            if (HasBeenVisited)
            {
                // draw the dot
                graphics.FillEllipse(Brushes.ForestGreen, _drawRect.Left + (_drawRect.Width / 3), _drawRect.Top + (_drawRect.Height / 3), _drawRect.Width / 3, _drawRect.Height / 3);
            }
            if (IsCurrentCell)
            {
                // draw the face
                graphics.DrawImage(Image.FromFile("ryanalpha1.png"), _drawRect.Left + (_drawRect.Width / 10), _drawRect.Top + (_drawRect.Height / 10), 
                    (_drawRect.Width * 4) / 5, (_drawRect.Height * 4) / 5);
            }
            if (IsEndCell)
            {
                // draw the end
                graphics.DrawImage(Image.FromFile("ohppb.png"), _drawRect.Left, _drawRect.Top, _drawRect.Width, _drawRect.Height);
            }
        }
    }

    /// <summary>
    /// Which way the wall connects two cells.
    /// </summary>
    public enum WallConnectability
    {
        /// <summary>
        /// Cells connect horizontally, left to right.
        /// </summary>
        ConnectsVertically,

        /// <summary>
        /// Cells connect vertically, up to down.
        /// </summary>
        ConnectsHorizontally
    }

    /// <summary>
    /// Dividers between each maze cell.
    /// </summary>
    class MazeCell_Wall
    {
        public WallConnectability WallConnectability { get; set; }
        /// <summary>
        /// The up or left cell.
        /// </summary>
        public MazeCell Cell1 { get; set; }

        /// <summary>
        /// The mergy cell that cell1 belongs to.
        /// </summary>
        public MazeCell_Mergy MergyCell1 { get; set; }

        /// <summary>
        /// The down or right cell.
        /// </summary>
        public MazeCell Cell2 { get; set; }

        /// <summary>
        /// The mergy cell that cell2 belongs to.
        /// </summary>
        public MazeCell_Mergy MergyCell2 { get; set; }

        public MazeCell_Wall(WallConnectability wallConnectability, MazeCell cell1, MazeCell_Mergy mergyCell1, MazeCell cell2, MazeCell_Mergy mergyCell2)
        {
            WallConnectability = wallConnectability;
            Cell1 = cell1;
            MergyCell1 = mergyCell1;
            Cell2 = cell2;
            MergyCell2 = mergyCell2;
        }
    }

    /// <summary>
    /// A maze cell, but not constrained to conventional dimensions. A cell composed of many cells.
    /// </summary>
    class MazeCell_Mergy
    {
        public List<MazeCell_Wall> Walls { get; set; } // the walls on this object

        public MazeCell_Mergy(List<MazeCell_Wall> walls)
        {
            Walls = walls;
        }
    }
}
