﻿
namespace Florence.ClientAssembly.Praise_Files
{
    public class Praise0_Input
    {
        static private bool ping_Active;

        public Praise0_Input()
        {
            ping_Active = false;
        }

        public bool GetFlag_IsPingActive() 
        {   
            return ping_Active; 
        }

        public void SetFlag_IsPingActive(bool value)
        {
            ping_Active = value;
        }
    }
}
