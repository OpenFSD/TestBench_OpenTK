using System.Diagnostics;
using System.Drawing;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL4;
using OpenTK.Input;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;
using Florence.ServerAssembly.Graphics;

namespace Florence.ServerAssembly
{
    public sealed class Game_InstanceForPlane : GameWindow
    {
        private bool done_once;

        private readonly string _title;
        private double _time;
        private readonly Color4 _backColor = new Color4(0.1f, 0.1f, 0.3f, 1.0f);
        private Matrix4 _projectionMatrix;
        private float _fov = 45f;

        private KeyboardState _lastKeyboardState;
        private MouseState _lastMouseState;

        private GameObjectFactory _gameObjectFactory;
        private readonly List<AGameObject> _gameObjects = new List<AGameObject>();
        
        
        private ShaderProgram _texturedProgram;
        private ShaderProgram _solidProgram;
        
        private bool cameraSelector = false;
        
        public Game_InstanceForPlane()
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

        private static void Initialise()
        {
            //_cameraFP.Initialise();
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
            models.Add("Floor", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\dotted.png", 8));

            models.Add("Player", new MipMapGeneratedRenderObject(RenderObjectFactory.CreateTexturedCube6(1, 1, 1), _texturedProgram.Id, "..\\..\\..\\..\\graphics\\Textures\\spacecraft.png", 8));

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

            _gameObjectFactory.Create_MapPlaneFloor();
            //_gameObjects.Add();

           _gameObjectFactory.Create_PlayerOnMapPlane();
            //_gameObjects.Add(_gameObjectFactory.Get_player());

//for (int index_X = 0; index_X < 10; index_X++) 
           // { 
            //    for(int index_Z = 0; index_Z < 10; index_Z++)
              //  {
                    _gameObjects.Add(_gameObjectFactory.CreateCube("Gameover", new Vector3(0f, 0f, 0f), new Vector3(1f)));
             //   }
           // }

            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Wooden", new Vector3(-10f, 0f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, 10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Golden", new Vector3(0f, -10f, 0f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, 10f), new Vector3(1f)));
            _gameObjects.Add(_gameObjectFactory.CreateSphericalAsteroid("Asteroid", new Vector3(0f, 0f, -10f), new Vector3(1f)));

            //_camera = new StaticCamera();

            _gameObjectFactory.Get_player().Create_Cameras_OnPlane();
            //_gameObjectFactory.Get_player().Get_CameraFPOP().Initialise();
            //_gameObjectFactory.Get_player().Get_CameraTPOP().Initialise();

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
                Get_gameObjectFactory().Get_player().Get_CameraFPOP().Update(_time, e.Time);
                break;

