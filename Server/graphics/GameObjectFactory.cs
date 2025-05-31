using System;
using System.Collections.Generic;
using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;
using ServerAssembly.GameInstance;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection;

namespace Florence.ServerAssembly.Graphics
{
    public class GameObjectFactory : IDisposable
    {
        private readonly Dictionary<string, ARenderable> _models;
        private Florence.ServerAssembly.GameInstance.Player _player;
        public GameObjectFactory(Dictionary<string, ARenderable> models)
        {
            _models = models;
        }
        public Asteroid Create_MapSphereFloor()
        {
            return CreateSphericalAsteroid("Earth", new Vector3(0f, 0f, 0f), new Vector3(100f));
        }
        public void Create_MapPlaneFloor()
        {

        }
        public void Create_PlayerOnMapPlane()
        {
            _player = new Florence.ServerAssembly.GameInstance.Player(
                _models["Player"],
                new OpenTK.Vector3(5, 0, 5),
                new OpenTK.Vector3(0, 0, 0),
                new OpenTK.Vector3(0, 0, 0),
                0
            );
            while (_player == null) { /* Wait while is created */ }
        }
        public void Create_PlayerOnMapSphere()
        {
            _player = new Florence.ServerAssembly.GameInstance.Player(
                _models["Player"],
                new OpenTK.Vector3(1f, 1f, 1f).Normalized() * 101f,
                Vector3.Zero,
                new OpenTK.Vector3((float)Math.PI/4, (float)Math.PI / 4, (float)Math.PI / 4),
                0
            );
            while (_player == null) { /* Wait while is created */ }
        }
        public Asteroid CreateSphericalAsteroid(string model, Vector3 position, Vector3 sclae)
        {
            var obj = new Asteroid(_models[model], position, Vector3.Zero, Vector3.Zero, 0.0f);
            obj.Set_Scale(new Vector3(sclae));
            return obj;
        }
        public AGameObject CreateCube(string model, Vector3 position, Vector3 rotation, Vector3 sclae)
        {
            var obj = new GameOverCube(_models[model], position, rotation, Vector3.Zero, 0.0f);
            obj.Set_Scale(new Vector3(sclae));
            return obj;
        }
        public void Dispose()
        {
            foreach (var obj in _models)
                obj.Value.Dispose();
        }
//Get
        public Dictionary<string, ARenderable> Get_models()
        {
            return _models;
        }
        public Florence.ServerAssembly.GameInstance.Player Get_player()
        {
            return _player;
        }
//Set

    }
}