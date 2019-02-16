using UnityEngine;
using System.Collections;
using SocketIO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using UnityEngine.UI;
public class connection : MonoBehaviour {
	public SocketIOComponent socket;
	public InputField NameField;
	private string check="";
	 public Player PlayerOBJ;
	public Text txt,listtxt,countList;
	[HideInInspector]
	public GameObject loginpanel;
	[HideInInspector]
	public Player current;
	public GameObject Buttns, listv;
    public Rigidbody bullet;
	// Use this for initialization
	void Start () {
		loginpanel = GameObject.FindWithTag ("LoginP");
		StartCoroutine (connect ());
		socket.On ("Connect", reciveCon);
		socket.On ("PLAY", OnPlay);
		socket.On ("OTHERPLAY", otherOnPlay);
		socket.On ("MOVING", otherMoving);
		socket.On ("LIST", OnlineIns);
		socket.On ("LEFTTHESERVER", DestroyDisconnectPlayers);
		socket.On ("GETLIST", GetList);
        socket.On("SHOOTING", OtherShooting);
    }
	public
		int i;
	public void OnClickOnLineList(){
		socket.Emit ("SENDLIST");
		listtxt.text = "";
		 i = 0;
		listv.SetActive (true);
		StartCoroutine (wait3 ());
	}
	void GetList(SocketIOEvent evt){
		i++;
		listtxt.text+= "UserId:"+evt.data.GetField("id").ToString()+" PlayerName:"+jsonToString (evt.data.GetField ("name").ToString (), "\"") + "\n";
		countList.text ="ONlineLIst ("+i.ToString ()+")";
	}
	IEnumerator wait3(){
		yield return new WaitForSeconds (2f);
		listtxt.text = "";
		listv.SetActive (false);
	}
	IEnumerator connect(){
		yield return new WaitForSeconds(0.5f);
		Dictionary<string,string> data = new Dictionary<string,string> ();
		data ["name"] = "check";
		socket.Emit("Connect",new JSONObject(data));
		yield return new WaitForSeconds(0.5f);
		if (check == "check") {
			txt.color=Color.green;
			txt.text+="\n"+ "Connected To The Server!";
			Debug.Log ("Connected To The Server!");

		} else {
			txt.color=Color.yellow;
			txt.text+="\n"+"Dar Hal Vasl Shodan Be Server...";
			Debug.Log ("Connecting...");
			refresh();
		}

	}
	void reciveCon(SocketIOEvent evt){
		check = jsonToString (evt.data.GetField ("name").ToString (), "\"");
	}
	string jsonToString(string target,string s){
		string[] neS = Regex.Split (target, s);
		return neS [1];
	}
	void refresh(){
		if (check == "") {
			StartCoroutine (connect ());
		} 
	}
	public void OnclickPlay(){
		if (NameField.text != "") {
			Dictionary<string,string> data = new Dictionary<string,string > ();
			data ["name"] = NameField.text;
			Vector3 position = new Vector3 (0, 0, 0);
			data ["position"] = position.x + "," + position.y + "," + position.z;
			socket.Emit ("PLAY", new JSONObject (data));
		} else {
			txt.text+="\n"+"Lotfan Nam Khod Ra Entekhab Konid!";
//			txt.text="this field must be not empty!";

		}
	}
	void OnPlay(SocketIOEvent evt){
		loginpanel.SetActive (false);
		Buttns.SetActive (true);
		txt.color=Color.black;
		txt.text+="\n"+"Salam "+jsonToString(evt.data.GetField("name").ToString(),"\"")+ " Welcome to Game!";
		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Welcome to Game!");
		GameObject CurrentPlayer = Instantiate (PlayerOBJ.gameObject, PlayerOBJ.postion, Quaternion.identity)as GameObject;
		 current = CurrentPlayer.GetComponent<Player> ();
		current.Changename (evt.data.GetField("id").ToString());
		current.name = jsonToString (evt.data.GetField ("name").ToString (), "\"");
		current.SetNameTxt(jsonToString (evt.data.GetField ("name").ToString (), "\""));
		current.transform.Find ("Text").GetComponent<TextMesh> ().color = Color.green;
		current.transform.Find ("Camera").gameObject.SetActive (true);
        current.GetComponent<gun>().active = true;
        current.transform.Find ("Camera").GetComponent<move> ().enabled = true;


	}
	void otherOnPlay(SocketIOEvent evt){
		txt.text+="\n"+"User: "+jsonToString(evt.data.GetField("name").ToString(),"\"")+ " Appaned to Game!";
		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Appaned to Game!");
		GameObject OtherPlayer = Instantiate (PlayerOBJ.gameObject, PlayerOBJ.postion, Quaternion.identity)as GameObject;
		Player Other = OtherPlayer.GetComponent<Player> ();
		Other.Changename (evt.data.GetField("id").ToString());
		Other.name = jsonToString (evt.data.GetField ("name").ToString (), "\"");
		Other.SetNameTxt(jsonToString (evt.data.GetField ("name").ToString (), "\""));
		Other.transform.Find ("Text").GetComponent<TextMesh> ().color = Color.red;
		Other.transform.Find ("Camera").gameObject.SetActive (false);
		Other.transform.Find ("Camera").GetComponent<move> ().enabled = false;
	}
	void OnlineIns(SocketIOEvent evt){
		txt.text+="\n"+"User: "+jsonToString(evt.data.GetField("name").ToString(),"\"")+ " Appaned to Game!";
		Debug.Log (jsonToString(evt.data.GetField("name").ToString(),"\"")+" Appaned to Game!");
		GameObject OtherPlayer = Instantiate (PlayerOBJ.gameObject, JsonToVec(jsonToString(evt.data.GetField("position").ToString(),"\"")), Quaternion.identity)as GameObject;
		Player Other = OtherPlayer.GetComponent<Player> ();
		Other.Changename (evt.data.GetField("id").ToString());
		Other.name = jsonToString (evt.data.GetField ("name").ToString (), "\"");
		Other.SetNameTxt(jsonToString (evt.data.GetField ("name").ToString (), "\""));
		Other.transform.Find ("Text").GetComponent<TextMesh> ().color = Color.red;
		Other.transform.Find ("Camera").gameObject.SetActive (false);
		Other.transform.Find ("Camera").GetComponent<move> ().enabled = false;
	}
	public void Moving(){
		Dictionary<string,string> data = new Dictionary<string,string > ();
		data ["id"] = current.pid ;
		Vector3 position = GameObject.Find (current.pid).transform.position;
        Vector3 rot = GameObject.Find(current.pid).transform.rotation.eulerAngles;
        data ["position"] = position.x + "," + position.y + "," + position.z;
        data["rotation"] = rot.x + "," + rot.y + "," + rot.z;

        socket.Emit ("MOVING", new JSONObject (data));
	}
    public void Shooting(Transform firepos, Quaternion firepos_rotation, string guntype,float bulletSpeed)
    {
        Dictionary<string, string> data = new Dictionary<string, string>();
        data["id"] = current.pid;
        data["firepos"] = firepos.position.x + "," + firepos.position.y + "," + firepos.position.z;
        data["firerot"] = firepos_rotation.x + "," + firepos_rotation.y + "," + firepos_rotation.z;
        data["guntype"] = guntype;
        data["bulletSpeed"] = bulletSpeed.ToString();

        socket.Emit("SHOOTING", new JSONObject(data));
    }

