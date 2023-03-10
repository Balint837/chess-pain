using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sakk
{
    public class Point {
        public int x { get; set; }
        public int y { get; set; }

        public Point(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public static Point operator+ (Point a, Point b)
        {
            return new Point(a.x + b.x, a.y + b.y);
        }
        public static Point operator- (Point a, Point b) {
            return new Point(a.x - b.x, a.y - b.y);

        }
        public static bool operator ==(Point a, Point b)
        {
            return a.Equals(b);
        }
        public static bool operator !=(Point a, Point b)
        {
            return !a.Equals(b);
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != typeof(Point)) return false;
            Point p = (Point)obj;
            return this.x == p.x && this.y == p.y;
        }

    }
}
