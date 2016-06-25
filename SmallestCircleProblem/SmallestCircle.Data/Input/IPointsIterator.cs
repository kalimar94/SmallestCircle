using System.Collections.Generic;

namespace SmallestCircle.Data.Input
{
    public interface IPointsIterator
    {
        Point GetNext();

        IEnumerable<Point> GetAll();

        IEnumerable<Point> GetMany(int count);

        int PointsCount { get; }

    }
}
