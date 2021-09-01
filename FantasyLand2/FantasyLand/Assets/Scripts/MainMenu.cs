using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
	public void PlayGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
	public void QuitGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +8);
		StartCoroutine(ThankYou());
		Debug.Log("Game Quit !!");
		Application.Quit();
	}
	public void Instruction(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +7);
	}
	public void Back()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -7);
	}
	private IEnumerator ThankYou()
    {
        yield return new WaitForSeconds(4);
	}
}
