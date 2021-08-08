using PredmetniZadatak3.model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace PredmetniZadatak3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region mapSizeFields
        private double maxX;
        private double minX;
        private double maxY;
        private double minY;

        private double mapSizeX, mapSizeY;
        private double edge;
        #endregion

        #region Collections
        // liste elemenata sistema
        private List<Substation> Subs = new List<Substation>();
        private List<Switch> Switches = new List<Switch>();
        private List<Node> Nodes = new List<Node>();
        private List<Line> Lines = new List<Line>();

        // pomocne liste
        private List<PointEntity> points = new List<PointEntity>();
        private List<GeomMod> allModels = new List<GeomMod>();
        private List<CubeEntity> drawnEntities = new List<CubeEntity>();
        private Dictionary<CubeEntity, int> numOfConnections = new Dictionary<CubeEntity, int>(); // za dodatni zadatak
        #endregion

        #region MouseFields
        private int zoom, max_zoom;

        private Point startPosition;
        private Point startPoint;
        private Point diff;
        private ToolTip tt = new ToolTip();
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            maxX = 19.894459;
            minX = 19.793909;
            maxY = 45.277031;
            minY = 45.2325;
            mapSizeX = maxX - minX;
            mapSizeY = maxY - minY;
            edge = 2;
            zoom = 1;
            max_zoom = 30;
            startPosition = new Point(0, 0);
            diff = new Point(0, 0);
            startPoint = new Point(0, 0);
        }

        private void loadBtn_Click(object sender, RoutedEventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("Geographic.xml");

            loadSubstations(doc);
            loadSwitches(doc);
            loadNodes(doc);
            loadLines(doc);
            drawSubstations();
            drawSwitches();
            drawNodes();
            drawLines();

        }

        #region LoadModel
        private void loadSubstations(XmlDocument doc)
        {

            XmlNodeList nodeList;
            nodeList = doc.DocumentElement.SelectNodes("/NetworkModel/Substations/SubstationEntity");
            foreach (XmlNode node in nodeList)
            {
                
                Substation substation = new Substation();
                substation.id = long.Parse(node.SelectSingleNode("Id").InnerText);
                substation.name = node.SelectSingleNode("Name").InnerText;
                substation.x = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                substation.y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                substation.toolTip = "Substation\nID: " + substation.id + "  Name: " + substation.name;

                ToLatLon(substation.x, substation.y, 34, out double lat, out double lon);
                substation.x = lon;
                substation.y = lat;

                if (substation.y >= minY && substation.y <= maxY)
                {
                    if (substation.x >= minX && substation.x <= maxX)
                    {
                        Subs.Add(substation);
                       
                    }
                }
            }
        }

        private void loadSwitches(XmlDocument doc)
        {
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/NetworkModel/Switches/SwitchEntity");
            foreach (XmlNode node in nodeList)
            {
                Switch s = new Switch();
                s.id = long.Parse(node.SelectSingleNode("Id").InnerText);
                s.name = node.SelectSingleNode("Name").InnerText;
                s.x = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                s.y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                s.Status = node.SelectSingleNode("Status").InnerText;
                s.toolTip = "Switch\nID: " + s.id + "  Name: " + s.name + "  Status: " + s.Status;

                ToLatLon(s.x, s.y, 34, out double lat, out double lon);
                s.x = lon;
                s.y = lat;

                if (s.y >= minY && s.y <= maxY)
                {
                    if (s.x >= minX && s.x <= maxX)
                    {
                        Switches.Add(s);
                    }
                }
            }
        }

        private void loadNodes(XmlDocument doc)
        {
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/NetworkModel/Nodes/NodeEntity");
            foreach (XmlNode node in nodeList)
            {
                Node n = new Node();
                n.id = long.Parse(node.SelectSingleNode("Id").InnerText);
                n.name = node.SelectSingleNode("Name").InnerText;
                n.x = double.Parse(node.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                n.y = double.Parse(node.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);
                n.toolTip = "Node\nID: " + n.id + "  Name: " + n.name;

                ToLatLon(n.x, n.y, 34, out double lat, out double lon);
                n.x = lon;
                n.y = lat;
                if (n.y >= minY && n.y <= maxY)
                {
                    if (n.x >= minX && n.x <= maxX)
                    {
                        Nodes.Add(n);
                    }
                }
            }
        }

        private void loadLines(XmlDocument doc)
        {
            XmlNodeList nodeList = doc.DocumentElement.SelectNodes("/NetworkModel/Lines/LineEntity");
            foreach (XmlNode node in nodeList)
            {
                Line line = new Line();
                line.id = long.Parse(node.SelectSingleNode("Id").InnerText, CultureInfo.InvariantCulture);
                line.name = node.SelectSingleNode("Name").InnerText;
                if (node.SelectSingleNode("IsUnderground").InnerText.Equals("false"))
                {
                    line.und = false;
                }
                else
                {
                    line.und = true;
                }
                line.type = node.SelectSingleNode("LineType").InnerText;
                line.firstEnd = long.Parse(node.SelectSingleNode("FirstEnd").InnerText);
                line.secondEnd = long.Parse(node.SelectSingleNode("SecondEnd").InnerText);
                line.vertices = new List<Point>();
                foreach (XmlNode pointNode in node.ChildNodes[9].ChildNodes) 
                {
                    Point pt = new Point();

                    pt.X = double.Parse(pointNode.SelectSingleNode("X").InnerText, CultureInfo.InvariantCulture);
                    pt.Y = double.Parse(pointNode.SelectSingleNode("Y").InnerText, CultureInfo.InvariantCulture);

                    ToLatLon(pt.X, pt.Y, 34, out double lat, out double lon);
                    pt.X = lon;
                    pt.Y = lat;

                    line.vertices.Add(pt);
                }
                bool outOfMap = false;
                foreach (Point pt in line.vertices)
                {
                    if (!(pt.X >= minX && pt.X <= maxX && pt.Y >= minY && pt.Y <= maxY))
                    {
                        outOfMap = true;
                        break;
                    }
                }
                if (!outOfMap) // ako ne postoji nijedna tacka koja je van mape dodaje se vod u listu
                    Lines.Add(line);
            }
        }
        #endregion

        #region DrawModel
        private void drawSubstations()
        {
            foreach (Substation s in Subs)
            {
                

                double offsetX = ((s.x - minX) / mapSizeX * edge) - 1;
                double offsetY = ((s.y - minY) / mapSizeY * edge) - 1;
                double offsetZ;

                var cube = DrawCube("sub", 0.01); // entiteti na mapi bice oznaceni kockama

                double step = 0.01;

                if (UsedPlace(s.x, s.y, out Point point))
                {
                    offsetZ = step + GetNumOfEntByPoint(point) * step;
                    IncrementNOE(point);
                    
                }
                else
                {
                    offsetZ = step;
                    PointEntity pointEntity = new PointEntity();
                    pointEntity.numOfEntities = 1;
                    pointEntity.position = new Point(s.x, s.y);
                    points.Add(pointEntity);
                    
                }

                cube.Transform = new TranslateTransform3D(offsetX - (0.01 / 2), offsetY - (0.01 / 2), offsetZ);
                GeomMod c = new GeomMod();
                c.cube = cube;
                c.tooltip = s.toolTip;
                allModels.Add(c);

                if (!ContainsId(s.id))
                {
                    CubeEntity ce = new CubeEntity();
                    ce.entityId = s.id;
                    ce.cube = cube;
                    drawnEntities.Add(ce);
                    numOfConnections.Add(ce, 0);
                }

                AllModelsGroup.Children.Add(cube);
            }
        }

        private void drawSwitches()
        {
            foreach (Switch s in Switches)
            {


               

                double offsetX = ((s.x - minX) / mapSizeX * edge) - 1;
                double offsetY = ((s.y - minY) / mapSizeY * edge) - 1;
                double offsetZ;

                GeometryModel3D cube;
                if (s.Status == "Closed")
                {
                    cube = DrawCube("sw1", 0.01); // entiteti na mapi bice oznaceni kockama
                }
                else
                {
                    cube = DrawCube("sw2", 0.01);
                }


                double step = 0.01;

                if (UsedPlace(s.x, s.y, out Point point))
                {
                    offsetZ = step + GetNumOfEntByPoint(point) * step;
                    IncrementNOE(point);
                    
                }
                else
                {
                    offsetZ = step;
                    PointEntity pointEntity = new PointEntity();
                    pointEntity.numOfEntities = 1;
                    pointEntity.position = new Point(s.x, s.y);
                    points.Add(pointEntity);
                   
                }

                cube.Transform = new TranslateTransform3D(offsetX - (0.01 / 2), offsetY - (0.01 / 2), offsetZ);
                GeomMod c = new GeomMod();
                c.cube = cube;
                c.tooltip = s.toolTip;
                allModels.Add(c);

                if (!ContainsId(s.id))
                {
                    CubeEntity ce = new CubeEntity();
                    ce.entityId = s.id;
                    ce.cube = cube;
                    drawnEntities.Add(ce);
                    numOfConnections.Add(ce, 0);
                }

                AllModelsGroup.Children.Add(cube);
            }
        }

        private void drawNodes()
        {
            foreach (Node s in Nodes)
            {
                

                double offsetX = ((s.x - minX) / mapSizeX * edge) - 1;
                double offsetY = ((s.y - minY) / mapSizeY * edge) - 1;
                double offsetZ;

                var cube = DrawCube("n", 0.01); // entiteti na mapi bice oznaceni kockama

                double step = 0.01;

                if (UsedPlace(s.x, s.y, out Point point))
                {
                    offsetZ = step + GetNumOfEntByPoint(point) * step;
                    IncrementNOE(point);
                    
                }
                else
                {
                    offsetZ = step;
                    PointEntity pointEntity = new PointEntity();
                    pointEntity.numOfEntities = 1;
                    pointEntity.position = new Point(s.x, s.y);
                    points.Add(pointEntity);
                   
                }

                cube.Transform = new TranslateTransform3D(offsetX - (0.01 / 2), offsetY - (0.01 / 2), offsetZ);
                GeomMod c = new GeomMod();
                c.cube = cube;
                c.tooltip = s.toolTip;
                allModels.Add(c);

                if (!ContainsId(s.id))
                {
                    CubeEntity ce = new CubeEntity();
                    ce.entityId = s.id;
                    ce.cube = cube;
                    drawnEntities.Add(ce);
                    numOfConnections.Add(ce, 0);
                }

                AllModelsGroup.Children.Add(cube);
            }
        }

        private void drawLines()
        {

            double firstEndX1, firstEndY1, secondEndX2, secondEndY2;
            int[] triangleIndices = new int[] { 0, 1, 2, 0, 2, 3 };
            foreach (var line in Lines)
            {
                long id1 = line.firstEnd;
                long id2 = line.secondEnd;
                WriteConnections(id1, id2);

                for (int i = 0; i < line.vertices.Count - 1; i++)
                {
                    double firstVertexLong = line.vertices[i].X;
                    double firstVertexLat = line.vertices[i].Y;

                    firstEndX1 = -1 + ((firstVertexLong - minX) / mapSizeX * edge);
                    firstEndY1 = -1 + ((firstVertexLat - minY) / mapSizeY * edge);

                    double secondVertexLong = line.vertices[i + 1].X;
                    double secondVertexLat = line.vertices[i + 1].Y;

                    secondEndX2 = -1 + ((secondVertexLong - minX) / mapSizeX * edge);
                    secondEndY2 = -1 + ((secondVertexLat - minY) / mapSizeY * edge);

                    Point3D a1 = new Point3D(firstEndX1, firstEndY1, 0.00005);
                    Point3D b1 = new Point3D(secondEndX2, secondEndY2, 0.00005);

                    Vector3D diffVector = b1 - a1;
                    Vector3D nVector = Vector3D.CrossProduct(diffVector, new Vector3D(0, 0, 1));
                    nVector = Vector3D.Divide(nVector, nVector.Length);
                    nVector = Vector3D.Multiply(nVector, 0.0005);

                    Point3D firstPoint = a1 + nVector;
                    Point3D secondPoint = a1 - nVector;
                    Point3D thirdPoint = b1 + nVector;
                    Point3D fourthPoint = b1 - nVector;

                    Point3D[] positions = new Point3D[] { secondPoint, firstPoint, thirdPoint, fourthPoint };

                    GeometryModel3D lineGeometry = new GeometryModel3D();
                    MeshGeometry3D meshGeometry = new MeshGeometry3D();
                    DiffuseMaterial material = new DiffuseMaterial();
                    material.Brush = Brushes.Orange;
                    lineGeometry.Material = material;
                    meshGeometry.Positions = new Point3DCollection(positions);
                    meshGeometry.TriangleIndices = new Int32Collection(triangleIndices);
                    lineGeometry.Geometry = meshGeometry;
                    lineGeometry.Transform = new TranslateTransform3D(0, 0, 0.01);
                    allModels.Add(new GeomMod(lineGeometry, $"Line\nID: {line.id}\nName: {line.name}\nFirst end: {line.firstEnd}\nSecond end: {line.secondEnd}"));
                    AllModelsGroup.Children.Add(lineGeometry);
                }
            }
        }
        #endregion


        #region Helpers
        // u zavisnosti od tipa entiteta odlucuje kakva kocka ce biti iscrtana
        private GeometryModel3D DrawCube(string entityType, double size)
        {
            Brush b;
            switch (entityType)
            {
                case "sub":
                    b = Brushes.Blue;
                    break;
                case "sw2":
                    b = Brushes.Green;
                    break;
                case "sw1":
                    b = Brushes.Red;
                    break;
                case "n":
                    b = Brushes.Black;
                    break;
                default:
                    b = Brushes.White;
                    break;
            }

            GeometryModel3D cube = new GeometryModel3D();
            MeshGeometry3D mg = new MeshGeometry3D();

            mg.Positions.Add(new Point3D(0, 0, 0));
            mg.Positions.Add(new Point3D(size, 0, 0));
            mg.Positions.Add(new Point3D(0, size, 0));
            mg.Positions.Add(new Point3D(size, size, 0));
            mg.Positions.Add(new Point3D(0, 0, size));
            mg.Positions.Add(new Point3D(size, 0, size));
            mg.Positions.Add(new Point3D(0, size, size));
            mg.Positions.Add(new Point3D(size, size, size));

            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(3);
            mg.TriangleIndices.Add(1);

            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(0);

            mg.TriangleIndices.Add(7);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(3);

            mg.TriangleIndices.Add(7);
            mg.TriangleIndices.Add(5);
            mg.TriangleIndices.Add(1);

            mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(5);
            mg.TriangleIndices.Add(7);

            mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(4);
            mg.TriangleIndices.Add(5);

            mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(4);

            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(4);

            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(7);
            mg.TriangleIndices.Add(3);

            mg.TriangleIndices.Add(2);
            mg.TriangleIndices.Add(6);
            mg.TriangleIndices.Add(7);

            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(1);
            mg.TriangleIndices.Add(5);

            mg.TriangleIndices.Add(0);
            mg.TriangleIndices.Add(5);
            mg.TriangleIndices.Add(4);
            cube.Geometry = mg;

            cube.Material = new DiffuseMaterial(b);
            return cube;
        }

        // proverava da li je mesto na kom treba da se iscrta objekat iskorisceno
        private bool UsedPlace(double x, double y, out Point p)
        {
            p = new Point();
            double min_distance = 0.02;
            foreach (PointEntity pe in points)
            {
                if (Distance(y, x, pe.position.Y, pe.position.X) <= min_distance)
                {
                    p = pe.position;
                    return true;
                }
            }
            return false;
        }

        // racuna udaljenost izmedju dve tacke na mapi
        private double Distance(double x1, double y1, double x2, double y2)
        {
            double distance = 0;
            double dx = x1 - x2;

            double rad_dx = dx * Math.PI / 180.0;
            double rad1 = y1 * Math.PI / 180.0;
            double rad2 = y2 * Math.PI / 180.0;

            distance = Math.Sin(rad1) * Math.Sin(rad2) + Math.Cos(rad1) * Math.Cos(rad2) * Math.Cos(rad_dx);
            distance = Math.Acos(distance);
            distance = (distance / Math.PI * 180.0) * 60 * 1.1515 * 1.609344;

            return distance;
        }

        // vraca broj konekcija za zadatu tacku
        private int GetNumOfEntByPoint(Point p)
        {
            foreach (var ep in points)
            {
                if (p == ep.position)
                {
                    return ep.numOfEntities;
                }
            }
            return -1; // ako je pretraga neuspesna
        }

        // uvecava broj entiteta u nekoj tacki kada se na njoj nesto novo iscrta
        private void IncrementNOE(Point p)
        {
            foreach (var ep in points)
            {
                if (p == ep.position)
                {
                    ep.UsedAgain();
                }
            }
        }

        // proverava da li je neki entitet vec iscrtan
        private bool ContainsId(long id)
        {
            foreach (var el in drawnEntities)
            {
                if (el.entityId == id)
                {
                    return true;
                }
            }
            return false;
        }

        // povecava broj konekcija entiteta koji su na krajevima voda
        private void WriteConnections(long id1, long id2)
        {
            foreach (CubeEntity e in numOfConnections.Keys)
            {
                if (e.entityId == id1)
                {
                    numOfConnections[e]++;
                    
                    break;
                }
            }

            foreach (CubeEntity e in numOfConnections.Keys)
            {
                if (e.entityId == id2)
                {
                    numOfConnections[e]++;
                    
                    break;
                }
            }
        }
        #endregion

        #region ToLatLon
        public void ToLatLon(double utmX, double utmY, int zoneUTM, out double latitude, out double longitude)
        {
            bool isNorthHemisphere = true;

            var diflat = -0.00066286966871111111111111111111111111;
            var diflon = -0.0003868060578;

            var zone = zoneUTM;
            var c_sa = 6378137.000000;
            var c_sb = 6356752.314245;
            var e2 = Math.Pow((Math.Pow(c_sa, 2) - Math.Pow(c_sb, 2)), 0.5) / c_sb;
            var e2cuadrada = Math.Pow(e2, 2);
            var c = Math.Pow(c_sa, 2) / c_sb;
            var x = utmX - 500000;
            var y = isNorthHemisphere ? utmY : utmY - 10000000;

            var s = ((zone * 6.0) - 183.0);
            var lat = y / (c_sa * 0.9996);
            var v = (c / Math.Pow(1 + (e2cuadrada * Math.Pow(Math.Cos(lat), 2)), 0.5)) * 0.9996;
            var a = x / v;
            var a1 = Math.Sin(2 * lat);
            var a2 = a1 * Math.Pow((Math.Cos(lat)), 2);
            var j2 = lat + (a1 / 2.0);
            var j4 = ((3 * j2) + a2) / 4.0;
            var j6 = ((5 * j4) + Math.Pow(a2 * (Math.Cos(lat)), 2)) / 3.0;
            var alfa = (3.0 / 4.0) * e2cuadrada;
            var beta = (5.0 / 3.0) * Math.Pow(alfa, 2);
            var gama = (35.0 / 27.0) * Math.Pow(alfa, 3);
            var bm = 0.9996 * c * (lat - alfa * j2 + beta * j4 - gama * j6);
            var b = (y - bm) / v;
            var epsi = ((e2cuadrada * Math.Pow(a, 2)) / 2.0) * Math.Pow((Math.Cos(lat)), 2);
            var eps = a * (1 - (epsi / 3.0));
            var nab = (b * (1 - epsi)) + lat;
            var senoheps = (Math.Exp(eps) - Math.Exp(-eps)) / 2.0;
            var delt = Math.Atan(senoheps / (Math.Cos(nab)));
            var tao = Math.Atan(Math.Cos(delt) * Math.Tan(nab));

            longitude = ((delt * (180.0 / Math.PI)) + s) + diflon;
            latitude = ((lat + (1 + e2cuadrada * Math.Pow(Math.Cos(lat), 2) - (3.0 / 2.0) * e2cuadrada * Math.Sin(lat) * Math.Cos(lat) * (tao - lat)) * (tao - lat)) * (180.0 / Math.PI)) + diflat;
        }
        #endregion

        #region MouseFunctions
        private void vp_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            vp.ReleaseMouseCapture();
        }

        private void vp_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            tt.IsOpen = false;
            vp.CaptureMouse();
            startPoint = e.GetPosition(this);
            diff.X = translacija.OffsetX;
            diff.Y = translacija.OffsetY;

            Point mousePosition = e.GetPosition(vp);
            Point3D testpoint3D = new Point3D(mousePosition.X, mousePosition.Y, 0);
            Vector3D testdirection = new Vector3D(mousePosition.X, mousePosition.Y, 10);

            PointHitTestParameters pointparams = new PointHitTestParameters(mousePosition);
            RayHitTestParameters rayparams = new RayHitTestParameters(testpoint3D, testdirection);
            VisualTreeHelper.HitTest(vp, null, HTResult, pointparams);
        }

        private HitTestResultBehavior HTResult(HitTestResult result)
        {
            RayHitTestResult rayResult = (RayHitTestResult)result;

            if (rayResult != null)
            {
                foreach (GeomMod gm in allModels) //prolazim kroz sve kocke da proverim koja je pritisnuta
                {
                    if (gm.cube == rayResult.ModelHit)
                    {
                        tt.Content = gm.tooltip;
                        tt.Dispatcher.Invoke(new Action(() => { tt.IsOpen = true; }));
                    }
                }
            }
            return HitTestResultBehavior.Stop;
        }

        private void vp_MouseMove(object sender, MouseEventArgs e)
        {
            Point currentPosition = e.GetPosition(this);

            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                tt.IsOpen = false;
                double offsetX = currentPosition.X - startPosition.X;
                double offsetY = currentPosition.Y - startPosition.Y;
                if ((xAxisAngleRotation.Angle + (0.2 * offsetY)) < 70 && (xAxisAngleRotation.Angle + (0.2 * offsetY)) > -70)
                    xAxisAngleRotation.Angle += 0.2 * offsetY;
                if ((yAxisAngleRotation.Angle + (0.2 * offsetX)) < 70 && (yAxisAngleRotation.Angle + (0.2 * offsetX)) > -70)
                    yAxisAngleRotation.Angle += 0.2 * offsetX;
            }

            startPosition = currentPosition;

            if (vp.IsMouseCaptured)
            {
                Point end = e.GetPosition(this);
                double offsetX = end.X - startPoint.X;
                double offsetY = end.Y - startPoint.Y;
                double w = Width;
                double h = Height;
                double translateX = (offsetX * 100) / w;
                double translateY = -(offsetY * 100) / h;
                translacija.OffsetX = diff.X + (translateX / (100 * skaliranje.ScaleX));
                translacija.OffsetY = diff.Y + (translateY / (100 * skaliranje.ScaleX));
            }
        }

        private void vp_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            double scale_step = 0.1;
            if (e.Delta > 0 && zoom < max_zoom)
            {
                skaliranje.ScaleX += scale_step;
                skaliranje.ScaleY += scale_step;
                skaliranje.ScaleZ += scale_step;

                zoom++;
                Camera.FieldOfView--;
            }
            else if (e.Delta <= 0 && zoom > -max_zoom)
            {
                skaliranje.ScaleX -= scale_step;
                skaliranje.ScaleY -= scale_step;
                skaliranje.ScaleZ -= scale_step;

                zoom--;
                Camera.FieldOfView++;
            }
        }

        private void vp_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.MiddleButton == MouseButtonState.Pressed)
            {
                startPosition = e.GetPosition(this);
            }
        }
        #endregion
    }
}
