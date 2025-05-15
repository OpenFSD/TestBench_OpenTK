﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Florence.ClientAssembly
{
    public class Data_Control
    {
        static private bool flag_IsLoaded_Stack_InputAction;
        static private bool flag_IsLoaded_Stack_OutputAction;
        static private bool[] isPraiseActive;

        public Data_Control()
        {
            flag_IsLoaded_Stack_InputAction = false;
            flag_IsLoaded_Stack_OutputAction = false;
            isPraiseActive = new bool[6];//Number Of Praises
            for (int index = 0; index < 6; index++)//Number Of Praises
            {
                isPraiseActive[index] = false;
            }
        }

        public void Push_Stack_InputActions(
            List<Florence.ClientAssembly.Inputs.Input> stack_InputAction,
            Florence.ClientAssembly.Inputs.Input buffer_Back_Input
        )
        {
            stack_InputAction.Add(buffer_Back_Input);
        }

        public void Push_Stack_OutputRecieve(
            List<Florence.ClientAssembly.Outputs.Output> stack_OutputRecieve,
            Florence.ClientAssembly.Outputs.Output buffer_TransmitOutput
        )
        {
            stack_OutputRecieve.Add(buffer_TransmitOutput);
        }

        public void Pop_Stack_InputActions(
            Florence.ClientAssembly.Inputs.Input buffer_Transmit_Input,
            List<Florence.ClientAssembly.Inputs.Input> stack_InputAction
        )
        {
            buffer_Transmit_Input = stack_InputAction.ElementAt(0);
            stack_InputAction.RemoveAt(0);
        }

        public void Pop_Stack_OutputRecieve(
            Florence.ClientAssembly.Outputs.Output buffer_Back_Output,
            List<Florence.ClientAssembly.Outputs.Output> stack_OutputRecieve
        )
        {
            buffer_Back_Output = stack_OutputRecieve.ElementAt(0);
            stack_OutputRecieve.RemoveAt(0);
        }

        public bool GetFlag_IsLoaded_Stack_InputAction()
        {
            return flag_IsLoaded_Stack_InputAction;
        }

        public bool GetFlag_IsLoaded_Stack_OutputAction()
        {
            return flag_IsLoaded_Stack_OutputAction;
        }

        public bool GetFlag_IsPraiseEvent(int praiseEventId)
        {
            return isPraiseActive[praiseEventId];
        }
        
        public void SetFlag_InputStackLoaded(bool value)
        {
            flag_IsLoaded_Stack_InputAction = value;
        }

        public void SetFlag_OutputStackLoaded(bool value)
        {
            flag_IsLoaded_Stack_OutputAction = value;
        }

        public void SetIsPraiseEvent(int praiseEventId, bool value)
        {
            isPraiseActive[praiseEventId] = value; 
        }

    }
}
