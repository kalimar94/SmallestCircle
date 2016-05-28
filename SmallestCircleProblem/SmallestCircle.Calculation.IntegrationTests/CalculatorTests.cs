using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.File;

namespace SmallestCircle.Calculation.Tests
{
    [TestClass]
    public class CalculatorTests
    {
        private Calculator calculator;
        private IPointsIterator iterator;

        const int PointsCount = 1000;
        const int Min = 0;
        const int Max = 1500;

        [TestInitialize]
        public void Init()
        {
            iterator = new FilePointsInterator(@"Resources\Points.txt");
            calculator = new Calculator(iterator);
        }


        [TestMethod]
        public void CalculateCircleTest()
        {
            var circle = calculator.CalculateCircle();
            Assert.IsTrue(circle.Radius <= 1500);
            Assert.IsTrue(0 <= circle.Center.X && circle.Center.X <= 1500);
            Assert.IsTrue(0 <= circle.Center.Y && circle.Center.Y <= 1500);
        }
    }
}