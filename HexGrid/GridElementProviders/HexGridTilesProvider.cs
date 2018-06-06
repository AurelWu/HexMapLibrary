using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static Wunderwunsch.HexMapLibrary.HexConstants;

namespace Wunderwunsch.HexMapLibrary
{
    public static partial class HexGrid
    {
        public static class GetTiles
        {
            /// <summary>
            /// Returns all Tiles which are within distance of the center point, either with or without the center point 
            /// </summary>            
            /// ![yellow = origin , green = disc tiles, when includeCenter = true the origin is part of the disc](GetTiles_Disc.png)
            public static List<Vector3Int> Disc(Vector3Int center, int radius, bool includeCenter)
            {
                if (radius < 1)
                {
                    throw new System.ArgumentException("radius needs to be larger than 0");
                }
                List<Vector3Int> positions = new List<Vector3Int>();

                int minX = center.x - radius;
                int maxX = center.x + radius;
                int minY = center.y - radius;
                int maxY = center.y + radius;

                for (int x = minX; x <= maxX; x++)
                {
                    for (int y = minY; y <= maxY; y++)
                    {
                        int z = -x - y;
                        if (Mathf.Abs(z - center.z) > radius) continue;
                        positions.Add(new Vector3Int(x, y, z));
                    }
                }
                if (!includeCenter) positions.Remove(center);
                return positions;
            }

            /// <summary>
            /// returns all tiles of a ring around center in no defined order
            /// </summary>
            /// ![yellow = origin , green = ring tiles](GetTiles_Ring.png)
            public static List<Vector3Int> Ring(Vector3Int center, int radius, int ringThicknessInwards)
            {
                if (radius < 1 )
                {
                    throw new System.ArgumentException("radius needs to be larger than 0");
                }
                if(ringThicknessInwards < 1 )
                {
                    throw new System.ArgumentException("ring thickness must be larger than 0");
                }

                List<Vector3Int> ring = new List<Vector3Int>();
                List<Vector3Int> allInManhattanrange = Disc(center, radius, true);
                foreach (var v in allInManhattanrange)
                {
                    if (GetDistance.BetweenTiles(center, v) > radius - ringThicknessInwards) ring.Add(v);
                }

                return ring;
            }


            /// <summary>
            /// returns all tiles of a ring around center in order specified by parameters (startDirection, clockwise)
            /// </summary>
            public static List<Vector3Int> Ring(Vector3Int center, int radius, TileDirection startDirection = TileDirection.TopRight, bool clockwise = true)
            {
                if(radius < 1)
                {
                    throw new System.ArgumentException("radius needs to be larger than 0");
                }
                List<Vector3Int> ringTiles = new List<Vector3Int>();
                System.Func<Vector3Int, Vector3Int, Vector3Int> rotationMethod;
                if (clockwise == true) rotationMethod = GetTile.FromTileRotated60DegreeClockwise;
                else rotationMethod = GetTile.FromTileRotated60DegreeCounterClockwise;

                Vector3Int startOfRing = center + (TileDirectionVectors[(int)startDirection] * radius);
                ringTiles.Add(startOfRing);

                Vector3Int[] ringCornerTiles = new Vector3Int[6];
                ringCornerTiles[0] = startOfRing;
                for(int i = 1; i < 6; i++)
                {
                    ringCornerTiles[i] = rotationMethod(center, ringCornerTiles[i - 1]);
                }

                for(int i = 0; i < 6; i++)
                {
                    List<Vector3Int> tilesOfRingSegment =GetTiles.Line(ringCornerTiles[i], ringCornerTiles[(i + 1) % 6], false);
                    ringTiles.AddRange(tilesOfRingSegment);
                }
                ringTiles.RemoveAt(ringTiles.Count - 1);
                return ringTiles;
            }

