using UnityEngine;
using System.Collections;

public class FPSCounter : MonoBehaviour {
	
	public Ocean ocean;
    public float updateInterval = 0.5F;
    private double lastInterval;
    private int frames = 0;
    private double fps;
	private bool guiEnabled=true;
    void Start() {
        lastInterval = Time.realtimeSinceStartup;
        frames = 0;
    }
	
	float LabelSlider(string labelText, Rect screenRect, float sliderValue, float min, float sliderMaxValue) {
		GUI.Label (screenRect, labelText+sliderValue);
		screenRect.x += screenRect.width; // <- Push the Slider to the end of the Label
		return GUI.HorizontalSlider(screenRect, sliderValue, min, sliderMaxValue);
	}
		
    void OnGUI() {
		//guiEnabled=GUI.Toggle(new Rect(70,0,130,20),guiEnabled,"Controls");
        GUILayout.Label("" + fps.ToString("f2"));
		/*if (guiEnabled) {
			ocean.renderReflection=GUI.Toggle(new Rect(20,20,130,20),ocean.renderReflection,"Reflection");
			ocean.renderRefraction=GUI.Toggle(new Rect(20,40,130,20),ocean.renderRefraction,"Refraction");
			ocean.scale=LabelSlider("Scale:",new Rect(20,60,130,20),ocean.scale,0.1f,5f);
			ocean.choppy_scale=LabelSlider("Choppy:",new Rect(20,80,130,20),ocean.choppy_scale,0f,15f);
			ocean.windx=LabelSlider("Wind:",new Rect(20,100,130,20),ocean.windx,0.1f,100f);
			ocean.normal_scale=(int)LabelSlider("Normal:",new Rect(20,120,130,20),ocean.normal_scale,0f,20f);
			ocean.normalStrength=LabelSlider("NStrength:",new Rect(20,140,130,20),ocean.normalStrength,0f,20f);
		//	ocean.uv_speed=LabelSlider("UV:",new Rect(20,160,130,20),ocean.uv_speed,0f,1f);
		}*/
		
    }
    void Update() {
        ++frames;
        double timeNow = Time.realtimeSinceStartup;
        if (timeNow > lastInterval + updateInterval) {
            fps = frames / (timeNow - lastInterval);
            frames = 0;
            lastInterval = timeNow;
        }
    }
}
