using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Wunderwunsch.HexMapLibrary;

namespace Wunderwunsch.HexMapLibrary.Generic
{
    /// <summary>
    /// generic Edge which can have any object with an parameterless constructor as content
    /// </summary>    
    public class Corner<C> : Corner where C : new()
    {
        /// <summary>
        /// can be any object, defines the actual content of the Edge
        /// </summary>
        public C Data { get; set; }

        /// <summary>
        /// constructor without passing an instance of E in , creating a default instance of E by calling it's parameterless constructor
        /// </summary>
        public Corner(Vector3Int position, int index, Vector2 normalizedPosition, bool createDefaultDataObject = true) : base(position, index, normalizedPosition)
        {
            if (createDefaultDataObject)
            {
                Data = new C();
            }
        }

        /// <summary>
        /// constructor including an instance of E
        /// </summary>  
        public Corner(Vector3Int position, int index, Vector2 normalizedPosition, C data) : base(position, index, normalizedPosition)
        {
            Data = data;
        }
    }
}
