using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    // Start is called before the first frame update
    public Slider slider;
    public Gradient gradient;
    public Image fill;
    public static HealthBar Instance;
    //public int currenthealth;
    
    private void Awake()
    {     
        if(Instance==null)
        {
            DontDestroyOnLoad(gameObject);
            Instance=this;
        }
        else if(Instance !=this)
        {
            Destroy(gameObject);
        }
    }
    public void SetMaxHealth(int health)
    {
        Instance.slider.maxValue=health;
        Instance.slider.value=health;
        Instance.fill.color=gradient.Evaluate(1f);
    }
    public void SetHealth(int health )
    {
        Instance.slider.value=health;
        Instance.fill.color=gradient.Evaluate(Instance.slider.normalizedValue);
    }
}
