using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public class MapDistanceCalculatorEdges
    {
        CoordinateWrapper coordinateWrapper;

        public MapDistanceCalculatorEdges(CoordinateWrapper coordinateWrapper)
        {
            this.coordinateWrapper = coordinateWrapper;
        }

        /// <summary>
        /// TODO
        /// </summary>            
        public int Grid(Vector3Int edgeA, Vector3Int edgeB)
        {
            if (coordinateWrapper != null) edgeB = coordinateWrapper.ShiftTargetToClosestPeriodicEdgePosition(edgeA, edgeB); //non-wrapping maps just return original
            return HexGrid.GetDistance.BetweenEdges(edgeA, edgeB);
        }

        /// <summary>
        /// TODO
        /// </summary> 
        public float Euclidean(Vector3Int edgeA, Vector3Int edgeB)
        {
            if (coordinateWrapper != null) edgeB = coordinateWrapper.ShiftTargetToClosestPeriodicEdgePosition(edgeA, edgeB); 
            return Vector3.Distance(HexConverter.EdgeCoordToCartesianCoord(edgeA), HexConverter.EdgeCoordToCartesianCoord(edgeB));
        }
    }
}