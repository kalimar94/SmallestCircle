using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data;
using System;

namespace SmallestCircle.Calculation.Geometry.Tests
{
    [TestClass]
    public class PointExtensionsTests
    {
        private void TestDistance(Point firstPoint, Point secondPoint, double expectedDistance)
        {
            Assert.AreEqual(expectedDistance, firstPoint.DistanceTo(secondPoint));
            Assert.AreEqual(expectedDistance, secondPoint.DistanceTo(firstPoint));
        }


        [TestMethod]
        public void CanCalculateDistanceFromZeroPoint()
        {
            var firstPoint = new Point(0, 0);
            var secondPoint = new Point(4, 4);

            TestDistance(firstPoint, secondPoint, Math.Sqrt(32));
        }

        [TestMethod]
        public void CanCalculateDistanceInHorizontalLine()
        {
            var firstPoint = new Point(1, 10);
            var secondPoint = new Point(1, 15);

            TestDistance(firstPoint, secondPoint, 5);
        }

        [TestMethod]
        public void CanCalculateDistanceInVerticalLine()
        {
            var firstPoint = new Point(10, 5);
            var secondPoint = new Point(15, 5);

            TestDistance(firstPoint, secondPoint, 5);
        }

        [TestMethod]
        public void CanCalculatePythagoreanDistance()
        {
            var firstPoint = new Point(1, 1);
            var secondPoint = new Point(4, 5);

            TestDistance(firstPoint, secondPoint, 5);
        }
    }
}