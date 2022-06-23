using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
namespace Template
{

    internal class SceneGraph
    {
        public List<Mesh> meshes = new List<Mesh>();
        public List<Light> lights = new List<Light>();
        public Matrix4 cameraTransform;
        Matrix4 orthographicViewVolume;
        //texture
        public Texture texture;
        public Shader shader;                          // shader to use for rendering
        public Shader postproc;                        // shader to use for post processing
        public RenderTarget target;                    // intermediate render target
        public ScreenQuad quad;                        // screen filling quad for post processing
        const float PI = 3.1415926535f;                // want some pie?
        float a = 0;                                       //
        float angle90degrees = PI / 2;
        Stopwatch timer;                        // timer for measuring frame duration
        public bool useRenderTarget = true;    //true if you want to use the rendering target

        public SceneGraph()
        {
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            //the orthographic view volume is a box in which all object to render are.
            orthographicViewVolume = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
        }

        public void Render(Vector3 cameraPosition, Vector3 upDirection, Vector3 lookAtDirection)
        {
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();
            shader.SetVec3("cameraPos", cameraPosition);
            cameraTransform = Matrix4.LookAt(cameraPosition,cameraPosition + lookAtDirection, upDirection);
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget) target.Bind();
            foreach (Mesh mesh in meshes)
            {
                mesh.Render(shader, mesh.localPosition * cameraTransform  * orthographicViewVolume, mesh.localPosition, texture);
            }
            if (useRenderTarget) quad.Render(postproc, target.GetTextureID());
            if (useRenderTarget) target.Unbind();
        }

        public void AddMesh(string fileName, Matrix4 pos)
        {
            Mesh mesh = new Mesh(fileName);
            mesh.localPosition = pos;
            meshes.Add(mesh);
        }
        public void AddLight(Vector3 pos, Vector3 color)
        {
            Light light = new Light(pos, color);
            lights.Add(light);
            shader.SetVec3("lightPos", light.lightPosition);
            shader.SetVec3("lightColor", light.lightColor);

        }

    }
}
