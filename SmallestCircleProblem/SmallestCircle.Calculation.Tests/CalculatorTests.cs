using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.Randomized;
using System;

namespace SmallestCircle.Calculation.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator calculator;
        private IPointsIterator iterator;

        const int PointsCount = 500;
        const int Min = 0;
        const int Max = 1500;


        [TestInitialize]
        public void Init()
        {
            iterator = new RandomPointGenerator(PointsCount, Min, Max);
            calculator = new Calculator(iterator);
        }


        [TestMethod]
        public void CalculateCircleTest()
        {
            var circle = calculator.CalculateCircle();

            var maxRadius = Max * Max;

            Assert.IsTrue(circle.Radius <= maxRadius, $"Radius too big {circle.Radius}");
            Assert.IsTrue(0 <= circle.Center.X && circle.Center.X <= 1500, $"Center X too big {circle.Radius}");
            Assert.IsTrue(0 <= circle.Center.Y && circle.Center.Y <= 1500, $"Center Y too big {circle.Radius}");

            Console.WriteLine("Found circle: " + circle);
        }
    }
}