using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using CPI311.GameEngine.Rendering;
using CPI311.GameEngine;

namespace Assignment5
{
    public class Player : GameObject
    {
        public TerrainRenderer Terrain { get; set; }

        public Player(TerrainRenderer terrain, ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {

            Terrain = terrain;

            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);
           
            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Sphere"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = 1.0f;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);
        }

        public override void Update()
        {
            // Control the player
            if (InputManager.IsKeyDown(Keys.W)) // move forward
                this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * 9;
            if (InputManager.IsKeyDown(Keys.S)) // move backwars
                this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * 9;
            if (InputManager.IsKeyDown(Keys.A)) // move left
                this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * 9;
            if (InputManager.IsKeyDown(Keys.D)) // move right
                this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * 9;

            // change the Y position corresponding to the terrain (maze)
            this.Transform.LocalPosition = new Vector3(
                this.Transform.LocalPosition.X,
                Terrain.GetAltitude(this.Transform.LocalPosition),
                this.Transform.LocalPosition.Z) + Vector3.Up;

            base.Update();
        }

    }
}
