using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation.Geometry.Tests
{
    [TestClass()]
    public class CircleExtensionsTests
    {
        [TestMethod()]
        public void ContainsPointTest()
        {
            Assert.Fail();
        }

        [TestMethod()]
        public void ContainsAllPointsTest()
        {
            CudafyModule km = CudafyTranslator.Cudafy();

            GPGPU gpu = CudafyHost.GetDevice(CudafyModes.Target);
            gpu.LoadModule(km);

            var points = gpu.Allocate(new[]
            {
                new Point(3,5),
                new Point(2,8),
                new Point(1,6),
                new Point(4,9)
            });

            var circle = new Circle(new Point(3, 10), 15);
     

            gpu.Launch(128, 1, "CircleExtensions.ContainsAllPoints", circle, points);


            Assert.Fail();
        }
    }
}