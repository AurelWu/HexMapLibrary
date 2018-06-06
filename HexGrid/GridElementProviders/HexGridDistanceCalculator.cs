using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetDistance
        {
            /// <summary>
            /// returns the tile-distance of the 2 input tiles
            /// </summary>
            public static int BetweenTiles(Vector3Int tileA, Vector3Int tileB)
            {
                int DeltaX = Mathf.Abs(tileA.x - tileB.x);
                int DeltaY = Mathf.Abs(tileA.y - tileB.y);
                int DeltaZ = Mathf.Abs(tileA.z - tileB.z);
                return Mathf.Max(DeltaX, DeltaY, DeltaZ);
            }

            /// <summary>
            /// returns the euclidean distance of the 2 input tiles.
            /// </summary>
            public static float BetweenTilesEuclidean(Vector3Int tileA, Vector3Int tileB)
            {
                Vector3 a = HexConverter.TileCoordToCartesianCoord(tileA);
                Vector3 b = HexConverter.TileCoordToCartesianCoord(tileB);
                return Vector3.Distance(a, b);
            }

            /// <summary>
            /// returns the edge-distance of the 2 inpute edges
            /// </summary>
            public static int BetweenEdges(Vector3Int edgeA, Vector3Int edgeB)
            {
                if (edgeA == edgeB) return 0;
                
                int DeltaX = Mathf.Abs(edgeA.x - edgeB.x);
                int DeltaY = Mathf.Abs(edgeA.y - edgeB.y);
                int DeltaZ = Mathf.Abs(edgeA.z - edgeB.z);
                int distance = Mathf.Max(DeltaX, DeltaY, DeltaZ);
                if( (HexUtility.GetEdgeAlignment(edgeA) == EdgeAlignment.ParallelToCubeX && edgeA.x == edgeB.x) ||
                    (HexUtility.GetEdgeAlignment(edgeA) == EdgeAlignment.ParallelToCubeY && edgeA.y == edgeB.y) ||
                    (HexUtility.GetEdgeAlignment(edgeA) == EdgeAlignment.ParallelToCubeZ && edgeA.z == edgeB.z))
                {                    
                    distance += 1;
                }
                return distance; 
            }

            /// <summary>
            /// returns the euclidean distance between the midpoints of both input edges
            /// </summary>
            public static float BetweenEdgesEuclidean(Vector3Int edgeA, Vector3Int edgeB)
            {
                Vector3 a = HexConverter.EdgeCoordToCartesianCoord(edgeA);
                Vector3 b = HexConverter.EdgeCoordToCartesianCoord(edgeB);
                return Vector3.Distance(a, b);
            }

            /// <summary>
            /// returns the corner-distance between both input corners
            /// </summary>
            public static int BetweenCorners(Vector3Int cornerA, Vector3Int cornerB)
            {
                int deltaX = Mathf.Abs(cornerA.x - cornerB.x);
                int deltaY = Mathf.Abs(cornerA.y - cornerB.y);
                int deltaZ = Mathf.Abs(cornerA.z - cornerB.z);
                int sum = deltaX + deltaY + deltaZ;
                int distance = sum / 3;
                if (sum % 3 == 2) distance += 1;
                return distance;
            }

            /// <summary>
            /// returns the euclidean distance between both input corners
            /// </summary>
            public static float BetweenCornersEuclidean(Vector3Int cornerA, Vector3Int cornerB)
            {
                Vector3 a = HexConverter.CornerCoordToCartesianCoord(cornerA);
                Vector3 b = HexConverter.CornerCoordToCartesianCoord(cornerB);
                return Vector3.Distance(a, b);
            }
        }
    }
}