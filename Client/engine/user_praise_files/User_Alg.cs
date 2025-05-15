﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Florence.ClientAssembly.Praise_Files
{
    public class User_Alg
    {
        static private Florence.ClientAssembly.Praise_Files.Praise1_Algorithm praise0_Algorithm = null;
        static private Florence.ClientAssembly.Praise_Files.Praise2_Algorithm praise1_Algorithm = null;

        public User_Alg() 
        {
            praise0_Algorithm = new Florence.ClientAssembly.Praise_Files.Praise1_Algorithm();
            while (praise0_Algorithm == null) { /* Wait while is created */ }

            praise1_Algorithm = new Florence.ClientAssembly.Praise_Files.Praise2_Algorithm();
            while (praise1_Algorithm == null) { /* Wait while is created */ }
        }

        public Florence.ClientAssembly.Praise_Files.Praise1_Algorithm GetPraise0_Algorithm()
        {
            return praise0_Algorithm;
        }
    }
}
