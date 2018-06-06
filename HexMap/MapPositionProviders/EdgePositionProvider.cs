using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public class EdgePositionProvider
    {
        protected readonly MapDistanceCalculatorTile GetTileDistance;
        protected readonly MapDistanceCalculatorCorners GetCornerDistance;
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly Dictionary<Vector3Int, int> edgeIndexByPosition;

        public EdgePositionProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int, int> EdgeIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.edgeIndexByPosition = EdgeIndexByPosition;
            GetTileDistance = new MapDistanceCalculatorTile(coordinateWrapper);
            GetCornerDistance = new MapDistanceCalculatorCorners(coordinateWrapper);
        }

        /// <summary>
        /// returns the periodic edge coordinate of the input cartesian coordinate
        /// </summary>        
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToClosestEdgeCoord(cartesianCoordinate);
            return coord;
        }

        /// <summary>
        /// returns the periodic edge coordinate of the input cartesian coordinate, the out parameter specifies if that coordinate belongs to the map
        /// </summary>       
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate, out bool edgeIsOnMap)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToClosestEdgeCoord(cartesianCoordinate);
            edgeIsOnMap = false;
            if (edgeIndexByPosition.ContainsKey(coord)) edgeIsOnMap = true;
            return coord;
        }

        /// <summary>
        /// returns the edge coordinate between the input corners
        /// </summary>
        public Vector3Int BetweenNeighbouringCorners(Vector3Int cornerA, Vector3Int cornerB)
        {
            if (GetCornerDistance.Grid(cornerA, cornerA) != 1) throw new System.ArgumentException("Corners don't have a distance of 1, therefore and not neighbours and share no Edge");
            Vector3Int cornerCoordinate = new Vector3Int((cornerA.x + cornerB.x) / 3, (cornerA.y + cornerB.y) / 3, (cornerA.z + cornerB.z) / 3);
            if (coordinateWrapper != null) cornerCoordinate = coordinateWrapper.WrapCornerCoordinate(cornerCoordinate);
            return cornerCoordinate;
        }
    
        /// <summary>
        /// returns the edge coordinate between the input tiles
        /// </summary>
        public Vector3Int BetweenNeighbouringTiles(Vector3Int tileA, Vector3Int tileB)
        {
            if (GetTileDistance.Grid(tileA, tileB) != 1) throw new System.ArgumentException("Tiles don't have a distance of 1, therefore and not neighbours and share no Edge");
            Vector3Int edgeCoordinate = tileA + tileB;
            if(coordinateWrapper!= null) edgeCoordinate = coordinateWrapper.WrapEdgeCoordinate(edgeCoordinate);
            return edgeCoordinate;            
        }
    }

}