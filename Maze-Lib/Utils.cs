using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// The directions that can be moved in the maze.
    /// </summary>
    public enum Directions
    {
        /// <summary>
        /// Up is up on the screen.
        /// </summary>
        Up,

        /// <summary>
        /// Down is down on the screen.
        /// </summary>
        Down,

        /// <summary>
        /// Left is left on the screen.
        /// </summary>
        Left,

        /// <summary>
        /// Right is right on the screen.
        /// </summary>
        Right
    }

    /// <summary>
    /// Random utilities that can be used anywhere in the program.
    /// </summary>
    public class Utils
    {
        /// <summary>
        /// Random to be used anywhere.
        /// </summary>
        public static Random Random = new Random();
    }
}
