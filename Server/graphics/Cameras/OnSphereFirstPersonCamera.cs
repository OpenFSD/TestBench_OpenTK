using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.GameInstance;

namespace Florence.ServerAssembly.Graphics.Cameras
{
    public class OnSphereFirstPersonCamera : ICamera
    {
        public Matrix4 LookAtMatrix { get; private set; }
        private Player _player;
        private Vector3 _offset;
        private Vector3 _position;
        private Vector3 _rotation;
        private float _pitch;
        private float _yaw;
        private Vector3 _fowards;
        private Vector3 _up;
        private Vector3 _right;
        private Vector3 _axisX;
        private Vector3 _axisY;
        private Vector3 _axisZ;

        public OnSphereFirstPersonCamera(Player player)
        {
            _player = player;
            _position = _player.Get_Position();
            _rotation = _player.Get_Rotation();

            _pitch = 0;
            _yaw = 0;

            _fowards = _player.Get_fowards();
            _up = _player.Get_up();
            _offset = _player.Get_up().Normalized() * 1.8f;
            _right = _player.Get_right();
        }

        public void ApplyMouseAtPlayerPosition(Player player, float deltaRadAroundUp, float deltaRadAroundRight)
        {
            Set_Pitch(Get_Pitch() + deltaRadAroundRight);
            Set_Yaw(Get_Yaw() + deltaRadAroundUp);

            Vector3 fowards = new Vector3(0, 0, 0);
            fowards.X = MathF.Cos(Get_Pitch()) * MathF.Cos(Get_Yaw());
            fowards.Y = MathF.Sin(Get_Pitch());
            fowards.Z = MathF.Cos(Get_Pitch()) * MathF.Sin(Get_Yaw());

            float pitch = Vector3.CalculateAngle(fowards, _player.Get_axisX());
            float yaw = Vector3.CalculateAngle(fowards, _player.Get_axisY());
            float roll = Vector3.CalculateAngle(fowards, _player.Get_axisZ());
            Set_Rotation(new Vector3(player.Get_Rotation().X + pitch, player.Get_Rotation().Y + yaw, player.Get_Rotation().Z + roll));
            Set_Rotation(Trim_Rotation_To_Fundermental_Octive(Get_Rotation()));

            Quaternion deltaRotationQuart = Quaternion.FromEulerAngles(Get_Rotation().X - player.Get_lastRotation().X, Get_Rotation().Y - player.Get_lastRotation().Y, Get_Rotation().Z - player.Get_lastRotation().Z);
            Set_fowards(Vector3.Transform(Get_fowards(), deltaRotationQuart));
            
            Set_up(Vector3.Transform(Get_up(), deltaRotationQuart));
            Set_offset(Get_up().Normalized() * 1.8f);
            
            Set_right(Vector3.Cross(Get_fowards(), Get_up()));
            
            Set_axisX(Vector3.Transform(Get_axisX(), deltaRotationQuart));
            Set_axisY(Vector3.Transform(Get_axisY(), deltaRotationQuart));
            Set_axisZ(Vector3.Cross(Get_axisX(), Get_axisY()));
        }

