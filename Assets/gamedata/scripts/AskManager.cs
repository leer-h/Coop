using UnityEngine;
using UnityEngine.UI;

public class AskManager : MonoBehaviour
{
    [SerializeField] private Text askText;

    [SerializeField] private Button button_a;
    [SerializeField] private Button button_b;
    [SerializeField] private Button button_c;
    [SerializeField] private Button button_d;

    [SerializeField] private Text buttonText_a;
    [SerializeField] private Text buttonText_b;
    [SerializeField] private Text buttonText_c;
    [SerializeField] private Text buttonText_d;

    private string textButtonA = "1";
    private string textButtonB = "2";
    private string textButtonC = "3";
    private string textButtonD = "4";

    private string textAsk = "У нас було два пакети трави, сімдесят п'ять пігулок мескаліну,\n" +
                            "п'ять листків з марками сильнодіючої кислоти й безліч усякого штибу\n" +
                            "різнокольорових стимуляторів, транків, метамфетамінів та екстазі,\n" +
                            "а ще кварта текіли, кварта рому, ящик Budweiser'а, пінта чистого ефіру\n" +
                            "й два десятки амілонітритів.";

    private void Start()
    {
        SetTextTo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            button_a.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            button_b.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            button_c.onClick.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            button_d.onClick.Invoke();
        }
    }

    private void SetTextTo()
    {
        askText.text = textAsk;
        buttonText_a.text = textButtonA;
        buttonText_b.text = textButtonB;
        buttonText_c.text = textButtonC;
        buttonText_d.text = textButtonD;
    }

    public void Pressed_A()
    {
        Debug.Log("A Pressed");
    }

    public void Pressed_B()
    {
        Debug.Log("B Pressed");
    }

    public void Pressed_C()
    {
        Debug.Log("C Pressed");
    }

    public void Pressed_D()
    {
        Debug.Log("D Pressed");
    }
}
