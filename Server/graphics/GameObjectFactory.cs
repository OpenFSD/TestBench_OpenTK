using OpenTK;
using Florence.ServerAssembly.Graphics.GameObjects;
using Florence.ServerAssembly.Graphics.Renderables;

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
        public Asteroid CreateLocalWorld()
        {
            return CreateSphericalAsteroid("Earth", new Vector3(0f, 0f, 0f), new Vector3(100f, 100f, 100f));
        }
        public void Create_MapPlaneFloor()
        {

        }
        public void Create_PlayerOnMapPlane()
        {
            _player = new Florence.ServerAssembly.GameInstance.Player(_models["Player"]);
            while (_player == null) { /* Wait while is created */ }
        }
        public void Create_PlayerOnMapSphere()
        {
            _player = new Florence.ServerAssembly.GameInstance.Player(_models["Player"]);
            while (_player == null) { /* Wait while is created */ }
        }
        public Asteroid CreateSphericalAsteroid(string model, Vector3 position, Vector3 sclae)
        {
            var obj = new Asteroid(_models[model]);
            obj.Set_Scale(new Vector3(sclae));
            return obj;
        }
        public AGameObject CreateCube(string model, Vector3 position,Vector3 sclae)
        {
            var obj = new GameOverCube(_models[model]);
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