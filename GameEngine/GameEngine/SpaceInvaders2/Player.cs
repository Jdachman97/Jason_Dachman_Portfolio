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


namespace CPI311.GameEngine
{
    public class Player : GameObject
    {
        public bool isActive;
        public int lives = 3;
        public Player(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Transform.LocalScale = new Vector3(400, 400, 400);
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("lowpoly2"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * Constants.ShipBoundingSphereScale * 400;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);

            isActive = true;
        }
        public override void Update()
        {
            if (isActive)
            {
                if (InputManager.IsKeyDown(Keys.A) && this.Transform.LocalPosition.X > -Constants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition += this.Transform.Left * Time.ElapsedGameTime * Constants.shipSpeed;
                }
                if (InputManager.IsKeyDown(Keys.D) && this.Transform.LocalPosition.X < Constants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition += this.Transform.Right * Time.ElapsedGameTime * Constants.shipSpeed;
                }
              /*  if (InputManager.IsKeyDown(Keys.W))
                {
                    this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * Constants.shipSpeed;
                }
                if (InputManager.IsKeyDown(Keys.S))
                {
                    this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * Constants.shipSpeed;
                }*/

                //screen wrapping
               /* if (this.Transform.LocalPosition.X > Constants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition -= Vector3.UnitX * 2 * Constants.PlayfieldSizeX;
                }

                if (this.Transform.LocalPosition.X < -Constants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition += Vector3.UnitX * 2 * Constants.PlayfieldSizeX;
                }

                if (this.Transform.LocalPosition.Z > Constants.PlayfieldSizeY)
                {
                    this.Transform.LocalPosition -= Vector3.UnitZ * 2 * Constants.PlayfieldSizeY;
                }

                if (this.Transform.LocalPosition.Z < -Constants.PlayfieldSizeY)
                {
                    this.Transform.LocalPosition += Vector3.UnitZ * 2 * Constants.PlayfieldSizeY;
                }*/
            }

            base.Update();
        }

        public override void Draw()
        {
            if (isActive) base.Draw();
        }
    }
}