using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmallestCircle.Data.Input
{
    public interface IAsyncPointsIterator
    {
        Task<Point?> GetNextAsync();

        Task<IEnumerable<Point?>> GetAllAsync();


        Task<IEnumerable<Point?>> GetManyAsync(int count);

        int PointsCount { get; }
    }
}
