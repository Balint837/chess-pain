using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace sakk
{
    public static class Utils
    {
        public static Dictionary<string, object> ParseFEN(string fenString)
        {
            Dictionary<string, object> result = new();
            var data = fenString.Split(" ");

            Dictionary<char, Type> pieceDict = new()
            {
            };
            var x = (x => x.GetType().GetConstructor(new Type[] { typeof(Point), typeof(bool) }).Invoke(null, new object[] { }));


            result["pieces"] = new List<ChessPiece>();

            foreach (var piece in data[0])
            {

            }

            result["turn"] = data[1] == "w";

            var castlingData = data[2];

        }
        public static T? Cast<T>(T? obj)
        {
            return obj;
        }
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

            var t1 = points1.Select(p=>p.y*8+p.x);
            var t2 = points2.Select(p=>p.y*8+p.x);
            return !t1.Except(t2).Any() && !t2.Except(t1).Any();
        }
    }
}
