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
        private Vector3 _lastPosition;
        private float _pitch;
        private float _yaw;
        private OnPlaneFirstPersonCamera _cameraFPOP;
        private OnPlaneThirdPersonCamera _cameraTPOP;
        private OnSphereFirstPersonCamera _cameraFPOS;
        private OnSphereThirdPersonCamera _cameraTPOS;
        private float cameraSpeed;
        private float sensitivity;

        public Player(ARenderable model, Vector3 position, Vector3 direction, Vector3 rotation, float velocity)
            : base(model, position, direction, rotation, velocity)
        {
            _firstMove = true;
            _firstMouseMove = true;
            //mousePos = new Vector2(0, 0);
            _lastPosition = position;
            _pitch = (float)(Math.PI / 4);
            _yaw = (float)(Math.PI / 4);
            _cameraFPOP = null;
            _cameraTPOP = null;
            _cameraFPOS = null;
            _cameraTPOS = null;
            cameraSpeed = 1.5f;
            sensitivity = 1f;
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
        //public Vector2 Get_MousePos()
        //{
        //    return mousePos;
        //}
        public Vector3 Get_LastPosition()
        {
             return _lastPosition;
        }
        public float Get_Pitch()
        {
            return _pitch;
        }

        public float Get_Yaw()
        {
            return _yaw;
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

        //public void Set_MousePos(Vector2 pos)
        //{
        //    mousePos = pos;
        //}

        public void Set_lastPosition(Vector3 position)
        {
            _lastPosition = position;
        }

        public void Set_Pitch(float value)
        {
            _pitch = value;
        }
        public void Set_Yaw(float value)
        {
            _yaw = value;
        }
    }
}

