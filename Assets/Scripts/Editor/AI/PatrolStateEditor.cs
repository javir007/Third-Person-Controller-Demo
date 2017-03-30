using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PatrolState))]
public class PatrolStateEditor : Editor {

	PatrolState patrolScript;

	string arrayDataString = "waypoints.Array.data[{0}]";
	string arraySizeString = "waypoints.Array.size";

	private void OnEnable(){
		patrolScript = (PatrolState)target;
	}

	//Recover the array of Vector3s from the serializeObjetc's properties
	private Vector3[] GetWayPoints(){
		int arrayCount = serializedObject.FindProperty(arraySizeString).intValue;
		Vector3[] wpArray = new Vector3[arrayCount];

		for(int i = 0; i < arrayCount; i++){
			wpArray[i] = serializedObject.FindProperty(string.Format(arrayDataString, i)).vector3Value;
		}
		return wpArray;
	}

	private void SetWayPoint(int index, Vector3 values){
		serializedObject.FindProperty(string.Format(arrayDataString, index)).vector3Value = values;
	}

	//Draw the default inspector
	public override void OnInspectorGUI(){
		serializedObject.Update();

		Vector3[] wpArray = GetWayPoints();
		EditorGUILayout.Space();

		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Number of WayPoints");
		int wpNumber = EditorGUILayout.IntField(wpArray.Length);
		if(GUI.changed){
			if(wpNumber < 2) wpNumber = 2; //waypoints need to be 2 or more
			serializedObject.FindProperty(arraySizeString).intValue = wpNumber;
			wpArray = GetWayPoints(); // need to get the array again because it might be shorter now
		}
		EditorGUILayout.EndHorizontal();

		GUILayout.Label("WayPoints", EditorStyles.boldLabel);
		for(int i = 0; i < wpArray.Length; i++){
			EditorGUILayout.BeginHorizontal();
			Vector3 res = EditorGUILayout.Vector3Field("WP " + 1, wpArray[i]);
			EditorGUILayout.EndHorizontal();

			//this fappends if the user has changed vector 3 values in the inspector
			if(GUI.changed){
				SetWayPoint(i, res);
			}
		}

		//Button to reset all WayPoints
		EditorGUILayout.Space();
		if(GUILayout.Button("Reset WayPoints")){
			patrolScript.Reset();
			EditorApplication.Beep();

			//force both the customInspector and the Scene view to show the changes
			Repaint();
			SceneView.RepaintAll();
		}

		//write the changes in the serializedObject
		serializedObject.ApplyModifiedProperties();
	}

	//draw the arrow gizmos for each waypoint
	public void OnSceneGUI(){

		Vector3[] wpArray = GetWayPoints();

		Handles.color = new Color32(156, 39, 176, 255);
		Handles.ArrowCap(0, wpArray[0], Quaternion.LookRotation(wpArray[1] - wpArray[0], Vector3.up), 1f);
		for(int i = 0; i < wpArray.Length; i++){

			//draw the gizmos
			EditorGUI.BeginChangeCheck();
			Vector3 gizmoPos = Handles.PositionHandle(wpArray[i], Quaternion.identity);

			//has the gizmo been moved?
			if(EditorGUI.EndChangeCheck()){
				SetWayPoint(i, gizmoPos);
			}

			//dotted lines and arrows
			Handles.SphereCap(0,wpArray[i],Quaternion.identity, 0.1f);
			if(i < wpArray.Length - 1){
				Handles.DrawDottedLine(wpArray[i], wpArray[i+1],0.5f);
			}else{
				Handles.DrawDottedLine(wpArray[i], wpArray[0], 0.5f);
			}

			serializedObject.ApplyModifiedProperties();
		}

		//numeric Labels
		Handles.BeginGUI();
		for(int i = 0; i < wpArray.Length; i++){
			Vector2 wpPoint = HandleUtility.WorldToGUIPoint(wpArray[i]);
			Rect guiRect = new Rect(wpPoint.x - 50f, wpPoint.y - 40f,100f, 20f);
			GUI.Box(guiRect, i.ToString());
		}
		Handles.EndGUI();
	}
}

