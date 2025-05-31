# How To Guide For Florence - Full stack Development TEMPLATE.

## TestBench: OpenTK

# Server

C:\Users\drago\source\repos\TestBench_OpenTK\Server\engine\Execute.cs

line 39: ````public void Create_And_Run_Graphics()````

 - Planar Map
````
using (Florence.ServerAssembly.Game_InstanceForPlane gameInstance = new Florence.ServerAssembly.Game_InstanceForPlane())
{
    gameInstance.Run(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_refreshRate());
}
````

 - Globular Map
````
using (Florence.ServerAssembly.Game_InstanceForSphere gameInstance = new Florence.ServerAssembly.Game_InstanceForSphere())
{
    gameInstance.Run(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_refreshRate());
}
````

## Game_InstanceForPlane()

line 141: 
````
protected override void OnUpdateFrame(FrameEventArgs e)
{
            _time += e.Time;
            foreach (var item in _gameObjects)
            {
                item.Update(_time, e.Time);
            }
            HandleKeyboard(e.Time);
            HandleMouse();
            switch (cameraSelector)
            {
            case false:
                Get_gameObjectFactory().Get_player().Get_CameraFPOS().Update(_time, e.Time);
                break;

            case true:
                Get_gameObjectFactory().Get_player().Get_CameraTPOS().Update(_time, e.Time);
                break;
            }
        }
````

## Game_InstanceForSphere()


# Client


