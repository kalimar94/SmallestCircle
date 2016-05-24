using System.Collections.Generic;

namespace SmallestCircle.Data.Input
{
    public interface IPointsIterator
    {
        Point GetNext();

        IEnumerable<Point> GetAll();
    }
}
