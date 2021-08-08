using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PredmetniZadatak3
{
    public class CubeEntity
    {
        public long entityId; //klasa koja povezuje konkretni entitet sa kockom koja ga predstavlja
        public GeometryModel3D cube;

        public CubeEntity() { }
    }
}
