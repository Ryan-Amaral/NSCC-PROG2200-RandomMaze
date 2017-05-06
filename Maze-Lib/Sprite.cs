using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// Allows objects in the game to be drawn.
    /// </summary>
    public interface Sprite
    {
        /// <summary>
        /// Draws the sprite to the screen.//
        /// </summary>
        /// <param name="graphics"></param>
        void Draw(Graphics graphics);
    }
}
