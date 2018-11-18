using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameFramework
{
    public class FPSController : MonoBehaviour
    {

        [SerializeField] private MouseLook ml;
        public Camera mCamera;
        public CharacterController controller;
        public Rigidbody rigidbody;
        public float speed = 1;
        public string targetChunk;
        public GameObject activeCube;
        public Chunk activeChunk;
        public Vector3 activeBlock;
        public bool isProcess = false;
        // Use this for initialization
        void Start()
        {
            mCamera = Camera.main;
            ml.Init(transform, mCamera.transform);
            rigidbody = this.GetComponent<Rigidbody>();
            controller = this.GetComponent<CharacterController>();
        }

        // Update is called once per frame
        void Update()
        {

            ml.LookRotation(transform, mCamera.transform);
            if (Input.GetKey("a"))
            {
                controller.SimpleMove(transform.right * -speed);
            }

            if (Input.GetKey("d"))
            {
                controller.SimpleMove(transform.right * speed);
            }

            if (Input.GetKey("w"))
            {
                controller.SimpleMove(transform.forward * speed);
            }

            if (Input.GetKey("s"))
            {
                controller.SimpleMove(transform.forward * -speed);
            }
            if (Input.GetKey(KeyCode.Space))
            {
                controller.SimpleMove(transform.up * speed);
            }
            if (Input.GetMouseButtonUp(1))
            {
                Debug.Log("Mosue Right Click!");
                if (activeChunk != null && activeBlock!=new Vector3(-999, -999, -999))
                {

                    activeChunk.DeleteBlock(activeBlock);
                    activeCube.SetActive(false);
                    
                }
            }
            Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f,0.5f,0.5f));
            Debug.DrawRay(transform.position, ray.direction,Color.red);
            RaycastHit hitInfo;
            if (Physics.Raycast(ray, out hitInfo))
            {
                targetChunk = hitInfo.collider.gameObject.name;
                this.FindBlockFromWorldPoint(hitInfo.collider.gameObject, hitInfo.point);
            }
            else
            {
                targetChunk = "no chunk hit!";

            }

        }
        private void OnGUI()
        {
            var oldColor = GUI.contentColor;
            GUI.contentColor = Color.black;
            GUI.Label(new Rect(20, 300, 200, 60), targetChunk);
            GUI.contentColor = oldColor;

       }

        public void FindBlockFromWorldPoint(GameObject go,Vector3 point)
        {
            var chunk = go.GetComponent<Chunk>();
            if (chunk != null)
            {
                activeChunk = chunk;
                var blockPoint = chunk.GetBlockFromWorldPoint(point);
                activeBlock = blockPoint;
                if (blockPoint != new Vector3(-999, -999, -999))
                {
                    var cubePos = blockPoint + new Vector3(0.5f - 8, 0.5f - chunk.sectionCount / 2 * 16, 0.5f - 8f)+go.transform.position;
                    this.activeCube.transform.position = cubePos;
                    this.activeCube.SetActive(true);
                }

            }
            else
            {
                activeChunk = null;
            }
        }

    }
}