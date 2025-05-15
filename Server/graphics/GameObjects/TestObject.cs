﻿using System;
using OpenTK;
using Florence.ServerAssembly.Graphics.Renderables;

namespace Florence.ServerAssembly.Graphics.GameObjects
{
    public class TestObject : AGameObject
    {
        public TestObject(ARenderable model, Vector4 position, Vector4 direction, Vector4 rotation, float velocity) 
            : base(model, position, direction, rotation, velocity)
        {
        }

        public override void Update(double time, double delta)
        {
            var k = (float)time * 0.4f;
            _rotation.X = k * 0.7f;
            _rotation.Y = k * 0.5f;
            _rotation.Z = -(float)Math.Sin(k * 0.4f);
            if (_position.Length > 30 || _position.Length < 0)
                _direction = -_direction;
            base.Update(time, delta);
        }
        
    }
}