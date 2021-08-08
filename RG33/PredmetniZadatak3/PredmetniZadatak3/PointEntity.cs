using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PredmetniZadatak3
{
    public class PointEntity
    {
        public Point position;
        public int numOfEntities;

        public void UsedAgain()
        {
            numOfEntities++;
        }

        public PointEntity() { }
    }
}
