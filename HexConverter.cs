using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Converts between cartesian coordinates and two different hexagonal-grid-coordinates (called "offset" and "cube")
    /// </summary>
    /// Using the cartesian coordinate system (the "normal" one you use all the time) does not work well with hexagons as you end up with fractional numbers which 
    /// make things very non-intuitive and which are also often not suited to calculate on the grid. Therefore it is useful to use other coordinate systems which allow 
    /// us to work with integer values and which are better suited to perform grid-based calculations.  
    /// 
    /// This library uses two different hexagonal coordinate systems called "offset" and "cube", we mostly use the cube coordinate system as this is a lot easier to work with 
    /// and has proper straight axes.  
    /// 
    /// For most tasks you can just use the library without worrying too much about the details of the coordinate systems, however if you are interested to dive a bit deeper, 
    /// I recommend reading Amit Patel's great introduction to hexagons: https://www.redblobgames.com/grids/hexagons/ .
    /// ![Coordinates & axes of cartesian coordinate system](CartesianCoordinatesCombined.png)  
    ///       
    /// 
    /// ![Coordinates & 'axes' of offset coordinate system](OffsetCoordinatesCombined.png)  
    /// 
    ///       
    /// ![Coordinates & axes of cube coordinate system](CubeCoordinatesCombined.png)
    public static class HexConverter
    {
        /// <summary>
        /// cached value of square root of 3.
        /// </summary>
        private static float sqrt3 = Mathf.Sqrt(3);

        /// <summary>
        /// converts the input cartesian coordinate to its equivalent offset tile coordinate 
        /// </summary>
        public static Vector2Int CartesianCoordToOffsetCoord(Vector3 cartesianCoord)
        {
            float x = cartesianCoord.x / sqrt3;
            float z = cartesianCoord.z;
            float temp = Mathf.Floor(x + z + 1);

            float c = Mathf.Floor((Mathf.Floor(2 * x + 1) + temp) / 3f);
            float r = Mathf.Floor((temp + Mathf.Floor(-x + z + 1)) / 3);
            return new Vector2Int((int)(c - (r + ((int)r & 1)) / 2), (int)r);
        }

        /// <summary>
        /// converts the input cartesian coordinate to its equivalent tile coordinate 
        /// </summary>
        public static Vector3Int CartesianCoordToTileCoord(Vector3 cartesianCoord)
        {
            float x = cartesianCoord.x / sqrt3;
            float z = cartesianCoord.z;
            float temp = Mathf.Floor(x + z + 1);

            float q = Mathf.Floor((Mathf.Floor(2 * x + 1) + temp) / 3f);
            float r = Mathf.Floor((temp + Mathf.Floor(-x + z + 1)) / 3);

            int cX = (int)q - (int)r;
            int cY = (int)r;
            int cZ = -cX - cY;
            return new Vector3Int(cX, cY, cZ);
        }

        /// <summary>
        /// convers the input offset tile coordinate to its equivalent cartesian coordinate
        /// </summary>
        public static Vector3 OffsetTileCoordToCartesianCoord(Vector2Int offsetTileCoord)
        {
            float offsetXAdjustment;
            if (offsetTileCoord.y % 2 == 0) offsetXAdjustment = 0;
            else offsetXAdjustment = 0.5f * sqrt3;

            float cartesianX = offsetTileCoord.x * sqrt3 + offsetXAdjustment;
            float cartesianZ = offsetTileCoord.y * 1.5f;
            return new Vector3(cartesianX, 0, cartesianZ);
        }

        /// <summary>
        /// converts the input offset tile coordinate to its equivalent tile coordinate 
        /// </summary>
        public static Vector3Int OffsetTileCoordToTileCoord(Vector2Int offsetTileCoord)
        {
            int x = offsetTileCoord.x - (offsetTileCoord.y - (offsetTileCoord.y & 1)) / 2;
            int y = offsetTileCoord.y;
            int z = -x - y;
            return new Vector3Int(x, y, z);
        }

        /// <summary>
        /// converts the input tile tile coordinate to its equivalent cartesian coordinate
        /// </summary>
        /// <param name="tileCoord">input tile coordinate</param>
        /// <param name="yCoord">explicitly sets cartesian y-coordinate</param>
        public static Vector3 TileCoordToCartesianCoord(Vector3Int tileCoord, float yCoord = 0)
        {
            float x = sqrt3 * (tileCoord.x + tileCoord.y / 2f);
            float z = 3 / 2f * tileCoord.y;
            float y = yCoord;
            return new Vector3(x, y, z);
        }

        /// <summary>
        /// converts the input tile coordinate to its equivalent offset coordinate 
        /// </summary>
        public static Vector2Int TileCoordToOffsetTileCoord(Vector3Int tileCoord)
        {
            int x = tileCoord.x + (tileCoord.y - (tileCoord.y & 1)) / 2;
            int y = tileCoord.y;
            return new Vector2Int(x, y);
        }

        /// <summary>
        /// returns the midpoint of the edge in cartesian coordinates.
        /// </summary>
        public static Vector3 EdgeCoordToCartesianCoord(Vector3Int edgeCoord, float yCoord = 0)
        {
            Vector3 worldPos = HexConverter.TileCoordToCartesianCoord(edgeCoord);
            worldPos = new Vector3(worldPos.x / 2f, yCoord, worldPos.z / 2f);
            return worldPos;
        }


        /// <summary>
        /// converts the input cartesian coordinate to the edge coordinate closest to it.
        /// </summary> //TODO: is a bit inefficient and allocates memory, so improve later
        public static Vector3Int CartesianCoordToClosestEdgeCoord(Vector3 cartesianCoord)
        {
            Vector3Int tileCoord = CartesianCoordToTileCoord(cartesianCoord);
            List<Vector3Int> edgeCoords = HexGrid.GetEdges.OfTile(tileCoord);
            Vector3Int closestEdge = new Vector3Int(-1, -1, -1); //invalid default coordinate
            float minDistanceSoFar = float.MaxValue;
            for (int i = 0; i < edgeCoords.Count; i++)
            {
                Vector3 worldPos = HexConverter.TileCoordToCartesianCoord(edgeCoords[i]);
                worldPos = new Vector3(worldPos.x / 2f, 1, worldPos.z / 2f);
                float distance = Vector3.Distance(worldPos, cartesianCoord);
                if (distance < minDistanceSoFar)
                {
                    closestEdge = edgeCoords[i];
                    minDistanceSoFar = distance;
                }
            }
            return closestEdge;
        }

        /// <summary>
        /// converts the input cartesian coordinate to the cord coordinate closest to it.
        /// </summary> //TODO: is a bit inefficient and allocates memory, so improve later
        /// <param name="cartesianCoord"></param>
        /// <returns></returns>
        public static Vector3Int CartesianCoordToClosestCornerCoord(Vector3 cartesianCoord)
        {
            Vector3Int tileCoord = CartesianCoordToTileCoord(cartesianCoord);
            List<Vector3Int> cornerCoords = HexGrid.GetCorners.OfTile(tileCoord);
            Vector3Int closestCorner = new Vector3Int(-1, -1, -1); //invalid default coordinate
            float minDistanceSoFar = float.MaxValue;
            for (int i = 0; i < cornerCoords.Count; i++)
            {
                Vector3 worldPos = HexConverter.TileCoordToCartesianCoord(cornerCoords[i]);
                worldPos = new Vector3(worldPos.x / 3f, 1, worldPos.z / 3f);
                float distance = Vector3.Distance(worldPos, cartesianCoord);
                if (distance < minDistanceSoFar)
                {
                    closestCorner = cornerCoords[i];
                    minDistanceSoFar = distance;
                }
            }
            return closestCorner;
        }

        /// <summary>
        /// returns the cartesian coordinate of the input corner coordinate.
        /// </summary>
        public static Vector3 CornerCoordToCartesianCoord(Vector3Int cornerCoord, float yCoord = 0)
        {
            Vector3 worldPos = HexConverter.TileCoordToCartesianCoord(cornerCoord);
            worldPos = new Vector3(worldPos.x / 3f, yCoord, worldPos.z / 3f);
            return worldPos;
        }

        public static Vector2 TileCoordToNormalizedPosition(Vector3Int tileCoordinate, float minX, float maxX, float minZ, float maxZ)
        {
            Vector3 cartesianCoord = HexConverter.TileCoordToCartesianCoord(tileCoordinate);
            return CartesianCoordToNormalizedPosition(cartesianCoord, minX, maxX, minZ,maxZ);
        }

        public static Vector2 EdgeCoordToNormalizedPosition(Vector3Int edgeCoordinate, float minX, float maxX, float minZ, float maxZ)
        {
            Vector3 cartesianCoord = HexConverter.EdgeCoordToCartesianCoord(edgeCoordinate);
            return CartesianCoordToNormalizedPosition(cartesianCoord, minX, maxX, minZ, maxZ);
        }

        public static Vector2 CornerCoordToNormalizedPosition(Vector3Int cornerCoordinate, float minX, float maxX, float minZ, float maxZ)
        {
            Vector3 cartesianCoord = HexConverter.CornerCoordToCartesianCoord(cornerCoordinate);
            return CartesianCoordToNormalizedPosition(cartesianCoord, minX, maxX, minZ, maxZ);
        }

        public static Vector2 CartesianCoordToNormalizedPosition(Vector3 cartesianCoordinate, float minX, float maxX, float minZ, float maxZ)
        {
            float normalizedX = Mathf.InverseLerp(minX, maxX,cartesianCoordinate.x);
            float normalizedZ = Mathf.InverseLerp(minZ, maxZ, cartesianCoordinate.z);
            return (new Vector2(normalizedX, normalizedZ));
        }
    }
}
