﻿using System;
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
    public class Ship : GameObject
    {
        public bool isActive;
        public Ship(ContentManager Content, Camera camera, GraphicsDevice graphicsDevice, Light light) : base()
        {
            // *** Add Rigidbody
            Rigidbody rigidbody = new Rigidbody();
            rigidbody.Transform = Transform;
            rigidbody.Mass = 1;
            Add<Rigidbody>(rigidbody);

            // *** Add Renderer
            Texture2D texture = Content.Load<Texture2D>("Square");
            Renderer renderer = new Renderer(Content.Load<Model>("p1_wedge"), Transform, camera, Content, graphicsDevice, light, 1, 20.0f, texture, null);
            Add<Renderer>(renderer);

            // *** Add collider
            SphereCollider sphereCollider = new SphereCollider();
            sphereCollider.Radius = renderer.ObjectModel.Meshes[0].BoundingSphere.Radius * GameConstants.ShipBoundingSphereScale;
            sphereCollider.Transform = Transform;
            Add<Collider>(sphereCollider);

            isActive = true;
        }
        public override void Update()
        {
            if (isActive)
            {
                if (InputManager.IsKeyDown(Keys.A))
                {
                    this.Transform.Rotate(Vector3.Up, Time.ElapsedGameTime * GameConstants.shipRotateSpeed);
                }
                if (InputManager.IsKeyDown(Keys.D))
                {
                    this.Transform.Rotate(Vector3.Down, Time.ElapsedGameTime * GameConstants.shipRotateSpeed);
                }
                if (InputManager.IsKeyDown(Keys.W))
                {
                    this.Transform.LocalPosition += this.Transform.Forward * Time.ElapsedGameTime * GameConstants.shipSpeed;
                }
                if (InputManager.IsKeyDown(Keys.S))
                {
                    this.Transform.LocalPosition += this.Transform.Backward * Time.ElapsedGameTime * GameConstants.shipSpeed;
                }

                //screen wrapping
                if (this.Transform.LocalPosition.X > GameConstants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition -= Vector3.UnitX * 2 * GameConstants.PlayfieldSizeX;
                }

                if (this.Transform.LocalPosition.X < -GameConstants.PlayfieldSizeX)
                {
                    this.Transform.LocalPosition += Vector3.UnitX * 2 * GameConstants.PlayfieldSizeX;
                }

                if (this.Transform.LocalPosition.Z > GameConstants.PlayfieldSizeY)
                {
                    this.Transform.LocalPosition -= Vector3.UnitZ * 2 * GameConstants.PlayfieldSizeY;
                }

                if (this.Transform.LocalPosition.Z < -GameConstants.PlayfieldSizeY)
                {
                    this.Transform.LocalPosition += Vector3.UnitZ * 2 * GameConstants.PlayfieldSizeY;
                }
            }

            base.Update();
        }

        public override void Draw()
        {
            if (isActive) base.Draw();
        }
    }
}