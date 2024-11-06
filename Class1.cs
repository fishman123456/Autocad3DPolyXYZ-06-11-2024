using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static System.Net.Mime.MediaTypeNames;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.Geometry;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.GraphicsInterface;


public class Class1
{
public void Draw3DPolyline(Point3dCollection points)
{
    Database db = Application.DocumentManager.MdiActiveDocument.Database;
    using (Transaction tr = db.TransactionManager.StartTransaction())
    {
        BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
        BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForWrite) as BlockTableRecord;

        Polyline3d poly = new Polyline3d();

        for (int i = 0; i < points.Count; i++)
        {
            Point3d currentPoint = points[i];
            Point3d nearestPoint = FindNearestPoint(currentPoint, points);

                //poly.AppendVertex(i, currentPoint, 0, 0, 0);
                PolylineVertex3d polylineVertex3D = new PolylineVertex3d();
                poly.AppendVertex(polylineVertex3D);
                //poly.AppendVertex(currentPoint);
            }

        btr.AppendEntity(poly);
        tr.AddNewlyCreatedDBObject(poly, true);
        tr.Commit();
    }
}

public Point3d FindNearestPoint(Point3d currentPoint, Point3dCollection points)
{
    double minDistance = double.MaxValue;
    Point3d nearestPoint = new Point3d();

    foreach (Point3d point in points)
    {
        if (point != currentPoint)
        {
            double distance = currentPoint.DistanceTo(point);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestPoint = point;
            }
        }
    }

    return nearestPoint;
}

}
