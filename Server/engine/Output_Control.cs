﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Florence.ServerAssembly.Outputs
{
    public class Output_Control
    {
        static private bool[] isSelected_PraiseEventId = new bool[0];
        static int numberOfPraises;

        public Output_Control() 
        {
            numberOfPraises = 2;
            isSelected_PraiseEventId = new bool[numberOfPraises];
        }

        void SelectSetOutputSubset(
            int praiseEventId
        )
        {
            switch (praiseEventId)
            {
                case 0:
                    Florence.ServerAssembly.Framework.GetGameServer().GetData().GetOutput_Instnace().GetBuffer_BackOutputDouble().SetInputBufferSubSet(
                        Framework.GetGameServer().GetData().GetUserI().GetPraise0_Input()
                    );
                    break;

                case 1:
                    Florence.ServerAssembly.Framework.GetGameServer().GetData().GetOutput_Instnace().GetBuffer_BackOutputDouble().SetInputBufferSubSet(
                        Framework.GetGameServer().GetData().GetUserI().GetPraise1_Input()
                    );
                    break;
            }
        }
    }
}
