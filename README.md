# SmallestCircle
Multi-threaded algorithm that calculates the smallest enclosing circle for a given set of points in a 2d plane

# Problem description
The smallest-circle problem or minimum covering circle problem is a mathematical problem of computing the smallest circle that contains all of a given set of points in the Euclidean plane.

#Properties and observations
- The minimum covering circle is unique.
- The minimal circle must pass through some of the points
- The circle can be determined by two points
   - In that case the distance between the points will be the diameter of the circle
- The circle can be determined by three points 
   - In this scenario the circle is the circumscribed circle of the triangle formed by the 3 points
- If a circle through two points contains all given points in the plain - it is the minimal one
   - There is no smaller circle through two points that would contain all
   - There is no circle through 3 points that can be smaller

# Incremental solution
- The algorithm maintains a list of all points processed until the current iteration
- Each iteration produces a circle that contains all points processed so far
- When the first two points are processed, the initial circle is created
- When a new point p is processed
   - Either the point is in the current circle and the solution from the previous iteration is preserved
   - If the point is outside the current circle a new circle is generated that encloses all points
        - The newly found circle will have the point on its boundary

# Naive approach - used in Multi-Threading scenarios
- Assume a circle C contains all points except p
- The new circle will be formed by p and one or two of the other points processed so far
- Generate all combinations of two points, where p is one of the points
  - If a circle enclosing all the points is found - it is the minimum enclosing circle for the set
- Generelate all combinations of three points, where p is one of them
  - Produce a circle based on each combination of points and select the minimal one

# Resources, references, similar solutions
- http://www.cs.uu.nl/docs/vakken/ga/slides4b.pdf
- https://www.nayuki.io/page/smallest-enclosing-circle
- https://en.wikipedia.org/wiki/Smallest-circle_problem
