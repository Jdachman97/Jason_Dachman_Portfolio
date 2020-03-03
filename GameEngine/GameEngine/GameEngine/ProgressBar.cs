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
    public class ProgressBar : Sprite
    {
        public Color FillColor { get; set; }
        public float Value { get; set; }

        public ProgressBar(Texture2D texture, float value) : base (texture)
        {
            Value = value;
            
           
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch); // let the sprite do its work 
            spriteBatch.Draw(Texture, Position, new Rectangle(0,0,0,0),
            FillColor, Rotation, Origin, Scale, this.spriteEffect, Layer);

            Source = new Rectangle(0, 0, (int)Value, Texture.Height);
           
        }

    }




}
