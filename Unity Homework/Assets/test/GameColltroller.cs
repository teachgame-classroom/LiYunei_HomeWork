
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameColltroller : MonoBehaviour
{
    public Transform hpWorldCanvas;
    private int counter = 0;
    private float fillMeter = 1.0f;

    void Start()
    {
        
    }

    void Update()
    {
        Vector3 camToHp = (hpWorldCanvas.position - Camera.main.transform.position).normalized;
        hpWorldCanvas.forward = camToHp;

        if (Input.GetKey(KeyCode.W))
        {
            fillMeter += Time.deltaTime * 0.2f;
        }
        else
        {
            fillMeter -= Time.deltaTime * 0.2f; 
        }

        fillMeter = Mathf.Clamp01(fillMeter);

        GameObject canvas = GameObject.Find("Canvas");
        Transform meterTrans = canvas.transform.Find("FillBar");

        meterTrans.localScale = Vector3.right * fillMeter + Vector3.up + Vector3.forward;

        Color meterColor = Color.Lerp(Color.red,Color.green,fillMeter);
        meterTrans.GetComponent<Image>().color = meterColor;

        Debug.Log("Meter:" + fillMeter);
    }

    public void SayHello()
    {
        Debug.Log("Hello");
    }

    public void OnValueChanged(string value)
    {
        Debug.Log("Change text:"+value);
    }

    public void OnEndEdit(string value)
    {
        Debug.Log("End test:" + value);
    }

    public void Count()
    {
        counter++;
        Debug.Log("Count:" + counter);

        GameObject canvas= GameObject.Find("Canvas");
        Transform textTrans = canvas.transform.Find("Text");
        Text text = textTrans.GetComponent<Text>();

        text.text = counter.ToString();
    }
}
