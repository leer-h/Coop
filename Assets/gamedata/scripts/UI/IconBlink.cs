using UnityEngine;
using UnityEngine.UI;

public class IconBlink : MonoBehaviour
{
    private Image _image;

    private int _color = 0;
    void Start()
    {
        _image = GetComponent<Image>();
    }
    void FixedUpdate()
    {
       var color =  _image.color.a;

       if (_color == 1)
       {
           if (color >= 0.9f) _color = 0;
       }
       else
       {
           if (color < 0.1f) _color = 1;
       }
       
   
       color = Mathf.Lerp(color, _color, Time.deltaTime * 1);

       _image.color = new Color(0.27f, 0.80f, 1, color);
    }
}
