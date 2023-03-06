﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace sakk
{
    public static class Utils
    {
        static List<Point> PointsOr(List<Point> points1, List<Point> points2) {
            
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
        static List<Point> PointsAnd(List<Point> points1, List<Point> points2)
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
    }
}
