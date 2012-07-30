using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using endureSDF;

namespace linqtsql
{
    public static class Model
    {
        private static Endurance context = null;
      

        public static Endurance Context
        {
            get
            {
                if (context == null)
                    context = new Endurance(@"Data Source=|DataDirectory|\Endurance.sdf");
                return context;
            }
        }       

    }
}
