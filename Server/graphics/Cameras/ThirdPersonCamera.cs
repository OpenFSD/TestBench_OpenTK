﻿using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;

namespace Florence.ServerAssembly.Graphics.Cameras
{
    public class ThirdPersonCamera : ICamera
    {
        public Matrix4 LookAtMatrix { get; private set; }
        private readonly AGameObject _target;
        private readonly Vector3 _offset;

        public ThirdPersonCamera(AGameObject target)
            : this(target, Vector3.Zero)
        {}
        public ThirdPersonCamera(AGameObject target, Vector3 offset)
        {
            _target = target;
            _offset = offset;
        }

        public void Update(double time, double delta)
        {
            LookAtMatrix = Matrix4.LookAt(
                new Vector3(_target.Position) + (_offset * new Vector3(_target.Direction)),  
                new Vector3(_target.Position), 
                Vector3.UnitY);
        }
    }
}