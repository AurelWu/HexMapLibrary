using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetCorners
        {
            /// <summary>
            /// returns the 6 corners of the input tile
            /// </summary>
            /// ![yellow = input tile , blue = result](GetCorners_OfTile.png)
            public static List<Vector3Int> OfTile(Vector3Int tile)
            {
                int x = tile.x * 3;
                int y = tile.y * 3;
                int z = tile.z * 3;
                Vector3Int top = new Vector3Int(x - 1, y + 2, z - 1);
                Vector3Int topRight = new Vector3Int(x + 1, y + 1, z - 2);
                Vector3Int bottomRight = new Vector3Int(x + 2, y - 1, z - 1);
                Vector3Int bottom = new Vector3Int(x + 1, y - 2, z + 1);
                Vector3Int bottomLeft = new Vector3Int(x - 1, y - 1, z + 2);
                Vector3Int topLeft = new Vector3Int(x - 2, y + 1, z + 1);
                return new List<Vector3Int> { top, topRight, bottomRight, bottom, bottomLeft, topLeft };
            }

            /// <summary>
            /// returns the 2 corners adjacent to the input edge
            /// </summary>
            /// ![green = input edge , blue = result](GetCorners_AdjacentToEdge.png)
            public static List<Vector3Int> OfEdge(Vector3Int edge)
            {
                List<Vector3Int> corners = new List<Vector3Int>();
                EdgeAlignment orientation = HexUtility.GetEdgeAlignment(edge);

                int x = edge.x * 3;
                int y = edge.y * 3;
                int z = edge.z * 3;

                if (orientation == EdgeAlignment.ParallelToCubeX)
                {
                    Vector3Int topLeft = new Vector3Int( (x - 2)/2, (y + 1)/2, (z + 1)/2);
                    Vector3Int bottomRight = new Vector3Int( (x + 2)/2, (y - 1)/2, (z - 1)/2);
                    corners.Add(topLeft);
                    corners.Add(bottomRight);
                }

                if (orientation == EdgeAlignment.ParallelToCubeY)
                {
                    Vector3Int top = new Vector3Int( (x - 1)/2 , (y + 2)/2 , (z - 1)/2);
                    Vector3Int bottom = new Vector3Int((x + 1)/2, (y - 2)/2, (z + 1)/2);
                    corners.Add(top);
                    corners.Add(bottom);
                }

                if (orientation == EdgeAlignment.ParallelToCubeZ)
                {
                    Vector3Int topRight = new Vector3Int((x + 1)/2, (y + 1)/2, (z - 2)/2);
                    Vector3Int bottomLeft = new Vector3Int((x - 1)/2, (y - 1)/2, (z + 2)/2);
                    corners.Add(topRight);
                    corners.Add(bottomLeft);
                }
                return corners;
            }

            /// <summary>
            /// returns the 3 corners adjacent to the input corner
            /// </summary>       
            /// ![green = input corner , blue = result](GetCorners_AdjacentToCorner.png)
            public static List<Vector3Int> AdjacentToCorner(Vector3Int corner)
            {
                Vector3Int a, b, c;
                CornerType cornerType = HexUtility.GetCornerType(corner);

                //its the same approach like getting adjacent tiles but inverted (and without dividing by 3)
                if (cornerType == CornerType.BottomOfYParallelEdge)
                {
                    a = new Vector3Int((corner.x - 1), (corner.y + 2), (corner.z - 1));
                    b = new Vector3Int((corner.x + 2), (corner.y - 1), (corner.z - 1));
                    c = new Vector3Int((corner.x - 1), (corner.y - 1), (corner.z + 2));
                }

                else
                {
                    a = new Vector3Int((corner.x + 1), (corner.y + 1), (corner.z - 2));
                    b = new Vector3Int((corner.x + 1), (corner.y - 2), (corner.z + 1));
                    c = new Vector3Int((corner.x - 2), (corner.y + 1), (corner.z + 1));
                }
                return new List<Vector3Int> { a, b, c };
            }

            /// <summary>
            /// returns all corners within distance of the input corner - optionally including that corner.
            /// </summary>      
            /// ![green = input corner , blue = result](GetCorners_WithinDistance.png)
            public static List<Vector3Int> WithinDistance(Vector3Int centerCorner, int maxDistance, bool includeCenter)
            {
                //there might be smarter ways but this should work.                
                HashSet<Vector3Int> corners = new HashSet<Vector3Int>();
                HashSet<Vector3Int> openCorners = new HashSet<Vector3Int>();
                openCorners.Add(centerCorner);

                for (int i = 0; i <= maxDistance; i++)
                {
                    HashSet<Vector3Int> cornersWithDistanceI = new HashSet<Vector3Int>();
                    foreach (var corner in openCorners)
                    {
                        corners.Add(corner);
                        var adjacent = GetCorners.AdjacentToCorner(corner);
                        foreach (var adj in adjacent)
                            if (!corners.Contains(adj))
                            {
                                cornersWithDistanceI.Add(adj);
                            }
                    }
                    openCorners = cornersWithDistanceI;
                }

                if (!includeCenter) corners.Remove(centerCorner);

                return corners.ToList();
            }

            /// <summary>
            /// returns all corners at the exact distance of the input corner.
            /// </summary>
            /// ![green = input corner , blue = result](GetCorners_AtExactDistance.png) 
            public static List<Vector3Int> AtExactDistance(Vector3Int centerCorner, int distance)
            {
                List<Vector3Int> allWithinDistance = WithinDistance(centerCorner, distance, true);
                List<Vector3Int> atExactDistance = new List<Vector3Int>();
                foreach(var corner in allWithinDistance)
                {
                    int dist = HexGrid.GetDistance.BetweenCorners(corner, centerCorner);
                    if (dist == distance) atExactDistance.Add(corner);                    
                }
                return atExactDistance;
            }

            /// <summary>
            /// returns the shortest path of corners from the origin to the target corner - optionally including the origin
            /// </summary>   
            /// ![green = origin , purple = target, blue/purple = result - origin can optionally be included](GetCorners_PathAlongGrid.png) 
            public static List<Vector3Int> PathAlongGrid(Vector3Int originCorner, Vector3Int targetCorner, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
            {
                if(originCorner == targetCorner)
                {
                    throw new System.ArgumentException("origin corner and target corner are the same - can't create a Path");
                }

                List<Vector3Int> corners = new List<Vector3Int>();
                if(includeOrigin) corners.Add(originCorner);
                Vector3Int previousCorner = originCorner;

                Vector3 cartesianOrigin = HexConverter.CornerCoordToCartesianCoord(originCorner) + new Vector3(horizontalNudgeFromOriginCenter, 0, 0);
                Vector3 cartesianTarget = HexConverter.CornerCoordToCartesianCoord(targetCorner);

                int dist = GetDistance.BetweenCorners(originCorner, targetCorner);
                for(int i = 1; i <= dist; i++)
                {
                    
                    Vector3 lerped = Vector3.Lerp(cartesianOrigin, cartesianTarget, (1f / dist) * i);

                    Vector3Int tileCoord = HexConverter.CartesianCoordToTileCoord(lerped);

                    List<Vector3Int> cornerCoords = HexGrid.GetCorners.OfTile(tileCoord);
                    cornerCoords.RemoveAll(x => HexGrid.GetDistance.BetweenCorners(previousCorner, x) != 1);

                    Vector3Int closestCorner = new Vector3Int();
                    float minDistanceSoFar = float.MaxValue;
                    for (int j = 0; j < cornerCoords.Count; j++)
                    {
                        Vector3 worldPos = HexConverter.CornerCoordToCartesianCoord(cornerCoords[j]);
                        float distance = Vector3.Distance(worldPos, lerped);
                        if (distance < minDistanceSoFar)
                        {
                            closestCorner = cornerCoords[j];
                            minDistanceSoFar = distance;
                        }
                    }

                    corners.Add(closestCorner);
                    previousCorner = closestCorner;
                }
                return corners;                
            }

            /// <summary>
            /// returns all corners of the input tiles which are adjacent to 1 or 2 tiles not belonging to the input set.
            /// </summary>
            /// ![green = input tiles , blue = result](GetCorners_TileBorders.png) 
            public static List<Vector3Int> TileBorders(IEnumerable<Vector3Int> tiles)
            {
                List<Vector3Int> corners = new List<Vector3Int>();
                Dictionary<Vector3Int, int> numberOfAdjacentTilesByCorner = new Dictionary<Vector3Int, int>();

                foreach (var tile in tiles)
                {
                    List<Vector3Int> cornerCoordsOfCell = GetCorners.OfTile(tile);
                    foreach (var c in cornerCoordsOfCell)
                    {
                        if (numberOfAdjacentTilesByCorner.Keys.Contains(c)) numberOfAdjacentTilesByCorner[c] += 1;
                        else numberOfAdjacentTilesByCorner.Add(c,1); ;
                    }
                }

                foreach(var kvp in numberOfAdjacentTilesByCorner)
                {
                    if(kvp.Value < 3)
                    {
                        corners.Add(kvp.Key);
                    }
                }

                return corners;
            }
        }
    }
}