using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{

    public class EdgePositionsProvider
    {
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly Dictionary<Vector3Int, int> EdgeIndexByPosition;

        public EdgePositionsProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int, int> EdgeIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.EdgeIndexByPosition = EdgeIndexByPosition;
        }

        /// <summary>
        /// returns all edges of the input tile
        /// </summary>        
        /// ![yellow = input tile , blue = result](GetEdges_OfTile.png)
        public List<Vector3Int> OfTile(Vector3Int tile)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.OfTile(tile);
            if (coordinateWrapper != null) edges = coordinateWrapper.WrapEdgeCoordinates(edges); 
            return edges;
        }

        /// <summary>
        /// returns all edges adjacent to the input corner which belong to the map
        /// </summary>        
        /// ![green = input corner , blue = result](Map_GetEdges_AdjacentToCorner_Combined.png)
        public List<Vector3Int> AdjacentToCorner(Vector3Int corner)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.AdjacentToCorner(corner);
            return GetValidEdgeCoordinates(edges);            
        }

        /// <summary>
        /// returns all edges adjacent to the input edge which belong to the map
        /// </summary>     
        /// ![green = input edge , blue = result](Map_GetEdges_AdjacentToEdge_Combined.png)
        public List<Vector3Int> AdjacentEdges(Vector3Int edge)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.AdjacentToEdge(edge);
            return GetValidEdgeCoordinates(edges);
        }

        /// <summary>
        /// returns all edges within range of the input edge which belong to the map
        /// </summary>      
        ///![green = input edge, blue = result] (Map_GetEdges_WithinDistanceOfEdge_Combined.png)
        public List<Vector3Int> WithinDistanceOfEdge(Vector3Int centerEdge, int maxDistance, bool includeSelf)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.WithinDistanceOfEdge(centerEdge, maxDistance, includeSelf);
            return GetValidEdgeCoordinates(edges);
        }

        /// <summary>
        /// returns all edges within range of the input corner which belong to the map
        /// </summary>        
        /// ![green = input corner , blue = result](Map_GetEdges_WithinDistanceOfCorner_Combined.png)
        public List<Vector3Int> WithinDistanceOfCorner(Vector3Int corner, int maxDistance)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.WithinDistanceOfCorner(corner, maxDistance);
            return GetValidEdgeCoordinates(edges);
        }

        /// <summary>
        /// returns all edges at the exact distance of the input edge which belong to the map
        /// </summary>  
        /// ![green = input corner , blue = result](Map_GetEdges_AtExactDistance_Combined.png)
        public List<Vector3Int> AtExactDistance(Vector3Int centerEdge, int distance)
        {
            List<Vector3Int> edges = HexGrid.GetEdges.AtExactDistance(centerEdge, distance);
            return GetValidEdgeCoordinates(edges);
        }

        /// <summary>
        /// returns a list of all edges which form the border(s) of the input Tile Coordinates. They are returned in an arbitrary order and might belong to different border paths.
        /// </summary>
        /// ![green = input tiles , blue = result](Map_GetEdges_TileBorders.png)
        public List<Vector3Int> TileBorders(IEnumerable<Vector3Int> tiles)
        {
            List<Vector3Int> edges = new List<Vector3Int>();

            foreach (var tile in tiles)
            {
                List<Vector3Int> edgeCoordsOfCell = OfTile(tile);
                foreach (var c in edgeCoordsOfCell)
                {
                    if (edges.Contains(c)) edges.Remove(c);
                    else edges.Add(c);
                }
            }
            return edges;
        }

        /// <summary>
        /// returns a list of all edges which form the border(s) of the input Tile Coordinates. They are returned in an arbitrary order and might belong to different border paths.
        /// </summary>
        public List<Vector3Int> TileBorders(IEnumerable<Tile> tiles)
        {
            List<Vector3Int> edges = new List<Vector3Int>();

            foreach (var tile in tiles)
            {
                List<Vector3Int> edgeCoordsOfCell = OfTile(tile.Position);
                foreach (var c in edgeCoordsOfCell)
                {
                    if (edges.Contains(c)) edges.Remove(c);
                    else edges.Add(c);
                }
            }
            return edges;
        }

        /// <summary>
        /// returns the shortest path of edges from origin to target corner
        /// </summary>
        /// ![green = origin corner , purple = target corner, blue = result](Map_GetEdges_PathBetweenCorners_Combined.png)
        public List<Vector3Int> PathBetweenCorners(Vector3Int originCorner, Vector3Int targetCorner, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            if (coordinateWrapper != null) targetCorner = coordinateWrapper.ShiftTargetToClosestPeriodicCornerPosition(originCorner, targetCorner);
            var edges = HexGrid.GetEdges.PathBetweenCorners(originCorner, targetCorner, horizontalNudgeFromOriginCenter);
            return GetValidEdgeCoordinates(edges);
            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// returns an list of each contiguous border path of the input Tile Coordinates. Each individual path is ordered in clockwise direction with an arbitrary starting point. The different Paths are returned in an arbitrary order        
        /// </summary>
        /// ![green = input tiles , blue = result](Map_GetEdges_BorderPaths.png)
        public List<List<Vector3Int>> BorderPaths(IEnumerable<Vector3Int> tiles, out List<List<EdgeDirection>> pathDirections)
        {
            if(coordinateWrapper == null)
            {
                return HexGrid.GetEdges.BorderPaths(tiles, out pathDirections);
            }

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
                    if (edge.y > maxYsoFar)
                    {
                        topRightEdge = edge;
                        maxXOfmaxYsoFar = edge.x;
                        maxYsoFar = edge.y;
                    }
                    else if (edge.y == maxYsoFar && edge.x > maxXOfmaxYsoFar)
                    {
                        topRightEdge = edge;
                        maxXOfmaxYsoFar = edge.x;
                        maxYsoFar = edge.y;
                    }
                }

                Vector3Int currentEdge = topRightEdge;
                EdgeDirection currentDirection = EdgeDirection.BottomRight;
                bool targetReached = false;
                int safety = 0;

                while (!targetReached)
                {
                    safety++;
                    if(safety > 250)
                    {
                        Debug.Log("safety reached!!! Error in while loop, preventing going infinite");
                        break;
                    }
                    borderPath.Add(currentEdge);
                    borderDirection.Add(currentDirection);
                    unusedEdges.Remove(currentEdge);

                    Vector3Int offsetClockwise = HexGrid.ClockWiseNeighbourOfEdgeByEdgeDirection[(int)currentDirection];
                    Vector3Int offsetCounterClockwise = HexGrid.CounterClockWiseNeighbourOfEdgeByEdgeDirection[(int)currentDirection];
                    Vector3Int potentialClockwiseNeighbour = coordinateWrapper.WrapEdgeCoordinate(currentEdge + offsetClockwise);
                    Vector3Int potentialCounterClockwiseNeighbour = coordinateWrapper.WrapEdgeCoordinate(currentEdge + offsetCounterClockwise);

                    if (unusedEdges.Contains(potentialCounterClockwiseNeighbour))
                    {
                        currentEdge = potentialCounterClockwiseNeighbour;
                        currentDirection = (EdgeDirection)((int)(currentDirection + 5) % 6);
                    }
                    else if (unusedEdges.Contains(potentialClockwiseNeighbour))
                    {
                        currentEdge = potentialClockwiseNeighbour;
                        currentDirection = (EdgeDirection)((int)(currentDirection + 1) % 6);
                    }
                    else //we didn't found any unused edge so we must be at end (change to flag I guess and replace do while with normal while
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
                for (int j = 0; j < pathDirections[i].Count; j++)
                {
                    pathDirections[i][j] = (EdgeDirection)(((int)pathDirections[i][j] + 3) % 6);
                }
            }
            return borderPaths;

        }

        /// <summary>
        /// wraps edge coordinates and then removes all which are out of map bounds
        /// </summary>        
        protected List<Vector3Int> GetValidEdgeCoordinates(List<Vector3Int> rawPositions)
        {
            List<Vector3Int> positions = rawPositions;
            if (coordinateWrapper != null) positions = coordinateWrapper.WrapEdgeCoordinates(positions);
            positions = HexUtility.RemoveInvalidCoordinates(positions, EdgeIndexByPosition);
            return positions;
        }


    }

}