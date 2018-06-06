using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Contains methods to create differently shaped map layouts. Can be used to easily initialize a HexMap instance
    /// </summary>
    /// <remarks>
    /// TODO add detailed description here
    /// </remarks>
    public static class HexMapBuilder
    {
        /// <summary>
        /// returns indexed positions which form a flat-top hexagonal shaped map with the specified radius
        /// </summary>
        /// <remarks>
        /// ![Hexagonal map with radius 4](HexagonalMap.png)
        /// </remarks>
        public static Dictionary<Vector3Int, int> CreateHexagonalShapedMap(int radius)
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            Dictionary<Vector3Int, int> TileToIndexMap = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);
            List<Vector3Int> tiles = HexGrid.GetTiles.Disc(Vector3Int.zero, radius, true);
            int idx = 0;
            foreach (var tile in tiles)
            {
                TileToIndexMap.Add(tile, idx);
                idx++;
            }
            return TileToIndexMap;
        }

        /// <summary>
        /// returns indexed positions which form a rectangular shaped map with the specified width(x) and length(y).
        /// Most common map shape for periodic maps        
        /// </summary>
        /// <remarks>
        /// ![Rectangular map. Mapsize: x = 9 and y = 9](RectangularMap.png)
        /// </remarks>
        public static Dictionary<Vector3Int, int> CreateRectangularShapedMap(Vector2Int mapSize)
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            Dictionary<Vector3Int, int> TileToIndexMap = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);
            int idx = 0;
            for (int y = 0; y < mapSize.y; y++)
            {
                for (int x = 0; x < mapSize.x; x++)
                {
                    Vector3Int cubeTileCoord = HexConverter.OffsetTileCoordToTileCoord(new Vector2Int(x, y));
                    TileToIndexMap.Add(cubeTileCoord, idx);
                    idx++;
                }
            }
            return TileToIndexMap;
        }

        /// <summary>
        /// returns indexed positions which form a rectangular shaped map with the specified width(x) and length(y). Odd-numbered rows are one tile shorter
        /// Best used for non-periodic maps where symmetry is important
        /// </summary>
        /// <remarks>
        /// ![Rectangular map with odd rows being one tile shorter. Mapsize: x = 9 and y = 9](RectangularMapOddRow1Shorter.png)
        /// </remarks>
        public static Dictionary<Vector3Int, int> CreateRectangularShapedMapOddRowsOneShorter(Vector2Int mapSize)
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            Dictionary<Vector3Int, int> TileToIndexMap = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);

            int idx = 0;
            for (int y = 0; y < mapSize.y; y++)
            {
                int oddRowShorter = y % 2;
                for (int x = 0; x < mapSize.x - oddRowShorter; x++)
                {
                    Vector3Int cubeTileCoord = HexConverter.OffsetTileCoordToTileCoord(new Vector2Int(x, y));
                    TileToIndexMap.Add(cubeTileCoord, idx);
                    idx++;
                }
            }
            return TileToIndexMap;
        }

        /// <summary>
        /// returns indexed positions which form a equilateral triangular map with the specified side length
        /// </summary>
        /// <remarks>
        /// ![Triangular map with with sidelength of 9](TriangularMap.png)
        /// </remarks>
        public static Dictionary<Vector3Int, int> CreateTriangularShapedMap(int sideLength)
        {
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            Dictionary<Vector3Int, int> TileToIndexMap = new Dictionary<Vector3Int, int>(vector3IntEqualityComparer);

            int idx = 0;
            for (int y = 0; y < sideLength; y++)
            {
                for (int x = y / 2; x < (sideLength - (y + 1) / 2); x++)
                {
                    Vector3Int cubeTileCoord = HexConverter.OffsetTileCoordToTileCoord(new Vector2Int(x, y));
                    TileToIndexMap.Add(cubeTileCoord, idx);
                    idx++;
                }
            }
            return TileToIndexMap;
        }
    }
}
