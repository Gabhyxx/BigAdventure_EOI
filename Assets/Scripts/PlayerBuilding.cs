using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerBuilding : MonoBehaviour
{
    [SerializeField] GameObject block;
    [SerializeField] Camera mainCam;

    private void Update()
    {
        BlockInteraction();
    }

    void BlockInteraction()
    {
        Ray ray = mainCam.ScreenPointToRay(Mouse.current.position.ReadValue());
        Debug.DrawRay(ray.origin, ray.direction, Color.red);

        int groundLayer = LayerMask.NameToLayer("Ground");

        if (Physics.Raycast(ray, out RaycastHit hit, float.MaxValue))
        {
            if (Input.GetButtonDown("Fire2"))
            {
                if (hit.collider.gameObject != gameObject)
                {
                    Vector3 pos = hit.point + (hit.normal * 0.5f);

                    //int posX = Mathf.RoundToInt(pos.x);
                    //int posY = Mathf.RoundToInt(pos.y);
                    //int posZ = Mathf.RoundToInt(pos.z);

                    //Vector3 roundPos = new Vector3(posX, posY, posZ);

                    Instantiate(block, pos, Quaternion.identity);

                    //Codigo Previo
                    //Vector3 diference = hit.point - hit.collider.transform.position;

                    //diference.x = (int)(diference.x / 0.5);
                    //diference.y = (int)(diference.y / 0.5);
                    //diference.z = (int)(diference.z / 0.5);
                    //Instantiate(block, hit.collider.transform.position + diference, Quaternion.identity);
                }
            }
            else if (Input.GetButtonDown("Fire1"))
            {
                if(hit.collider.gameObject != gameObject && hit.collider.gameObject.layer != groundLayer)
                {
                    Destroy(hit.collider.gameObject);
                } 
            }
        }
    }
}
