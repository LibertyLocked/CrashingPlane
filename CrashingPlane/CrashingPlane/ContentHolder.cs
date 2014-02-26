using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;

namespace CrashingPlane
{
    /// <summary>
    /// A staic class that holds all content assets.
    /// </summary>
    class ContentHolder
    {
        public static bool Loaded = false;
        public static string StatusText = " LOADING ";

        public static SpriteFont MainFont;  // loaded by game. do NOT load here.
        public static SpriteFont BigFont;

        // Menu textures
        public static Texture2D Blank;
        public static Texture2D TitleTexture, PressStartTexture;

        // Entities textures
        public static Texture2D ChopperSheet;
        public static Texture2D FuelCanTexture;
        public static Texture2D BirdSheet;

        // Day & night textures
        public static Texture2D DaySkyTexture, NightSkyTexture;
        public static Texture2D MoonTexture, SunTexture;

        // Scrolling backgrounds
        public static Texture2D[] BackgroundClouds = new Texture2D[5];
        public static Texture2D[] BackgroundMountain1 = new Texture2D[5];
        public static Texture2D[] BackgroundGround = new Texture2D[5];
        public static Texture2D[] BackgroundTree1 = new Texture2D[5];
        public static Texture2D[] BackgroundTree2 = new Texture2D[2];
        
        public static void LoadAllContent(ContentManager content)
        {
            if (!Loaded)
            {
                StatusText = " WAITING FOR THREAD ";
                System.Threading.Thread.Sleep(2000);    // Just to admire the cool loading screen

                StatusText = " LOADING SOUNDS ";

                StatusText = " LOADING TEXTURES ";
                BigFont = content.Load<SpriteFont>(@"fonts\bigFont");

                Blank = content.Load<Texture2D>(@"textures\blank");
                TitleTexture = content.Load<Texture2D>(@"textures\titles\title");
                PressStartTexture = content.Load<Texture2D>(@"textures\titles\pressStart");

                ChopperSheet = content.Load<Texture2D>(@"textures\entities\chopper");
                //FuelCanTexture = content.Load<Texture2D>(@"textures\entities\fuelcan");
                //BirdSheet = content.Load<Texture2D>(@"textures\entities\bird");

                StatusText = " LOADING ENVIRONMENT ";
                // Load static sky backgrounds
                MoonTexture = content.Load<Texture2D>(@"textures\backgrounds\static\moon");
                SunTexture = content.Load<Texture2D>(@"textures\backgrounds\static\sun");

                // Load scrolling backgrounds
                for (int i = 0; i < BackgroundClouds.Length; i++)
                    BackgroundClouds[i] = content.Load<Texture2D>(@"textures\backgrounds\clouds\" + i);
                for (int i = 0; i < BackgroundMountain1.Length; i++)
                    BackgroundMountain1[i] = content.Load<Texture2D>(@"textures\backgrounds\mountain1\" + i);
                for (int i = 0; i < BackgroundGround.Length; i++)
                    BackgroundGround[i] = content.Load<Texture2D>(@"textures\backgrounds\ground\" + i);
                for (int i = 0; i < BackgroundTree1.Length; i++)
                    BackgroundTree1[i] = content.Load<Texture2D>(@"textures\backgrounds\tree1\" + i);
                for (int i = 0; i < BackgroundTree2.Length; i++)
                    BackgroundTree2[i] = content.Load<Texture2D>(@"textures\backgrounds\tree2\" + i);

                Loaded = true;
            }
        }
    }
}
