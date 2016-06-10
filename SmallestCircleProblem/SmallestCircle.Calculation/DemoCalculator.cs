using SmallestCircle.Calculation.Geometry;
using SmallestCircle.Data;
using SmallestCircle.Data.Input;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SmallestCircle.Calculation
{

    public class DemoCalculator : MultiCalculator
    {
        private Circle circle;

        public DemoCalculator(IAsyncPointsIterator iterator, int threadsCount, bool isQuiet)
         :base(iterator, threadsCount, isQuiet)
        {

        }

        public async Task<Circle> CalculateCircleAsync(CancellationToken token)
        {
            var nextPointTask = iterator.GetNextAsync();
            if (points.Count < 2)
            {
                var firstPoints = (await iterator.GetManyAsync(2)).ToArray();
               
                circle = CreateCircle.FromTwoPoints(firstPoints[0], firstPoints[1]);
                points.AddRange(firstPoints);
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
                    circle = FindCircleCombination(nextPoint, points);
                    RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
                }

                points.Add(nextPoint);
                nextPoint = await nextPointTask;
                await Task.Delay(300);
            }
            RaiseCircleFound(this, new OnCircleDrawEventArgs(circle));
            return circle;
        }

    }
}
