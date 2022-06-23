using System;
using System.Collections.Generic;
using OpenTK;

namespace Template
{
    class MyApplication
    {
        // member variables
        public Surface screen;                  // background surface for printing etc.
        SceneGraph sceneGraph;
        bool useRenderTarget = false;
        const float PI = 3.1415926535f;
        Vector3 upDirection = new Vector3(1, 1, 0);
        Vector3 lookAtDirection = new Vector3(1, -1, 0);
        public Vector3 cameraPos = new Vector3();

        // initialize
        public void Init()
        {
            sceneGraph = new SceneGraph();
            sceneGraph.useRenderTarget = useRenderTarget;
            //add meshes to the sceneGraph
            sceneGraph.AddMesh(CreateMesh("../../assets/teapot.obj", "../../assets/wood.jpg", Matrix4.CreateScale(0.5f)));
            sceneGraph.AddMesh(CreateMesh("../../assets/floor.obj", "../../assets/wood.jpg", Matrix4.CreateScale(4.0f)));
            // create shaders
            sceneGraph.shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            sceneGraph.postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            //add lights
            sceneGraph.AddLight(new Vector3(2, 10, 2), new Vector3(255, 255, 255));
            // create the render target
            sceneGraph.target = new RenderTarget(screen.width, screen.height);
            sceneGraph.quad = new ScreenQuad();
            cameraPos = new Vector3(0, 14, 0);
            upDirection.Normalize();
            lookAtDirection.Normalize();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0x000010);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            sceneGraph.Render(cameraPos, upDirection, lookAtDirection);
        }

        public void Move(int direction)
        {
            switch (direction)
            {
                case (0): //Right
                    cameraPos -= Vector3.Cross(upDirection, lookAtDirection);
                    break;

                case (1): //Left
                    cameraPos += Vector3.Cross(upDirection, lookAtDirection);
                    break;

                case (2): //up
                    cameraPos += upDirection;
                    break;

                case (3): //down
                    cameraPos -= upDirection;
                    break;

                case (4): //forward
                    cameraPos += lookAtDirection;
                    break;

                case (5): //backward
                    cameraPos -= lookAtDirection;
                    break;

                case (6): //rotate the lookAtDirection around the updirection
                    lookAtDirection = Vector3.TransformPerspective(lookAtDirection, Matrix4.CreateFromAxisAngle(upDirection, PI / 16));
                    break;

                case (7):// rotate the updirection around the lookatdirection
                    upDirection = Vector3.TransformPerspective(upDirection, Matrix4.CreateFromAxisAngle(lookAtDirection, PI / 16));
                    break;
            }
        }

        public Mesh CreateMesh(string filename, string texture, Matrix4 transform, List<Mesh> children = null)
        {
            Mesh mesh = new Mesh(filename);
            mesh.texture = new Texture(texture);
            mesh.localPosition = transform;
            if (children != null)
                foreach (Mesh child in children)
                    mesh.AddChild(child);
            return mesh;
        }
    }
}