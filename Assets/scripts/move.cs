using UnityEngine;
using System.Collections;

public class move : MonoBehaviour {
	float speedx=10f,speedy=10f;
	public GameObject player;
	// Update is called once per frame
	void Update () {
		player.transform.Rotate (new Vector3 (0, Input.GetAxis ("Mouse X") * speedx, 0));
		//this.transform.Rotate(new Vector3 (Input.GetAxis ("Mouse Y") *-1* speedx,0, 0));
		if(Input.GetKey(KeyCode.W)){
			player.transform.Translate(0,0,6f*(Time.deltaTime));
		}
		if(Input.GetKey(KeyCode.S)){
			player.transform.Translate(0,0,-6f*(Time.deltaTime));
		}
		if(Input.GetKey(KeyCode.A)){
			player.transform.Translate(-6f*(Time.deltaTime),0,0);
		}
		if(Input.GetKey(KeyCode.D)){
			player.transform.Translate(6f*(Time.deltaTime),0,0);
		}
        if (Input.GetKey(KeyCode.Space))
        {
            player.transform.Translate(0, 20f * (Time.deltaTime), 0);
            
        }
        if (Input.anyKey)
        {
            GameObject.Find("SocketIO").GetComponent<connection>().Moving();
        }
    }
}
