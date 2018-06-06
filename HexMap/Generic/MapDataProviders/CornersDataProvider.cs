using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class CornersDataProvider<C> where C : new()
    {
        private Dictionary<Vector3Int, Corner<C>> cornersByPosition;
        private CornersPositionsProvider cornersPositionsProvider;

        public CornersDataProvider(Dictionary<Vector3Int, Corner<C>> cornersByPosition, CornersPositionsProvider cornersPositionsProvider)
        {
            this.cornersByPosition = cornersByPosition;
            this.cornersPositionsProvider = cornersPositionsProvider;
        }

        /// <summary>
        /// returns the 6 corners of the input tile
        /// </summary>
        /// ![yellow = input tile , blue = result](Map_GetCorners_OfTile.png)
        public List<Corner<C>> OfTile(Vector3Int tile)
        {
            List<Vector3Int> positions = cornersPositionsProvider.OfTile(tile);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the 6 corners of the input tile
        /// </summary>
        public List<Corner<C>> OfTile(Tile tile)
        {
            List<Vector3Int> positions = cornersPositionsProvider.OfTile(tile.Position);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the 2 corners adjacent to the input edge coordinate
        /// </summary>
        /// ![green = input edge , blue = result](Map_GetCorners_AdjacentToEdge.png)
        public List<Corner<C>> OfEdge(Vector3Int edge)
        {
            List<Vector3Int> positions = cornersPositionsProvider.OfEdge(edge);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the 2 corners adjacent to the input edge
        /// </summary>
        public List<Corner<C>> OfEdge(Edge edge)
        {
            List<Vector3Int> positions = cornersPositionsProvider.OfEdge(edge.Position);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the corners adjacent to the input corner which belong to the map
        /// </summary>   
        /// ![green = input corner , blue = result](Map_GetCorners_AdjacentToCorner_Combined.png)
        public List<Corner<C>> AdjacentToCorner(Vector3Int corner)
        {
            List<Vector3Int> positions = cornersPositionsProvider.AdjacentToCorner(corner);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the 3 corners adjacent to the input corner which belong to the map
        /// </summary>   
        public List<Corner<C>> AdjacentToCorner(Corner corner)
        {
            List<Vector3Int> positions = cornersPositionsProvider.AdjacentToCorner(corner.Position);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns all corners within distance of the input corner coordinate - optionally including that corner.
        /// </summary>  
        /// ![green = input corner , blue = result](Map_GetCorners_WithinDistance.png)
        public List<Corner<C>> WithinDistance(Vector3Int centerCorner, int maxDistance, bool includeCenter)
        {
            List<Vector3Int> positions = cornersPositionsProvider.WithinDistance(centerCorner,maxDistance,includeCenter);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns all corners within distance of the input corner - optionally including that corner.
        /// </summary>  
        public List<Corner<C>> WithinDistance(Corner<C> centerCorner, int maxDistance, bool includeCenter)
        {
            List<Vector3Int> positions = cornersPositionsProvider.WithinDistance(centerCorner.Position, maxDistance, includeCenter);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns all corners at the exact distance of the input corner coordinate.
        /// </summary>
        /// ![green = input corner , blue = result](Map_GetCorners_AtExactDistance.png) 
        public List<Corner<C>> AtExactDistance(Vector3Int centerCorner, int distance)
        {
            List<Vector3Int> positions = cornersPositionsProvider.AtExactDistance(centerCorner, distance);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns all corners at the exact distance of the input corner.
        /// </summary>
        public List<Corner<C>> AtExactDistance(Corner<C> centerCorner, int distance)
        {
            List<Vector3Int> positions = cornersPositionsProvider.AtExactDistance(centerCorner.Position, distance);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the shortest path of corners from the origin to the target corner - optionally including the origin
        /// </summary> 
        /// ![green = origin , purple = target, blue/purple = result - origin can optionally be included](Map_GetCorners_PathAlongGrid.png)
        public List<Corner<C>> PathAlongGrid(Tile<C> originCorner, Tile<C> targetCorner, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            List<Vector3Int> positions = cornersPositionsProvider.PathAlongGrid(originCorner.Position, targetCorner.Position, includeOrigin, horizontalNudgeFromOriginCenter);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the shortest path of corners from the origin coordinate to the target corner coordinate - optionally including the origin
        /// </summary> 
        public List<Corner<C>> PathAlongGrid(Vector3Int originCorner, Vector3Int targetCorner, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            List<Vector3Int> positions = cornersPositionsProvider.PathAlongGrid(originCorner, targetCorner, includeOrigin, horizontalNudgeFromOriginCenter);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns all the border corners of a set of tiles. 
        /// </summary>
        ///  ![green = input tiles , blue = result](Map_GetCorners_TileBorders.png)
        public List<Corner<C>> TileBorders(IEnumerable<Vector3Int> tiles)
        {
            List<Vector3Int> positions = cornersPositionsProvider.TileBorders(tiles);
            return GetCornersFromCoordinates(positions);
        }

        /// <summary>
        /// returns the corners belonging to the input corner coordinates
        /// </summary>
        public List<Corner<C>> GetCornersFromCoordinates<Coords>(Coords coordinates) where Coords : ICollection<Vector3Int>
        {
            List<Corner<C>> tiles = new List<Corner<C>>();
            foreach (Vector3Int position in coordinates)
            {
                tiles.Add(cornersByPosition[position]);
            }
            return tiles;
        }
    }
}