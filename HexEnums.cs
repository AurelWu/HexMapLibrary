using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary
{
    public enum EdgeAlignment // undirected
    {
        ParallelToCubeX = 0,
        ParallelToCubeZ = 1,
        ParallelToCubeY = 2,
    }

    public enum EdgeDirection
    {
        //Top = 0,
        //TopRight = 1,
        //BottomRight =2,
        //Bottom = 3,
        //BottomLeft = 4,
        //TopLeft = 5,
        Top = 0,
        TopRight = 1,
        BottomRight = 2,
        Bottom = 3,
        BottomLeft = 4,
        TopLeft =5,

    }

    public enum TileDirection
    {
        TopRight = 0,
        Right = 1,
        BottomRight = 2,
        BottomLeft = 3,
        Left = 4,
        TopLeft = 5,
    }

    public enum CornerType
    {
        TopOfYParallelEdge,
        BottomOfYParallelEdge,
    }

    public enum MapElementType
    {
        Tile,
        Edge,
        Corner
    }

    //public enum Nudge
    //{
    //    None,
    //    HorizontallyPositive,
    //    HorizontallyNegative,
    //}
}