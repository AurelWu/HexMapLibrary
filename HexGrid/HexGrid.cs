using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Contains many helper methods facilitating working with a hexagonal coordinate system. Assumes the grid being on the (infinite) XZ-Plane.    
    /// The methods of this class are also used by HexMap (which adds bound checks and wrapping on top of it)
    /// </summary>
    public static partial class HexGrid
    {
        //subClasses are all in their own file

        //Array Index is aligned with HexEnums.TileDirection value
        internal static Vector3Int[] TileDirectionVectors = new Vector3Int[] 
        {   new Vector3Int(0,1,-1), //TopRight
            new Vector3Int(1,0,-1), //Right
            new Vector3Int(1,-1,0), //BottomRight
            new Vector3Int(0,-1,1), //BottomLeft
            new Vector3Int(-1,0,1), //Left
            new Vector3Int(-1,1,0)  //TopLeft
        };

        //Array Index is aligned with HexEnums.EdgeDirection value
        internal static Vector3Int[] ClockWiseNeighbourOfEdgeByEdgeDirection = new Vector3Int[] 
        {   new Vector3Int(0, +1, -1), //CW Neighbour of Edge with Top Direction 
            new Vector3Int(+1, 0, -1), //CW Neighbour of Edge with TopRight Direction
            new Vector3Int(+1, -1, 0), //CW Neighbour of Edge with BottomRight Direction
            new Vector3Int(0, -1, +1), //CWNeighbour of Edge with Bottom Direction
            new Vector3Int(-1, 0, +1), //CW Neighbour of Edge with BottomLeft Direction
            new Vector3Int(-1, +1, 0)  //CW Neighbour of Edge with TopLeft Direction
        };

        //Array Index is aligned with HexEnums.EdgeDirection value
        internal static Vector3Int[] CounterClockWiseNeighbourOfEdgeByEdgeDirection = new Vector3Int[]
        {   new Vector3Int(-1, +1, 0), //CCW Neighbour of Edge with Top Direction
            new Vector3Int(0, +1, -1),  //CCW TopRight Direction
            new Vector3Int(+1, 0, -1), //CCW BottomRight Direction
            new Vector3Int(+1, -1, 0), //CCW Bottom Direction
            new Vector3Int(0, -1, +1), //CCW BottomLeft Direction
            new Vector3Int(-1, 0, +1) //CCW TopLeft Direction

        };
        
    }
}