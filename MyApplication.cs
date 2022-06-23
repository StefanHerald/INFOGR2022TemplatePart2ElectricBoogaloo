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
        public Vector3 cameraPos = new Vector3();
        // initialize
        public void Init()
        {
            sceneGraph = new SceneGraph();
            sceneGraph.useRenderTarget = useRenderTarget;
            sceneGraph.AddMesh("../../assets/teapot.obj", Matrix4.CreateScale(0.5f) );
            sceneGraph.AddMesh("../../assets/floor.obj", Matrix4.CreateScale(4.0f) );
            // create shaders
            sceneGraph.shader = new Shader("../../shaders/vs.glsl", "../../shaders/fs.glsl");
            sceneGraph.postproc = new Shader("../../shaders/vs_post.glsl", "../../shaders/fs_post.glsl");
            //add lights
            sceneGraph.AddLight(Vector3.One, new Vector3(255,255,255));
            // load a texture
            sceneGraph.texture = new Texture("../../assets/wood.jpg");
            // create the render target
            sceneGraph.target = new RenderTarget(screen.width, screen.height);
            sceneGraph.quad = new ScreenQuad();
        }

        // tick for background surface
        public void Tick()
        {
            screen.Clear(0x000010);
        }

        // tick for OpenGL rendering code
        public void RenderGL()
        {
            float angle90degrees = PI / 2;
            Matrix4 Tcamera = Matrix4.CreateTranslation(new Vector3(cameraPos.X, 14.5f + cameraPos.Y, cameraPos.Z)) * Matrix4.CreateFromAxisAngle(new Vector3(1, 0, 0), angle90degrees);
            Matrix4 Tview = Matrix4.CreatePerspectiveFieldOfView(1.2f, 1.3f, .1f, 1000);
            // Tcamera = Matrix4.Zero;
            sceneGraph.Render(new Vector3(5,10,5), new Vector3(1, 0, 0), new Vector3(0, -1, 0));
        }
    }
}