using OpenTK;
using OpenTK.Graphics.OpenGL4;
using Florence.ServerAssembly.Graphics.Cameras;
using Florence.ServerAssembly.Graphics.Renderables;

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
        protected float _speed;
        protected Matrix4 _modelView;
        protected Vector3 _scale;

        public AGameObject(ARenderable model, Vector3 position, Vector3 direction, Vector3 rotation, float speed)
        {
            _model = model;
            _position = position;
            _direction = direction;
            _rotation = rotation;
            _fowards = new Vector3(1f, 0f, 0f).Normalized();
            _up = position.Normalized();
            _right = Vector3.Cross(_fowards, _up);
            _speed = speed;
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

        public void Clamp_Rotations(Vector3 value)
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
            _rotation = new Vector3(rot_X, rot_Y, rot_Z);
        }

//get
        public Vector3 Get_Direction()
        {
            return _direction;
        }
        public Vector3 Get_Scale()
        {
            return _scale;
        }
        public Vector3 Get_Position()
        {
            return _position;
        }
        public Vector3 Get_Fowards()
        {
            return _fowards;
        }
        public Vector3 Get_Rotation()
        {
            return _rotation;
        }

//set
        public void Set_Direction(Vector3 value)
        {
            _direction = value;
        }
        public void Set_Scale(Vector3 scale)
        {
            _scale = scale;
        }
        public void Set_Position(Vector3 position)
        {
            _position = position;
        }

        public void Set_Rotation(Vector3 value)
        {
            _rotation = value;
        }
    }
}