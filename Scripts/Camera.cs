using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(this.transform.position);
        //Debug.Log((float)(playerObject.transform.position.y + 3.5));
        ////move camera up or down according to player's status
        //this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y + 3.5), -10);

        //we decide not to move camera, unless it hits upper scene level
        if ((int)(playerObject.transform.position.y - 3.5) % 7 == 0)
        {
            //if gap between player and camera too large, shorten it
            if (playerObject.transform.position.y - this.transform.position.y >= 3.5)
            {
                //move camera up
                this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y + 3.5), -10);
            }
            //otherwise if too short, enlarge that
            else if (playerObject.transform.position.y - this.transform.position.y <= -3.5)
            {
                //move camera down
                this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y - 3.5), -10);
            }
            //otherwise do nothing
        }
        else
        {
            if (this.transform.position.y - playerObject.transform.position.y >= 3.5) {
                this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y - 3.5), -10);

            }


        }
    }
}
