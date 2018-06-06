using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Wunderwunsch.HexMapLibrary.Generic
{

    public class HexMap<T,E,C> : HexMap<T,E> where T : new() where E : new() where C : new()
    {
        public CornerDataProvider<C> GetCorner { get; private set; }
        public CornersDataProvider<C> GetCorners { get; private set; }

        public Dictionary<Vector3Int, Corner<C>> CornersByPosition { get; private set; }
        public Corner<C>[] Corners { get; private set; }




        public HexMap(Dictionary<Vector3Int, int> tileIndexByPosition, CoordinateWrapper coordinateWrapper = null) : base(tileIndexByPosition, coordinateWrapper)
        {
            CreateCornerData();

            GetCorner = new CornerDataProvider<C>(CornersByPosition, base.GetCornerPosition);
            GetCorners = new CornersDataProvider<C>(CornersByPosition, base.GetCornerPositions);
            Debug.Log("HexMap<T,E,C> Constructor called");
        }

        public void CreateCornerData() 
        {
            Vector3 center = MapSizeData.center;
            Vector3 extents = MapSizeData.extents;
            Vector3IntEqualityComparer vector3IntEqualityComparer = new Vector3IntEqualityComparer();

            float minX = center.x - extents.x;
            float maxX = center.x + extents.x;
            float minZ = center.z - extents.z;
            float maxZ = center.z + extents.z;

            CornersByPosition = new Dictionary<Vector3Int, Corner<C>>(vector3IntEqualityComparer);
            Corners = new Corner<C>[CornerIndexByPosition.Count];

            foreach (var kvp in CornerIndexByPosition)
            {
                Vector2 normalizedPosition = HexConverter.CornerCoordToNormalizedPosition(kvp.Key, minX, maxX, minZ, maxZ);
                Corner<C> corner = new Corner<C>(kvp.Key, kvp.Value, normalizedPosition);
                CornersByPosition.Add(kvp.Key, corner);
                Corners[corner.Index] = corner;
            }
        }
    }
}