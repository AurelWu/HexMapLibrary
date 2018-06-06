using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    { 
        public class GetTile
        {
            /// <summary>
            /// returns the tile coordinate of the input cartesian coordinate
            /// </summary>
            public static Vector3Int OfCartesianCoordinate(Vector3 cartesianCoordinate)
            {
                return HexConverter.CartesianCoordToTileCoord(cartesianCoordinate);
            }

            /// <summary>
            /// rotates the input tile coordinate 60° Clockwise around the specified center point
            /// </summary>
            /// TODO Add image
            public static Vector3Int FromTileRotated60DegreeClockwise(Vector3Int center, Vector3Int pointToRotate)
            {
                Vector3Int direction = pointToRotate - center;
                int rotatedX = -direction.z;
                int rotatedY = -direction.x;
                int rotatedZ = -direction.y;
                Vector3Int rotated = new Vector3Int(rotatedX, rotatedY, rotatedZ) + center;
                return rotated;

            }

            /// <summary>
            /// rotates the input tile coordinate 60° Counter-Clockwise around the specified center point
            /// </summary>
            /// TODO Add image
            public static Vector3Int FromTileRotated60DegreeCounterClockwise(Vector3Int center, Vector3Int pointToRotate)
            {
                Vector3Int direction = pointToRotate - center;
                int rotatedX = -direction.y;
                int rotatedY = -direction.z;
                int rotatedZ = -direction.x;
                Vector3Int rotated = new Vector3Int(rotatedX, rotatedY, rotatedZ) + center;
                return rotated;
            }            
        }
    }


}