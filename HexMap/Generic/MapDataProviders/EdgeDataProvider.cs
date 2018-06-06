using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class EdgeDataProvider<E> where E : new()
    {
        private Dictionary<Vector3Int, Edge<E>> edgesByPosition;
        private EdgePositionProvider edgePositionProvider;

        public EdgeDataProvider(Dictionary<Vector3Int, Edge<E>> edgesByPosition, EdgePositionProvider edgePositionProvider)
        {
            this.edgesByPosition = edgesByPosition;
            this.edgePositionProvider = edgePositionProvider;
        }

        /// <summary>
        /// returns the periodic edge of the input cartesian coordinate. returns null if outside of map bounds
        /// </summary>      
        public Edge<E> FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            Vector3Int coord = edgePositionProvider.FromCartesianCoordinate(cartesianCoordinate);
            if (!edgesByPosition.ContainsKey(coord)) return null;
            return edgesByPosition[coord];
        }

        /// <summary>
        /// returns the edge between the input corners
        /// </summary>
        public Edge<E> BetweenNeighbouringCorners(Vector3Int cornerA, Vector3Int cornerB)
        {
            Vector3Int coord = edgePositionProvider.BetweenNeighbouringCorners(cornerA,cornerB);
            if (!edgesByPosition.ContainsKey(coord)) return null;
            return edgesByPosition[coord];
        }

        /// <summary>
        /// returns the edge coordinate between the input tiles
        /// </summary>
        public Edge<E> BetweenNeighbouringTiles(Vector3Int tileA, Vector3Int tileB)
        {
            Vector3Int coord = edgePositionProvider.BetweenNeighbouringTiles(tileA, tileB);
            if (!edgesByPosition.ContainsKey(coord)) return null;
            return edgesByPosition[coord];
        }
    }
}