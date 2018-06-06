using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public class MapDistanceCalculatorTile
    {
        CoordinateWrapper coordinateWrapper;

        public MapDistanceCalculatorTile(CoordinateWrapper coordinateWrapper)
        {
            this.coordinateWrapper = coordinateWrapper;
        }

        /// <summary>
        /// returns the manhattan Distance between tileA and tileB or in other words how many edges you need to cross when moving from tileA to tileB accounting for map wrapping
        /// </summary>            
        public int Grid(Vector3Int tileA, Vector3Int tileB)
        {
            if(coordinateWrapper!=null) tileB = coordinateWrapper.ShiftTargetToClosestPeriodicTilePosition(tileA, tileB); //non-wrapping maps just return original
            return HexGrid.GetDistance.BetweenTiles(tileA, tileB);
        }

        /// <summary>
        /// returns the euclidian distance of a straight line from the center of tileA to the center of tileB accounting for map wrapping
        /// </summary> 
        public float Euclidean(Vector3Int tileA, Vector3Int tileB)
        {
            if(coordinateWrapper!=null) tileB = coordinateWrapper.ShiftTargetToClosestPeriodicTilePosition(tileA, tileB);
            return Vector3.Distance(HexConverter.TileCoordToCartesianCoord(tileA), HexConverter.TileCoordToCartesianCoord(tileB));
        }
    }
}