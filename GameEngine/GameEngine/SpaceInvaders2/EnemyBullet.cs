using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using CPI311.GameEngine.Rendering;

namespace CPI311.GameEngine
{
    public class EnemyBullet : GameObject
    {
        public bool isActive;

        public EnemyBullet(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Transform.LocalScale = new Vector3(0.4f, 0.4f, 0.4f);
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("bullet3"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);

            //*** Additional Property (for Asteroid, isActive = true)
            isActive = false;
        }

        public override void Update()
        {
            if (!isActive) return;

            if (Transform.Position.X > Constants.PlayfieldSizeX ||
               Transform.Position.X < -Constants.PlayfieldSizeX ||
               Transform.Position.Z > Constants.PlayfieldSizeY ||
               Transform.Position.Z < -Constants.PlayfieldSizeY)
            {
                isActive = false;
                Rigidbody.Velocity = Vector3.Zero; // stop moving
            }

            base.Update();

        }
        public override void Draw()
        {
            if (isActive) base.Draw();
        }

    }
}