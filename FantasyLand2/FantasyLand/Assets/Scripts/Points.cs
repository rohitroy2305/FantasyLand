using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Points : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TextMeshProUGUI points;
    void Start()
    {
         points.text = PermanentUI.perm.coins.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
