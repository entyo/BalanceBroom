public class GameOver : UnityEngine.MonoBehaviour {
	public void Start () {
		this.gameObject.GetComponent<UnityEngine.UI.Text>().enabled = false;
	}
	
	public void End () {
		this.gameObject.GetComponent<UnityEngine.UI.Text>().enabled = true;
	}
}
