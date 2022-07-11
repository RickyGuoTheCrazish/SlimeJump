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
            if (playerObject.transform.position.y - this.transform.position.y >= 3.5)
            {
                //move camera up
                this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y + 3.5), -10);
            }
            else if (playerObject.transform.position.y - this.transform.position.y <= -3.5)
            {
                //move camera down if not down to the dust.....
                if (this.transform.position.y - 3.5 >= 0)
                {
                    this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y - 3.5), -10);
                }

            }
            //otherwise do nothing
        }
        else
        {
            if (this.transform.position.y - playerObject.transform.position.y >= 3.5)
            {
                //move camera down if not down to the dust.....
                if (this.transform.position.y - 3.5 >= 0)
                {
                    this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y - 3.5), -10);
                }

                //this.transform.position = new Vector3(0, (float)(playerObject.transform.position.y - 3.5), -10);

            }


        }


        if (playerObject.transform.position.y >= 47.5 )
        {
            this.transform.position = new Vector3(0, 51.5f, -10);
        }
        else if (playerObject.transform.position.y >= 40.5 && playerObject.transform.position.y < 47.5)
        {
            this.transform.position = new Vector3(0, 45f, -10);
        }

        else if (playerObject.transform.position.y >= 38.5 && playerObject.transform.position.y < 40.5)
        {
            this.transform.position = new Vector3(0, 43.5f, -10);
        }
        else if (playerObject.transform.position.y >= 28.5 && playerObject.transform.position.y < 38.5)
        {
            this.transform.position = new Vector3(0, 32.5f, -10);
        }

        else if (playerObject.transform.position.y >= 18.5 && playerObject.transform.position.y < 28.5) {
            this.transform.position = new Vector3(0, 23.5f, -10);
        }
        else if (playerObject.transform.position.y >= 8.5 && playerObject.transform.position.y < 18.5)
        {
            this.transform.position = new Vector3(0, 13.5f, -10);
        }
        else if (playerObject.transform.position.y < 8.5 && playerObject.transform.position.y >= 4)
        {
            this.transform.position = new Vector3(0, 4, -10);
        }
        else
        {
            this.transform.position = new Vector3(0, 0, -10);
        }





        //make sure that camera not go down to dust....
        if (this.transform.position.y < 3.5)
        {
            //otherwise reset camera at initial position
            this.transform.position = new Vector3(0, 0, -10);
        }

        

    }
}
