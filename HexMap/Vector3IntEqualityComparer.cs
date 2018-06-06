using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Add this as custom IEqualityComparer to any Dictionary using Vector3Int as key, 
    /// this is unfortunately necessary to prevent boxing (and therefore memory allocations) 
    /// as Unity omitted to implement the IEquatable<Vector3Int> interface on Vector3Int
    /// Will be obsolete with 2018.2 :-)
    /// </summary>
    public class Vector3IntEqualityComparer : IEqualityComparer<Vector3Int>
    {
        /// <summary>
        /// strongly typed equality comparison removing the need to cast to object
        /// </summary>
        public bool Equals(Vector3Int v1, Vector3Int v2)
        {
            return v1.x == v2.x && v1.y == v2.y && v1.z == v2.z;
        }

        /// <summary>
        /// HashCode implementation equal to the one Vector3Int uses.
        /// </summary>        
        public int GetHashCode(Vector3Int v)
        {
            unchecked
            {
                return v.x.GetHashCode() ^ v.y.GetHashCode() << 2 ^ v.z.GetHashCode() >> 2;
            }
        }
    }
}