using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{    
    public class TilePositionsProvider
    {
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly Dictionary<Vector3Int, int> TileIndexByPosition;

        public TilePositionsProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int, int> TileIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.TileIndexByPosition = TileIndexByPosition;
        }

        /// <summary>
        /// returns the map tiles adjacent to the input corner
        /// </summary>        
        /// ![blue = input corner , green = result](Map_GetTiles_AdjacentToCorner_Combined.png)
        public List<Vector3Int> AdjcacentToCorner(Vector3Int corner)
        {            
            List<Vector3Int> tiles = HexGrid.GetTiles.AdjacentToCorner(corner);
            tiles = GetValidTileCoordinates(tiles);
            return tiles;
        }

        /// <summary>
        /// returns the map tiles adjacent to the input edge
        /// </summary>  
        /// ![blue = input edge , green = result](Map_GetTiles_AdjacentToEdge_Combined.png)
        public List<Vector3Int> AdjacentToEdge(Vector3Int edge)
        {
            List<Vector3Int> tiles = HexGrid.GetTiles.AdjacentToEdge(edge);
            tiles = GetValidTileCoordinates(tiles);
            return tiles;
        }

        /// <summary>
        /// returns the map tiles directly neigbouring the input tile
        /// </summary>      
        /// ![yellow = input tile , green = result](Map_GetTiles_AdjacentToTile_Combined.png)
        public List<Vector3Int> AdjacentToTile(Vector3Int center)
        {
            List<Vector3Int> neighbours = HexGrid.GetTiles.AdjacentToTile(center);
            neighbours = GetValidTileCoordinates(neighbours);
            return neighbours;
        }

        /// <summary>
        /// returns all map tiles of a cone starting at a point with given direction, length and width
        /// </summary>
        /// ![yellow = origin , blue = direction, green = result](Map_GetTiles_Cone_Combined.png)
        public List<Vector3Int> Cone(Vector3Int origin, Vector3Int targetDirection, float coneHalfAngle, int coneLength)
        {
            List<Vector3Int> cone = HexGrid.GetTiles.Cone(origin, targetDirection, coneHalfAngle, coneLength);
            cone = GetValidTileCoordinates(cone);
            return cone;
        }



        /// <summary>
        /// returns all map tiles of a ring around center in no defined order
        /// </summary>
        /// ![yellow = origin , green = ring tiles](Map_GetTiles_Ring_Combined.png)
        public List<Vector3Int> Ring(Vector3Int center, int radius, int thicknessInwards)
        {
            List<Vector3Int> ring = HexGrid.GetTiles.Ring(center, radius, thicknessInwards);
            ring = GetValidTileCoordinates(ring);
            return ring;
        }

        /// <summary>
        ///  returns all map tiles of a ring around center in order specified by parameters (startDirection, clockwise)
        /// </summary>
        public List<Vector3Int> Ring(Vector3Int center, int radius, TileDirection startDirection, bool clockwise)
        {
            List<Vector3Int> positions = HexGrid.GetTiles.Ring(center, radius, startDirection, clockwise);
            positions = GetValidTileCoordinates(positions);
            return positions;
        }

        /// <summary>
        /// returns tiles forming a line between origin and target tile, optionally including the origin tile itself
        /// </summary>        
        /// ![yellow = origin , blue = target, yellow/blue = line, when includeOrigin = true the origin belongs is part of the line](Map_GetTiles_Lines_Combined.png)
        public List<Vector3Int> Line(Vector3Int origin, Vector3Int target, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
        {
            if (coordinateWrapper != null) target = coordinateWrapper.ShiftTargetToClosestPeriodicTilePosition(origin, target);
            List<Vector3Int> lineTiles = HexGrid.GetTiles.Line(origin, target, includeOrigin, horizontalNudgeFromOriginCenter);
            lineTiles = GetValidTileCoordinates(lineTiles);
            return lineTiles;
        }

        /// <summary>
        /// Returns all tiles of the map which are within distance of the center point, either with or without the center point
        /// </summary>
        /// ![yellow = input , green = result](Map_GetTiles_Disc_Combined.png)
        public List<Vector3Int> Disc(Vector3Int center, int range, bool includeCenter)
        {
            List<Vector3Int> positions = HexGrid.GetTiles.Disc(center, range, includeCenter);
            positions = GetValidTileCoordinates(positions);
            return positions;
        }

        /// <summary>
        /// splits the input collection into separated Lists for each contiguous area formed by the input tile coordinates.
        /// </summary>
        /// ![left = input tile list , right = output separated into different areas](Map_GetTiles_ContiguousCombined.png)
        public List<List<Vector3Int>> ContiguousAreasOfInputTiles(ICollection<Vector3Int> inputTiles)
        {
            if (inputTiles.Count == 0) return new List<List<Vector3Int>>();

            List<List<Vector3Int>> areas = new List<List<Vector3Int>>();
            HashSet<Vector3Int> unusedTiles = new HashSet<Vector3Int>(inputTiles);

            while (unusedTiles.Count > 0)
            {
                HashSet<Vector3Int> area = new HashSet<Vector3Int>();
                Queue<Vector3Int> queue = new Queue<Vector3Int>();
                queue.Enqueue(unusedTiles.First());
                while (queue.Count > 0)
                {
                    Vector3Int current = queue.Dequeue();
                    area.Add(current);
                    unusedTiles.Remove(current);
                    List<Vector3Int> neighbours = AdjacentToTile(current); //only that line is different from static version, should refactor at some point
                    neighbours.RemoveAll(x => !unusedTiles.Contains(x));
                    foreach (var tile in neighbours)
                    {
                        queue.Enqueue(tile);
                    }
                }
                areas.Add(area.ToList());
            }
            return areas;
        }

        /// <summary>
        /// Returns the subset of coordinates of the input which are a valid part of the map, accounting for map wrap around.
        /// </summary>
        protected List<Vector3Int> GetValidTileCoordinates(List<Vector3Int> rawPositions)
        {
            List<Vector3Int> positions = rawPositions;
            if (coordinateWrapper != null) positions = coordinateWrapper.WrapTileCoordinates(positions);
            positions = HexUtility.RemoveInvalidCoordinates(positions, TileIndexByPosition);
            return positions;
        }
    }

}