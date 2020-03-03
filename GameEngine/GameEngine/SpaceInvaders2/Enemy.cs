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
    public class Enemy : GameObject
    {
        public bool isActive { get; set; }
        public bool isFollowing { get; set; }
        public double followingModifier { get; set; }

        public int movementDirection { get; set; }
        public int scoreValue { get; set; }

        public Enemy(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light)
            : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            // rigidbody.Transform.LocalScale = new Vector3(4, 4, 4);
            rigidbody.Transform.Rotate(Vector3.Left, 1.5f);
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("Squid"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * Constants.EnemyBoundingSphereScale * 35;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);

            //*** Additional Property (for Asteroid, isActive = true)
            isActive = true;
        }

        public override void Update()
        {
            if (!isActive) return;

            base.Update();

        }


        public override void Draw()
        {
            if (isActive) base.Draw();
        }


    }
}
