using MazeLibrary.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeLibrary.Algorithms
{
    //AlgorithmBase is a class for allowing events I've made to pass to the ui code, while which ever algorithms currently running to continue being made
    public class AlgorithmBase
    {
        //Event handlers for each event: wall created, passage created or the routepath getting a new cell added
        public event EventHandler<CellWallCreatedEventArgs> RaiseCellWallCreatedEvent;
        public event EventHandler<CellPassageCreatedEventArgs> RaiseCellPassageCreatedEvent;
        public event EventHandler<RoutePathAddedNewCellEventArgs> RaiseRoutePathAddedNewCellEvent;
        public event EventHandler<HighlightPathAddedNewCellEventArgs> RaiseHighlightPathAddedNewCellEvent;

        //A delay will feature in every algorithm forcing the algorithm to pause for the milliseconds chosen, allowing the canvas to refresh and the user time to view any changes
        protected int _delay;

        //For raising a cell wall created event arg
        protected virtual void OnRaiseCellWallCreatedEvent(CellWallCreatedEventArgs e)
        {
            EventHandler<CellWallCreatedEventArgs> handler = RaiseCellWallCreatedEvent;
            //Ensures that the event will only update the ui if an actual change has occurred
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //For raising a cell passage created event arg
        protected virtual void OnRaiseCellPassageCreatedEvent(CellPassageCreatedEventArgs e)
        {
            EventHandler<CellPassageCreatedEventArgs> handler = RaiseCellPassageCreatedEvent;
            //Ensures that the event will only update the ui if an actual change has occurred
            if (handler != null)
            {
                handler(this, e);
            }
        }

        //For raising a route being added to event arg

        protected virtual void OnRaiseRoutePathAddedNewCellEvent(RoutePathAddedNewCellEventArgs e)
        {
            EventHandler<RoutePathAddedNewCellEventArgs> handler = RaiseRoutePathAddedNewCellEvent;
            //Ensures that the event will only update the ui if an actual change has occurred
            if (handler != null)
            {
                handler(this, e);
            }
        }

        protected virtual void OnRaiseHighlightPathAddedNewCellEvent(HighlightPathAddedNewCellEventArgs e)
        {
            EventHandler<HighlightPathAddedNewCellEventArgs> handler = RaiseHighlightPathAddedNewCellEvent;
            if (handler != null)
            {
                handler(this, e);
            }
        }
    }
}
