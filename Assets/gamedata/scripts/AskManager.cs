using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using DG.Tweening;

public class AskManager : MonoBehaviourPun
{
    [SerializeField] private SubjectManager subjectManager;

    [SerializeField] private GameObject askCanvas;
    [SerializeField] private GameObject subjectCanvas;

    [SerializeField] private Text askText;

    [SerializeField] private Button button_a;
    [SerializeField] private Button button_b;
    [SerializeField] private Button button_c;
    [SerializeField] private Button button_d;

    private string textButtonA = "1";
    private string textButtonB = "2";
    private string textButtonC = "3";
    private string textButtonD = "4";

    private string textAsk = "У нас було два пакети трави, сімдесят п'ять пігулок мескаліну,\n" +
                             "п'ять листків з марками сильнодіючої кислоти й безліч усякого штибу\n" +
                             "різнокольорових стимуляторів, транків, метамфетамінів та екстазі,\n" +
                             "а ще кварта текіли, кварта рому, ящик Budweiser'а, пінта чистого ефіру\n" +
                             "й два десятки амілонітритів.";

    private void Awake()
    {
        photonView.RPC(nameof(RpcSetTextTo), RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            //Vector3 msgPos = new(0, 200, 0);
            //HudMsg.SetHudMsg("Test", msgPos, 50, true, 3);
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            //photonView.RPC(nameof(RpcSetTextTo), RpcTarget.AllBuffered);
            SwitchToSubject();
        }
    }

    [PunRPC]
    private void RpcSetTextTo()
    {
        askText.text = textAsk;
        button_a.GetComponentInChildren<Text>().text = textButtonA;
        button_b.GetComponentInChildren<Text>().text = textButtonB;
        button_c.GetComponentInChildren<Text>().text = textButtonC;
        button_d.GetComponentInChildren<Text>().text = textButtonD;
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

    private void SwitchToSubject()
    {
        subjectCanvas.SetActive(true);
        askCanvas.SetActive(false);
        this.enabled = false;
        subjectManager.enabled = true;
    }
}
