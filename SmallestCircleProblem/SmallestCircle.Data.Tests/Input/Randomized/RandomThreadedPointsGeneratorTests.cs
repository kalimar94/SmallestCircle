using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.Randomized.Tests
{
    [TestClass]
    public class RandomThreadedPointsGeneratorTests
    {
        private RandomThreadedPointsGenerator generator;

        const int Min = 0;
        const int Max = 1000;
        const int Count = 20;

        [TestInitialize]
        public void Init()
        {
            generator = new RandomThreadedPointsGenerator(Count, Min, Max);
        }

        [TestMethod]
        public async Task RandomThreadedPointsGeneratorTest()
        {
            var randomPoint = await generator.GetNextAsync();

            Assert.IsTrue(Min <= randomPoint.X && randomPoint.X <= Max);
            Assert.IsTrue(Min <= randomPoint.Y && randomPoint.Y <= Max);
        }

        [TestMethod]
        public async Task GetAllAsyncTest()
        {
            var randomPoints = await generator.GetAllAsync();

            Assert.AreEqual(Count, randomPoints.Count());
            Assert.IsTrue(randomPoints.All(p => Min <= p.X && p.X <= Max));
            Assert.IsTrue(randomPoints.All(p => Min <= p.Y && p.Y <= Max));
        }
    }
}