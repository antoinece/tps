using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class Counter : MonoBehaviour
{
    private int _points; 
    public TextMeshProUGUI score;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void AddPoints()
    {
        _points++;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log(_points.ToString());
        score.text = _points.ToString();
    }
}
