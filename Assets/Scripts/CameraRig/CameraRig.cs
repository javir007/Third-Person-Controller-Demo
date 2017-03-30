using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraRig : MonoBehaviour {

	private Animator anim;
	public Transform target;
	public bool autoTargetPlayer;
	public LayerMask wallLayers;

	[System.Serializable]
	public class CameraSettings{
		[Header("Camera Options")]
		public float mouseXSensitivity = 5.0f;
		public float mouseYSensitivity = 5.0f;
		public float minAngle = -30.0f;
		public float maxAngle = 70.0f;
		public float rotationSpeed = 5.0f;
		public float maxCheckDist = 0.1f;

		[Header("Zoom")]
		public float fieldofView = 70.0f;
		public float zoomFieldofView = 10.0f;
		public float zoomSpeed = 3.0f;

		[Header("Visual Options")]
		public float hideMeshWhenDistance = 0.5f;

		public float camPositionOffset;
		public Vector3 PositionCamera;
		
	}
	[SerializeField] 
	public CameraSettings cameraSettings;

	[System.Serializable]
	public class InputSettings{
		public string verticalAxis = "Mouse X";
		public string horizontalAxis = "Mouse Y";
		

	}
	[SerializeField] public InputSettings input;

	[System.Serializable]
	public class MovementSettings{
		public float movementLerpSpeed = 5.0f;
	}
	[SerializeField] public MovementSettings movement;


	Transform pivot;
	Camera mainCamera;
	float newX = 0.0f;
	float newY = 0.0f;
	float newZoom = -30f;
	// Use this for initialization
	void Start () {
		mainCamera = Camera.main;
		pivot = transform.GetChild(0);
		anim = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(target){
			if(Application.isPlaying){
				RotateCamera();
				//CheckMeshRenderer();
				Zoom(newZoom);
			}
		}
	}

	void LateUpdate(){
		if(!target){
			TargetPlayer();
		}else{
			Vector3 targetPosition = target.position;
			Quaternion targetRotation = target.rotation;
			FollowTarget(targetPosition, targetRotation);
		}
	}
	//Finds the player GameObject and sets it as target
	void TargetPlayer(){
		if(autoTargetPlayer){
			GameObject player = GameObject.FindGameObjectWithTag("Player");

			if(player){
				Transform playerT = player.transform;
				target = playerT;
			}
			
		}
	}
	//Following the target with Time.deltaTime smoothly
	void FollowTarget(Vector3 targetPosition, Quaternion targetRotation){
		if(!Application.isPlaying){
			transform.position = targetPosition;
			transform.rotation = targetRotation;
		}else{
			Vector3 newPos = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * movement.movementLerpSpeed);
			transform.position = newPos;
		}
	}

    //Rotate the Camera with input
    void RotateCamera(){
        if(!pivot)
			return;

		newX += cameraSettings.mouseXSensitivity * Input.GetAxis(input.verticalAxis);
		//newY += cameraSettings.mouseYSensitivity * Input.GetAxis(input.horizontalAxis);
		newZoom += cameraSettings.mouseYSensitivity * Input.GetAxis("Mouse ScrollWheel");

		newZoom = Mathf.Clamp(newZoom,-50,10);

		Vector3 eulerAngleAxis = new Vector3();
		//eulerAngleAxis.x = newY;
		eulerAngleAxis.y = newX;

		//newX = Mathf.Repeat(newX, 360);
		newY = Mathf.Clamp(newY, cameraSettings.minAngle, cameraSettings.maxAngle);

		Quaternion newRotation = Quaternion.Slerp(pivot.localRotation, Quaternion.Euler(eulerAngleAxis), Time.deltaTime * cameraSettings.rotationSpeed);

		pivot.localRotation = newRotation;
    }

    
	//This move the camera forward when we hit a wall
	void moveCamUp(RaycastHit hit, Vector3 pivotPos, Vector3 dir, Transform cameraT){
		float hitDist = hit.distance;
		Vector3 sphereCastCenter = pivotPos + (dir.normalized * hitDist);
		Vector3 cameraTPose = cameraT.position;
		cameraT.position = sphereCastCenter;
	}

	//Position the camera localPosition to a givien location
	void PositionCamera(Vector3 cameraPos){
		if(!mainCamera)
			return;

		Transform mainCamT = mainCamera.transform;
		Vector3 mainCamPos = mainCamT.localPosition;
		Vector3 newPos = Vector3.Lerp(mainCamPos, cameraPos, Time.deltaTime * movement.movementLerpSpeed);
		mainCamT.localPosition = newPos;
	}

	//Hides the mesh targets mesh renderer when too close
	void CheckMeshRenderer(){
		if(!mainCamera || ! target)
			return;

		SkinnedMeshRenderer[] meshes = target.GetComponentsInChildren<SkinnedMeshRenderer>();
		Transform mainCamT = mainCamera.transform;
		Vector3 mainCamPos = mainCamT.position;
		Vector3 targetPos = target.position;
		float dist = Vector3.Distance(mainCamPos, targetPos + target.up);

		if(meshes.Length > 0){
			for(int i = 0; i < meshes.Length; i++){
				if(dist <= cameraSettings.hideMeshWhenDistance){
					meshes[i].enabled = false;
				}else{
					meshes[i].enabled = true;
				}
			}
		}

	}

	//Zooms the camera in and out
	void Zoom(float zoom){
		if(!mainCamera)
			return;

			anim.SetFloat("Zoom", zoom);
		
	}
}
