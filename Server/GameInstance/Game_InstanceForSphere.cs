using Florence.ServerAssembly.GameInstance;
using Florence.ServerAssembly.Graphics;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using System.Diagnostics;
using System.Drawing;

namespace Florence.GameInstance
{
    public sealed class Game_InstanceForSphere : GameWindow
    {
        private bool done_once;

        private readonly string _title;
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);

        private Matrix4 _projectionMatrix;
        private float _fov = 90f;

        private KeyboardState _lastKeyboardState;
        private MouseState _lastMouseState;

        private GameObjectFactory _gameObjectFactory;
        private readonly List<APlayerObject> _playerObjects = new List<APlayerObject>();
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        
        private bool cameraSelector = false;

        public Game_InstanceForSphere()
            : base(1920, // initial width
                1080, // initial height
                GraphicsMode.Default,
                "",  // initial title
                GameWindowFlags.Fullscreen,
                DisplayDevice.Default,
                4, // OpenGL major version
                5, // OpenGL minor version
                GraphicsContextFlags.ForwardCompatible)
        {
            _title += "dreamstatecoding.blogspot.com: OpenGL Version: " + GL.GetString(StringName.Version);
        }

        protected override void OnResize(EventArgs e)
        {
            GL.Viewport(0, 0, Width, Height);
            CreateProjection();
        }

