using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data;

namespace SmallestCircle.Calculation.Geometry.Tests
{
    [TestClass]
    public class CreateCircleTests
    {
        [TestMethod]
        public void CanCreateTwoPointCircle()
        {
            var firstPoint = new Point(2, 10);
            var secondPoint = new Point(4, 5);

            var circle = CreateCircle.FromTwoPoints(firstPoint, secondPoint);

            Assert.AreEqual(circle.Center.DistanceTo(firstPoint), circle.Center.DistanceTo(secondPoint));
            Assert.AreEqual(circle.Radius, circle.Center.DistanceTo(firstPoint));
        }
    }
}