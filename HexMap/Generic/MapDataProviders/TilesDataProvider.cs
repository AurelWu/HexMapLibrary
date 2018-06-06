using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    public class TilesDataProvider<T> where T : new()
    {
        private Dictionary<Vector3Int, Tile<T>> tilesByPosition;
        private TilePositionsProvider tilesPositionsProvider;

        public TilesDataProvider(Dictionary<Vector3Int, Tile<T>> tilesByPosition, TilePositionsProvider tilesPositionsProvider)
        {
            this.tilesByPosition = tilesByPosition;
            this.tilesPositionsProvider = tilesPositionsProvider;
        }

        /// <summary>
        /// returns the map tiles adjacent to the input tile
        /// </summary>   
        /// ![yellow = input tile , green = result](Map_GetTiles_AdjacentToTile_Combined.png)
        public List<Tile<T>> AdjacentToTile(Tile tile)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjacentToTile(tile.Position);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns the map tiles adjacent to the input tile
        /// </summary>   
        public List<Tile<T>> AdjacentToTile(Vector3Int tilePosition)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjacentToTile(tilePosition);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns the map tiles adjacent to the input edge
        /// </summary>   
        /// ![blue = input edge , green = result](Map_GetTiles_AdjacentToEdge_Combined.png)
        public List<Tile<T>> AdjacentToEdge(Vector3Int edge)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjacentToEdge(edge);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns the map tiles adjacent to the input edge
        /// </summary>   
        public List<Tile<T>> AdjacentToEdge(Edge edge)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjacentToEdge(edge.Position);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns the map tiles adjacent to the input corner
        /// </summary>  
        /// ![blue = input corner , green = result](Map_GetTiles_AdjacentToCorner_Combined.png)
        public List<Tile<T>> AdjacentToCorner(Vector3Int corner)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjcacentToCorner(corner);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns the map tiles adjacent to the input corner
        /// </summary>   
        public List<Tile<T>> AdjacentToCorner(Corner corner)
        {
            List<Vector3Int> positions = tilesPositionsProvider.AdjcacentToCorner(corner.Position);
            return GetTilesFromCoordinates(positions);
        }


        /// <summary>
        /// returns all map tiles of a cone starting at a point with given direction, length and width
        /// </summary>
        /// ![yellow = origin , blue = direction, green = result](Map_GetTiles_Cone_Combined.png)
        public List<Tile<T>> Cone(Tile origin, Vector3Int targetDirection, float coneHalfAngle, int coneLength)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Cone(origin.Position,targetDirection,coneHalfAngle, coneLength);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns all map tiles of a cone starting at a point with given direction, length and width
        /// </summary>
        public List<Tile<T>> Cone(Vector3Int origin, Vector3Int targetDirection, float coneHalfAngle, int coneLength)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Cone(origin, targetDirection, coneHalfAngle, coneLength);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns tiles forming a line between origin and target tile, optionally including the origin tile itself
        /// </summary>
        /// ![yellow = origin , blue = target, yellow/blue = line, when includeOrigin = true the origin belongs is part of the line](Map_GetTiles_Lines_Combined.png)
        public List<Tile<T>> Line(Tile origin, Vector3Int target, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Line(origin.Position, target, includeOrigin, horizontalNudgeFromOriginCenter);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns tiles forming a line between origin and target tile, optionally including the origin tile itself
        /// </summary>
        public List<Tile<T>> Line(Vector3Int origin, Vector3Int target, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Line(origin, target, includeOrigin, horizontalNudgeFromOriginCenter);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns all map tiles of a ring around center in no defined order
        /// </summary>
        /// ![yellow = origin , green = ring tiles](Map_GetTiles_Ring_Combined.png)
        public List<Tile<T>> Ring(Tile center, int radius, int thicknessInwards)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Ring(center.Position, radius, thicknessInwards);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// returns all map tiles of a ring around center in no defined order
        /// </summary>
        public List<Tile<T>> Ring(Vector3Int center, int radius, int thicknessInwards)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Ring(center, radius, thicknessInwards);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        ///  returns all map tiles of a ring around center in order specified by parameters (startDirection, clockwise)
        /// </summary>
        public List<Tile<T>> Ring(Tile center, int radius, TileDirection startDirection, bool clockwise)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Ring(center.Position, radius, startDirection, clockwise);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        ///  returns all map tiles of a ring around center in order specified by parameters (startDirection, clockwise)
        /// </summary>
        public List<Tile<T>> Ring(Vector3Int center, int radius, TileDirection startDirection, bool clockwise)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Ring(center, radius, startDirection, clockwise);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// Returns all tiles of the map which are within distance of the center point, either with or without the center point
        /// </summary>
        /// ![yellow = input , green = result](Map_GetTiles_Disc_Combined.png)
        public List<Tile<T>> Disc(Tile center, int range, bool includeCenter)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Disc(center.Position, range, includeCenter);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// Returns all tiles of the map which are within distance of the center point, either with or without the center point
        /// </summary>
        public List<Tile<T>> Disc(Vector3Int center, int range, bool includeCenter)
        {
            List<Vector3Int> positions = tilesPositionsProvider.Disc(center, range, includeCenter);
            return GetTilesFromCoordinates(positions);
        }

        /// <summary>
        /// splits the input collection into 1 List for each contiguous area formed by the input tiles
        /// </summary>
        /// ![left = input tile list , right = output separated into different areas](Map_GetTiles_ContiguousCombined.png)
        public List<List<Tile<T>>> ContiguousAreasOfInputTiles(ICollection<Tile> inputTiles)
        {
            List<List<Tile<T>>> areas = new List<List<Tile<T>>>();
            List<Vector3Int> tiles = new List<Vector3Int>();
            foreach(var tile in inputTiles)
            {
                tiles.Add(tile.Position);
            }
            var areasPositions = tilesPositionsProvider.ContiguousAreasOfInputTiles(tiles);
            foreach (var area in areasPositions)
            {
                areas.Add(GetTilesFromCoordinates(area));
            }
            return areas;
        }

        /// <summary>
        /// splits the input collection into 1 List for each contiguous area formed by the input tile coordinates.
        /// </summary>
        /// ![left = input tile list , right = output separated into different areas](Map_GetTiles_ContiguousCombined.png)
        public List<List<Tile<T>>> ContiguousAreasOfInputTiles(ICollection<Vector3Int> inputTiles)
        {
            List<List<Tile<T>>> areas = new List<List<Tile<T>>>();
            var areasPositions = tilesPositionsProvider.ContiguousAreasOfInputTiles(inputTiles);
            foreach(var area in areasPositions)
            {
                areas.Add(GetTilesFromCoordinates(area));
            }
            return areas;            
        }

        /// <summary>
        /// Returns the tiles belonging to the input coordinates
        /// </summary>
        private List<Tile<T>> GetTilesFromCoordinates(ICollection<Vector3Int> coordinates)
        { 
            List<Tile<T>> tiles = new List<Tile<T>>();
            foreach (Vector3Int position in coordinates)
            {
                tiles.Add(tilesByPosition[position]);
            }
            return tiles;
        }


    }
}