using UnityEngine;

     public class MinimapCameraFollow : MonoBehaviour
     {
        private Transform ship;
        private float height = 10f;
        private void Start()
        {
            ship = GameObject.Find("Ship").transform; 
        }

         private void LateUpdate()
         {
             if (ship != null)
             {
                 transform.position = new Vector3(ship.position.x, ship.position.y + height, ship.position.z);
                 transform.rotation = Quaternion.Euler(0f, 0f, 0f);
             }
         }
     }