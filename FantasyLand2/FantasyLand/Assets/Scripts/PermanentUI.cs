using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityStandardAssets.CrossPlatformInput;
public class PermanentUI : MonoBehaviour
{
    //Player stats
    public int coins = 0;
    public int currenthealth ;
    public int maxhealth=10 ;
    public int life=5;
    public int levelNo=0;
    public TextMeshProUGUI coinText;
    //public TextMeshProUGUI healthAmount;
    public TextMeshProUGUI lifeCount;
    public TextMeshProUGUI Level;

    public static PermanentUI perm;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);      
        //singleton
        if (!perm)
        {
            perm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void Reset()
    {
        life = 5;
        //healthAmount.text = health.ToString();
        lifeCount.text = life.ToString();
        coins = 0;
        coinText.text = coins.ToString();
    }
}
