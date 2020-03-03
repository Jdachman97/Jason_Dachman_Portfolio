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
    public class RightBarrier : GameObject
    {

        public RightBarrier(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light)
            : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Transform.LocalScale = new Vector3(5, 1, 100);
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("CubeRig"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
           // SphereCollider sphereCollider = new SphereCollider();
            BoxCollider boxCollider = new BoxCollider();
           // sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * 5000;
            boxCollider.Size = 2500;
            //sphereCollider.Transform = Transform;
            boxCollider.Transform = Transform;    
            boxCollider.Transform.LocalPosition = new Vector3(Constants.PlayfieldSizeX + 5000, 0, -Constants.PlayfieldSizeY + 10000);
            Console.WriteLine(boxCollider.Transform.LocalPosition);
           // sphereCollider.Transform.LocalScale = new Vector3(5, 1, 1000);
           // boxCollider.Transform.LocalScale = new Vector3(5, 1, 1000);
            Add<Collider>(boxCollider);

            //*** Additional Property (for Asteroid, isActive = true)

        }

        public override void Update()
        {


            base.Update();

        }


        public override void Draw()
        {
            base.Draw();
        }


    }
}