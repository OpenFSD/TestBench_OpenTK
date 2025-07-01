using OpenTK;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;

namespace Florence.ServerAssembly.GameInstance
{
    public class Player : APlayerObject
    {
        private bool _firstMove;
        private bool _firstMouseMove;
        private OnPlaneFirstPersonCamera _cameraFPOP;
        private OnPlaneThirdPersonCamera _cameraTPOP;
        private OnSphereFirstPersonCamera _cameraFPOS;
        private OnSphereThirdPersonCamera _cameraTPOS;
        private float cameraSpeed;
        private float sensitivity;

        private Vector3 _lastPosition;
        private Vector3 _lastRotation;

        public Player(ARenderable model)
            : base(model)
        {
            _firstMove = true;
            _firstMouseMove = true;
            _cameraFPOP = null;
            _cameraTPOP = null;
            _cameraFPOS = null;
            _cameraTPOS = null;
            cameraSpeed = 1.5f;
            sensitivity = 1f;

            _lastPosition = new Vector3(0);
            _lastRotation = new Vector3(0);
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

        public void CenterPlayerFreeLook()
        {
            Vector3 fowards = new Vector3(0, 0, 0);
            fowards.X = MathF.Cos(Get_CameraFPOS().Get_Pitch()) * MathF.Cos(Get_CameraFPOS().Get_Yaw());
            fowards.Y = MathF.Sin(Get_CameraFPOS().Get_Pitch());
            fowards.Z = MathF.Cos(Get_CameraFPOS().Get_Pitch()) * MathF.Sin(Get_CameraFPOS().Get_Yaw());

            float pitch = Vector3.CalculateAngle(fowards, Get_axisX());
            float yaw = Vector3.CalculateAngle(fowards, Get_axisY());
            float roll = Vector3.CalculateAngle(fowards, Get_axisZ());
            Set_Rotation(new Vector3(pitch, yaw, roll));
            Set_Rotation(Trim_Rotation_To_Fundermental_Octive(Get_Rotation()));

            Vector3 deltaRotations = new Vector3(
                Get_Rotation().X - Get_lastRotation().X,
                Get_Rotation().Y - Get_lastRotation().Y,
                Get_Rotation().Z - Get_lastRotation().Z
            );

            Quaternion deltaRotationQuat = Quaternion.FromEulerAngles(deltaRotations.X, deltaRotations.Y, deltaRotations.Z);
            Set_fowards(Vector3.Transform(Get_fowards(), deltaRotationQuat));
            Set_up(Vector3.Transform(Get_up(), deltaRotationQuat));
            Set_right(Vector3.Cross(Get_fowards(), Get_up()));

            Set_axisX(Vector3.Transform(Get_axisX(), deltaRotationQuat).Normalized());
            Set_axisY(Vector3.Transform(Get_axisY(), deltaRotationQuat).Normalized());
            Set_axisZ(Vector3.Cross(Get_axisX(), Get_axisY()));
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
        public Vector3 Get_lastPosition()
        {
            return _lastPosition;
        }
        public Vector3 Get_lastRotation()
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

        public void Set_lastRotation(Vector3 fowards)
        {
            _lastRotation = fowards;
        }
    }
}

