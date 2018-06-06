using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public class CornerPositionProvider
    {
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly MapDistanceCalculatorCorners distanceCalculator;
        protected readonly Dictionary<Vector3Int, int> cornerIndexByPosition;

        public CornerPositionProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int, int> cornerIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.cornerIndexByPosition = cornerIndexByPosition;
            this.distanceCalculator = new MapDistanceCalculatorCorners(coordinateWrapper);
        }

        /// <summary>
        /// returns the periodic corner coordinate of the input cartesian coordinate
        /// </summary>        
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToClosestCornerCoord(cartesianCoordinate);
            return coord;
        }

        /// <summary>
        /// returns the periodic corner coordinate of the input cartesian coordinate, the out parameter specifies if that coordinate belongs to the map
        /// </summary> 
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate, out bool cornerIsOnMap)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToClosestCornerCoord(cartesianCoordinate);
            cornerIsOnMap = false;
            if (cornerIndexByPosition.ContainsKey(coord)) cornerIsOnMap = true;
            return coord;
        }
    }
}