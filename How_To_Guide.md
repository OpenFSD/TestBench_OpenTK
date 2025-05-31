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

# Client


