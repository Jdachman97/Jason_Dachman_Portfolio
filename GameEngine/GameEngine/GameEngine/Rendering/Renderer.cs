﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CPI311.GameEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CPI311.GameEngine.Rendering
{
    public class Renderer : Component, IRenderable
    {
        public Material Material { get; set; }
        public Model ObjectModel;
        public Transform ObjectTransform;
        public int CurrentTechnique;
        public GraphicsDevice g;
        public Camera Camera;
        public Light Light;


        public Renderer(Model objModel, Transform objTransform, Camera camera,
          ContentManager content, GraphicsDevice graphicsDevice, Light light, int currentTechnique, float shininess, Texture2D texture, String filename)  {
            if (filename != null)
                Material = new Material(objTransform.World, camera, content, light, filename, currentTechnique, shininess, texture);
            else

                Material = null;
                ObjectModel = objModel;
                ObjectTransform = objTransform;
                Camera = camera;
                Light = light;
         
                g = graphicsDevice;
                CurrentTechnique = currentTechnique;
}

        public virtual void Draw()
        {
            if (Material != null)
            {
                Material.Camera = Camera; // Update Material's properties
                Material.World = ObjectTransform.World;
                Material.Light = Light;
                Material.CurrentTechnique = CurrentTechnique;
                for (int i = 0; i < Material.Passes; i++)
                {
                    Material.Apply(i); // Look at the Material's Apply method
                    foreach (ModelMesh mesh in ObjectModel.Meshes)
                    {
                       // foreach(BasicEffect effect in mesh.Effects)
                        //{
                          //  effect.Texture = Material.diffuseTexture;
                           
                        //}
                        foreach (ModelMeshPart part in mesh.MeshParts)
                        {
                            g.SetVertexBuffer(part.VertexBuffer);
                            g.Indices = part.IndexBuffer;
                            g.DrawIndexedPrimitives(PrimitiveType.TriangleList, part.VertexOffset, part.StartIndex, part.PrimitiveCount);
                        }
                    }
                }
            }
            else
                ObjectModel.Draw(ObjectTransform.World, Camera.View, Camera.Projection);
        }
    }

}
