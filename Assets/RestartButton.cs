public class RestartButton : UnityEngine.MonoBehaviour {
	public void Start () {
		this.Hide();
	}

	public void Show() {
		this.gameObject.SetActive(true);
	}

	public void Hide() {
		this.gameObject.SetActive(false);
	}

	public void OnClick() {
		UnityEngine.Debug.Log("HI! I'm OnClick");
		this.Hide();
		UnityEngine.SceneManagement.SceneManager.LoadScene( UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex );
	}
	
}