            /// <summary>
            /// returns all tiles of a cone starting at a point with given direction, length and width
            /// </summary>    
            /// ![yellow = origin , blue = direction, green/blue = cone tiles](GetTiles_Cone.png)
            public static List<Vector3Int> Cone(Vector3Int origin, Vector3Int targetDirection, float coneWidthHalfAngle, int coneLength)
            {
                Vector3 originCartesian = HexConverter.TileCoordToCartesianCoord(origin);
                IEnumerable<Vector3Int> ring = GetTiles.Ring(origin, coneLength, 1);
                HashSet<Vector3Int> cone = new HashSet<Vector3Int>();
                foreach (Vector3Int target in ring)
                {
                    Vector3 targetWorldPos = HexConverter.TileCoordToCartesianCoord(target);
                    Vector3 lookWorldPos = HexConverter.TileCoordToCartesianCoord((origin + targetDirection));
                    float angle = Vector3.Angle(targetWorldPos - originCartesian, lookWorldPos - originCartesian);
                    if (Mathf.Abs(angle) > coneWidthHalfAngle + 0.001) continue;

                    List<Vector3Int> linePointsL = GetTiles.Line(origin, target, false, -0.00001f); //for more consistent results we use 2 lines, one slightly left of center
                    List<Vector3Int> linePointsR = GetTiles.Line(origin, target, false, +0.00001f); //and this her slightly right of center
                    cone.UnionWith(linePointsL);
                    cone.UnionWith(linePointsR);
                }
                return cone.ToList();

            }

            /// <summary>
            /// returns tiles forming a line between origin and target tile, optionally including the origin tile itself
            /// </summary>    
            /// TODO: explain nudge
            /// ![yellow = origin , blue = target, yellow/blue = line, when includeOrigin = true the origin belongs is part of the line](GetTiles_Line.png)
            public static List<Vector3Int> Line(Vector3Int origin, Vector3Int target, bool includeOrigin, float horizontalNudgeFromOriginCenter = NudgePositive)
            {
                if (origin == target)
                {
                    throw new System.ArgumentException("origin corner and target corner are the same - can't create a Path");
                }


                List<Vector3Int> lineCells = new List<Vector3Int>();
                if (includeOrigin) lineCells.Add(origin);

                int dist = GetDistance.BetweenTiles(origin, target);
                for (int i = 1; i <= dist; i++)
                {
                    Vector3 lerped = HexUtility.LerpCubeCoordinates(origin, target, horizontalNudgeFromOriginCenter, (1f / dist) * i);
                    Vector3Int cell = HexUtility.RoundCubeCoordinate(lerped);
                    lineCells.Add(cell);
                }
                return lineCells;
            }

            /// <summary>
            /// returns the tiles directly neigbouring the input tile
            /// </summary>    
            /// ![yellow = origin, green = adjacent tiles](GetTiles_AdjacentToTile.png)
            public static List<Vector3Int> AdjacentToTile(Vector3Int center)
            {
                List<Vector3Int> neighbours = new List<Vector3Int>
                {
                    center + TileDirectionVectors[0],//new Vector3Int(center.x, center.y + 1, center.z - 1), //top right 
                    center + TileDirectionVectors[1],//new Vector3Int(center.x + 1, center.y, center.z - 1), //right
                    center + TileDirectionVectors[2],//new Vector3Int(center.x + 1, center.y - 1, center.z), //bottom right
                    center + TileDirectionVectors[3],//new Vector3Int(center.x, center.y - 1, center.z + 1), //bottom left
                    center + TileDirectionVectors[4],//new Vector3Int(center.x - 1, center.y, center.z + 1), //left
                    center + TileDirectionVectors[5],//new Vector3Int(center.x - 1, center.y + 1, center.z)  //topleft
                };
                //using explicit values might be faster than using values
                return neighbours;
            }

            /// <summary>
            /// returns the tiles adjacent to the input edge
            /// </summary>    
            /// ![yellow = input edge, green = adjacent tiles](GetTiles_AdjacentToEdge.png)
            public static List<Vector3Int> AdjacentToEdge(Vector3Int edge)
            {
                int tileAx = 0;
                int tileAy = 0;
                int tileAz = 0;
                int tileBx = 0;
                int tileBy = 0;
                int tileBz = 0;

                if (edge.x % 2 == 0)
                {
                    tileAx = edge.x / 2;
                    tileBx = edge.x / 2;
                    tileAy = (edge.y - 1) / 2;
                    tileAz = (edge.z + 1) / 2;
                    tileBy = (edge.y + 1) / 2;
                    tileBz = (edge.z - 1) / 2;
                }

                else if (edge.y % 2 == 0)
                {
                    tileAy = edge.y / 2;
                    tileBy = edge.y / 2;
                    tileAx = (edge.x + 1) / 2;
                    tileAz = (edge.z - 1) / 2;
                    tileBx = (edge.x - 1) / 2;
                    tileBz = (edge.z + 1) / 2;
                }
                else
                {
                    tileAz = edge.z / 2;
                    tileBz = edge.z / 2;
                    tileAx = (edge.x - 1) / 2;
                    tileAy = (edge.y + 1) / 2;
                    tileBx = (edge.x + 1) / 2;
                    tileBy = (edge.y - 1) / 2;
                }

                List<Vector3Int> tiles = new List<Vector3Int>
            {
                new Vector3Int(tileAx, tileAy, tileAz),
                new Vector3Int(tileBx, tileBy, tileBz)
            };
                return tiles;

            }

