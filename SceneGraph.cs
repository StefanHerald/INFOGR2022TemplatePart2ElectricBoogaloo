using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;
namespace Template
{

    internal class SceneGraph
    {
        public List<Mesh> meshes = new List<Mesh>();
        public List<Light> lights = new List<Light>();
        public Matrix4 cameraTransform;
        Matrix4 cameraToRaster;
        public bool onUpdateCamera;
        //texture
        public Shader shader;                          // shader to use for rendering
        public Shader postproc;                        // shader to use for post processing
        public RenderTarget target;                    // intermediate render target
        public ScreenQuad quad;                        // screen filling quad for post processing
        const float PI = 3.1415926535f;                // want some pie?
        float a = 0;
        Stopwatch timer;                        // timer for measuring frame duration
        public bool useRenderTarget = true;    //true if you want to use the rendering target
        //the variables that create the frustrum
        float fovy = 1.2f;
        float aspect = 1.3f;
        float zNear = 0.1f;
        float zFar = 1000f;
        /// <summary>
        /// init 
        /// </summary>
        public SceneGraph()
        {
            timer = new Stopwatch();
            //the orthographic view volume is a box in which all object to render are.
            cameraToRaster = Matrix4.CreatePerspectiveFieldOfView(fovy, aspect, zNear, zFar);
        }
        /// <summary>
        /// Render the scene
        /// </summary>
        /// <param name="cameraPosition"></param>
        /// <param name="upDirection"></param>
        /// <param name="lookAtDirection"></param>
        public void Render(Vector3 cameraPosition, Vector3 upDirection, Vector3 lookAtDirection)
        {
            float frameDuration = timer.ElapsedMilliseconds;
            //reset and start the timer
            timer.Reset();
            timer.Start();
            //add the camera to the shader
            shader.SetVec3("cameraPos", cameraPosition);
            //generate the world to camera matrix
            cameraTransform = Matrix4.LookAt(cameraPosition, cameraPosition + lookAtDirection, upDirection);
            //update a
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            //do the render for each mesh, with the shader, the object to screen transform and an object to world transform
            if (useRenderTarget) target.Bind();
            foreach (Mesh mesh in meshes)
            {
                Matrix4 M = mesh.localPosition * cameraTransform * cameraToRaster;
                mesh.Render(shader, M, mesh.localPosition);
            }
            if (useRenderTarget) quad.Render(postproc, target.GetTextureID());
            if (useRenderTarget) target.Unbind();
            onUpdateCamera = false;
        }

        /// <summary>
        /// Add a mesh to the sceneGraph
        /// </summary>
        /// <param name="mesh"></param> The mesh (with childeren)
        public void AddMesh(Mesh mesh)
        {
            meshes.Add(mesh);
        }
        /// <summary>
        /// add a light to the sceneGraph
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="color"></param>
        public void AddLight(Vector3 pos, Vector3 color, bool isSpot = false, Vector3 Center = new Vector3(), Vector3 Radius = new Vector3(), Vector3 ray1 = new Vector3(), Vector3 ray2 = new Vector3())
        {
            Light light = new Light(pos, color, isSpot, Center, Radius, ray1, ray2);
            lights.Add(light);
            shader.SetVec3("lightPos", light.lightPosition);
            shader.SetVec3("lightColor", light.lightColor);
            if(isSpot)
            {
                shader.SetBool("SpotB", 1);
                shader.SetVec3("ray1", toArray(light.ray1));
                shader.SetVec3("ray2", toArray(light.ray2));
                shader.SetVec3("Length", toArray(new Vector3(light.xLength, 0, light.zLength)));
            }
        }
        }
    }
}
