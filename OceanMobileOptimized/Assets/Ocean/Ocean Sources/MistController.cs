using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Wind))]

/* Give thanks to the Unity3d community, I'm just one of many to work on this.
 * http://forum.unity3d.com/threads/16540-Wanted-Ocean-shader
 * You are free to use this as you please as long as you do some good deed on the day fist usage.
 * Any changes and improvements you make to this, although not required, would be great
 * This is new class for simulating mist by MindBlocks team
 * */

public class MistController : MonoBehaviour
{

	public GameObject mist;
	public GameObject mistLow;
	private Wind wind;
	private Transform player;
	
	void OnEnable ()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		wind  = gameObject.GetComponent<Wind>();
		StartCoroutine(AddMist());
	}

	IEnumerator AddMist ()
	{
		while(true){
			Vector3 pos = new Vector3(player.position.x + Random.Range(-30, 30), player.position.y + 5, player.position.z + Random.Range(-30, 30));
			if(wind.humidity >= 0.7f){
			    GameObject mistParticles = Instantiate(mist, pos, new Quaternion(0,0,0,0)) as GameObject;
				mistParticles.transform.parent = player;
			}else if(wind.humidity > 0.4f){
			    GameObject mistParticles = Instantiate(mist, pos, new Quaternion(0,0,0,0)) as GameObject;
			    mistParticles.transform.parent = player;
				yield return new WaitForSeconds(0.5f);
			}else{
			    GameObject mistParticles = Instantiate(mistLow, pos, new Quaternion(0,0,0,0)) as GameObject;
				mistParticles.transform.parent = player;
			    yield return new WaitForSeconds(1f);
			}
			yield return new WaitForSeconds(0.5f);
			
		}
	}
}
