using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Base Class for Tile and Edge , containing index, position and normalized position of the tile or edge.
    /// </summary>
    public abstract class MapElement
    {
        dynamic map;
        /// <summary>
        /// index of the MapElement, HexMap.Tiles , HexMap.Edges and HexMap.Corners arrays are mapped based on this. Can be arbitrarily chosen as long as it starts with 0 and has no gaps.
        /// </summary>        
        public int Index { get; private set; }

        /// <summary>
        /// coordinate of the MapElement
        /// </summary>
        public Vector3Int Position { get; private set; }

        /// <summary>
        /// returns the cartesian coordinate of the MapElement
        /// </summary>
        abstract public Vector3 CartesianPosition { get; }

        /// <summary>
        /// position normalised to a range of [0...1] based on the boundingbox of the map tiles,edges or corners
        /// </summary>        
        public Vector2 NormalizedPosition { get; private set; }

        /// <summary>
        /// base constructor called by derived classes of MapElement
        /// </summary>
        protected MapElement(Vector3Int position, int index, Vector2 normalizedPosition)
        {            
            Position = position;
            Index = index;
            NormalizedPosition = normalizedPosition;
        }

        /// <summary>
        /// returns a string with 3 Lines, one each for Index, Position and NormalizedPosition
        /// </summary>        
        public override string ToString()
        {
            return "Index: " + Index + "\r\n" + "Position: " + Position + "\r\n" + "NormalizedPosition: " + NormalizedPosition;
        }
    }
}
