using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public abstract class CoordinateWrapper
    {
        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3 WrapCartesianCoordinate(Vector3 position);

        /// <summary>
        /// TODO ADD DESCRIPTION
        public abstract Vector3Int WrapTileCoordinate(Vector3Int position);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract List<Vector3Int> WrapTileCoordinates(List<Vector3Int> collection);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3Int ShiftTargetToClosestPeriodicTilePosition(Vector3Int origin, Vector3Int target);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3Int ShiftTargetToClosestPeriodicEdgePosition(Vector3Int origin, Vector3Int target);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3Int ShiftTargetToClosestPeriodicCornerPosition(Vector3Int origin, Vector3Int target);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3Int WrapEdgeCoordinate(Vector3Int position);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract List<Vector3Int> WrapEdgeCoordinates(List<Vector3Int> collection);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract Vector3Int WrapCornerCoordinate(Vector3Int position);

        /// <summary>
        /// TODO ADD DESCRIPTION
        /// </summary>
        public abstract List<Vector3Int> WrapCornerCoordinates(List<Vector3Int> collection);
    }       
}