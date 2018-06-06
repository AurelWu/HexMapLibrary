using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{

    public class HexMap<T> : HexMap where T : new()
    {
        public TileDataProvider<T> GetTile { get; private set; }
        public TilesDataProvider<T> GetTiles { get; private set; }
        public Dictionary<Vector3Int, Tile<T>> TilesByPosition { get; private set; }
        public Tile<T>[] Tiles { get; private set; }

        public HexMap(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null) : base(tileIndexByPosition, coordinateWrapper)
        {
            CreateTileData();
            GetTile = new TileDataProvider<T>(TilesByPosition, base.GetTilePosition);
            GetTiles = new TilesDataProvider<T>(TilesByPosition, base.GetTilePositions);
        }

        public void CreateTileData()
        {
            Vector3 center = MapSizeData.center;
            Vector3 extents = MapSizeData.extents;
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();
            TilesByPosition = new Dictionary<Vector3Int, Tile<T>>(vector3IntEqualityComparer);
            Tiles = new Tile<T>[TileIndexByPosition.Count];
            float minX = center.x - extents.x;
            float maxX = center.x + extents.x;
            float minZ = center.z - extents.z;
            float maxZ = center.z + extents.z;

            foreach (var kvp in TileIndexByPosition)
            {                
                Vector2 normalizedPosition = HexConverter.TileCoordToNormalizedPosition(kvp.Key,minX,maxX,minZ,maxZ);
                Tile<T> tile = new Tile<T>(kvp.Key, kvp.Value, normalizedPosition);
                TilesByPosition.Add(kvp.Key, tile);
                Tiles[tile.Index] = tile;
            }
        }        
    }
}