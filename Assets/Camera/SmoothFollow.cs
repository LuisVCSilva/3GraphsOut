using UnityEngine;

	public class SmoothFollow : MonoBehaviour
	{

		public float buttonValue = 0.0f;

		// The target we are following
		[SerializeField]
		private Transform target;
		// The distance in the x-z plane to the target
		[SerializeField]
		private float distance = 10.0f;
		// the height we want the camera to be above the target
		//[SerializeField]
		//private float height = 5.0f;

		[SerializeField]
		private float rotationDamping;
		[SerializeField]
		private float heightDamping;

		// Use this for initialization
		void Start() {
		}


		// Update is called once per frame
		void LateUpdate()
		{
			GameObject grafo = GameObject.FindWithTag("Grafo");
			GameObject centroid = GameObject.Find("centroid");
			target = centroid!=null ? centroid.transform : null;

			// Early out if we don't have a target
			if (!target || !grafo)
			{
				distance = Mathf.Lerp(distance,10.0f,0.3f);
				return;
			}
			else if(grafo.GetComponent<Grafo>()!=null)
			{
			distance = Mathf.Lerp(distance,Mathf.Sqrt(grafo.GetComponent<Grafo>().getArea())+5.0f,Time.deltaTime*5);
			// Calculate the current rotation angles
			var wantedRotationAngle = target.eulerAngles.y;

			var wantedHeight = target.position.y;

			var currentRotationAngle = transform.eulerAngles.y;
			var currentHeight = 0.0f;

			// Damp the rotation around the y-axis
			currentRotationAngle = Mathf.LerpAngle(currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);

			// Damp the height
			currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * Time.deltaTime);

			// Convert the angle into a rotation
			//var currentRotation = Quaternion.Euler(0, currentRotationAngle, 0);

			// Set the position of the camera on the x-z plane to:
			// distance meters behind the target
			//transform.position = target.position;
			//transform.position -= currentRotation * Vector3.forward * distance;

			// Set the height of the camera
			//transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);

			// Always look at the target
			transform.LookAt(target);
			transform.RotateAround(Vector3.zero, Vector3.up, buttonValue*10.0f);
		}
	}
}