        public void AlignePlayerAtGlobalSurfacePosition(Player player, double dt)
        {
            System.Console.WriteLine("TESTBENCH => entered AlignePlayerAtGlobalSurfacePosition");

            _player.Set_Position(_player.Get_Position() + (Vector3.Multiply(_player.Get_Direction(), (float)(player.Get_cameraSpeed() * dt))).Normalized() * 100f);
            _player.Get_CameraFPOS().Set_offset(_player.Get_Position().Normalized() * 1.8f);

            float angleWithX = Vector3.CalculateAngle(_player.Get_Position(), _player.Get_axisX());
            float angleWithY = Vector3.CalculateAngle(_player.Get_Position(), _player.Get_axisY());
            float angleWithZ = Vector3.CalculateAngle(_player.Get_Position(), _player.Get_axisZ());

            _player.Set_Rotation(new Vector3(angleWithX, angleWithY, angleWithZ));
            _player.Set_Rotation(player.Trim_Rotation_To_Fundermental_Octive(_player.Get_Rotation()));

            Vector3 deltaRotations = new Vector3(
                _player.Get_Rotation().X - _player.Get_lastRotation().X,
                _player.Get_Rotation().Y - _player.Get_lastRotation().Y,
                _player.Get_Rotation().Z - _player.Get_lastRotation().Z
            );

            Quaternion deltaRotationQuat = Quaternion.FromEulerAngles(deltaRotations.X, deltaRotations.Y, deltaRotations.Z);
            _player.Set_fowards(Vector3.Transform(_player.Get_fowards(), deltaRotationQuat));
            _player.Set_up(Vector3.Transform(_player.Get_up(), deltaRotationQuat));
            _player.Set_right(Vector3.Cross(_player.Get_fowards(), _player.Get_up()));

            _player.Set_axisX(Vector3.Transform(_player.Get_axisX(), deltaRotationQuat).Normalized());
            _player.Set_axisY(Vector3.Transform(_player.Get_axisY(), deltaRotationQuat).Normalized());
            _player.Set_axisZ(Vector3.Cross(_player.Get_axisX(), _player.Get_axisY()));
        }
        
        public void Update(double time, double delta)
        {
            LookAtMatrix = Matrix4.LookAt(
                new Vector3(_player.Get_Position()) + _offset,
                new Vector3(_player.Get_Position() + _fowards) + _offset,
                new Vector3(_player.Get_Position() + _up) + _offset
            );
        }

        public Vector3 Trim_Rotation_To_Fundermental_Octive(Vector3 value)
        {
            float rot_X = value.X;
            if (rot_X > (float)(Math.PI / 180) * 360)
            {
                rot_X = (rot_X - (float)(Math.PI * 2));
            }
            if (rot_X <= (Math.PI / 180) * 0)
            {
                rot_X = (rot_X + (float)(Math.PI * 2));
            }
            float rot_Y = value.Y;
            if (rot_Y > (float)(Math.PI / 180) * 360)
            {
                rot_Y = (rot_Y - (float)(Math.PI * 2));
            }
            if (rot_Y <= (Math.PI / 180) * 0)
            {
                rot_Y = (rot_Y + (float)(Math.PI * 2));
            }
            float rot_Z = value.Z;
            if (rot_Z > (float)(Math.PI / 180) * 360)
            {
                rot_Z = (rot_Z - (float)(Math.PI * 2));
            }
            if (rot_Z <= (Math.PI / 180) * 0)
            {
                rot_Z = (rot_Z + (float)(Math.PI * 2));
            }
            return new Vector3(rot_X, rot_Y, rot_Z);
        }
        public void Update_Pitch(float deltaRadY)
        {
            Set_Pitch(Get_Pitch() + deltaRadY);
            if (Get_Pitch() > (float)(Math.PI / 180) * 85)
            {
                Set_Pitch((float)(Math.PI / 180) * 85);
            }
            if (Get_Pitch() <= (Math.PI / 180) * -85)
            {
                Set_Pitch((float)(Math.PI / 180) * -85);
            }
        }
        public void Update_Yaw(float deltaRadX)
        {
            Set_Yaw(Get_Yaw() + deltaRadX);
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
        public Vector3 Get_offset()
        {
            return _offset;
        }
        public Vector3 Get_Position()
        {
            return _position;
        }
        public Vector3 Get_Rotation()
        {
            return _rotation;
        }
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
        public Vector3 Get_axisX()
        {
            return _axisX;
        }
        public Vector3 Get_axisY()
        {
            return _axisY;
        }
        public Vector3 Get_axisZ()
        {
            return _axisZ;
        }

        //set
        public void Set_offset(Vector3 value)
        {
            _offset = value;
        }
        public void Set_Position(Vector3 value)
        {
            _position = value;
        }
        public void Set_Rotation(Vector3 value)
        {
            _rotation = value;
        }
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
        public void Set_axisX(Vector3 value)
        {
            _axisX = value;
        }
        public void Set_axisY(Vector3 value)
        {
            _axisY = value;
        }
        public void Set_axisZ(Vector3 value)
        {
            _axisZ = value;
        }
    }
}