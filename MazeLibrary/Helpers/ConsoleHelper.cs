using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Helpers
{
    public static class ConsoleHelper
    {
        private const string Block = "██";
        private const string Space = "  ";
        private const string Star = "**";

        public static string Draw(Grid grid)
        {
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < grid.Height; i++)
            {
                StringBuilder topLine = new StringBuilder();
                StringBuilder middleLine = new StringBuilder();
                StringBuilder bottomLine = new StringBuilder();
                for (int j = 0; j < grid.Width; j++)
                {
                    Cell cell = grid.GetCell(j, i);

                    if (cell == null)
                    {
                        topLine.AppendFormat("{0}{0}{0}", Star);
                        middleLine.AppendFormat("{0}{0}{0}", Star);
                        bottomLine.AppendFormat("{0}{0}{0}", Star);
                    }
                    else
                    {
                        topLine.Append(GetTopLine(cell));
                        middleLine.Append(GetMiddleLine(cell));
                        bottomLine.Append(GetBottomLine(cell));
                    }
                }
                sb.AppendLine(topLine.ToString());
                sb.AppendLine(middleLine.ToString());
                sb.AppendLine(bottomLine.ToString());
                topLine.Clear();
                middleLine.Clear();
                bottomLine.Clear();
            }
            return sb.ToString();
        }

        private static string GetTopLine(Cell cell)
        {
            CellEdge northCellEdge = cell.GetEdge(Direction.North);
            CellEdge eastCellEdge = cell.GetEdge(Direction.East);
            CellEdge southCellEdge = cell.GetEdge(Direction.South);
            CellEdge westCellEdge = cell.GetEdge(Direction.West);

            StringBuilder sb = new StringBuilder();
            //0,0
            sb.Append(Block);

            //1,0
            if (northCellEdge is CellWall)
            {
                sb.Append(Block);
            }
            else
            {
                sb.Append(Space);
            }

            //2,0
            sb.Append(Block);

            return sb.ToString();
        }

        private static string GetMiddleLine(Cell cell)
        {
            CellEdge northCellEdge = cell.GetEdge(Direction.North);
            CellEdge eastCellEdge = cell.GetEdge(Direction.East);
            CellEdge southCellEdge = cell.GetEdge(Direction.South);
            CellEdge westCellEdge = cell.GetEdge(Direction.West);

            StringBuilder sb = new StringBuilder();

            //0,1
            if (westCellEdge is CellWall)
            {
                sb.Append(Block);
            }
            else
            {
                sb.Append(Space);
            }

            //1,1
            sb.Append(Space);

            //2,1
            if (eastCellEdge is CellWall)
            {
                sb.Append(Block);
            }
            else
            {
                sb.Append(Space);
            }

            return sb.ToString();
        }

        private static string GetBottomLine(Cell cell)
        {
            CellEdge northCellEdge = cell.GetEdge(Direction.North);
            CellEdge eastCellEdge = cell.GetEdge(Direction.East);
            CellEdge southCellEdge = cell.GetEdge(Direction.South);
            CellEdge westCellEdge = cell.GetEdge(Direction.West);

            StringBuilder sb = new StringBuilder();
            //0,2
            sb.Append(Block);

            //1,2
            if (southCellEdge is CellWall)
            {
                sb.Append(Block);
            }
            else
            {
                sb.Append(Space);
            }

            //2,2
            sb.Append(Block);

            return sb.ToString();
        }
    }
}