    void DestroyDisconnectPlayers(SocketIOEvent evt){
		Destroy (GameObject.Find(evt.data.GetField ("id").ToString ()));
		txt.text+="\n"+"Player: "+jsonToString(evt.data.GetField("name").ToString(),"\"")+" Left The From Server!";
	}
	void otherMoving(SocketIOEvent evt){
		GameObject PObj = GameObject.Find (evt.data.GetField ("id").ToString());
       Vector3 rot=JsonToVec(jsonToString(evt.data.GetField("rotation").ToString(), "\""));
        Quaternion new_rot = new Quaternion(rot.x, rot.y, rot.z,0f);
        Debug.Log(new_rot);

        //PObj.transform.rotation = Quaternion.RotateTowards(transform.rotation, new_rot, 1500 * Time.deltaTime);
        PObj.transform.eulerAngles = Vector3.Lerp(transform.rotation.eulerAngles, rot, 1500 * Time.deltaTime);
        //PObj.transform.rotation = Quaternion.Lerp(PObj.transform.rotation, new_rot, 1500 * (Time.deltaTime));
        PObj.transform.position=Vector3.Lerp(PObj.transform.position,JsonToVec(jsonToString(evt.data.GetField("position").ToString(),"\"")),1500*(Time.deltaTime));

       // PObj.transform.rotation = Quaternion.Euler(rot.z, rot.y, rot.z);
    }
    void OtherShooting(SocketIOEvent evt)
    {
        Vector3 firepos=JsonToVec(jsonToString(evt.data.GetField("firepos").ToString(), "\""));
        Vector3 firerot_ = JsonToVec(jsonToString(evt.data.GetField("firerot").ToString(), "\""));
        Quaternion firerot = new Quaternion(firerot_.x, firerot_.y, firerot_.z,0);
        float bulletSpeed= float.Parse(jsonToString(evt.data.GetField("bulletSpeed").ToString(), "\""));

        Rigidbody bulletClone = Instantiate(bullet, firepos, Quaternion.identity) as Rigidbody;
        bulletClone.velocity = transform.forward * bulletSpeed;

    }

    public Vector3 JsonToVec(string target){
		Vector3 newvector;
		string[] newS = Regex.Split (target, ",");
		newvector = new Vector3 (float.Parse (newS [0]), float.Parse (newS [1]), float.Parse (newS [2]));
		return newvector;
	}

}
 