using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetCorner
        {

            /// <summary>
            /// returns the corner coordinate closest to the input cartesian coordinate
            /// </summary>
            public static Vector3Int ClosestToCartesianCoordinate(Vector3 cartesianCoordinate)
            {
                return HexConverter.CartesianCoordToClosestCornerCoord(cartesianCoordinate);
            }

            /// <summary>
            /// returns the shared corner of the input tile coordinates
            /// </summary>
            /// TODO Add image
            public static Vector3Int BetweenNeighbouringTileCoordinates(Vector3Int a, Vector3Int b, Vector3Int c)
            {
                if (GetDistance.BetweenTiles(a, b) != 1) throw new System.ArgumentException("Tiles a and b don't have a distance of 1, therefore and not neighbours and share no Edge");
                if (GetDistance.BetweenTiles(a, c) != 1) throw new System.ArgumentException("Tiles a and c don't have a distance of 1, therefore and not neighbours and share no Edge");
                if (GetDistance.BetweenTiles(b, c) != 1) throw new System.ArgumentException("Tiles b and c don't have a distance of 1, therefore and not neighbours and share no Edge");

                Vector3Int cornerCoordinate = a + b + c;
                return cornerCoordinate;
            }
        }
    }
}