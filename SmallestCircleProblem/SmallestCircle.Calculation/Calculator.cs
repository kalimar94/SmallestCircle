using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System.Collections.Generic;
using System.Linq;
using System;
using SmallestCircle.Calculation.Geometry;

namespace SmallestCircle.Calculation
{
    public class Calculator
    {
        private IPointsIterator iterator;
        private List<Point> points;

       
        public Calculator(IPointsIterator iterator)
        {
            this.iterator = iterator;
            this.points = new List<Point>(iterator.PointsCount);
        }       

        public Circle CalculateCircle()
        {
            var firstPoints = iterator.GetMany(2).ToArray();
            var circle = CreateCircle.FromTwoPoints(firstPoints[0].Value, firstPoints[1].Value);

            points.AddRange(firstPoints.Select(x=> x.Value));
            var nextPoint = iterator.GetNext();

            while (nextPoint != null)
            {
                if (!circle.ContainsPoint(nextPoint.Value))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint.Value, points);
                }

                points.Add(nextPoint.Value);
                nextPoint = iterator.GetNext();
            }
            return circle;
        }

        private Circle FindCircleCombination(Point newPoint, List<Point> existingPoints)
        {
            Circle? minCircle = null;

            // Try all circles that are formed as a combination of the new point and one of the existing ones
            foreach (var otherPoint in existingPoints)
            {
                var circle = CreateCircle.FromTwoPoints(newPoint, otherPoint);

                if (circle.ContainsAllPoints(existingPoints))
                {
                    if (minCircle == null || circle < minCircle)
                        return circle;
                }
            }

            // Try all circles that are formed as a combination of the new point and two of the existing ones

            for (int i = 0; i < existingPoints.Count; i++)
            {
                for (int j = i + 1; j < existingPoints.Count; j++)
                {
                    var circle = CreateCircle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                    if (circle.ContainsAllPoints(existingPoints))
                    {
                        if (minCircle == null || circle < minCircle)
                            minCircle = circle;
                    }
                }
            }
           
            return minCircle.Value;
        }
    }
}
