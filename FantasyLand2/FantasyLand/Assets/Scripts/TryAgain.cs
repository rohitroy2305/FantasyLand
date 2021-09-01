using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TryAgain : MonoBehaviour
{
	public void TryGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -4);
	}
	public void MenuGame(){
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -5);
	}
}
