using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class EdgesDataProvider<E> where E : new()
    {
        private Dictionary<Vector3Int, Edge<E>> edgesByPosition;
        private EdgePositionsProvider edgesPositionsProvider;

        public EdgesDataProvider(Dictionary<Vector3Int, Edge<E>> edgesByPosition, EdgePositionsProvider edgesPositionsProvider)
        {
            this.edgesByPosition = edgesByPosition;
            this.edgesPositionsProvider = edgesPositionsProvider;
        }

        /// <summary>
        /// returns all edges adjacent to the input edge coordinate which belong to the map
        /// </summary>        
        /// ![green = input edge , blue = result](Map_GetEdges_AdjacentToEdge_Combined.png)
        public List<Edge<E>> AdjacentToEdge(Vector3Int edge)
        {
            var edgePositions = edgesPositionsProvider.AdjacentEdges(edge);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges adjacent to the input edge which belong to the map
        /// </summary>        
        public List<Edge<E>> AdjacentToEdge(Edge edge)
        {
            var edgePositions = edgesPositionsProvider.AdjacentEdges(edge.Position);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges adjacent to the input corner coordinate which belong to the map
        /// </summary> 
        /// ![green = input corner , blue = result](Map_GetEdges_AdjacentToCorner_Combined.png)
        public List<Edge<E>> AdjacentToCorner(Vector3Int corner)
        {
            var edgePositions = edgesPositionsProvider.AdjacentToCorner(corner);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges adjacent to the input corner which belong to the map
        /// </summary>        
        public List<Edge<E>> AdjacentToCorner(Corner corner)
        {
            var edgePositions = edgesPositionsProvider.AdjacentToCorner(corner.Position);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges of the input tile coordinate
        /// </summary>    
        /// ![yellow = input tile , blue = result](GetEdges_OfTile.png)
        public List<Edge<E>> OfTile(Vector3Int tile)
        {
            var edgePositions = edgesPositionsProvider.OfTile(tile);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges of the input tile
        /// </summary>        
        public List<Edge<E>> OfTile(Tile tile)
        {
            var edgePositions = edgesPositionsProvider.OfTile(tile.Position);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges within range of the input edge coordinate which belong to the map
        /// </summary>   
        /// ![green = input edge, blue = result] (Map_GetEdges_WithinDistanceOfEdge_Combined.png)
        public List<Edge<E>> WithinDistanceOfEdge(Vector3Int centerEdge, int maxDistance, bool includeSelf)
        {
            var edgePositions = edgesPositionsProvider.WithinDistanceOfEdge(centerEdge, maxDistance, includeSelf);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges within range of the input edge which belong to the map
        /// </summary>        
        public List<Edge<E>> WithinDistanceOfEdge(Edge centerEdge, int maxDistance, bool includeSelf)
        {
            var edgePositions = edgesPositionsProvider.WithinDistanceOfEdge(centerEdge.Position, maxDistance, includeSelf);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges within range of the input corner coordinate which belong to the map
        /// </summary>
        /// ![green = input corner , blue = result](Map_GetEdges_WithinDistanceOfCorner_Combined.png)
        public List<Edge<E>> WithinDistanceOfCorner(Vector3Int centerCorner, int maxDistance)
        {
            var edgePositions = edgesPositionsProvider.WithinDistanceOfCorner(centerCorner, maxDistance);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges within range of the input corner which belong to the map
        /// </summary>        
        public List<Edge<E>> WithinDistanceOfCorner(Corner centerCorner, int maxDistance)
        {
            var edgePositions = edgesPositionsProvider.WithinDistanceOfCorner(centerCorner.Position, maxDistance);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all edges at the exact distance of the input edge coordinate which belong to the map
        /// </summary>  
        /// ![green = input corner , blue = result](Map_GetEdges_AtExactDistance_Combined.png)
        public List<Edge<E>> AtExactDistance(Vector3Int centerEdge, int distance)
        {
            var edgePositions = edgesPositionsProvider.AtExactDistance(centerEdge, distance);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns the shortest path of edges from origin to target corner
        /// </summary>
        /// ![green = origin corner , purple = target corner, blue = result](Map_GetEdges_PathBetweenCorners_Combined.png)
        public List<Edge<E>> PathBetweenCorners(Vector3Int originCorner, Vector3Int targetCorner, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            var edgePositions = edgesPositionsProvider.PathBetweenCorners(originCorner, targetCorner, horizontalNudgeFromOriginCenter);
            return GetEdgesFromCoordinates(edgePositions);
        }

        /// <summary>
        /// returns all the border edges of a set of tiles. 
        /// </summary>
        /// ![green = input tiles , blue = result](Map_GetEdges_TileBorders.png)
        public List<Edge<E>> TileBorders(IEnumerable<Vector3Int> tiles)
        {
            var edges = edgesPositionsProvider.TileBorders(tiles);
            return GetEdgesFromCoordinates(edges);
        }

        /// <summary>
        /// returns all the border edges of a set of tiles. 
        /// </summary>
        public List<Edge<E>> TileBorders(IEnumerable<Tile> tiles)
        {
            var edges = edgesPositionsProvider.TileBorders(tiles);
            return GetEdgesFromCoordinates(edges);
        }

        /// <summary>
        /// returns the edges belonging to the input edge coordinates
        /// </summary>
        private List<Edge<E>> GetEdgesFromCoordinates<Coords>(Coords coordinates) where Coords : ICollection<Vector3Int>
        {
            List<Edge<E>> edges = new List<Edge<E>>();
            foreach (Vector3Int position in coordinates)
            {
                edges.Add(edgesByPosition[position]);
            }
            return edges;
        }
    }
}