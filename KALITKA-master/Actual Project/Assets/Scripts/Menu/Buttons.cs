using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour {

	public GameObject m_on, m_off, medal;
	public Sprite layer_blue, layer_red;

	void Start () {		
		if (gameObject.name == "Music") {
			if (PlayerPrefs.GetString ("Music") == "no") {
				m_on.SetActive (false);
				m_off.SetActive (true);
			} else {
				m_on.SetActive (true);
				m_off.SetActive (false);
			}
		}
	}

	void OnMouseDown () {
		GetComponent <SpriteRenderer> ().sprite = layer_red;
	}

	void OnMouseUp () {
		GetComponent <SpriteRenderer> ().sprite = layer_blue;
	}

	void OnMouseUpAsButton() {
		Debug.Log("name = " + gameObject.name);

		if(PlayerPrefs.GetString ("Music") != "no")
			GameObject.Find ("Click Audio").GetComponent <AudioSource> ().Play ();
		switch (gameObject.name) {
		case "Play":
			SceneManager.LoadScene ("GameMenu");
			break;
		case "Rating":
			Application.OpenURL ("https://vk.com/parnisharabotyaga");
			break;
		case "Replay":
            SceneManager.LoadScene("Game");
			break;
		case "Home":
            SceneManager.LoadScene("Menu");
			break;
		case " Feedback":
			Application.OpenURL ("https://vk.com/marrakesht");
			break;
		case "Settings":
            SceneManager.LoadScene("Settings");
			break;
		case "Close":
            SceneManager.LoadScene("Menu");
			break;
		case "Music":
			if (PlayerPrefs.GetString ("Music") != "no") {
				PlayerPrefs.SetString ("Music", "no");
				m_on.SetActive (false);
				m_off.SetActive (true);
			} else {
				PlayerPrefs.SetString ("Music", "yes");
				m_on.SetActive (true);
				m_off.SetActive (false);
			}
			break;
		}
	}

	public void game_with_bot(){
		SceneManager.LoadScene("Game");
	}


	public void game_1_v_1(){
		SceneManager.LoadScene("Game");
	}
}