using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAnimation : MonoBehaviour
{
    Graphic graphic;

    // Start is called before the first frame update
    void Start()
    {
        graphic = GetComponent<Graphic>();
    }

    // Update is called once per frame
    void Update()
    {
        Color c = graphic.color;
        c.a = (Mathf.Sin(Time.time* 4f) + 1);
        graphic.color = c;
    }
}