            /// <summary>
            /// returns the tiles adjacent to the input corner
            /// </summary>   
            /// ![yellow = input corner , green = adjacent tiles](GetTiles_AdjacentToCorner.png)
            public static List<Vector3Int> AdjacentToCorner(Vector3Int corner)
            {
                //we have 2 different Configurations: either adjacent Tiles are TopRight,Bottom and TopLeft
                //or they are top, bottom right and bottom left
                //we just try one configuration first if we don't end up with a valid tile coordinate then it must be the other one

                Vector3Int a, b, c;  //coordinates of the 3 tiles we are going to return                      
                                     //first we try top, bottom right, bottom left
                                     //check if a is a tile coordinate,
                float a1 = (corner.x - 1) / 3f;
                float a2 = (corner.y + 2) / 3f;
                float a3 = (corner.z - 1) / 3f;
                //Debug.Log(a1 + " " + a2 + " " + a3 + " ");
                if (Mathf.Abs(a1 - (int)a1) < 0.01f && Mathf.Abs(a2 - (int)a2) < 0.01f && Mathf.Abs(a3 - (int)a3) < 0.01f)
                {
                    a = new Vector3Int((corner.x - 1) / 3, (corner.y + 2) / 3, (corner.z - 1) / 3);
                    b = new Vector3Int((corner.x + 2) / 3, (corner.y - 1) / 3, (corner.z - 1) / 3);
                    c = new Vector3Int((corner.x - 1) / 3, (corner.y - 1) / 3, (corner.z + 2) / 3);
                }

                //top right,bottom and topleft are the adjacent tiles of this corner 
                else
                {
                    a = new Vector3Int((corner.x + 1) / 3, (corner.y + 1) / 3, (corner.z - 2) / 3);
                    b = new Vector3Int((corner.x + 1) / 3, (corner.y - 2) / 3, (corner.z + 1) / 3);
                    c = new Vector3Int((corner.x - 2) / 3, (corner.y + 1) / 3, (corner.z + 1) / 3);
                }

                return new List<Vector3Int> { a, b, c };
            }

            /// <summary>
            /// splits the input collection into 1 List for each contiguous area formed by the input tile coordinates.
            /// </summary>    
            /// ![left = input tile list , right = output separated into different areas](Map_GetTiles_ContiguousCombined.png)
            public static List<List<Vector3Int>> ContiguousAreasOfInputTiles(ICollection<Vector3Int> inputTiles)
            {
                if(inputTiles.Count == 0) return new List<List<Vector3Int>>();

                List<List<Vector3Int>> areas = new List<List<Vector3Int>>();
                HashSet<Vector3Int> unusedTiles = new HashSet<Vector3Int>(inputTiles);
                
                while(unusedTiles.Count > 0)
                {
                    HashSet<Vector3Int> area = new HashSet<Vector3Int>();
                    Queue<Vector3Int> queue = new Queue<Vector3Int>();
                    queue.Enqueue(unusedTiles.First());   
                    while(queue.Count > 0)
                    {
                        Vector3Int current = queue.Dequeue();
                        area.Add(current);
                        unusedTiles.Remove(current);
                        List<Vector3Int> neighbours = GetTiles.AdjacentToTile(current);
                        neighbours.RemoveAll(x => !unusedTiles.Contains(x));
                        foreach(var tile in neighbours)
                        {
                            queue.Enqueue(tile);
                        }
                    }
                    areas.Add(area.ToList());
                }

                return areas;
            }
        }
    }
}