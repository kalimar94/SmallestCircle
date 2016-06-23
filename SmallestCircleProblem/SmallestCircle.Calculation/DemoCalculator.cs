using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    public class DemoCalculator : MultiCalculator
    {
        private Circle circle;

        public LinkedList<Point> Points => this.points;

        public DemoCalculator(IAsyncPointsIterator iterator, int threadsCount, bool isQuiet)
         :base(iterator, threadsCount)
        {

        }

        public async Task<Circle> CalculateCircleAsync(CancellationToken token)
        {
            var nextPointTask = iterator.GetNextAsync();
            if (points.Count < 2)
            {
                // Read the first two points:
                var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
               
                circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);

                RaisePointProcessed(this, new OnPointDrawEventArgs(firstPoints[0]));
                RaisePointProcessed(this, new OnPointDrawEventArgs(firstPoints[1]));
                RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
                points.AddLast(firstPoints[0]);
                points.AddLast(firstPoints[1]);
            }

            var count = iterator.PointsCount;
            var nextPoint = await nextPointTask;

            while (nextPoint != null && !token.IsCancellationRequested)
            {
                RaisePointProcessed(this, new OnPointDrawEventArgs(nextPoint));
                await Task.Delay(300);

                nextPointTask = iterator.GetNextAsync();

                if (!circle.ContainsPoint(nextPoint))
                {
                    // Update the circle to contain the new point as well:
                    circle = FindCircleCombination(nextPoint);
                    RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
                }

                points.AddFirst(nextPoint);
                nextPoint = await nextPointTask;
                await Task.Delay(300);
            }
            RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
            return circle;
        }

    }
}