        protected override void OnLoad(EventArgs e)
        {
            Debug.WriteLine("OnLoad");
            VSync = VSyncMode.Off;
            CreateProjection();
            _solidProgram = new ShaderProgram();
            _solidProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\..\\graphics\\Shaders\\1Vert\\simplePipeVert.c");
            _solidProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\..\\graphics\\Shaders\\5Frag\\simplePipeFrag.c");
            _solidProgram.Link();

            _texturedProgram = new ShaderProgram();
            _texturedProgram.AddShader(ShaderType.VertexShader, "..\\..\\..\\..\\graphics\\Shaders\\1Vert\\simplePipeTexVert.c");
            _texturedProgram.AddShader(ShaderType.FragmentShader, "..\\..\\..\\..\\graphics\\Shaders\\5Frag\\simplePipeTexFrag.c");
            _texturedProgram.Link();

            var models = new Dictionary<string, ARenderable>();
            models.Add("Player", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\dotted.png", 8));
            models.Add("Earth", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\grass.jpg", 8));
            models.Add("Wooden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\wooden.png", 8));
            models.Add("Golden", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\golden.bmp", 8));
            models.Add("Asteroid", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\moonmap1k.jpg", 8));
            models.Add("Spacecraft", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\spacecraft.png", 8));
            models.Add("Gameover", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\gameover.png", 8));
            models.Add("Bullet", new MipMapGeneratedRenderObject(new IcoSphereFactory().Create(3), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\dotted.png", 8));

            //models.Add("TestObject", new TexturedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg"));
            //models.Add("TestObjectGen", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side.jpg", 8));
            //models.Add("TestObjectPreGen", new MipMapManualRenderObject(RenderObjectFactory.CreateTexturedCube(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\graphics\Textures\asteroid texture one side mipmap levels 0 to 8.bmp", 9));

            _gameObjectFactory = new GameObjectFactory(models);
            _gameObjects.Add(_gameObjectFactory.CreateLocalWorld());
            //_gameObjects.Add(_gameObjectFactory.Create_MapSphereFloor());
            _gameObjectFactory.Create_PlayerOnMapSphere();
            _playerObjects.Add(_gameObjectFactory.Get_player());

            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", _gameObjectFactory.Get_player().Get_Position() + new Vector3(10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", _gameObjectFactory.Get_player().Get_Position() + new Vector3(-10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", _gameObjectFactory.Get_player().Get_Position() + new Vector3(0f, 10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", _gameObjectFactory.Get_player().Get_Position() + new Vector3(0f, -10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", _gameObjectFactory.Get_player().Get_Position() + new Vector3(0f, 0f, 10f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", _gameObjectFactory.Get_player().Get_Position() + new Vector3(0f, 0f, -10f), new Vector3(1f)));

            //_camera = new StaticCamera();

            _gameObjectFactory.Get_player().Create_Cameras_OnSphere();
            //_gameObjectFactory.Get_player().Get_CameraFPOS().Initialise_Cameras();

            CursorVisible = false;

            GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
            GL.PatchParameter(PatchParameterInt.PatchVertices, 3);
            GL.PointSize(3);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
            Closed += OnClosed;
            Debug.WriteLine("OnLoad .. done");
        }

        private void OnClosed(object sender, EventArgs eventArgs)
        {
            Exit();
        }

        public override void Exit()
        {
            Debug.WriteLine("Exit called");
            _gameObjectFactory.Dispose();
            _solidProgram.Dispose();
            _texturedProgram.Dispose();
            base.Exit();
        }

        private void CreateProjection()
        {

            var aspectRatio = (float)Width / Height;
            _projectionMatrix = Matrix4.CreatePerspectiveFieldOfView(
                _fov * ((float)Math.PI / 180f), // field of view angle, in radians
                aspectRatio,                // current window aspect ratio
                0.1f,                       // near plane
                4000f);                     // far plane
        }

        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            _time += e.Time;
            foreach (var item in _gameObjects)
            {
                item.Update(_time, e.Time);
            }
            HandleKeyboard(e.Time);
            HandleMouse();
            switch (cameraSelector)
            {
            case false:
                Get_gameObjectFactory().Get_player().Get_CameraFPOS().Update(_time, e.Time);
                break;

            case true:
                Get_gameObjectFactory().Get_player().Get_CameraTPOS().Update(_time, e.Time);
                break;
            }
        }

        private void HandleMouse()
        {
            Console.WriteLine("TESTBENCH => HandleMouse");

            MouseState mouseState = Mouse.GetCursorState();
            var player = _gameObjectFactory.Get_player();
            var camera = player.Get_CameraFPOS();
            if (_gameObjectFactory.Get_player().Get_IsFirstMouseMove() == true)
            {
                camera.Set_Position(player.Get_Position());
                camera.Set_Pitch(0);
                camera.Set_Yaw(0);
                camera.Set_Rotation(player.Get_Rotation());
                camera.Set_fowards(player.Get_fowards());
                camera.Set_up(player.Get_up());
                camera.Set_offset(player.Get_up().Normalized() * 1.8f);
                camera.Set_right(player.Get_right());
                camera.Set_axisX(player.Get_axisX());
                camera.Set_axisY(player.Get_axisY());
                camera.Set_axisZ(player.Get_axisZ());

                OpenTK.Input.Mouse.SetPosition((double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));
                _gameObjectFactory.Get_player().Set_IsFirstMouseMove(false);
            }
            else
            {
                switch (Get_cameraSelector())
                {
                    case false:
                        if (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(1) == false)
                        {
                            if ((mouseState.X != (float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2))
                                || (mouseState.Y != (float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2)))
                            {
                                Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(1, true);
                                float anglePerPixle = Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_fov() / Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y();
                                float deltaDegX = anglePerPixle * (mouseState.X - (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2));
                                float deltaDegY = -( anglePerPixle * (mouseState.Y - (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2)));
                                System.Console.WriteLine("TESTBENCH => deltaX = " + deltaDegX + "  deltaY = " + deltaDegY);
                                float deltaRadAroundUp = (float)((Math.PI / 180) * deltaDegX);
                                float deltaRadAroundRight = (float)((Math.PI / 180) * deltaDegY);

                                camera.ApplyMouseAtPlayerPosition(player, deltaRadAroundUp, deltaRadAroundRight);

                                OpenTK.Input.Mouse.SetPosition((double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));
                                Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(1, false);
                            }
                        }
                        break;

                    case true:

                        break;

                }
                _lastMouseState = mouseState;
                player.Set_lastRotation(camera.Get_Rotation());
                Console.WriteLine("TESTBENCH => HandleMouse .. Done");
            }
        }
        private void HandleKeyboard(double dt)
        {
            System.Console.WriteLine("TESTBENCH => Entered HandleKeyboard");
            var keyState = Keyboard.GetState();
            var player = _gameObjectFactory.Get_player();
            var camera = player.Get_CameraFPOS();
            Vector3 fowards = new Vector3(0);
            Vector3 backwards = new Vector3(0);
            Vector3 left = new Vector3(0);
            Vector3 right = new Vector3(0);

            if (_gameObjectFactory.Get_player().Get_IsFirstMove() == true)
            {
                player.Set_Position(new Vector3(1, 1, 1).Normalized());
                player.CenterPlayerFreeLook();
                _gameObjectFactory.Get_player().Set_IsFirstMove(false);
            }
            else
            {
                switch (Get_cameraSelector())
                {
                case false:
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Exit();
                    }
                    if (keyState.IsKeyDown(Key.Enter))
                    {

                    }
                    if (keyState.IsKeyDown(Key.Space))
                    {
                        Set_cameraSelector(!Get_cameraSelector());
                    }
                    if (keyState.IsKeyDown(Key.W))
                    {
                        System.Console.WriteLine("TESTBENCH => fowards");
                        fowards = camera.Get_fowards();
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(2, true);
                    }
                    if (keyState.IsKeyDown(Key.S))
                    {
                        System.Console.WriteLine("TESTBENCH => backwards");
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(3, true);
                        backwards = -camera.Get_fowards();

                    }
                    if (keyState.IsKeyDown(Key.A))
                    {
                        System.Console.WriteLine("TESTBENCH => right");
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(4, true);
                        right = -camera.Get_right();

                    }
                    if (keyState.IsKeyDown(Key.D))
                    {
                        System.Console.WriteLine("TESTBENCH => left");
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(5, true);
                        left = camera.Get_right();

                    }
                    if (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(2) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(3) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(4) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(5) == true
                    )
                    {
                        player.Set_Direction(player.Get_Position() + new Vector3(fowards + backwards + right + left).Normalized());
                        //camera.AlignePlayerAtGlobalSurfacePosition(player, dt);
                    }
                    for (int index = 2; index < 6; index++)
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(index, false);
                    }
                    break;

                case true:
                    if (keyState.IsKeyDown(Key.Escape))
                    {
                        Exit();
                    }
                    if (keyState.IsKeyDown(Key.Space))
                    {
                        Set_cameraSelector(!Get_cameraSelector());
                    }
                    if ((keyState.IsKeyDown(Key.W)) || (keyState.IsKeyDown(Key.S)) || (keyState.IsKeyDown(Key.A)) || (keyState.IsKeyDown(Key.D)))
                    {
                        if (keyState.IsKeyDown(Key.W))
                        {
                            Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(2, true);

                        }
                        if (keyState.IsKeyDown(Key.S))
                        {
                            Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(3, true);

                        }
                        if (keyState.IsKeyDown(Key.A))
                        {
                            Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(4, true);

                        }
                        if (keyState.IsKeyDown(Key.D))
                        {
                            Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(5, true);

                        }
                    }
                    break;
                }
            }
            _lastKeyboardState = keyState;
            player.Set_lastPosition(player.Get_Position());
            player.Set_lastRotation(player.Get_Rotation());
        }
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            Title = $"{_title}: FPS:{1f / e.Time:0000.0}, obj:{_gameObjects.Count}";
            GL.ClearColor(Color.Black);// _backColor);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            int lastProgram = -1;
            foreach (var obj in _gameObjects)
            {
                var program = obj.Model.Program;
                if (lastProgram != program)
                    GL.UniformMatrix4(20, false, ref _projectionMatrix);
                lastProgram = obj.Model.Program;
                switch (cameraSelector)
                {
                case false:
                    obj.Render(Get_gameObjectFactory().Get_player().Get_CameraFPOS());
                    break;

                case true:
                    obj.Render(Get_gameObjectFactory().Get_player().Get_CameraTPOS());
                    break;
                }
            }
            SwapBuffers();
        }

        public bool Get_cameraSelector()
        {
            return cameraSelector;
        }

        public GameObjectFactory Get_gameObjectFactory()
        {
            return _gameObjectFactory;
        }

        public void Set_cameraSelector(bool value)
        {
            cameraSelector = value;
        }
    }
}
