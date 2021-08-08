using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Media3D;

namespace PredmetniZadatak3.model
{
    public class GeomMod
    {
        public GeometryModel3D cube; // klasa koja ce povezivati svaku kocku na mapi sa tooltipom
        public string tooltip;

        public GeomMod() { }

        public GeomMod(GeometryModel3D cube, string tooltip)
        {
            this.cube = cube;
            this.tooltip = tooltip;
        }
    }
}
