using OpenTK;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;

namespace Florence.ServerAssembly.GameInstance
{
    public class Player : AGameObject
    {
        private bool _firstMove;
        private bool _firstMouseMove;
        //private Vector2 _lastMousePos;
        private Vector3 _lastFowards;
        private Vector3 _lastPosition;
        private Vector3 _lastRotation;
        private OnPlaneFirstPersonCamera _cameraFPOP;
        private OnPlaneThirdPersonCamera _cameraTPOP;
        private OnSphereFirstPersonCamera _cameraFPOS;
        private OnSphereThirdPersonCamera _cameraTPOS;
        private float cameraSpeed;
        private float sensitivity;

        [Obsolete]
        public Player(ARenderable model, Vector3 position, Vector3 direction, Vector3 rotation, float speed)
            : base(model, position, direction, rotation, speed)
        {
            _firstMove = true;
            _firstMouseMove = true;
            //mousePos = new Vector2(0, 0);
            _lastFowards = new Vector3(1f, 0f, 0f);
            _lastPosition = new Vector3(1f, 1f, 1f);
            //_lastRotation = new Vector3((float)(Math.PI / 4), (float)(Math.PI / 4), (float)(Math.PI / 4));
            _cameraFPOP = null;
            _cameraTPOP = null;
            _cameraFPOS = null;
            _cameraTPOS = null;
            cameraSpeed = 1.5f;
            sensitivity = 1f;
        }
        public void Initialise()
        {
            Get_CameraFPOS().Set_Yaw(0);
            System.Console.WriteLine("yaw => " + Get_CameraFPOS().Get_Yaw());
            Get_CameraFPOS().Set_Pitch(0);
            System.Console.WriteLine("pitch => " + Get_CameraFPOS().Get_Pitch());

            float temp = (Vector3.Cross(new Vector3(Get_Position().X, 0, 0), new Vector3(Get_LastPosition().X, 0, 0))).Length / ((new Vector3(Get_LastPosition().X, 0, 0)).Length * (new Vector3(Get_Position().X, 0, 0)).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundX = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, Get_Position().Y, 0), new Vector3(0, Get_LastPosition().Y, 0))).Length / ((new Vector3(0, Get_LastPosition().Y, 0)).Length * (new Vector3(0, Get_Position().Y, 0)).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundY = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, 0, Get_Position().Z), new Vector3(0, 0, Get_LastPosition().Z))).Length / ((new Vector3(0, 0, Get_LastPosition().Z)).Length * (new Vector3(0, 0, Get_Position().Z)).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundZ = (float)Math.Asin(temp);

            Set_Rotation(new Vector3(angleAroundX, angleAroundY, angleAroundZ));
            System.Console.WriteLine("TESTBENCH => delta_angleAroundX = " + angleAroundX + "  delta_angleAroundY = " + angleAroundY + "  delta_angleAroundZ = " + angleAroundZ);
            Clamp_Rotations(Get_Rotation());
            
            Quaternion quart = Quaternion.FromEulerAngles(Get_Rotation().X, Get_Rotation().Y, Get_Rotation().Z);
            Get_CameraFPOS().Set_fowards(Vector3.Transform(Get_CameraFPOS().Get_fowards(), quart));
            Get_CameraFPOS().Set_up(Vector3.Transform(Get_CameraFPOS().Get_up(), quart));
            Get_CameraFPOS().Set_right(Vector3.Cross(Get_CameraFPOS().Get_fowards(), Get_CameraFPOS().Get_up()));

            _lastFowards = Get_Fowards();
        }
        public void Create_Cameras_OnPlane()
        {
            _cameraFPOP = new Florence.ServerAssembly.Graphics.Cameras.OnPlaneFirstPersonCamera(this);
            while (_cameraFPOP == null) { }

            _cameraTPOP = new Florence.ServerAssembly.Graphics.Cameras.OnPlaneThirdPersonCamera(this);
            while (_cameraTPOP == null) { }
        }
        public void Create_Cameras_OnSphere()
        {
            _cameraFPOS = new Florence.ServerAssembly.Graphics.Cameras.OnSphereFirstPersonCamera(this);
            while (_cameraFPOS == null) { }

            _cameraTPOS = new Florence.ServerAssembly.Graphics.Cameras.OnSphereThirdPersonCamera(this);
            while (_cameraTPOS == null) { }
        }
//Get
        public OnPlaneFirstPersonCamera Get_CameraFPOP()
        {
            return _cameraFPOP;
        }
        public OnPlaneThirdPersonCamera Get_CameraTPOP()
        {
            return _cameraTPOP;
        }
        public OnSphereFirstPersonCamera Get_CameraFPOS()
        {
            return _cameraFPOS;
        }
        public OnSphereThirdPersonCamera Get_CameraTPOS()
        {
            return _cameraTPOS;
        }
        public bool Get_IsFirstMove()
        {
            return _firstMove;
        }
        public bool Get_IsFirstMouseMove()
        {
            return _firstMouseMove;
        }

        public Vector3 Get_LastPosition()
        {
             return _lastPosition;
        }
        public Vector3 Get_LastFowards()
        {
            return _lastFowards;
        }
        public Vector3 Get_LastRotation()
        {
            return _lastRotation;
        }
        public float Get_cameraSpeed()
        {
            return cameraSpeed;
        }

        public float Get_sensitivity()
        {
            return sensitivity;
        }


//Set
        public void Set_IsFirstMove(bool value)
        {
            _firstMove = value;
        }
        public void Set_IsFirstMouseMove(bool value)
        {
            _firstMouseMove = value;
        }

        public void Set_lastPosition(Vector3 position)
        {
            _lastPosition = position;
        }
        public void Set_lastFowards(Vector3 vector)
        {
            _lastFowards = vector;
        }
    }
}

