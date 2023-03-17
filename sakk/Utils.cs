using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sakk
{
    public static class Utils
    {
        public static void DisplayPointList(List<Point> points)
        {
            MessageBox.Show('[' + string.Join(", ", points.Select(p => $"({p.x}, {p.y})")) + ']');
        }
        public static List<Point> PointsOr(List<Point> points1, List<Point> points2) {
            
            List<Point> result = new List<Point>();
            foreach (Point p in points1)
            {
                result.Add(p);
            }
            foreach (Point  p in points2)
            {
                if (!result.Contains(p)) {
                    result.Add(p);
                }
            }
            return result;
        }
        public static List<Point> PointsAnd(List<Point> points1, List<Point> points2)
        {

            List<Point> result = new List<Point>();
           
            foreach (Point p in points1)
            {
                if (points2.Contains(p))
                {
                    result.Add(p);
                }
            }
            return result;
        }

        public static List<Point> FilterPoints(List<Point> pointsList)
        {
            List<Point> result = new();
            foreach (Point p in pointsList.Where(x => IsPointLegal(x)))
            {
                if (!result.Contains(p))
                {
                    result.Add(p);
                }
            };
            return result;
        }
        public static bool IsPointLegal(Point p)
        {
            return p.x > -1 && p.x < 8 && p.y > -1 && p.y < 8;
        }

        public static bool IsPointSetsEqual(List<Point> points1, List<Point> points2)
        {
            return points1.OrderBy(p => p.y * 8 + p.x).SequenceEqual(points2.OrderBy(p => p.y * 8 + p.x));
        }
    }
}
