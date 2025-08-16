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

    private string textAsk = "� ��� ���� ��� ������ �����, ������� �'��� ������ ��������,\n" +
                            "�'��� ������ � ������� ���������� ������� � ����� ������� �����\n" +
                            "�������������� �����������, ������, ������������ �� ������,\n" +
                            "� �� ������ �����, ������ ����, ���� Budweiser'�, ���� ������� �����\n" +
                            "� ��� ������� ����������.";

    private void Start()
    {
        SetTextTo();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            Vector3 msgPos = new(0, 200, 0);
            HudMsg.SetHudMsg("Test", msgPos, 50, true, 3);
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
