using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// non-generic base class for a tile containing positional data, either use generic version or extend from this class for your own implementation
    /// </summary>
    public class Corner : MapElement
    {

        /// <summary>
        /// returns the cartesian coordinate of the tile center
        /// </summary>
        public override Vector3 CartesianPosition { get { return HexConverter.CornerCoordToCartesianCoord(Position); } }

        /// <summary>
        /// Initializes a new corner instance
        /// </summary>>
        public Corner(Vector3Int position, int index, Vector2 normalizedPosition) : base(position, index, normalizedPosition)
        {
            ;
        }
    }
}
