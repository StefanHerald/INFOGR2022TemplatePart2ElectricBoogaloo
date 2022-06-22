using System.Diagnostics;
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
        Vector3 upDirection = new Vector3(1, 0, 0);
        Vector3 lookAtDirection = new Vector3(0, -1, 0);
        public Vector3 cameraPos = new Vector3();
        internal enum movementDirections {left, right, up, down, forward, backward }

        // initialize
        public void Init()
        {
            sceneGraph = new SceneGraph();
            sceneGraph.useRenderTarget = useRenderTarget;
            sceneGraph.AddMesh("../../assets/teapot.obj", Matrix4.CreateScale(0.5f));
            sceneGraph.AddMesh("../../assets/floor.obj", Matrix4.CreateScale(4.0f));
            // create shaders
            sceneGraph.shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            sceneGraph.postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            // load a texture
            sceneGraph.texture = new Texture("../../assets/wood.jpg");
            // create the render target
            sceneGraph.target = new RenderTarget(screen.width, screen.height);
            sceneGraph.quad = new ScreenQuad();
            cameraPos = new Vector3(0, 14, 0);
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
                case (0): //left
                    cameraPos -= Vector3.Cross(upDirection, lookAtDirection);
                    break;

                case (1): //right
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
            }
        }
    }
}