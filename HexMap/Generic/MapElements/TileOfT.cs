using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Wunderwunsch.HexMapLibrary;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    /// <summary>
    /// generic Tile which can have any object with a parameterless constructor as content
    /// </summary>    
    public class Tile<T> : Tile where T : new()
    {
        /// <summary>
        /// can be any object, defines the actual content of the Tile
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// constructor without passing an instance of T in, creating a default instance of E by calling it's parameterless constructor
        /// </summary>
        public Tile(Vector3Int position, int index, Vector2 normalizedPosition, bool createDefaultDataObject = true) : base(position, index, normalizedPosition)
        {
            if (createDefaultDataObject)
            {
                Data = new T();
            }
        }

        /// <summary>
        /// constructor including an instance of T
        /// </summary>        
        public Tile(Vector3Int position, int index, Vector2 normalizedPosition, T data) : base(position, index, normalizedPosition)
        {
            Data = data;
        }
    }
}
