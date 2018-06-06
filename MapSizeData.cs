using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public struct MapSizeData
    {
        /// <summary>
        /// lowest x Value of any map tile
        /// </summary>
        public readonly int offsetTileMinValX;

        /// <summary>
        /// highest x Value of any map tile
        /// </summary>
        public readonly int offsetTileMaxValX;

        /// <summary>
        /// lowest z Value of any map tile
        /// </summary>
        public readonly int offsetTileMinValZ;

        /// <summary>
        /// highest z Value of any map tile
        /// </summary>
        public readonly int offsetTileMaxValZ;

        /// <summary>
        /// map center (in cartesian coordinates)
        /// </summary>
        public readonly Vector3 center;

        /// <summary>
        /// half-size of the map extents along each axis
        /// </summary>
        public readonly Vector3 extents;

        public MapSizeData(int offsetTileMinValX, int offsetTileMaxValX, int offsetTileMinValZ, int offsetTileMaxValZ, Vector3 center, Vector3 extents)
        {
            this.offsetTileMinValX = offsetTileMinValX;
            this.offsetTileMaxValX = offsetTileMaxValX;
            this.offsetTileMinValZ = offsetTileMinValZ;
            this.offsetTileMaxValZ = offsetTileMaxValZ;
            this.center = center;
            this.extents = extents;
        }
    }
}
