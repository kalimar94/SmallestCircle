using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input.File.Tests
{
    [TestClass]
    public class FilePointsInteratorTests
    {
        const string TestFile = @"Resources\PointsSample.txt";

        FilePointsInterator iterator;

        [TestInitialize]
        public void Init()
        {
            iterator = new FilePointsInterator(TestFile);
        }

        [TestCleanup]
        public void CleanUp()
        {
            iterator.Dispose();
        }

        [TestMethod]
        public void CanReadAllPoints()
        {
            var allPoints = iterator.GetAll();
            Assert.AreEqual(10, allPoints.Count());
        }

        [TestMethod]
        public async Task CanReadAllPointsAsync()
        {
            var allPoints = await iterator.GetAllAsync();
            Assert.AreEqual(10, allPoints.Count());
        }

        [TestMethod]
        public void CanReadPoint()
        {
            var point = iterator.GetNext();
            Assert.IsNotNull(point);
        }

        [TestMethod]
        public async Task CanReadPointAync()
        {
            var point = await iterator.GetNextAsync();
            Assert.IsNotNull(point);
        }

        [TestMethod]
        public void ReturnsNullWhenFileEnds()
        {
            var points = new List<Point>();
            var current = iterator.GetNext();

            while (current != null)
            {
                points.Add(current);
                current = iterator.GetNext();
            } 

            Assert.AreEqual(10, points.Count);
        }

        [TestMethod]
        public async Task ReturnsNullWhenFileEndsAsync()
        {
            var points = new List<Point>();
            var current = iterator.GetNext();

            while (current != null)
            {
                points.Add(current);
                current = await iterator.GetNextAsync();
            }

            Assert.AreEqual(10, points.Count);
        }
    }
}