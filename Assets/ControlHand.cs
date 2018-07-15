public class ControlHand : UnityEngine.MonoBehaviour
{
    private UnityEngine.Gyroscope gyro;

    // Use this for initialization
    void Start()
    {
        gyro = UnityEngine.Input.gyro;
        gyro.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = new UnityEngine.Quaternion(gyro.attitude.x, 0, gyro.attitude.y, 1);
        UnityEngine.Debug.Log(gyro.attitude);
    }
}