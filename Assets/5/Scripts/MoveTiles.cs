using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTiles : MonoBehaviour
{
    public Renderer rend;
    public GameObject target;
    //public GameObject tilePrefab;

    //public int lastInstance = 0;
    //public int newInstance = 0;
    private void Start()
    {
        rend = gameObject.GetComponent<Renderer>();
    }
    void Update()
    {
        //if (target.transform.position.x < (lastInstance + 30)) //if within 30 of last block
        //{
        //    lastInstance = Mathf.FloorToInt(target.transform.position.x / 10);
        //    if (newInstance == lastInstance) //wait until position of target has increased by 10
        //    {
        //        GameObject instanceTile = (GameObject)Instantiate(tilePrefab); //instantiate tile
        //        instanceTile.transform.position = Vector3.zero; //instantiate at (0,0,0)
        //        instanceTile.transform.position = Vector3.right * 10 * newInstance; //move new tile ahead
        //        newInstance += 1;
        //    }
        //}

        rend.material.SetVector("_Player_Position", target.transform.position);
        rend.material.SetFloat("_XxX", target.transform.position.x);
        Debug.Log(rend.material.GetVector("_Player_Position"));
    }
}
