using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public class MapDistanceCalculatorCorners
    {
        CoordinateWrapper coordinateWrapper;

        public MapDistanceCalculatorCorners(CoordinateWrapper coordinateWrapper)
        {
            this.coordinateWrapper = coordinateWrapper;
        }

        /// <summary>
        /// TODO
        /// </summary>            
        public int Grid(Vector3Int cornerA, Vector3Int cornerB)
        {
            if (coordinateWrapper != null) cornerB = coordinateWrapper.ShiftTargetToClosestPeriodicCornerPosition(cornerA, cornerB); //non-wrapping maps just return original
            return HexGrid.GetDistance.BetweenCorners(cornerA, cornerB);
        }
        /// <summary>
        /// TODO
        /// </summary> 
        public float Euclidean(Vector3Int cornerA, Vector3Int cornerB)
        {
            if (coordinateWrapper != null) cornerB = coordinateWrapper.ShiftTargetToClosestPeriodicCornerPosition(cornerA, cornerB);
            return HexGrid.GetDistance.BetweenCornersEuclidean(cornerA, cornerB);
        }
    }
}