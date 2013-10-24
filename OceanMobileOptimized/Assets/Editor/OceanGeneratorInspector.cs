
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

/* Give thanks to the Unity3d community, I'm just one of many to work on this.
 * http://forum.unity3d.com/threads/16540-Wanted-Ocean-shader
 * You are free to use this as you please as long as you do some good deed on the day fist usage.
 * Any changes and improvements you make to this, although not required, would be great
 * This is the new aditor class for better ocean inspector by MindBlocks team
 * */

[CustomEditor(typeof(Ocean))]
public class OceanGeneratorInspector : Editor
{

	private bool ElementsExpand = false;
	
	public override void OnInspectorGUI ()
	{
		EditorGUIUtility.LookLikeControls(80f);
		Ocean ocean = target as Ocean;

	   EditorGUILayout.Separator();
		    Rect r = EditorGUILayout.BeginVertical();
            EditorGUI.DropShadowLabel(r, "Ocean generator");
            GUILayout.Space(16);
            EditorGUILayout.EndVertical();

		    EditorGUILayout.LabelField("Target/Player");
		    EditorGUILayout.BeginHorizontal();
		    EditorGUILayout.LabelField("Follow");
		    GUILayout.Space(-145);
		    ocean.followMainCamera = EditorGUILayout.Toggle(ocean.followMainCamera);
		    GUILayout.Space(-170);
		    ocean.player = (Transform) EditorGUILayout.ObjectField(ocean.player , typeof(Transform),true);
		    EditorGUILayout.EndHorizontal();

		    EditorGUILayout.LabelField("Ocean material");
			ocean.material = (Material) EditorGUILayout.ObjectField(ocean.material , typeof(Material),true);
		
		    EditorGUILayout.LabelField("Ocean shader");
			ocean.oceanShader = (Shader) EditorGUILayout.ObjectField(ocean.oceanShader , typeof(Shader),true);
		
		EditorGUILayout.Separator();
		
		    EditorGUILayout.LabelField("Chunks count");
			ocean.tiles = (int)EditorGUILayout.Slider(ocean.tiles, 1, 9);
		    
		    ocean.size = EditorGUILayout.Vector3Field("Chunk size",ocean.size);
		
		    EditorGUILayout.LabelField("Chunk poly count");
		    EditorGUILayout.BeginHorizontal();
		    EditorGUILayout.LabelField("Width");
		    GUILayout.Space(-80);
		    ocean.width = EditorGUILayout.IntField(ocean.width);
		    EditorGUILayout.LabelField("Height");
		    GUILayout.Space(-80);
		    ocean.height = EditorGUILayout.IntField(ocean.height);
		    EditorGUILayout.EndHorizontal();
			if (GUILayout.Button("Preview Dimensions"))
			{
			OceanGeneratorInspector.createTiles(ocean);
			}
		
		EditorGUILayout.Separator();
		
		    EditorGUILayout.LabelField("Scale");
			ocean.scale = (float)EditorGUILayout.Slider(ocean.scale, 0, 9);
		
		    EditorGUILayout.LabelField("Choppy scale");
			ocean.choppy_scale = (float)EditorGUILayout.Slider(ocean.choppy_scale, 0, 9);
		
		    EditorGUILayout.LabelField("Waves speed");
			ocean.speed = (float)EditorGUILayout.Slider(ocean.speed, 0.1f, 3f);
		
		    EditorGUILayout.LabelField("Wake distance");
			ocean.wakeDistance = (float)EditorGUILayout.Slider(ocean.wakeDistance, 1f, 15f);
		  
		EditorGUILayout.Separator();

		
		    EditorGUILayout.BeginHorizontal();
		    EditorGUILayout.LabelField("Render reflection");
		    GUILayout.Space(-30);
		    ocean.renderReflection = EditorGUILayout.Toggle(ocean.renderReflection);
		    GUILayout.Space(-30);
		    EditorGUILayout.EndHorizontal();
		    
		    EditorGUILayout.LabelField("Render textures size");
		    EditorGUILayout.BeginHorizontal();
		    EditorGUILayout.LabelField("Width");
		    GUILayout.Space(-80);
		    ocean.renderTexWidth = EditorGUILayout.IntField(ocean.renderTexWidth);
		    EditorGUILayout.LabelField("Height");
		    GUILayout.Space(-80);
		    ocean.renderTexHeight = EditorGUILayout.IntField(ocean.renderTexHeight);
		    EditorGUILayout.EndHorizontal();
		
		    EditorGUILayout.BeginHorizontal();
		    EditorGUILayout.LabelField("Render layers");
		    GUILayout.Space(-100);
		    int mask = LayerMaskField(ocean.renderLayers);

			if (ocean.renderLayers != mask)
			{
				ocean.renderLayers = mask;
			}
		    EditorGUILayout.EndHorizontal();
		
		EditorGUILayout.Separator();
		
		    EditorGUILayout.LabelField("Sun transform");
			ocean.sun = (Transform) EditorGUILayout.ObjectField(ocean.sun , typeof(Transform),true);
		    
		    ocean.SunDir = EditorGUILayout.Vector3Field("Sun direction",ocean.SunDir);
		
		EditorGUILayout.Separator();
		
		    ocean.waterType = (WaterType)EditorGUILayout.EnumPopup("Water type", ocean.waterType);
		
		    this.ElementsExpand = EditorGUILayout.Foldout(this.ElementsExpand, "Water colors");
            if(this.ElementsExpand) {
			    EditorGUILayout.LabelField("Normal water color");
                ocean.waterColor = EditorGUILayout.ColorField(ocean.waterColor);
			    EditorGUILayout.LabelField("Normal water surface color");
                ocean.surfaceColor = EditorGUILayout.ColorField(ocean.surfaceColor);
			    
			    EditorGUILayout.LabelField("Ice water color");
                ocean.iceWaterColor = EditorGUILayout.ColorField(ocean.iceWaterColor);
			    EditorGUILayout.LabelField("Ice water surface color");
                ocean.iceSurfaceColor = EditorGUILayout.ColorField(ocean.iceSurfaceColor);
			    
                EditorGUILayout.LabelField("Dark water color");
                ocean.darkWaterColor = EditorGUILayout.ColorField(ocean.darkWaterColor);
			    EditorGUILayout.LabelField("Dark water surface color");
                ocean.darkSurfaceColor = EditorGUILayout.ColorField(ocean.darkSurfaceColor);
			
			    EditorGUILayout.LabelField("Islands water color");
                ocean.islandsWaterColor = EditorGUILayout.ColorField(ocean.islandsWaterColor);
			    EditorGUILayout.LabelField("Islands water surface color");
                ocean.islandsSurfaceColor = EditorGUILayout.ColorField(ocean.islandsSurfaceColor);
            }
		EditorGUILayout.Separator();
				
		
		if (GUI.changed)
        {
            EditorUtility.SetDirty (ocean);
        }	
	}

