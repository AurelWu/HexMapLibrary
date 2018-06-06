using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetEdges
        {
            /// <summary>
            /// returns the 6 edges of a tile
            /// </summary>        
            /// ![yellow = input tile , blue = result](GetEdges_OfTile.png)
            public static List<Vector3Int> OfTile(Vector3Int tile)
            {
                int x = 2 * tile.x;
                int y = 2 * tile.y;
                int z = 2 * tile.z;
                List<Vector3Int> edgeCoords = new List<Vector3Int>
                {
                    new Vector3Int(x+1 , y , 2*(tile.z-1)+1 ),
                    new Vector3Int(x+1 , 2*(tile.y-1)+1 , z ),
                    new Vector3Int(x , 2*(tile.y-1)+1 , z+1 ),
                    new Vector3Int(2*(tile.x-1)+1 , y , z+1 ),
                    new Vector3Int(2*(tile.x-1)+1 , y+1 , z ),
                    new Vector3Int(x , y+1 , 2*(tile.z-1)+1 ),
                };
                return edgeCoords;

            }

            /// <summary>
            /// returns the 4 edges which are adjacent to an edge
            /// </summary>      
            /// ![green = input edge , blue = result](GetEdges_AdjacentToEdge.png)
            public static List<Vector3Int> AdjacentToEdge(Vector3Int edge)
            {
                List<Vector3Int> edgeCoords = new List<Vector3Int>();
                EdgeAlignment orientiation = HexUtility.GetEdgeAlignment(edge);
                if(orientiation == EdgeAlignment.ParallelToCubeY)
                {
                    Vector3Int topRightEdge = edge + new Vector3Int(0, +1, -1);
                    Vector3Int bottomRightEdge = edge + new Vector3Int(+1, -1, 0);
                    Vector3Int bottomLeftEdge = edge + new Vector3Int(0, -1, +1);
                    Vector3Int topLeftEdge = edge + new Vector3Int(-1, +1, 0);
                    edgeCoords.Add(topRightEdge);
                    edgeCoords.Add(bottomRightEdge);
                    edgeCoords.Add(bottomLeftEdge);
                    edgeCoords.Add(topLeftEdge);
                }
                else if(orientiation == EdgeAlignment.ParallelToCubeX)
                {
                    Vector3Int rightEdge = edge + new Vector3Int(+1, 0, -1);
                    Vector3Int bottomEdge = edge + new Vector3Int(+1, -1, 0);
                    Vector3Int leftEdge = edge + new Vector3Int(-1, 0, +1);
                    Vector3Int topEdge = edge + new Vector3Int(-1, +1, 0);
                    edgeCoords.Add(rightEdge);
                    edgeCoords.Add(bottomEdge);
                    edgeCoords.Add(leftEdge);
                    edgeCoords.Add(topEdge);
                }
                else if(orientiation == EdgeAlignment.ParallelToCubeZ)
                {
                    Vector3Int rightEdge = edge + new Vector3Int(+1, 0, -1);
                    Vector3Int bottomEdge = edge + new Vector3Int(0, -1, +1);
                    Vector3Int leftEdge = edge + new Vector3Int(-1, 0, +1);
                    Vector3Int topEdge = edge + new Vector3Int(0, +1, -1);
                    edgeCoords.Add(rightEdge);
                    edgeCoords.Add(bottomEdge);
                    edgeCoords.Add(leftEdge);
                    edgeCoords.Add(topEdge);
                }
                return edgeCoords;
            }

            /// <summary>
            /// returns the 3 edges which share the input corner
            /// </summary>     
            /// ![green = input corner , blue = result](GetEdges_AdjacentToCorner.png)
            public static List<Vector3Int> AdjacentToCorner(Vector3Int corner)
            {
                //INTERIM INEFFICIENT SOLUTION: we get the neighbouring tile coordinates then Edge AB AC and BC
                //SHOULD just directly offset the coordinates depending on the 2 different corner position types

                List<Vector3Int> tiles =GetTiles.AdjacentToCorner(corner);
                Vector3Int edgeA = GetEdge.BetweenTiles(tiles[0], tiles[1]);
                Vector3Int edgeB = GetEdge.BetweenTiles(tiles[0], tiles[2]);
                Vector3Int edgeC = GetEdge.BetweenTiles(tiles[1], tiles[2]);
                return new List<Vector3Int> { edgeA, edgeB, edgeC };
            }

            /// <summary>
            /// returns all edges with are within the specified range of the input edge (either with or without the center itself)
            /// </summary>
            /// ![green = input edge , blue = result](GetEdges_WithinDistance.png)
            public static List<Vector3Int> WithinDistanceOfEdge (Vector3Int centerEdge, int maxDistance, bool includeCenter)
            {
                //there might be smarter ways but this should work.                
                HashSet<Vector3Int> edgesInRange = new HashSet<Vector3Int>();
                HashSet<Vector3Int> openEdges = new HashSet<Vector3Int>();                
                openEdges.Add(centerEdge);

                for(int i = 0; i <= maxDistance; i++)
                {
                    HashSet<Vector3Int> edgesWithDistanceI = new HashSet<Vector3Int>();
                    foreach(var openEdge in openEdges)
                    {
                        edgesInRange.Add(openEdge);
                        var adjacent = GetEdges.AdjacentToEdge(openEdge);
                        foreach(var adj in adjacent)
                        if (!edgesInRange.Contains(adj))
                        {
                            edgesWithDistanceI.Add(adj);
                        }
                    }
                    openEdges = edgesWithDistanceI;
                }
            
                if (!includeCenter) edgesInRange.Remove(centerEdge);

                return edgesInRange.ToList();
            }

            /// <summary>
            /// returns all edges with are within the specified range of the input corner
            /// </summary>
            /// ![green = input corner , blue = result](GetEdges_WithinDistanceOfCorner.png)
            public static List<Vector3Int> WithinDistanceOfCorner(Vector3Int centerCorner, int maxDistance)
            {
                if (maxDistance == 0) return new List<Vector3Int>();
                //there might be smarter ways but this should work.                
                HashSet<Vector3Int> edgesInRange = new HashSet<Vector3Int>();
                HashSet<Vector3Int> openEdges = new HashSet<Vector3Int>();

                var adjacentToCorner = GetEdges.AdjacentToCorner(centerCorner);
                adjacentToCorner.ForEach(x => openEdges.Add(x));                

                for (int i = 1; i <= maxDistance; i++)
                {
                    HashSet<Vector3Int> edgesWithDistanceI = new HashSet<Vector3Int>();
                    foreach (var edge in openEdges)
                    {
                        edgesInRange.Add(edge);
                        var adjacent = GetEdges.AdjacentToEdge(edge);
                        foreach (var adj in adjacent)
                            if (!edgesInRange.Contains(adj))
                            {
                                edgesWithDistanceI.Add(adj);
                            }
                    }
                    openEdges = edgesWithDistanceI;
                }

                return edgesInRange.ToList();
            }

            /// <summary>
            /// returns all the edges with are exactly at the specified distance of the input edge
            /// </summary>
            /// ![green = input corner , blue = result](GetEdges_AtExactDistance.png)
            public static List<Vector3Int> AtExactDistance(Vector3Int centeredge, int distance)
            {
                //inefficient but does its job for now 
                //not Tested yet though
                List<Vector3Int> allWithinDistance = WithinDistanceOfEdge(centeredge, distance, true);
                List<Vector3Int> atExactDistance = new List<Vector3Int>();
                foreach (var edge in allWithinDistance)
                {
                    int dist = HexGrid.GetDistance.BetweenEdges(edge, centeredge);
                    if (dist == distance) atExactDistance.Add(edge);
                }
                return atExactDistance;
            }

            /// <summary>
            /// returns the shortest path of edges from origin to target corner
            /// </summary>
            /// ![green = origin corner , purple = target corner, blue = result](GetEdges_PathBetweenCorners.png)
            public static List<Vector3Int> PathBetweenCorners(Vector3Int originCorner, Vector3Int targetCorner, float horizontalNudgeFromOriginCenter = NudgePositive)
            {
                List<Vector3Int> corners = GetCorners.PathAlongGrid(originCorner, targetCorner, true, horizontalNudgeFromOriginCenter);
                List<Vector3Int> edges = new List<Vector3Int>();
                
                for(int i = 0; i <corners.Count-1; i++)
                {
                    edges.Add(HexGrid.GetEdge.BetweenCorners(corners[i], corners[i + 1]));
                }

                return edges;
            }

            /// <summary>
            /// returns all the border edges of a set of tiles. 
            /// </summary>
            /// ![green = input tiles , blue = result](GetEdges_TileBorders.png)
            public static List<Vector3Int> TileBorders(IEnumerable<Vector3Int> tiles)
            {
                List<Vector3Int> edges = new List<Vector3Int>();

                foreach (var tile in tiles)
                {
                    List<Vector3Int> edgeCoordsOfCell = GetEdges.OfTile(tile);
                    foreach (var c in edgeCoordsOfCell)
                    {
                        if (edges.Contains(c)) edges.Remove(c);
                        else edges.Add(c);
                    }
                }
                return edges;
            }

            /// <summary>
            /// returns all the border paths of one contiguous Area. Outer path is clockwise, others are counterclockwise
            /// </summary>
            /// ![green = input tiles , blue = result](GetEdges_BorderPaths.png)
            public static List<List<Vector3Int>> BorderPaths(IEnumerable<Vector3Int> tiles, out List<List<EdgeDirection>> pathDirections)
            {
                pathDirections = new List<List<EdgeDirection>>();
                List<List<Vector3Int>> borderPaths = new List<List<Vector3Int>>();
                List<Vector3Int> edgesUnordered = TileBorders(tiles);
                HashSet<Vector3Int> unusedEdges = new HashSet<Vector3Int>(edgesUnordered); //we remove every edge which we used from this collection to find out which are still left after we finished a path

                //we do it as long as we didn't use all edges because that means we didn't get all paths yet.
                while (unusedEdges.Count > 0)
                {
                    List<Vector3Int> borderPath = new List<Vector3Int>();
                    List<EdgeDirection> borderDirection = new List<EdgeDirection>();
                    Vector3Int topRightEdge = new Vector3Int();

                    //can we just use any edge instead? -> nah we want to ensure it being clockwise... 

                    int maxYsoFar = int.MinValue;
                    int maxXOfmaxYsoFar = int.MinValue;
                    //now we pick one of the top most edges which is parallel to the X-axis of our cube 
                    foreach (Vector3Int edge in unusedEdges)
                    {
                        EdgeAlignment orientation = HexUtility.GetEdgeAlignment(edge);
                        if (orientation != EdgeAlignment.ParallelToCubeX) continue;
                        if(edge.y > maxYsoFar) 
                        {
                            topRightEdge = edge;
                            maxXOfmaxYsoFar = edge.x;
                            maxYsoFar = edge.y;
                        }          
                        else if(edge.y == maxYsoFar && edge.x > maxXOfmaxYsoFar)
                        {
                            topRightEdge = edge;
                            maxXOfmaxYsoFar = edge.x;
                            maxYsoFar = edge.y;
                        }
                    }

                    Vector3Int currentEdge = topRightEdge;
                    EdgeDirection currentDirection = EdgeDirection.BottomRight;
                    bool targetReached = false;

                    while(!targetReached)
                    {
                        borderPath.Add(currentEdge);
                        borderDirection.Add(currentDirection);
                        unusedEdges.Remove(currentEdge);

                        Vector3Int offsetClockwise = ClockWiseNeighbourOfEdgeByEdgeDirection[(int)currentDirection];
                        Vector3Int offsetCounterClockwise = CounterClockWiseNeighbourOfEdgeByEdgeDirection[(int)currentDirection];

                        if (unusedEdges.Contains(currentEdge + offsetCounterClockwise))
                        {
                            currentEdge = currentEdge + offsetCounterClockwise;
                            currentDirection = (EdgeDirection)((int)(currentDirection + 5) % 6);
                        }
                        else if (unusedEdges.Contains(currentEdge + offsetClockwise))
                        {
                            currentEdge = currentEdge + offsetClockwise;
                            currentDirection = (EdgeDirection)((int)(currentDirection + 1) % 6);
                        }
                        else //we didn't find any unused edge so we must be at end
                        {
                            targetReached = true;
                        }
                    }
                    
                    borderPaths.Add(borderPath);
                    pathDirections.Add(borderDirection);
                }

                for (int i = 1; i < borderPaths.Count; i++)
                {
                    borderPaths[i].Reverse();
                    pathDirections[i].Reverse();
                    for(int j = 0; j < pathDirections[i].Count; j++)
                    {
                        pathDirections[i][j] = (EdgeDirection)(((int)pathDirections[i][j] + 3) % 6);
                    }
                }
                return borderPaths;
            }
        }
    }
}