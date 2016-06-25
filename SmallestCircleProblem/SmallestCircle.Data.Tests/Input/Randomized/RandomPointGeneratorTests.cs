using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Data.Input.Randomized;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.Randomized.Tests
{
    [TestClass()]
    public class RandomPointGeneratorTests
    {
        private RandomPointGenerator generator;

        const int Min = 0;
        const int Max = 1000;
        const int Count = 20;

        [TestInitialize]
        public void Init()
        {
            generator = new RandomPointGenerator(Count, Min, Max);
        }


        [TestMethod]
        public void RandomThreadedPointsGeneratorTest()
        {
            var randomPoint =  generator.GetNext().Value;

            Assert.IsTrue(Min <= randomPoint.X && randomPoint.X <= Max);
            Assert.IsTrue(Min <= randomPoint.Y && randomPoint.Y <= Max);
        }

        [TestMethod]
        public void GetAllAsyncTest()
        {
            var randomPoints = generator.GetAll().Select(x=> x.Value).ToList();

            Assert.AreEqual(Count, randomPoints.Count);
            Assert.IsTrue(randomPoints.All(p => Min <= p.X && p.X <= Max));
            Assert.IsTrue(randomPoints.All(p => Min <= p.Y && p.Y <= Max));
        }

    }
}