using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System.Collections.Generic;
using System.Linq;
using System;

namespace SmallestCircle.Calculation
{
    public class Calculator
    {
        private IPointsIterator iterator;
        private List<Point> points;

        public Calculator(IPointsIterator iterator)
        {
            this.iterator = iterator;
        }

        public Circle CalculateCircle()
        {
            var firstPoints = iterator.GetMany(2).ToArray();
            var circle = Circle.FromTwoPoints(firstPoints[0], firstPoints[1]);

            points.AddRange(firstPoints);
            var nextPoint = iterator.GetNext();

            while (nextPoint != null)
            {
                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint, points);
                }

                points.Add(nextPoint);
                nextPoint = iterator.GetNext();
            }

            return circle;
        }

        private Circle FindCircleCombination(Point newPoint, List<Point> existingPoints)
        {
            // Try all circles that are formed as a combination of the new point and one of the existing ones
            foreach (var otherPoint in existingPoints)
            {
                var circle = Circle.FromTwoPoints(newPoint, otherPoint);

                if (existingPoints.All(circle.ContainsPoint))
                {
                    return circle;
                }
            }

            // Try all circles that are formed as a combination of the new point and two of the existing ones

            for (int i = 0; i < existingPoints.Count; i++)
            {
                for (int j = i + 1; j < existingPoints.Count; j++)
                {
                    var circle = Circle.FromThreePoints(newPoint, existingPoints[i], existingPoints[j]);

                    if (existingPoints.All(circle.ContainsPoint))
                    {
                        return circle;
                    }
                }
            }

            throw new ArithmeticException("Unable to find a circle that contains all the points!");
        }
    }
}
