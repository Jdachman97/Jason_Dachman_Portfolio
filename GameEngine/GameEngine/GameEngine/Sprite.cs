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
    public class Sprite
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public Rectangle Source { get; set; }
        public Color Color { get; set; }
        public Single Rotation { get; set; }
        public Vector2 Origin { get; set; }
        public Vector2 Scale { get; set; }
        public SpriteEffects spriteEffect { get; set; }
        public Single Layer { get; set; }

        public Sprite(Texture2D texture)
        {
            Texture = texture;
            Position = Vector2.Zero;
            Source = new Rectangle(0, 0, Texture.Width, Texture.Height);
            Color = Color.White;
            Scale = new Vector2(1, 1);
            Rotation = 0;
            Origin = new Vector2(Texture.Width / 2, Texture.Height / 2);
            spriteEffect = SpriteEffects.None;
            Layer = 1;

        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(
                Texture,
                Position,
                Source,
                Color,
                Rotation,
                Origin,
                Scale,
                spriteEffect,
                Layer
            );
        }


        public virtual void Update()
        {

        }
    }
    


}
