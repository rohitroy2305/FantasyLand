using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Restart : MonoBehaviour
{
	public void RestartGame(){
		//PermanentUI perm2 = new PermanentUI();
		//perm2.Reset();
		PermanentUI.perm.life=5;
		PermanentUI.perm.coins = 0;
		PermanentUI.perm.levelNo=0;
        PermanentUI.perm.Level.text = PermanentUI.perm.levelNo.ToString();
		PermanentUI.perm.lifeCount.text = PermanentUI.perm.life.ToString();
		PermanentUI.perm.coinText.text = PermanentUI.perm.coins.ToString();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -5);
	}
	public void MenuGame(){
		PermanentUI.perm.life=5;
		PermanentUI.perm.coins = 0;
		PermanentUI.perm.levelNo=0;
        PermanentUI.perm.Level.text = PermanentUI.perm.levelNo.ToString();
		PermanentUI.perm.lifeCount.text = PermanentUI.perm.life.ToString();
		PermanentUI.perm.coinText.text = PermanentUI.perm.coins.ToString();
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex -6);
	}
}
