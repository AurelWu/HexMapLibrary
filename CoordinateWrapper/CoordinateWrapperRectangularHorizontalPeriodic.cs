using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{    
    public class CoordinateWrapperRectangularHorizontalPeriodic : CoordinateWrapper
    {
        //TODO CORNER AND EDGE AT the very TOP RIGHT NEED SPECIAL LOGIC :(

        /// <summary>
        /// mapSize in tiles along offset coordinates
        /// </summary>
        private Vector2Int mapSize;
    
        /// <summary>
        /// cached value of square root of 3.
        /// </summary>
        private static float sqrt3 = Mathf.Sqrt(3);
    
        /// <summary>
        /// constructor which sets the mapSize;
        /// </summary>        
        public CoordinateWrapperRectangularHorizontalPeriodic(Vector2Int mapSize)
        {
            this.mapSize = mapSize;
        }
    
        /// <summary>
        /// Returns the closest periodic coordinate of the input cartesian coordinate.
        /// </summary>
        public override Vector3 WrapCartesianCoordinate(Vector3 position)
        {
            Vector2Int offsetPosition = HexConverter.CartesianCoordToOffsetCoord(position); //We need to use the upperBound parameter instead of using the mapsize Property because for edges we need mapsize.y*2
            if (offsetPosition.x < 0)
            {
                position = new Vector3(position.x + (mapSize.x * sqrt3), position.y, position.z);
            }
            else if (offsetPosition.x >= mapSize.x)
            {
                position = new Vector3(position.x - (mapSize.x * sqrt3), position.y, position.z);
            }
            return position;
        }

        /// <summary>
        /// Returns the closest periodic coordinate of the input tile coordinate - Assumes coordinates are not further than 1 map size away from actual map
        public override Vector3Int WrapTileCoordinate(Vector3Int position)
        {
            Vector2Int offsetPosition = HexConverter.TileCoordToOffsetTileCoord(position); //We need to use the upperBound parameter instead of using the mapsize Property because for edges we need mapsize.y*2
            offsetPosition.x = (offsetPosition.x % mapSize.x);
            if (offsetPosition.x < 0)
            {
                offsetPosition.x = mapSize.x + offsetPosition.x;
            }
            return HexConverter.OffsetTileCoordToTileCoord(offsetPosition);
        }

        /// <summary>
        /// Returns the closest periodic coordinates of a collection of tile coordinates. Assumes coordinates are not further than 1 map size away from actual map
        /// </summary>
        public override List<Vector3Int> WrapTileCoordinates(List<Vector3Int> collection)
        {
            List<Vector3Int> wrappedCollection = new List<Vector3Int>();
            foreach (Vector3Int position in collection)
            {
                Vector3Int wrappedPos = WrapTileCoordinate(position);
                wrappedCollection.Add(wrappedPos);
            }
            return wrappedCollection;
        }
    
        /// <summary>
        /// This returns the closest "virtual" position of target tile position on a wrapping map, intended to be used in distance calculations
        /// TODO ADD BETTER EXPLANATION
        /// </summary>
        public override Vector3Int ShiftTargetToClosestPeriodicTilePosition(Vector3Int origin, Vector3Int target)
        {
            Vector2Int originOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(origin);
            Vector2Int targetOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(target);
            int distance = Mathf.Abs(originOffsetCoord.x - targetOffsetCoord.x);
            if (distance * 2 <= mapSize.x) return target;
    
            //now we check if the target is "right" or "left of the origin and shift its imaginary position to the opposite
            if (originOffsetCoord.x < targetOffsetCoord.x) //target is right of the origin so we shift it left!
            {
                targetOffsetCoord.x -= mapSize.x;
            }
            else targetOffsetCoord.x += mapSize.x; //target is left of the origin  so we shift it right
            return HexConverter.OffsetTileCoordToTileCoord(targetOffsetCoord);
        }

        public override Vector3Int ShiftTargetToClosestPeriodicEdgePosition(Vector3Int origin, Vector3Int target)
        {
            Vector2Int originOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(origin);
            Vector2Int targetOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(target);
            int distance = Mathf.Abs(originOffsetCoord.x - targetOffsetCoord.x);
            if (distance * 2 <= mapSize.x * 2) return target;

            if (originOffsetCoord.x < targetOffsetCoord.x) //target is right of the origin so we shift it left!
            {
                targetOffsetCoord.x -= mapSize.x * 2;
            }
            else targetOffsetCoord.x += mapSize.x * 2;
            return HexConverter.OffsetTileCoordToTileCoord(targetOffsetCoord);
        }

        public override Vector3Int ShiftTargetToClosestPeriodicCornerPosition(Vector3Int origin, Vector3Int target)
        {
            Vector2Int originOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(origin);
            Vector2Int targetOffsetCoord = HexConverter.TileCoordToOffsetTileCoord(target);
            int distance = Mathf.Abs(originOffsetCoord.x - targetOffsetCoord.x);
            if (distance * 2 <= mapSize.x *3) return target;

            if (originOffsetCoord.x < targetOffsetCoord.x) //target is right of the origin so we shift it left!
            {
                targetOffsetCoord.x -= mapSize.x *3;
            }
            else targetOffsetCoord.x += mapSize.x *3;
            return HexConverter.OffsetTileCoordToTileCoord(targetOffsetCoord);

            //throw new System.NotImplementedException();
        }

        /// <summary>
        /// returns the closest periodic edge position of the input edge position - Assumes coordinates are not further than 1 map size away from actual map
        /// </summary>
        public override Vector3Int WrapEdgeCoordinate(Vector3Int position)
        {
            Vector2Int offsetPosition = HexConverter.TileCoordToOffsetTileCoord(position);
            offsetPosition.x = (offsetPosition.x % (mapSize.x * 2));
            if (offsetPosition.x == (mapSize.x * 2) - 1)
            {
                offsetPosition.x = -1;
            }
            if (offsetPosition.x < -1)
            {
                offsetPosition.x = (mapSize.x * 2) + offsetPosition.x;
            }
    
            Vector3Int edgeCoord = HexConverter.OffsetTileCoordToTileCoord(offsetPosition);
            return edgeCoord;
        }

        /// <summary>
        /// Returns the closest periodic coordinates of a collection of edge coordinates. Assumes coordinates are not further than 1 map size away from actual map
        /// </summary>
        public override List<Vector3Int> WrapEdgeCoordinates(List<Vector3Int> collection)
        {
            List<Vector3Int> wrappedCollection = new List<Vector3Int>();
            foreach (Vector3Int position in collection)
            {
                Vector3Int wrappedPos = WrapEdgeCoordinate(position);
                wrappedCollection.Add(wrappedPos);
            }
            return wrappedCollection;
        }


        /// <summary>
        /// returns the closest periodic corner position of the input corner position - Assumes coordinates are not further than 1 map size away from actual map
        /// </summary>
        public override Vector3Int WrapCornerCoordinate(Vector3Int position)
        {
            //Debug.Log(mapSize.x);
            Vector2Int offsetPosition = HexConverter.TileCoordToOffsetTileCoord(position);
            Debug.Log(position);
            //Debug.Log(-2 / 3);
            offsetPosition.x = (offsetPosition.x % (mapSize.x * 3));
            if ((offsetPosition.x == (mapSize.x * 3) - 2))
            {
                offsetPosition.x = -2;
            }
            if (offsetPosition.x < -2)
            {
                offsetPosition.x = (mapSize.x * 3) + offsetPosition.x;
            }
    
            Vector3Int cornerCoord = HexConverter.OffsetTileCoordToTileCoord(offsetPosition);
            return cornerCoord;
        }

        /// <summary>
        /// Returns the closest periodic coordinates of a collection of corner coordinates. Assumes coordinates are not further than 1 map size away from actual map
        /// </summary>
        public override List<Vector3Int> WrapCornerCoordinates(List<Vector3Int> collection)
        {
            List<Vector3Int> wrappedCollection = new List<Vector3Int>();
            foreach (Vector3Int position in collection)
            {
                Vector3Int wrappedPos = WrapCornerCoordinate(position);
                wrappedCollection.Add(wrappedPos);
            }
            return wrappedCollection;
        }


    }

}