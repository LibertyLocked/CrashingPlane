using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace CrashingPlane
{
    class GlobalHelper
    {
        public static bool DebugMode = false;
        public static GameState CurrentGameState = GameState.Loading;   // game starts in loading state
        public static PlayerIndex? ActivePlayerIndex = null;

        // constant game size to be scaled from
        public const int GameWidth = 1920;
        public const int GameHeight = 1080;

        /// <summary>
        /// Gets the resolution width at which the game is rendering.
        /// </summary>
        public static int RenderWidth
        {
            get { return WindowWidth; }
        }

        /// <summary>
        /// Gets the resolution height at which the game is rendering.
        /// </summary>
        public static int RenderHeight
        {
            get { return (int)((float)WindowWidth / GameWidth * GameHeight); }
        }

        // These must be set at start up.
        public static int WindowWidth;
        public static int WindowHeight;

        /// <summary>
        /// A get only property indicating whether the game is rendering at native resolution.
        /// </summary>
        public static bool IsRenderNative
        {
            get { return (GameWidth == RenderWidth && GameHeight == RenderHeight); }
        }

        /// <summary>
        /// Gets the origin point where the render target should be drawn.
        /// </summary>
        public static Vector2 RenderOrigin
        {
            get { return new Vector2(0, (GlobalHelper.WindowHeight - GlobalHelper.RenderHeight) / 2); }
        }
    }
}
