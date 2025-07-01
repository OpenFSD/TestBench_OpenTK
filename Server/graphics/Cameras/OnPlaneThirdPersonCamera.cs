using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;

namespace Florence.ServerAssembly.Graphics.Cameras
{
    public class OnPlaneThirdPersonCamera : ICamera
    {
        public Matrix4 LookAtMatrix { get; private set; }
        private readonly APlayerObject _target;
        private readonly Vector3 _offset;
        private float _pitch;
        private float _yaw;
        private Vector3 _fowards;
        private Vector3 _up;
        private Vector3 _right;

        public OnPlaneThirdPersonCamera(APlayerObject target)
            : this(target, Vector3.Zero)
        {}
        public OnPlaneThirdPersonCamera(APlayerObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            _pitch = 0f;
            _yaw = 0f;
            _fowards = -Vector3.UnitY;
            _up = Vector3.UnitZ;
            _right = Vector3.Cross(_fowards, _up);
        }

        public void Update(double time, double delta)
        {
            LookAtMatrix = Matrix4.LookAt(
                new Vector3(_target.Get_Position()) + (_offset * new Vector3(_target.Get_Position())),  
                new Vector3(_target.Get_Position()), 
                Vector3.UnitY);
        }

        public void Update_Pitch(float deltaDegY)
        {
            Set_Pitch(Get_Pitch() + (float)((Math.PI / 180) * deltaDegY));
            if (Get_Pitch() > (float)(Math.PI / 180) * 85)
            {
                Set_Pitch((float)(Math.PI / 180) * 85);
            }
            if (Get_Pitch() <= (Math.PI / 180) * -85)
            {
                Set_Pitch((float)(Math.PI / 180) * -85);
            }
        }
        public void Update_Yaw(float deltaDegX)
        {
            Set_Yaw(Get_Yaw() + (float)((Math.PI / 180) * deltaDegX));
            if (Get_Yaw() > (float)(Math.PI / 180) * 180)
            {
                Set_Yaw(Get_Yaw() - (float)(Math.PI * 2));
            }
            if (Get_Yaw() <= (Math.PI / 180) * -180)
            {
                Set_Yaw(Get_Yaw() + (float)(Math.PI * 2));
            }
        }

//get
        public float Get_Pitch()
        {
            return _pitch;
        }

        public float Get_Yaw()
        {
            return _yaw;
        }
        public Vector3 Get_fowards()
        {
            return _fowards;
        }
        public Vector3 Get_up()
        {
            return _up;
        }
        public Vector3 Get_right()
        {
            return _right;
        }

//set
        public void Set_Pitch(float value)
        {
            _pitch = value;
        }
        public void Set_Yaw(float value)
        {
            _yaw = value;
        }
        public void Set_fowards(Vector3 value)
        {
            _fowards = value;
        }
        public void Set_up(Vector3 value)
        {
            _up = value;
        }
        public void Set_right(Vector3 value)
        {
            _right = value;
        }
    }
}