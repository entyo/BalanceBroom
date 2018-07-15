public class ControlHand : UnityEngine.MonoBehaviour
{
    private UnityEngine.Gyroscope gyro;
	private const float SPEED = 3.0f;
	private UnityEngine.Vector3 lastA = UnityEngine.Vector3.zero;

    // Use this for initialization
    void Start()
    {
        gyro = UnityEngine.Input.gyro;
        gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
		// Acceleration
		var dir = UnityEngine.Vector3.zero;
        dir.x = -UnityEngine.Input.acceleration.x;
        dir.y = -UnityEngine.Input.acceleration.y;
		dir.z = 0;
		UnityEngine.Debug.Log(dir);

        if(dir.sqrMagnitude > 1){
            dir.Normalize();
        }

        dir *= UnityEngine.Time.deltaTime;

        transform.Translate(dir * SPEED);

		// gyro
        transform.rotation = new UnityEngine.Quaternion(-gyro.attitude.x, 0, -gyro.attitude.y, 1);
    }
}