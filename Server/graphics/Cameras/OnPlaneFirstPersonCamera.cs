using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;

namespace Florence.ServerAssembly.Graphics.Cameras
{
    public class OnPlaneFirstPersonCamera : ICamera
    {
        public Matrix4 LookAtMatrix { get; private set; }
        private readonly APlayerObject _target;
        private readonly Vector3 _offset;
        private float _pitch;
        private float _yaw;
        private Vector3 _fowards;
        private Vector3 _up;
        private Vector3 _right;

        public OnPlaneFirstPersonCamera(APlayerObject target)
            : this(target, Vector3.Zero)
        {}
        public OnPlaneFirstPersonCamera(APlayerObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
            _pitch = (float)(Math.PI / 4);
            _yaw = (float)(Math.PI / 4);
            _up = Vector3.UnitY;
            _fowards = new Vector3(1f, 1f, 1f).Normalized();
            _right = Vector3.Cross(_fowards, _up);
        }
       
        public void Update(double time, double delta)
        {
            LookAtMatrix = Matrix4.LookAt(
                new Vector3(_target.Get_Position()) + _offset,  
                new Vector3(_target.Get_Position() + _fowards) + _offset, 
                _up);
        }
        public void Update_Pitch(float deltaDegY)
        {
            Set_Pitch(Get_Pitch() + (float)((Math.PI / 180) * deltaDegY));
            if(Get_Pitch() > (float)(Math.PI / 180) * 85)
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
            if (Get_Yaw() > (float)(Math.PI / 180) * 360)
            {
                Set_Yaw(Get_Yaw() - (float)(Math.PI * 2));
            }
            if (Get_Yaw() <= (Math.PI / 180) * 0)
            {
                Set_Yaw(Get_Yaw() + (float)(Math.PI * 2));
            }
        }
        public void Update_Fowards_Rotations(Vector3 newFowards)
        {
            float temp = (Vector3.Cross(new Vector3(newFowards.X, 0, 0), new Vector3(Get_fowards().X, 0, 0))).Length / (new Vector3(Get_fowards().X, 0, 0).Length * new Vector3(newFowards.X, 0, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundX = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, newFowards.Y, 0), new Vector3(0, Get_fowards().Y, 0))).Length / (new Vector3(0, Get_fowards().Y, 0).Length * new Vector3(0, newFowards.Y, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundY = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, 0, newFowards.Z), new Vector3(0, 0, Get_fowards().Z))).Length / (new Vector3(0, 0, Get_fowards().Z).Length * new Vector3(0, 0, newFowards.Z).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundZ = (float)Math.Asin(temp);

            _target.Set_Rotation(new Vector3(angleAroundX, angleAroundY, angleAroundZ));
            System.Console.WriteLine("TESTBENCH => delta_angleAroundX = " + angleAroundX + "  delta_angleAroundY = " + angleAroundY + "  delta_angleAroundZ = " + angleAroundZ);
        }
        /*
        public void Update_Fowards_Rotations(Vector3 newFowards)
        {
            float temp = (Vector3.Cross(new Vector3(newFowards.X, 0, 0), new Vector3(Get_fowards().X, 0, 0))).Length / (new Vector3(Get_fowards().X, 0, 0).Length * new Vector3(newFowards.X, 0, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundX = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, newFowards.Y, 0), new Vector3(0, Get_fowards().Y, 0))).Length / (new Vector3(0, Get_fowards().Y, 0).Length * new Vector3(0, newFowards.Y, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundY = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, 0, newFowards.Z), new Vector3(0, 0, Get_fowards().Z))).Length / (new Vector3(0, 0, Get_fowards().Z).Length * new Vector3(0, 0, newFowards.Z).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundZ = (float)Math.Asin(temp);

            _target.Set_Rotation(new Vector3(angleAroundX, angleAroundY, angleAroundZ));
            System.Console.WriteLine("TESTBENCH => delta_angleAroundX = " + angleAroundX + "  delta_angleAroundY = " + angleAroundY + "  delta_angleAroundZ = " + angleAroundZ);
        }*/
        /*
        public Vector3 Update_Position_Rotations(Vector3 newPosition)
        {
            var player = Florence.ServerAssembly.Framework.GetGameServer().GetData().GetGame_InstanceOP().Get_gameObjectFactory().Get_player();

            float temp = (Vector3.Cross(new Vector3(newPosition.X, 0, 0), new Vector3(player.Get_LastPosition().X, 0, 0))).Length / (new Vector3(player.Get_LastPosition().X, 0, 0).Length * new Vector3(newPosition.X, 0, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundX = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, newPosition.Y, 0), new Vector3(0, player.Get_LastPosition().Y, 0))).Length / (new Vector3(0, player.Get_LastPosition().Y, 0).Length * new Vector3(0, newPosition.Y, 0).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundY = (float)Math.Asin(temp);

            temp = (Vector3.Cross(new Vector3(0, 0, newPosition.Z), new Vector3(0, 0, player.Get_LastPosition().Z))).Length / (new Vector3(0, 0, player.Get_LastPosition().Z).Length * new Vector3(0, 0, newPosition.Z).Length);
            temp = Math.Clamp(temp, -1f, 1f);
            float angleAroundZ = (float)Math.Asin(temp);

            _target.Set_Rotation(new Vector3(angleAroundX, angleAroundY, angleAroundZ));
            System.Console.WriteLine("TESTBENCH => delta_angleAroundX = " + angleAroundX + "  delta_angleAroundY = " + angleAroundY + "  delta_angleAroundZ = " + angleAroundZ);
            return new Vector3(angleAroundX, angleAroundY, angleAroundZ);
        }*/

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