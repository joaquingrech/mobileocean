using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Ocean))]

/* Give thanks to the Unity3d community, I'm just one of many to work on this.
 * http://forum.unity3d.com/threads/16540-Wanted-Ocean-shader
 * You are free to use this as you please as long as you do some good deed on the day fist usage.
 * Any changes and improvements you make to this, although not required, would be great
 * This is new class added for wind speed calculation by MindBlocks team
 * */

public class Wind : MonoBehaviour {

	public float humidity;
	public float waveSacale = 4;
	private Ocean ocean;
	public bool forceStorm = false;
	
	//Humidity values
	public float prevValue = 0.1f;
    public float nextValue = 0.4f;
    private float prevTime = 1;
    private const float timeFreq = 1f/ 280f;
	
	IEnumerator Start() {
		ocean = gameObject.GetComponent<Ocean>();
		
		while (true) {
			yield return new WaitForEndOfFrame();
			
			if(forceStorm)
	            humidity = 1f;
		    else
    		    humidity = GetHumidity();
			
			if(ocean != null)
			    ocean.SetWaves(Mathf.Lerp(0, waveSacale, humidity));
		}
	}
	
	void ForceStorm(bool force)
    {
	    forceStorm = force;
    }
	
	//This function return smooth random value from 0 to 1, used for smooth waves scale calculation ®"MindBlocks"
	private float GetHumidity() {
        float time = Time.time;

        int intTime = (int)(time * timeFreq);
        int intPrevTime = (int)(prevTime * timeFreq);

        if (intTime != intPrevTime){
            prevValue = nextValue;
            nextValue = Random.value;
        }
        prevTime = time;
        float frac = time * timeFreq - intTime;

        return Mathf.SmoothStep(prevValue, nextValue, frac);
    }
}
