using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using SmallestCircle.Data.Input.Randomized;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation.Tests
{
    [TestClass()]
    public class MultiCalculatorTests
    {
        private MultiCalculator calculator;
        private IAsyncPointsIterator iterator;

        const int PointsCount = 500;
        const int Min = 0;
        const int Max = 1500;

        const int ThreadsCount = 8;

        [TestInitialize]
        public void Init()
        {
            iterator = new RandomThreadedPointsGenerator(PointsCount, Min, Max);
            calculator = new MultiCalculator(iterator, ThreadsCount);
        }


        [TestMethod]
        public async Task ThreadedCalculateCircleTest()
        {
            var circle = await calculator.CalculateCircleAync();

            var maxRadius = Max * Max;

            Assert.IsTrue(circle.Radius <= maxRadius, $"Radius too big {circle.Radius}");
            Assert.IsTrue(0 <= circle.Center.X && circle.Center.X <= 1500, $"Center X too big {circle.Radius}");
            Assert.IsTrue(0 <= circle.Center.Y && circle.Center.Y <= 1500, $"Center Y too big {circle.Radius}");
        }
    }
}