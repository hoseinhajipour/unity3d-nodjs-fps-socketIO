using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	public string name,pid;
	public Vector3 postion=new Vector3(0,0,0);
	public TextMesh TxtNameSet;
	// Use this for initialization
	public void Changename (string Pname) {
		this.gameObject.name = Pname;
		pid = Pname;
	}
	public void SetNameTxt(string name){
		TxtNameSet.text = name;
	}

}