            case true:
                Get_gameObjectFactory().Get_player().Get_CameraTPOP().Update(_time, e.Time);
                break;
            }
        }

        private void HandleMouse()
        {
            Console.WriteLine("TESTBENCH => HandleMouse");

            MouseState mouseState = Mouse.GetCursorState();
            var player = _gameObjectFactory.Get_player();

            if (_gameObjectFactory.Get_player().Get_IsFirstMouseMove() == true)
            {
                OpenTK.Input.Mouse.SetPosition((double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));
                //_gameObjectFactory.Get_player().Set_MousePos(new Vector2((float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2)));
                _gameObjectFactory.Get_player().Set_IsFirstMouseMove(false);
            }
            else
            {
                switch (Get_cameraSelector())
                {
                    case false:
                        if (Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(1) == false)
                        {
                            if ((mouseState.X != (float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2))
                                || (mouseState.Y != (float)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2)))
                            {
                                Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(1, true);

                                System.Console.WriteLine("TESTBENCH => mouse X = " + mouseState.X + "  mouse Y = " + mouseState.Y);

                                //float sensitivity = _gameObjectFactory.Get_player().Get_sensitivity();
                                float anglePerPixle = Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_fov() / Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y();
                                float deltaDegX = anglePerPixle * (mouseState.X - (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2));
                                float deltaDegY = anglePerPixle * (mouseState.Y - (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));
                                System.Console.WriteLine("TESTBENCH => deltaX = " + deltaDegX + "  deltaY = " + deltaDegY);

                                player.Get_CameraFPOP().Update_Yaw(deltaDegX);
                                System.Console.WriteLine("yaw => " + player.Get_CameraFPOP().Get_Yaw());
                                player.Get_CameraFPOP().Update_Pitch(deltaDegY);
                                System.Console.WriteLine("pitch => " + player.Get_CameraFPOP().Get_Pitch());

                                Vector3 fowards = new Vector3(0, 0, 0);
                                fowards.X = MathF.Cos(player.Get_CameraFPOP().Get_Pitch()) * MathF.Cos(player.Get_CameraFPOP().Get_Yaw());
                                fowards.Y = MathF.Sin(player.Get_CameraFPOP().Get_Pitch());
                                fowards.Z = MathF.Cos(player.Get_CameraFPOP().Get_Pitch()) * MathF.Sin(player.Get_CameraFPOP().Get_Yaw());

                                player.Get_CameraFPOP().Update_Fowards_Rotations(fowards);

                                player.Trim_Rotation_To_Fundermental_Octive(player.Get_Rotation());
                                Quaternion quart = Quaternion.FromEulerAngles(player.Get_Rotation().X, player.Get_Rotation().Y, player.Get_Rotation().Z);
                                player.Get_CameraFPOP().Set_fowards(Vector3.Transform(fowards, quart));
                                player.Get_CameraFPOP().Set_up(Vector3.Transform(player.Get_CameraFPOP().Get_up(), quart));
                                player.Get_CameraFPOP().Set_right(Vector3.Cross(player.Get_CameraFPOP().Get_fowards(), player.Get_CameraFPOP().Get_up()));

                                OpenTK.Input.Mouse.SetPosition((double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));

                                Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(1, false);
                                //Florence.ServerAssembly.Framework.GetGameServer().GetData().GetInput_Instnace().GetBuffer_Front_InputDouble().SetPraiseEventId(1);
                                //Florence.ServerAssembly.Praise_Files.Praise1_Input input_subset_Praise1 = (Florence.ServerAssembly.Praise_Files.Praise1_Input)Florence.ServerAssembly.Framework.GetGameServer().GetData().GetInput_Instnace().GetBuffer_Front_InputDouble().Get_InputBufferSubset();
                                //input_subset_Praise1.Set_Mouse_X(new_mouseState.X);
                                //input_subset_Praise1.Set_Mouse_Y(new_mouseState.Y);
                                //Florence.ServerAssembly.Framework.GetGameServer().GetData().Flip_InBufferToWrite();
                                //Florence.ServerAssembly.Networking.CreateAndSendNewMessage(1);//todo
                            }
                        }
                        break;

                    case true:

                        break;

                }
                _lastMouseState = mouseState;
                Console.WriteLine("TESTBENCH => HandleMouse .. Done");
            }
        }
        private void HandleKeyboard(double dt)
        {
            var keyState = Keyboard.GetState();
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
/*
                    Vector3 fowards = new Vector3(0);
                    Vector3 backwards = new Vector3(0);
                    Vector3 left = new Vector3(0);
                    Vector3 right = new Vector3(0);
                    if (keyState.IsKeyDown(Key.W))
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(2, true);
                        fowards = _gameObjectFactory.Get_player().Get_CameraFPOP().Get_fowards();
                    }
                    if (keyState.IsKeyDown(Key.S))
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(3, true);
                        backwards = - _gameObjectFactory.Get_player().Get_CameraFPOP().Get_fowards();
                    }
                    if (keyState.IsKeyDown(Key.A))
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(4, true);
                        right = _gameObjectFactory.Get_player().Get_CameraFPOP().Get_right();
                    }
                    if (keyState.IsKeyDown(Key.D))
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(5, true);
                        left = - _gameObjectFactory.Get_player().Get_CameraFPOP().Get_right();
                    }
                    if (Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(2) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(3) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(4) == true
                        || Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().GetFlag_IsPraiseEvent(5) == true
                    )
                    {
                        var player = _gameObjectFactory.Get_player();
                        var camera = _gameObjectFactory.Get_player().Get_CameraFPOP();

                        _gameObjectFactory.Get_player().Set_direction(_gameObjectFactory.Get_player().Get_position() + new Vector3(fowards + backwards + right + left).Normalized());
                        _gameObjectFactory.Get_player().Set_newPosition(_gameObjectFactory.Get_player().Get_position() + (Vector3.Multiply(_gameObjectFactory.Get_player().Get_direction(), (float)(_gameObjectFactory.Get_player().Get_cameraSpeed() * dt))));
                        _gameObjectFactory.Get_player().Set_newPosition(_gameObjectFactory.Get_player().Get_position().Normalized() * 101f);
                        
                        player.Clamp_Rotations(camera.Calculate_Position_Rotations(_gameObjectFactory.Get_player().Get_position()));
                        
                        Quaternion quart = Quaternion.FromEulerAngles(player.Get_Rotation().X, player.Get_Rotation().Y, player.Get_Rotation().Z);
                        camera.Set_fowards(Vector3.Transform(camera.Get_fowards(), quart));
                        camera.Set_up(Vector3.Transform(camera.Get_up(), quart));
                        camera.Set_right(Vector3.Cross(camera.Get_fowards(), camera.Get_up()));

                        OpenTK.Input.Mouse.SetPosition((double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_X() / 2), (double)(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_ScreenSize_Y() / 2));

                    }
                    for (int index = 2; index < 6; index++)
                    {
                        Florence.ServerAssembly.Framework.GetGameServer().GetData().GetData_Control().SetIsPraiseEvent(index, false);
                    }*/
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
            _lastKeyboardState = keyState;
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
                    obj.Render(Get_gameObjectFactory().Get_player().Get_CameraFPOP());
                    break;

                case true:
                    obj.Render(Get_gameObjectFactory().Get_player().Get_CameraTPOP());
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
