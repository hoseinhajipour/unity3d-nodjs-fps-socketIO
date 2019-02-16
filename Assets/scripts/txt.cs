
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class txt : MonoBehaviour {
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<Text> ().color = Color.black;
		this.gameObject.GetComponent<Text> ().text += "Welcome!";
	}
	void Update(){
		int num=0;
		foreach (char m in this.gameObject.GetComponent<Text>().text){
			num+=1;
		if(num>200){
				this.gameObject.GetComponent<Text>().text="";
				}
	}
			   }

}
