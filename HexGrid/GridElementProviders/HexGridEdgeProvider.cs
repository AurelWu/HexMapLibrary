using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetEdge
        {
            /// <summary>
            /// returns the edge coordinate closest to the input cartesian coordinate
            /// </summary>
            public static Vector3Int ClosestToCartesianCoordinate(Vector3 cartesianCoordinate)
            {
                return HexConverter.CartesianCoordToClosestEdgeCoord(cartesianCoordinate);
            }

            /// <summary>
            /// returns the edge shared by 2 neighbouring tiles
            /// </summary>
            public static Vector3Int BetweenTiles(Vector3Int a, Vector3Int b)
            {
                if (HexGrid.GetDistance.BetweenTiles(a, b) != 1) throw new System.ArgumentException("Tiles don't have a distance of 1, therefore are not neighbours and share no Edge");

                Vector3Int edgeCoordinate = a + b;
                return edgeCoordinate;
            }

            /// <summary>
            /// returns the edge shared by 2 neighbouring corners
            /// </summary>
            /// TODO Add image
            public static Vector3Int BetweenCorners(Vector3Int CornerA, Vector3Int CornerB)
            {        
                if(HexGrid.GetDistance.BetweenCorners(CornerA,CornerB) != 1) throw new System.ArgumentException("Corners don't have a distance of 1, therefore are not neighbours and share no Edge");
                Vector3Int sum =CornerA + CornerB;
                return new Vector3Int(sum.x/3,sum.y/3,sum.z/3);
            }
        }
    }   
}