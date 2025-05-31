# How To Guide For Florence - Full stack Development TEMPLATE.

## Server

### Planar Map
````
using (Florence.ServerAssembly.Game_InstanceForPlane gameInstance = new Florence.ServerAssembly.Game_InstanceForPlane())
{
    gameInstance.Run(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_refreshRate());
}
````

### Globular Map
````
using (Florence.ServerAssembly.Game_InstanceForSphere gameInstance = new Florence.ServerAssembly.Game_InstanceForSphere())
{
    gameInstance.Run(Florence.ServerAssembly.Framework.GetGameServer().GetData().GetSettings().Get_refreshRate());
}
````

## Client


