using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{
    public class CornersPositionsProvider
    {
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly Dictionary<Vector3Int, int> CornerIndexByPosition;

        public CornersPositionsProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int, int> CornerIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.CornerIndexByPosition = CornerIndexByPosition;
        }

        /// <summary>
        /// returns the 6 corners of the input tile
        /// </summary>
        /// ![yellow = input tile , blue = result](Map_GetCorners_OfTile.png)
        public List<Vector3Int> OfTile(Vector3Int tile)
        {
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.OfTile(tile);
            if (coordinateWrapper != null) cornerCoordinates = coordinateWrapper.WrapCornerCoordinates(cornerCoordinates);
            return cornerCoordinates;
        }

        /// <summary>
        /// returns the 2 corners adjacent to the input edge
        /// </summary>
        /// ![green = input edge , blue = result](Map_GetCorners_AdjacentToEdge.png)
        public List<Vector3Int> OfEdge(Vector3Int edge)
        {
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.OfEdge(edge);
            if (coordinateWrapper != null) cornerCoordinates = coordinateWrapper.WrapCornerCoordinates(cornerCoordinates);
            return cornerCoordinates;
        }

        /// <summary>
        /// returns the 3 corners adjacent to the input corner which belong to the map
        /// </summary>   
        /// ![green = input corner , blue = result](Map_GetCorners_AdjacentToCorner_Combined.png)
        public List<Vector3Int> AdjacentToCorner(Vector3Int corner)
        {
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.AdjacentToCorner(corner);
            return GetValidCornerCoordinates(cornerCoordinates);
        }

        /// <summary>
        /// returns all corners within distance of the input corner - optionally including that corner.
        /// </summary>  
        /// ![green = input corner , blue = result](Map_GetCorners_WithinDistance.png)
        public List<Vector3Int> WithinDistance(Vector3Int centerCorner, int maxDistance, bool includeSelf)
        {
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.WithinDistance(centerCorner, maxDistance, includeSelf);
            return GetValidCornerCoordinates(cornerCoordinates);
        }

        /// <summary>
        /// returns all corners at the exact distance of the input corner.
        /// </summary>
        /// ![green = input corner , blue = result](Map_GetCorners_AtExactDistance.png) 
        public List<Vector3Int> AtExactDistance(Vector3Int centerCorner, int distance)
        {
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.AtExactDistance(centerCorner, distance);
            return GetValidCornerCoordinates(cornerCoordinates);
        }

        /// <summary>
        /// returns the shortest path of corners from the origin to the target corner - optionally including the origin
        /// </summary> 
        /// ![green = origin , purple = target, blue/purple = result - origin can optionally be included](Map_GetCorners_PathAlongGrid.png)
        public List<Vector3Int> PathAlongGrid(Vector3Int originCorner, Vector3Int targetCorner, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            if (coordinateWrapper != null) targetCorner = coordinateWrapper.ShiftTargetToClosestPeriodicCornerPosition(originCorner, targetCorner);
            List<Vector3Int> cornerCoordinates = HexGrid.GetCorners.PathAlongGrid(originCorner, targetCorner, includeOrigin, horizontalNudgeFromOriginCenter);
            if (coordinateWrapper != null) cornerCoordinates = coordinateWrapper.WrapCornerCoordinates(cornerCoordinates);
            return cornerCoordinates;

        }

        /// <summary>
        /// returns all corners of the input tiles which are adjacent to 1 or 2 tiles not belonging to the input set.
        /// </summary>       
        /// ![green = input tiles , blue = result](Map_GetCorners_TileBorders.png)
        public List<Vector3Int> TileBorders(IEnumerable<Vector3Int> tiles)
        {
            List<Vector3Int> corners = HexGrid.GetCorners.TileBorders(tiles);
            if (coordinateWrapper != null) corners = coordinateWrapper.WrapCornerCoordinates(corners);
            return corners;
        }

        /// <summary>
        /// Wraps the input corner positions (if the map is periodic) and removes all those which are still out of the map bounds after that
        /// </summary>        
        protected List<Vector3Int> GetValidCornerCoordinates(List<Vector3Int> rawPositions)
        {
            List<Vector3Int> positions = rawPositions;
            if (coordinateWrapper != null) positions = coordinateWrapper.WrapCornerCoordinates(positions);
            positions = HexUtility.RemoveInvalidCoordinates(positions, CornerIndexByPosition);
            return positions;
        }
    }
}