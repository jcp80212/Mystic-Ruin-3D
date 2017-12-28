using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using System;
using RPG.Character;

namespace RPG.CameraUI
{
    public class CameraRaycaster : MonoBehaviour
    {

        [SerializeField] Texture2D walkCursor = null;
        [SerializeField] Texture2D targetCursor = null;
        [SerializeField] Texture2D playerCursor = null;

        [SerializeField] Vector2 cursorHotspot = new Vector2(0, 0);
        const int POTENTIALLY_WALK_LAYER = 8;

        float maxRaycastDepth = 100f; // Hard coded value

        Rect screenRectForRaycasting = new Rect(0, 0, Screen.width, Screen.height);

        //New Delegates
        //enemy delegate
        public delegate void OnMouseOverEnemy(EnemyAI enemy);
        public event OnMouseOverEnemy onMouseOverEnemy;
        //walkable layer delegate
        public delegate void OnMouseOverPotentiallyWalkable(Vector3 destination);
        public event OnMouseOverPotentiallyWalkable onMouseOverPotentiallyWalkable;


        void Update()
        {
            // Check if pointer is over an interactable UI element
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //Implement UI interaction
            }
            else
            {
                PerformRaycast();
            }


        }

        void PerformRaycast()
        {
            if (screenRectForRaycasting.Contains(Input.mousePosition))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                //Specify layer priorities here
                if (RaycastForPlayer(ray)) { return; }
                if (RaycastForEnemy(ray)) { return; }
                if (RaycastForWalkable(ray)) { return; }
            }
        }
        private bool RaycastForEnemy(Ray ray)
        {
            RaycastHit hitInfo;

            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);

            if(hitInfo.collider == null)
            {
                return false;
            }

            if (hitInfo.collider.gameObject)
            {
                var gameObjectHit = hitInfo.collider.gameObject;
                var enemyHit = gameObjectHit.GetComponent<EnemyAI>();
                if (enemyHit)
                {
                    Cursor.SetCursor(targetCursor, cursorHotspot, CursorMode.Auto);
                    onMouseOverEnemy(enemyHit);
                    return true;
                }
            }
            return false;
        }

        private bool RaycastForPlayer(Ray ray)
        {
            RaycastHit hitInfo;

            Physics.Raycast(ray, out hitInfo, maxRaycastDepth);

            if (hitInfo.collider == null)
            {
                return false;
            }

            if (hitInfo.collider.gameObject)
            {
                var gameObjectHit = hitInfo.collider.gameObject;
                var playerHit = gameObjectHit.GetComponent<PlayerControl>();
                if (playerHit)
                {
                    Cursor.SetCursor(playerCursor, cursorHotspot, CursorMode.Auto);
                    if(Input.GetMouseButtonUp(0))
                    {
                        GameManager.manager.SelectedCharacter = playerHit.gameObject;
                    }
                    return true;
                }
            }
            return false;
        }

        private bool RaycastForWalkable(Ray ray)
        {
            RaycastHit hitInfo;
            LayerMask potentiallyWalkableLayer = 1 << POTENTIALLY_WALK_LAYER;
            bool potentiallyWalkableHit = Physics.Raycast(ray, out hitInfo, maxRaycastDepth, potentiallyWalkableLayer);
            if (potentiallyWalkableHit)
            {
                Cursor.SetCursor(walkCursor, cursorHotspot, CursorMode.Auto);
                if(onMouseOverPotentiallyWalkable != null)
                    onMouseOverPotentiallyWalkable(hitInfo.point);
                return true;
            }
            return false;
        }
    }
}