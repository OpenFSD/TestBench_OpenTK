using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.Renderables;
using Florence.ServerAssembly.GameInstance;

namespace Florence.ServerAssembly.Graphics.GameObjects
{
    public abstract class AGameObject
    {
        public ARenderable Model => _model;

        private static int GameObjectCounter;
        public readonly int GameObjectNumber;
       
        protected ARenderable _model;

        protected Vector3 _position;
        protected Vector3 _direction;
        protected Vector3 _rotation;

        private Vector3 _fowards;
        private Vector3 _up;
        private Vector3 _right;

        private Vector3 _axisX;
        private Vector3 _axisY;
        private Vector3 _axisZ;

        private float _speed;
        private Matrix4 _modelView;
        private Vector3 _scale;

        public AGameObject(ARenderable model)
        {
            _model = model;

            _position = new OpenTK.Vector3(1, 1, 1).Normalized() * 100f;
            _direction = Vector3.Zero;
            _rotation = new Vector3((float)Math.PI / 4, (float)Math.PI / 4, (float)Math.PI * 3 / 4);

            _fowards = Vector3.UnitX;
            _up = Vector3.UnitZ;
            _right = Vector3.UnitY;

            _axisX = Vector3.UnitX;
            _axisZ = Vector3.UnitZ;
            _axisY = Vector3.UnitY;

            _speed = 1.5f;
            _scale = new Vector3(1);
            GameObjectNumber = GameObjectCounter++;
        }

        public virtual void Update(double time, double delta)
        {

            _position += _direction * (_speed * (float) delta);
        }
        public virtual void Render(ICamera camera)
        {
            _model.Bind();
            var t2 = Matrix4.CreateTranslation(_position.X, _position.Y, _position.Z);
            var r1 = Matrix4.CreateRotationX(_rotation.X);
            var r2 = Matrix4.CreateRotationY(_rotation.Y);
            var r3 = Matrix4.CreateRotationZ(_rotation.Z);
            var s = Matrix4.CreateScale(_scale);
            _modelView = r1*r2*r3*s*t2*camera.LookAtMatrix;
            GL.UniformMatrix4(21, false, ref _modelView);
            _model.Render();
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

        //get
        public Vector3 Get_Position()
        {
            return _position;
        }
        public Vector3 Get_Direction()
        {
            return _direction;
        }
        public Vector3 Get_Rotation()
        {
            return _rotation;
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
        public Vector3 Get_Scale()
        {
            return _scale;
        }
        //set
        public void Set_Position(Vector3 position)
        {
            _position = position;
        }
        public void Set_Direction(Vector3 value)
        {
            _direction = value;
        }
        public void Set_Rotation(Vector3 value)
        {
            _rotation = value;
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
        public void Set_Scale(Vector3 scale)
        {
            _scale = scale;
        }
    }
}