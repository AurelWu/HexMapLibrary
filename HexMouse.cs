using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wunderwunsch.HexMapLibrary
{
    /// <summary>
    /// Updates every frame with the position of the mouse cursor on the XZ-Plane in different coordinate systems. 
    /// </summary>
    public class HexMouse : MonoBehaviour
    {
        /// <summary>
        /// Map which is assigned to the mouse - should always be the current visible map in cases where you have multiple maps.
        /// if it is null then it will just skip map wrapping and clamping.
        /// </summary>
        private HexMap hexMap;

        /// <summary>
        /// collision plane to cast rays against to get mouse position;
        /// </summary>
        private Plane plane;

        /// <summary>
        /// Indicates whether the cursor is on the map.
        /// </summary>
        public bool CursorIsOnMap { get; private set; }

        ///// <summary>
        ///// Indicates whether the mouse coordinates gets clamped to closest valid position on map.
        ///// Cartesian coordinates are never clamped.
        ///// </summary>
        //public bool ClampToClosestValid { get; private set; }

        /// <summary>
        /// cartesian coordinate without map wrap considered
        /// </summary>
        public Vector3 CartesianCoordInfiniteGrid { get; private set; }
        /// <summary>
        /// cartesian coordinate with map wrap considered
        /// </summary>
        public Vector3 CartesianCoordWrapped { get; private set; }

        /// <summary>
        /// cube coordinate without map wrap
        /// </summary>
        public Vector3Int CubeCoordRaw { get; private set; }
        /// <summary>
        /// cube coordinate with map wrap
        /// </summary>
        public Vector3Int TileCoord { get; private set; }

        /// <summary>
        /// offset coordinate without map wrap
        /// </summary>
        public Vector2Int OffsetCoordInfiniteGrid { get; private set; }
        /// <summary>
        /// offset coordinate with map wrap
        /// </summary>
        public Vector2Int OffsetCoord { get; private set; }

        /// <summary>
        /// closest edge coordinate without map wrap
        /// </summary>
        public Vector3Int ClosestEdgeCoordInfiniteGrid { get; private set; }
        /// <summary>
        /// closest edge coordinate with map wrap
        /// </summary>
        public Vector3Int ClosestEdgeCoord { get; private set; }

        /// <summary>
        /// equals Camera.main.ScreenPointToRay(Input.mousePosition);
        /// </summary>
        public Ray SelectionRay { get; private set; }

        /// <summary>
        /// closest corner coordinate without map wrap
        /// </summary>
        public Vector3Int ClosestCornerCoordInfiniteGrid { get; private set; }

        /// <summary>
        /// closest corner coordinate with map wrap
        /// </summary>
        public Vector3Int ClosestCornerCoord { get; private set; }


        /// <summary>
        /// Update is called once per frame
        /// </summary>
        public virtual void Update()
        {
            UpdateMousePositionData();
        }

        /// <summary>
        /// Call this at start of game or when the map changes to assign the HexMap
        /// </summary>        
        public void Init(HexMap hexMap)
        {
            this.hexMap = hexMap;
            UpdateMousePositionData();
            plane = new Plane(Vector3.up,0);
        }

        /// <summary>
        /// returns the cartesian of the mouse, using the active Camera and casting a ray on the XZ-plane
        /// </summary>
        private Vector3 GetPlanePosition()
        {
            //Debug.Log("plane Normal: " + plane.normal);
            Vector3 mousePos = Input.mousePosition;
            //Debug.Log(mousePos);            

            Ray ray = Camera.main.ScreenPointToRay(mousePos);            
            //Debug.Log("Ray origin: " + ray.origin);
            //Debug.Log("Ray direction: " + ray.direction);

            //plane = new Plane(Vector3.up, Camera.main.transform.position.y - Camera.main.nearClipPlane);
            float dist;            
            Vector3 point = Vector3.zero;
            plane.Raycast(ray, out dist);
            point = ray.GetPoint(dist);
            //Debug.Log("Plane hitPoint: " + point);
            point = new Vector3(point.x, 0, point.z);
            return point;
        }


        /// <summary>
        /// updates all the mouse position data
        /// </summary>
        private void UpdateMousePositionData()
        {
            CursorIsOnMap = false;
            CartesianCoordInfiniteGrid = GetPlanePosition();
            CartesianCoordWrapped = CartesianCoordInfiniteGrid;

            CubeCoordRaw = HexConverter.CartesianCoordToTileCoord(CartesianCoordInfiniteGrid);
            TileCoord = CubeCoordRaw;

            OffsetCoordInfiniteGrid = HexConverter.CartesianCoordToOffsetCoord(CartesianCoordInfiniteGrid);
            OffsetCoord = OffsetCoordInfiniteGrid;

            ClosestEdgeCoordInfiniteGrid = HexConverter.CartesianCoordToClosestEdgeCoord(CartesianCoordInfiniteGrid);
            ClosestEdgeCoord = ClosestEdgeCoordInfiniteGrid;

            ClosestCornerCoordInfiniteGrid = HexConverter.CartesianCoordToClosestCornerCoord(CartesianCoordInfiniteGrid);
            ClosestCornerCoord = ClosestCornerCoordInfiniteGrid;

            SelectionRay = Camera.main.ScreenPointToRay(Input.mousePosition);


            if (hexMap != null)
            {
                if (hexMap.CoordinateWrapper != null)
                {
                    CartesianCoordWrapped = hexMap.CoordinateWrapper.WrapCartesianCoordinate(CartesianCoordInfiniteGrid);
                }

                CursorIsOnMap = hexMap.GetTilePosition.IsInputCoordinateOnMap(CartesianCoordWrapped);

                TileCoord = HexConverter.CartesianCoordToTileCoord(CartesianCoordWrapped);
                OffsetCoord = HexConverter.TileCoordToOffsetTileCoord(TileCoord);
                ClosestEdgeCoord = HexConverter.CartesianCoordToClosestEdgeCoord(CartesianCoordWrapped);
                ClosestCornerCoord = HexConverter.CartesianCoordToClosestCornerCoord(CartesianCoordWrapped);             
            }
        }
    }
}