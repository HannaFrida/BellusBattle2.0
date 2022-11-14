using System.Collections.Generic;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(Camera))]
public class CameraFocus : MonoBehaviour
{
	[SerializeField, Tooltip("Offset relative to its' starting position.")]
	private Vector3 offset;
	[SerializeField, Tooltip("How smooth the camera repositions itself.")]
	private float smoothTime = 0.5f;
	[SerializeField, Tooltip("The furthest out the camera can zoom out.")]
	private float minZoom = 40.0f;
	[SerializeField, Tooltip("The closest in the camera can zoom in.")]
	private float maxZoom = 10.0f;
	[SerializeField] 
	private float zoomLimiter = 50.0f;
	
	private List<Transform> _targets = new();
	private Vector3 _velocity;
	private Camera _cam;
	private bool _isOrthographic;
	private Vector3 _startPos;

	private void Start(){
		_cam = GetComponent<Camera>();
		
		//Register to events.
		PlayerSpawnEvent.RegisterListener(AddTarget);
		PlayerDeathEvent.RegisterListener(RemoveTarget);
	}

	private void LateUpdate()
	{
		Bounds bounds = GetTargetsBounds();
		Reposition(bounds);
		Focus(bounds);
	}

    /*
     * Adds a target for the camera to follow.
     */
	private void AddTarget(PlayerSpawnEvent pse)
	{
		if (_targets.Contains(pse.playerGo.transform)){return;}
		_targets.Add(pse.playerGo.transform);
	}
	
	/*
	 * Removes a target for the camera to stop following.
	 */
	private void RemoveTarget(PlayerDeathEvent pde){
		if (_targets.Contains(pde.kille.transform) == false){return;}
		_targets.Remove(pde.kille.transform);
	}

	/*
	 * Repositions the camera over time to the center of the bounds offset with the given offset.
	 */
	private void Reposition(Bounds bounds)
	{
		Vector3 median = GetMedian(bounds);
		Vector3 newPos = median + offset;
		transform.position = Vector3.SmoothDamp(transform.position, newPos, ref _velocity, smoothTime);
	}

	/*
	 * Focuses the camera over time based on the width of the bounding box scaled with the zoom limiter.
	 */
	private void Focus(Bounds bounds)
	{
		float newZoom = Mathf.Lerp(maxZoom, minZoom, (bounds.size.x + bounds.size.y) / zoomLimiter);
		if (_isOrthographic){
			_cam.orthographicSize = Mathf.Lerp(_cam.orthographicSize, newZoom, Time.deltaTime);
		}
		else{
			_cam.fieldOfView = Mathf.Lerp(_cam.fieldOfView, newZoom, Time.deltaTime);
		}
	}

	/*
	 * Returns the center of the bounding box.
	 */
	private Vector3 GetMedian(Bounds bounds)
	{
		if (_targets.Count == 1)
		{
			return _targets[0].position;
		}
		
		return bounds.center;
	}

	/*
	 * Gets the a bounding box encapsulating all targets.
	 */
	private Bounds GetTargetsBounds(){
		Vector3 centerPos = _targets.Count == 0 ? Vector3.zero : _targets[0].position;
		
		Bounds bounds = new Bounds(centerPos, Vector3.zero);
		for (int i = 0; i < _targets.Count; i++)
		{
			bounds.Encapsulate(_targets[i].position);
		}
		return bounds;
	}

	private void OnDestroy(){
		//Unregister from the event.
		PlayerSpawnEvent.UnregisterListener(AddTarget);
		PlayerDeathEvent.UnregisterListener(RemoveTarget);
	}

	private void OnValidate(){
		_isOrthographic = gameObject.GetComponent<Camera>().orthographic;
	}
}