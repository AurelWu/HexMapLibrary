using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class CornerDataProvider<C> where C : new()
    {
        private Dictionary<Vector3Int, Corner<C>> cornersByPosition;
        private CornerPositionProvider cornerPositionProvider;

        public CornerDataProvider(Dictionary<Vector3Int, Corner<C>> cornersByPosition, CornerPositionProvider cornerPositionProvider)
        {
            this.cornersByPosition = cornersByPosition;
            this.cornerPositionProvider = cornerPositionProvider;
        }

        /// <summary>
        /// returns the periodic corner of the input cartesian coordinate
        /// </summary>        
        public Corner<C> FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            Vector3Int coord = cornerPositionProvider.FromCartesianCoordinate(cartesianCoordinate);
            if (!cornersByPosition.ContainsKey(coord)) return null;
            return cornersByPosition[coord];
        }
    }
}