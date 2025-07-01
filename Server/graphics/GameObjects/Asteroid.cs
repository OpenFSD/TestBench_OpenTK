using System;
using OpenTK;
using Florence.ServerAssembly.Graphics.Renderables;

namespace Florence.ServerAssembly.Graphics.GameObjects
{
    public class Asteroid : AGameObject
    {
        public int Score { get; set; }
        private ARenderable _original;
        public Asteroid(ARenderable model) 
            : base(model)
        {
            _original = model;
        }


        public override void Update(double time, double delta)
        {
            _rotation.X = (float)Math.Sin((time + GameObjectNumber) * 0.3);
            _rotation.Y = (float)Math.Cos((time + GameObjectNumber) * 0.5);
            _rotation.Z = (float)Math.Cos((time + GameObjectNumber) * 0.2);
            base.Update(time, delta);
        }
    }
}