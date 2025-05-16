﻿using System.Runtime.InteropServices;

namespace Florence.ServerAssembly
{
    public class Framework
    {
        static private Florence.ServerAssembly.Server game_server = null;
        //static private Valve.Networking networkingServer = null;

        public Framework()
        {
            System.Console.WriteLine("entered => Florence.ServerAssembly.Framework()");//TestBench

            game_server = new Florence.ServerAssembly.Server();
            while (game_server == null) { /* Wait whileis created */ }
            game_server.GetExecute().Initialise();
            System.Console.WriteLine("created => Florence.ServerAssembly.Server()");//TestBench

            Florence.ServerAssembly.Framework.GetGameServer().GetExecute().Create_And_Run_Graphics();

            System.Console.WriteLine("Framework . Done");//TestBench
        }

        static public Florence.ServerAssembly.Server GetGameServer()
        {
            return game_server;
        }

        /*
        static public Valve.Networking GetNetworkingServer()
        {
             return networkingServer;
        }
        */
    }
}
