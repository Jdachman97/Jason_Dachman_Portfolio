using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CPI311.GameEngine;

namespace CPI311.GameEngine
{
    public class AnimatedSprite : Sprite
    {
        public int Frames { get; set; }
        public float FrameX { get; set; }
        public float FrameY { get; set; }
        public float Speed { get; set; }




        public AnimatedSprite(Texture2D texture, int frames = 1) : base(texture)
        {
            Frames = frames;
            FrameX = 0;
            FrameY = 0;
            Speed = 8;
        }
        public override void Update()
        {
            FrameX += Speed * Time.ElapsedGameTime;
            // Something more needs to be done (when animation ends)  
            Source = new Rectangle((Texture.Width / 8) * (int)FrameX, Texture.Height / 5 * (int)FrameY, Texture.Width/8, Texture.Height / 5);
            Origin = new Vector2(7, 7);
           
            if(FrameX >= 7)
            {
                FrameX = 0;
            }
        }


    }



}
