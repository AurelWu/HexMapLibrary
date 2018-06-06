using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

//using GetTiles = Wunderwunsch.HexMapLibrary.ITile;

namespace Wunderwunsch.HexMapLibrary 
{    
    public class HexMap
    {
        /// <summary>
        /// Dictionary of the indices of tiles by their position
        /// </summary>
        public Dictionary<Vector3Int, int> TileIndexByPosition { get; protected set; }

        /// <summary>
        /// Array of all tile positions ordered by their index;
        /// </summary>
        public Vector3Int[] TilePositions { get; protected set; }
        /// <summary>
        /// Total amount of tiles the map contains. Readonly
        /// </summary>
        public int TileCount { get { return TileIndexByPosition.Count; } }

        /// <summary>
        /// contains methods to retrieve exactly one TilePosition
        /// </summary>
        public TilePositionProvider GetTilePosition { get; protected set; }

        /// <summary>
        /// contains methods to retrieve multiple Tiles
        /// </summary>
        public TilePositionsProvider GetTilePositions { get; protected set; }

        public MapDistanceCalculatorTile GetTileDistance { get; protected set; }

        /// <summary>
        /// Dictionary of the indices of edges by their position
        /// </summary>
        public Dictionary<Vector3Int, int> EdgeIndexByPosition { get; protected set; }

        /// <summary>
        /// Array of all edge positions ordered by their index;
        /// </summary>
        public Vector3Int[] EdgePositions { get; protected set; }

        /// <summary>
        /// Total amount of edges the map contains. Readonly
        /// </summary>
        public int EdgeCount { get { return EdgeIndexByPosition.Count; } }

        /// <summary>
        /// contains methods to retrieve exactly one EdgePosition
        /// </summary>
        public EdgePositionProvider GetEdgePosition { get; protected set; }

        /// <summary>
        /// contains methods to retrieve multiple Edges
        /// </summary>
        public EdgePositionsProvider GetEdgePositions { get; protected set; }

        public MapDistanceCalculatorEdges GetEdgeDistance { get; protected set; }

        /// <summary>
        /// Dictionary of the indices of edges by their position
        /// </summary>
        public Dictionary<Vector3Int, int> CornerIndexByPosition { get; protected set; }

        /// <summary>
        /// Array of all corner positions ordered by their index;
        /// </summary>
        public Vector3Int[] CornerPositions { get; protected set; }

        /// <summary>
        /// Total amount of edges the map contains. Readonly
        /// </summary>
        public int CornerCount { get { return CornerIndexByPosition.Count; } }

        /// <summary>
        /// contains methods to retrieve exactly one CornerPosition
        /// </summary>
        public CornerPositionProvider GetCornerPosition { get; protected set; }

        /// <summary>
        /// contains methods to retrieve multiple Corners
        /// </summary>
        public CornersPositionsProvider GetCornerPositions { get; protected set; }

        public MapDistanceCalculatorCorners GetCornerDistance { get; protected set; }

        public MapSizeData MapSizeData { get; protected set; }

        public CoordinateWrapper CoordinateWrapper { get; protected set; }

        public HexMap(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null) 
        {
            this.CoordinateWrapper = coordinateWrapper;
            this.TileIndexByPosition = tileIndexByPosition;
            TilePositions = new Vector3Int[TileCount];
            foreach(var kvp in TileIndexByPosition)
            {
                TilePositions[kvp.Value] = kvp.Key;
            }
            this.GetTilePosition = new TilePositionProvider(coordinateWrapper, tileIndexByPosition);
            this.GetTilePositions = new TilePositionsProvider(coordinateWrapper, tileIndexByPosition);

            MapSizeData = HexUtility.CalculateMapCenterAndExtents(TileIndexByPosition.Keys);


            CreateEdgeIndex();
            this.GetEdgePosition = new EdgePositionProvider(CoordinateWrapper, EdgeIndexByPosition);
            this.GetEdgePositions = new EdgePositionsProvider(CoordinateWrapper, EdgeIndexByPosition);

            CreateCornerIndex();
            this.GetCornerPosition = new CornerPositionProvider(CoordinateWrapper, CornerIndexByPosition);
            this.GetCornerPositions = new CornersPositionsProvider(CoordinateWrapper, CornerIndexByPosition);
        }

        private void CreateEdgeIndex()
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            EdgeIndexByPosition = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);            
            int edgeIndex = 0;
            foreach (var kvp in TileIndexByPosition)
            {
                List<Vector3Int> edges = HexGrid.GetEdges.OfTile(kvp.Key);
                foreach (Vector3Int edge in edges)
                {
                    Vector3Int edgeToAdd = edge;
                    if (CoordinateWrapper != null) edgeToAdd = CoordinateWrapper.WrapEdgeCoordinate(edgeToAdd);
                    if (!EdgeIndexByPosition.ContainsKey(edgeToAdd))
                    {
                        EdgeIndexByPosition.Add(edgeToAdd, edgeIndex);
                        edgeIndex++;
                    }

                }
            }

            EdgePositions = new Vector3Int[EdgeIndexByPosition.Count];
            foreach(var kvp in EdgeIndexByPosition)
            {
                EdgePositions[kvp.Value] = kvp.Key;
            }
        }

        private void CreateCornerIndex()
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            CornerIndexByPosition = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);
            CornerPositions = new Vector3Int[CornerIndexByPosition.Count];
            int cornerIndex = 0;
            foreach (var kvp in TileIndexByPosition)
            {
                List<Vector3Int> corners = HexGrid.GetCorners.OfTile(kvp.Key);
                foreach (Vector3Int corner in corners)
                {
                    Vector3Int cornerToAdd = corner;
                    if (CoordinateWrapper != null) cornerToAdd = CoordinateWrapper.WrapCornerCoordinate(cornerToAdd);
                    if (!CornerIndexByPosition.ContainsKey(cornerToAdd))
                    {
                        CornerIndexByPosition.Add(cornerToAdd, cornerIndex);
                        cornerIndex++;
                    }
                }
            }

            CornerPositions = new Vector3Int[CornerIndexByPosition.Count];
            foreach (var kvp in CornerIndexByPosition)
            {
                CornerPositions[kvp.Value] = kvp.Key;
            }
        }
    }
}