	private static Mesh CreateMesh(float width, float height)
	{
		Mesh m = new Mesh();
		m.name = "ScriptedMesh";
		m.vertices = new Vector3[] {
			new Vector3(-width, 0.01f, -height),
			new Vector3(width, 0.01f, -height),
			new Vector3(width, 0.01f, height),
			new Vector3(-width, 0.01f, height)
		};
		m.uv = new Vector2[] {
			new Vector2 (0, 0),
			new Vector2 (0, 1),
			new Vector2(1, 1),
			new Vector2 (1, 0)
		};
		m.triangles = new int[] { 0, 2, 1, 0, 3, 2};
		m.RecalculateNormals();
		
		return m;
	}

	private static void createTiles(Ocean ocean) {

	//	int width=ocean.width;
	//	int height=ocean.height;
		Vector3 size=ocean.size;
		int tiles=ocean.tiles;

		if (ocean.parentTile!=null) {
			DestroyImmediate(ocean.parentTile);
			ocean.parentTile=null;
		}
		GameObject parentTile=new GameObject("ParentTile");
		GameObject tile;
		//int chDist; // Chebychev distance	
		for (int y=0; y<tiles; y++) {
			for (int x=0; x<tiles; x++) {
				//chDist = System.Math.Max (System.Math.Abs (tiles_y / 2 - y), System.Math.Abs (tiles_x / 2 - x));
				//chDist = chDist > 0 ? chDist - 1 : 0;
				float cy = y - Mathf.Floor(tiles * 0.5f);
				float cx = x - Mathf.Floor(tiles * 0.5f);
				tile = new GameObject ("WaterTile");
				Vector3 pos=tile.transform.position;
				pos.x = cx * size.x;
				pos.y = 0f;
				pos.z = cy * size.z;
				tile.transform.position=pos;
				MeshFilter m=tile.AddComponent<MeshFilter>();
				m.mesh=CreateMesh(size.x,size.z);

				tile.AddComponent ("MeshRenderer");
				tile.renderer.material = ocean.material;
				
				//Make child of this object, so we don't clutter up the
				//scene hierarchy more than necessary.
				tile.transform.parent = parentTile.transform;
				
				//Also we don't want these to be drawn while doing refraction/reflection passes,
				//so we'll add the to the water layer for easy filtering.
				tile.layer = LayerMask.NameToLayer ("Water");
			}
		}

		parentTile.transform.parent=ocean.transform;
		parentTile.transform.localPosition=Vector3.zero;
		ocean.parentTile=parentTile;
	}

