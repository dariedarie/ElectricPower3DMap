using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PredmetniZadatak3
{
    public class Switch
    {
        public long id;
        public string name;
        public double x;
        public double y;
        public string toolTip;
        private string status;



        public Switch()
        {

        }

        public string Status
        {
            get
            {
                return status;
            }

            set
            {
                status = value;
            }
        }
    }
}
