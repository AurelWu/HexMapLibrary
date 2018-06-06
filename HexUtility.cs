using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Contains helper methods which you usually won't need to use directly, but which are used by the library
    /// </summary>
    public static class HexUtility
    {
        public static Dictionary<EdgeAlignment, int> anglebyEdgeAlignment = 
            new Dictionary<EdgeAlignment, int>()
            {
                { EdgeAlignment.ParallelToCubeY, 0 },
                { EdgeAlignment.ParallelToCubeX, 120 },
                { EdgeAlignment.ParallelToCubeZ, 240 }
            };

        public static Dictionary<EdgeDirection, int> anglebyEdgeDirection =
             new Dictionary<EdgeDirection, int>()
            {
                { EdgeDirection.Top, 0 },
                { EdgeDirection.TopRight, 60 },
                { EdgeDirection.BottomRight, 120 },
                { EdgeDirection.Bottom, 180 },
                { EdgeDirection.BottomLeft, 240 },
                { EdgeDirection.TopLeft, 300 }
            };
        /// <summary>
        /// returns a lerped non-integer cube coordinate between origin and target cube coordinate.
        /// </summary>
        /// <param name="start">start value</param>
        /// <param name="target">target value</param>
        /// <param name="nudgeXfromCenter">offset of start value in x-direction</param>
        /// <param name="t">interpolation value, uses Mathf.Lerp which clamps value betweeen 0 and 1</param>
        /// <returns></returns>
        public static Vector3 LerpCubeCoordinates(Vector3Int start, Vector3Int target, float nudgeXfromCenter, float t)
        {
            float x = Mathf.Lerp(start.x + nudgeXfromCenter, target.x, t);
            float y = Mathf.Lerp(start.y, target.y, t);
            float z = Mathf.Lerp(start.z - nudgeXfromCenter, target.z, t);
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// rounds a non-integer cube coordinate to the closest integer cube coordinate
        /// </summary>
        public static Vector3Int RoundCubeCoordinate(Vector3 cubeFloatCoords)
        {
            int rx = Mathf.RoundToInt(cubeFloatCoords.x);
            int ry = Mathf.RoundToInt(cubeFloatCoords.y);
            int rz = Mathf.RoundToInt(cubeFloatCoords.z);

            float diffX = Mathf.Abs(rx - cubeFloatCoords.x);
            float diffY = Mathf.Abs(ry - cubeFloatCoords.y);
            float diffZ = Mathf.Abs(rz - cubeFloatCoords.z);

            if (diffX > diffY && diffX > diffZ)
            {
                rx = -ry - rz;
            }
            else if (diffY > diffZ)
            {
                ry = -rx - rz;
            }
            else
            {
                rz = -rx - ry;
            }

            return new Vector3Int(rx, ry, rz);
        }


        /// <summary>
        /// returns the angle of the edge. returns 0 if parallel y-axis, 120 if along x-axis and 240 if along z-axis.
        /// </summary>
        public static EdgeAlignment GetEdgeAlignment(Vector3Int edge)
        {
            if (edge.y % 2 == 0)
            {
                return EdgeAlignment.ParallelToCubeY;
            }
            else if (edge.x % 2 == 0)
            {
                return EdgeAlignment.ParallelToCubeX;
            }
            else return EdgeAlignment.ParallelToCubeZ;
        }

        public static CornerType GetCornerType(Vector3Int corner)
        {            
            float a1 = (corner.x - 1) / 3f;
            float a2 = (corner.y + 2) / 3f;
            float a3 = (corner.z - 1) / 3f;

            if ((Mathf.Abs(a1 - (int)a1) < 0.01f && Mathf.Abs(a2 - (int)a2) < 0.01f && Mathf.Abs(a3 - (int)a3) < 0.01f))
            {
                return CornerType.TopOfYParallelEdge;
            }
            else return CornerType.BottomOfYParallelEdge;                
        }

        public static MapSizeData CalculateMapCenterAndExtents(IEnumerable<Vector3Int> tiles, float yOffset = 0)
        {
            int minXOffset = int.MaxValue;
            int maxXOffset = int.MinValue;
            int minYOffset = int.MaxValue;
            int maxYOffset = int.MinValue;

            foreach (var coord in tiles)
            {
                Vector2Int offsetCoord = HexConverter.TileCoordToOffsetTileCoord(coord);
                if (offsetCoord.x < minXOffset) minXOffset = offsetCoord.x;
                if (offsetCoord.x > maxXOffset) maxXOffset = offsetCoord.x;
                if (offsetCoord.y < minYOffset) minYOffset = offsetCoord.y;
                if (offsetCoord.y > maxYOffset) maxYOffset = offsetCoord.y;
            }

            Vector3 worldMin = HexConverter.OffsetTileCoordToCartesianCoord(new Vector2Int(minXOffset, minYOffset));
            Vector3 worldMax = HexConverter.OffsetTileCoordToCartesianCoord(new Vector2Int(maxXOffset, maxYOffset));

            Vector3 center = ((worldMin + worldMax) / 2);
            Vector3 extents = new Vector3((worldMax.x - worldMin.x) / 2f, 0, (worldMax.z - worldMin.z) / 2f);

            MapSizeData mapSizeData = new MapSizeData(minXOffset, maxXOffset, minYOffset, maxYOffset, center, extents);
            return mapSizeData;
        }

        /// <summary>
        /// Helper Method for GetValidCoordinates which just compares input Coordinates with the set of valid coordinates of the map and removes all invalid.
        /// </summary>
        public static List<Vector3Int> RemoveInvalidCoordinates(List<Vector3Int> inputCollection, Dictionary<Vector3Int, int> validCollection)
        {
            for (int i = 0; i < inputCollection.Count; i++)
            {
                
                Vector3Int element = inputCollection.ElementAt(i);

                if (!validCollection.ContainsKey(element))
                {
                    inputCollection.Remove(element);
                    i--;
                }
            }
            return inputCollection;
        }
    }
}