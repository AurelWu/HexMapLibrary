using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{

    public class TilePositionProvider
    {
        protected readonly CoordinateWrapper coordinateWrapper;
        protected readonly Dictionary<Vector3Int, int> tileIndexByPosition;

        public TilePositionProvider(CoordinateWrapper coordinateWrapper, Dictionary<Vector3Int,int> TileIndexByPosition)
        {
            this.coordinateWrapper = coordinateWrapper;
            this.tileIndexByPosition = TileIndexByPosition;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input cartesian coordinate
        /// </summary>        
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToTileCoord(cartesianCoordinate);            
            return coord;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input cartesian coordinate, the out parameter specifies if that coordinate belongs to the map
        /// </summary>        
        public Vector3Int FromCartesianCoordinate(Vector3 cartesianCoordinate, out bool tileIsOnMap)
        {
            if (coordinateWrapper != null) cartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(cartesianCoordinate);
            Vector3Int coord = HexConverter.CartesianCoordToTileCoord(cartesianCoordinate);
            tileIsOnMap = false;
            if (tileIndexByPosition.ContainsKey(coord)) tileIsOnMap = true;
            return coord;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input tile coordinate rotated 60° Clockwise around the specified center tile
        /// </summary>
        public Vector3Int FromTileRotated60DegreeClockwise(Vector3Int center, Vector3Int pointToRotate)
        {
            Vector3Int rotated = HexGrid.GetTile.FromTileRotated60DegreeClockwise(center, pointToRotate);
            if (coordinateWrapper != null) rotated = coordinateWrapper.WrapTileCoordinate(rotated);
            return rotated;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input tile coordinate rotated 60° Clockwise around the specified center tile, the out parameter specifies if that coordinate belongs to the map
        /// </summary>
        public Vector3Int FromTileRotated60DegreeClockwise(Vector3Int center, Vector3Int pointToRotate, out bool tileIsOnMap)
        {
            Vector3Int rotated = HexGrid.GetTile.FromTileRotated60DegreeClockwise(center, pointToRotate);
            if (coordinateWrapper != null) rotated = coordinateWrapper.WrapTileCoordinate(rotated);
            tileIsOnMap = false;
            if (tileIndexByPosition.ContainsKey(rotated)) tileIsOnMap = true;
            return rotated;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input tile coordinate rotated 60° Counter-clockwise around the specified center tile
        /// </summary>
        public Vector3Int FromTileRotated60DegreeCounterClockwise(Vector3Int center, Vector3Int pointToRotate)
        {
            Vector3Int rotated = HexGrid.GetTile.FromTileRotated60DegreeCounterClockwise(center, pointToRotate);
            if (coordinateWrapper != null) rotated = coordinateWrapper.WrapTileCoordinate(rotated);
            return rotated;
        }

        /// <summary>
        /// returns the periodic tile coordinate of the input tile coordinate rotated 60° Counter-clockwise around the specified center tile, the out parameter specifies if that coordinate belongs to the map
        /// </summary>
        public Vector3Int FromTileRotated60DegreeCounterClockwise(Vector3Int center, Vector3Int pointToRotate, out bool tileIsOnMap)
        {
            Vector3Int rotated = HexGrid.GetTile.FromTileRotated60DegreeCounterClockwise(center, pointToRotate);
            if (coordinateWrapper != null) rotated = coordinateWrapper.WrapTileCoordinate(rotated);
            tileIsOnMap = false;
            if (tileIndexByPosition.ContainsKey(rotated)) tileIsOnMap = true;
            return rotated;
        }

        /// <summary>
        /// returns if the input tile coordinate belongs to the map
        /// </summary>
        public bool IsInputCoordinateOnMap(Vector3Int inputTileCoordinate)
        {
            if (coordinateWrapper != null) inputTileCoordinate = coordinateWrapper.WrapTileCoordinate(inputTileCoordinate);
            if (tileIndexByPosition.ContainsKey(inputTileCoordinate)) return true;
            else return false;
        }

        /// <summary>
        /// returns if the input cartesian coordinate belongs to the map
        /// </summary>
        public bool IsInputCoordinateOnMap(Vector3 inputCartesianCoordinate)
        {
            if (coordinateWrapper != null) inputCartesianCoordinate = coordinateWrapper.WrapCartesianCoordinate(inputCartesianCoordinate);
            Vector3Int tileCoordinate = HexConverter.CartesianCoordToTileCoord(inputCartesianCoordinate);
            if (tileIndexByPosition.ContainsKey(tileCoordinate)) return true;
            else return false;
        }

    }

}