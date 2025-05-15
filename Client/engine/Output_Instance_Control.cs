﻿
namespace Florence.ClientAssembly.Outputs
{
    public class Output_Instance_Control
    {
        static private bool[] isSelected_PraiseEventId = { false, false };

        public Output_Instance_Control()
        {
            isSelected_PraiseEventId = new bool[2] { false, false };
        }

        public bool Get_IsSelected_PraiseEventId(int index)
        {
            return isSelected_PraiseEventId[index];
        }

        public int GetLength_IsSelected_PraiseEventId()
        {
            return isSelected_PraiseEventId.Length;
        }

        public void SetIsSelected_PraiseEventId(int index, bool value)
        {
            isSelected_PraiseEventId[index] = value;
        }
    }
}
