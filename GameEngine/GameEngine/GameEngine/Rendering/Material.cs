using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace CPI311.GameEngine.Rendering
{
    public class Material
    {
        public Effect effect;

        

        public int Passes { get { return effect.CurrentTechnique.Passes.Count; } }
        public int CurrentTechnique { get; set; }
        public Vector3 Diffuse { get; set; }
        public Vector3 Ambient { get; set; }
        public Vector3 Specular { get; set; }
        public float Shininess { get; set; }
        public Matrix World { get; set; }
        public Camera Camera { get; set; }
        public int currentTechnique { get; set; }
        public Texture2D diffuseTexture { get; set; }
        public Light Light { get; set; }
       


        public Material(Matrix world, Camera camera, ContentManager content,Light light, String fileLoc, int currentTechnique, float shininess, Texture2D texture)
        {
            effect = content.Load<Effect>(fileLoc);
            World = world;
            Camera = camera;
            Light = light;
            Shininess = shininess;
            CurrentTechnique = currentTechnique;
            Ambient = light.Ambient.ToVector3();
            Diffuse = light.Diffuse.ToVector3();
            Specular = light.Specular.ToVector3();
            diffuseTexture = texture;
        }
        public virtual void Apply(int currentPass)
        {
            effect.CurrentTechnique = effect.Techniques[currentTechnique]; //"0" is the first technique
            effect.Parameters["World"].SetValue(World);
            effect.Parameters["View"].SetValue(Camera.View);
            effect.Parameters["Projection"].SetValue(Camera.Projection);
            effect.Parameters["LightPosition"].SetValue(Light.Transform.Position);
            effect.Parameters["CameraPosition"].SetValue(Camera.Transform.Position);
            effect.Parameters["Shininess"].SetValue(Shininess);
            effect.Parameters["AmbientColor"].SetValue(Ambient);
            effect.Parameters["DiffuseColor"].SetValue(Diffuse);
            effect.Parameters["SpecularColor"].SetValue(Specular);
            effect.Parameters["DiffuseTexture"].SetValue(diffuseTexture);

            effect.CurrentTechnique.Passes[currentPass].Apply();
        }

    }

}
