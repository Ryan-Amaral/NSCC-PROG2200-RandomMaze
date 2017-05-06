using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// The screen that shows when the player wins
    /// </summary>
    public class WinScreen : Sprite
    {
        private Rectangle _drawRect;
        private Font _font;
        private string _winMessage;
        private Point _point;

        /// <summary>
        /// Creates a new WinScreen with the draw area.
        /// </summary>
        /// <param name="drawRect"></param>
        public WinScreen(Rectangle drawRect)
        {
            _drawRect = drawRect;
            _font = new Font("ComicSans", 85);
            _winMessage = "You Win!!!";
            _point = _drawRect.Location;
        }

        /// <summary>
        /// Draws the winning message.
        /// </summary>
        /// <param name="graphics"></param>
        public void Draw(Graphics graphics)
        {
            graphics.DrawString(_winMessage, _font, Brushes.White, _point);
        }
    }
}