	public static int LayerMaskField (string label, int mask, params GUILayoutOption[] options)
	{
		List<string> layers = new List<string>();
		List<int> layerNumbers = new List<int>();

		string selectedLayers = "";

		for (int i = 0; i < 32; ++i)
		{
			string layerName = LayerMask.LayerToName(i);

			if (!string.IsNullOrEmpty(layerName))
			{
				if (mask == (mask | (1 << i)))
				{
					if (string.IsNullOrEmpty(selectedLayers))
					{
						selectedLayers = layerName;
					}
					else
					{
						selectedLayers = "Mixed";
					}
				}
			}
		}

		if (Event.current.type != EventType.MouseDown && Event.current.type != EventType.ExecuteCommand)
		{
			if (mask == 0)
			{
				layers.Add("Nothing");
			}
			else if (mask == -1)
			{
				layers.Add("Everything");
			}
			else
			{
				layers.Add(selectedLayers);
			}
			layerNumbers.Add(-1);
		}

		layers.Add((mask == 0 ? "[+] " : "      ") + "Nothing");
		layerNumbers.Add(-2);

		layers.Add((mask == -1 ? "[+] " : "      ") + "Everything");
		layerNumbers.Add(-3);

		for (int i = 0; i < 32; ++i)
		{
			string layerName = LayerMask.LayerToName(i);

			if (layerName != "")
			{
				if (mask == (mask | (1 << i)))
				{
					layers.Add("[+] " + layerName);
				}
				else
				{
					layers.Add("      " + layerName);
				}
				layerNumbers.Add(i);
			}
		}

		bool preChange = GUI.changed;

		GUI.changed = false;

		int newSelected = 0;

		if (Event.current.type == EventType.MouseDown)
		{
			newSelected = -1;
		}

		if (string.IsNullOrEmpty(label))
		{
			newSelected = EditorGUILayout.Popup(newSelected, layers.ToArray(), EditorStyles.layerMaskField, options);
		}
		else
		{
			newSelected = EditorGUILayout.Popup(label, newSelected, layers.ToArray(), EditorStyles.layerMaskField, options);
		}

		if (GUI.changed && newSelected >= 0)
		{
			if (newSelected == 0)
			{
				mask = 0;
			}
			else if (newSelected == 1)
			{
				mask = -1;
			}
			else
			{
				if (mask == (mask | (1 << layerNumbers[newSelected])))
				{
					mask &= ~(1 << layerNumbers[newSelected]);
				}
				else
				{
					mask = mask | (1 << layerNumbers[newSelected]);
				}
			}
		}
		else
		{
			GUI.changed = preChange;
		}
		return mask;
	}

	public static int LayerMaskField (int mask, params GUILayoutOption[] options)
	{
		return LayerMaskField(null, mask, options);
	}
}