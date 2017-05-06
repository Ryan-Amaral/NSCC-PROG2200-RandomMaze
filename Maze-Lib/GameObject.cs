using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Maze_Lib
{
    /// <summary>
    /// Allows objects in the game to update their state.
    /// </summary>
    public interface GameObject
    {
        /// <summary>
        /// Updates the state of the sprite.
        /// </summary>
        void Update();
    }
}
