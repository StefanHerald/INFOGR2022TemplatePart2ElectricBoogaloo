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
        public Matrix4 cameraTransform;
        Matrix4 orthographicViewVolume;
        Matrix4 viewPort;
        Matrix4 perspective;
        Matrix4 M;
        //the bottom and the top of the view volume in object space, to be used in the orthographic view volume tranformation
        Vector3 bottom = new Vector3(-1000, -1000, -1000f);
        Vector3 top = new Vector3(1000, 1000, 1000);
        //texture
        public Texture texture;
        public Shader shader;                          // shader to use for rendering
        public Shader postproc;                        // shader to use for post processing
        public RenderTarget target;                    // intermediate render target
        public ScreenQuad quad;                        // screen filling quad for post processing
        const float PI = 3.1415926535f;
        float a = 0;
        Stopwatch timer;                        // timer for measuring frame duration
       public bool useRenderTarget = true;

        public SceneGraph()
        {
            timer = new Stopwatch();
            timer.Reset();
            timer.Start();
            //the orthographic view volume is a box in which all object to render are. This box is arbitrarily placed, and has to be moved back to center at the origin and rotated.
            orthographicViewVolume = new Matrix4(2 / (top.X - bottom.X), 0, 0, 0,
                2 / (top.Y - bottom.Y), 0, 0, 0,
                2 / (top.Z - bottom.Z), 0, 0, 0,
                0, 0, 0, 1) *
                new Matrix4(1, 0, 0, -(top.X + bottom.X) / 2,
                0, 1, 0, -(top.Y + bottom.Y) / 2,
                0, 0, 1, -(top.Z + bottom.Z) / 2,
                0, 0, 0, 1);

            //the viewport transformation
            viewPort = new Matrix4(1, 0, 0, OpenTKApp.app.screen.width / 2,
                0, 1, 0, OpenTKApp.app.screen.height / 2,
                0, 0, 1, 0,
                0, 0, 0, 1) * new Matrix4(OpenTKApp.app.screen.width / 2, 0, 0, 0,
                0, OpenTKApp.app.screen.height / 2, 0, 0,
                0, 0, 1, 0,
                0, 0, 0, 1);

            //perspective transformation
            perspective = new Matrix4(bottom.Z, 0, 0, 0,
                0, bottom.Z, 0, 0,
                0, 0, bottom.Z - top.Z, -top.Z * bottom.Z,
                0, 0, 1, 0);
        }

        public void Render(Matrix4 cameraPosition, Vector3 upDirection, Vector3 lookAtDirection)
        {
            float angle90degrees = PI / 2;
            float frameDuration = timer.ElapsedMilliseconds;
            timer.Reset();
            timer.Start();

            Vector3 w = -lookAtDirection;
            w.Normalize();
            Vector3 u = Vector3.Cross(upDirection, w);
            u.Normalize();
            Vector3 v = Vector3.Cross(w, u);
            //add the constant camera matrix transformation
            cameraTransform = new Matrix4(u.X, u.Y, u.Z, 0f,
                v.X, v.Y, v.Z, 0,
                w.X, w.Y, w.Z, 0,
                0, 0, 0, 1) *
                new Matrix4(1, 0, 0, -cameraPosition.M11,
                0, 1, 0, -cameraPosition.M22,
                0, 0, 1, -cameraPosition.M33,
                0, 0, 0, 1);
            
            a += 0.001f * frameDuration;
            if (a > 2 * PI) a -= 2 * PI;

            if (useRenderTarget) target.Bind();
            foreach (Mesh mesh in meshes)
            {
                mesh.Render(shader, mesh.localPosition * cameraTransform /* perspective * orthographicViewVolume * viewPort */, texture);
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
    }
}
