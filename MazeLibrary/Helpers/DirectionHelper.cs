using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Helpers
{
    //A class to help the grids usage of the enum class Direction
    public static class DirectionHelper
    {
        private static Random _random = new Random();

        //A point array storing the 4 directions interms of coordinates
        private static Point[] _points = new Point[]
        {
            new Point(0, -1), //N
            new Point(1, 0),  //E
            new Point(0, 1),  //S
            new Point(-1, 0), //W
        };

        //A Direction array for storing the opposite directions to the enum class Direction
        private static Direction[] _opposites = new Direction[]
        {
            Direction.South,
            Direction.West,
            Direction.North,
            Direction.East,
        };

        //Assignment of the number 4 into a constant count. This is for the 4 possible directions
        public const int Count = 4;

        //Function returning a random direction by choosing a random number between 0 and 3 (inclusive) and casting it as a Direction
        public static Direction RandomDirection()
        {
            int randomNumber = _random.Next(0, Count);
            return (Direction)randomNumber;
        }

        //Function for returning the corrasponding _points point based on the passed Direction arguement
        public static Point GetOffset(Direction d)
        {
            return _points[(int)d];
        }

        //Function returning the corrasponding _opposites direction based on the passed Direction arguement
        public static Direction GetOpposite(Direction direction)
        {
            return _opposites[(int)direction];
        }
    }
}
