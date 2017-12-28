using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.CameraUI
{
    public class CameraFollow : MonoBehaviour
    {
        
        [SerializeField] GameObject eventSystemPrefab = null;

        [SerializeField] Transform target;

        [SerializeField] float smoothSpeed = 0.125f;
        [SerializeField] Vector3 offset;

        GameObject player;

        // Use this for initialization
        void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player");
            /*
            transform.position = player.transform.position + offset;
            transform.LookAt(player.transform);
            */
            
            
            //Instantiate(eventSystemPrefab);
        }

        private void Update()
        {
            if (Input.GetMouseButton(2))
            {
                transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X")*2, 0));
                transform.position = new Vector3(0, -Input.GetAxis("Mouse Y"), 0) + transform.position;
                //print(Input.GetAxis("Mouse X"));
            }
        }

        // Update is called once per frame
        void LateUpdate()
        {
            /*
            Vector3 desiredPosition = player.transform.position + offset;
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed*Time.deltaTime);
            transform.position = smoothedPosition;
            transform.LookAt(player.transform);
            */
            
            transform.position = new Vector3(player.transform.position.x, transform.position.y, player.transform.position.z);
        }
    }
}
