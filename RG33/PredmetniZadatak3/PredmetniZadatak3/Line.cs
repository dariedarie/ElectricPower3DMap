using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace PredmetniZadatak3
{
    public class Line
    {
        public long id;
        public string name;
        public bool und;
        public string type;
        public long firstEnd;
        public long secondEnd;
        public List<Point> vertices;

        public Line()
        {

        }
    }
}
