using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SubjectManager : MonoBehaviourPun
{
    [SerializeField] private AskManager askManager;

    [SerializeField] private GameObject askCanvas;
    [SerializeField] private GameObject subjectCanvas;

    [SerializeField] private Button button_1;
    [SerializeField] private Button button_2;
    [SerializeField] private Button button_3;
    [SerializeField] private Button button_4;
    [SerializeField] private Button button_5;

    private string textButton1 = "1";
    private string textButton2 = "2";
    private string textButton3 = "3";
    private string textButton4 = "4";
    private string textButton5 = "5";

    void Awake()
    {
        photonView.RPC(nameof(RpcSetTextToSubjects), RpcTarget.AllBuffered);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SwitchToAsk();
        }
    }

    [PunRPC]
    private void RpcSetTextToSubjects()
    {
        button_1.GetComponentInChildren<Text>().text = textButton1;
        button_2.GetComponentInChildren<Text>().text = textButton2;
        button_3.GetComponentInChildren<Text>().text = textButton3;
        button_4.GetComponentInChildren<Text>().text = textButton4;
        button_5.GetComponentInChildren<Text>().text = textButton5;
    }

    public void Pressed_1()
    {
        Debug.Log("1 Pressed");
    }

    public void Pressed_2()
    {
        Debug.Log("2 Pressed");
    }

    public void Pressed_3()
    {
        Debug.Log("3 Pressed");
    }

    public void Pressed_4()
    {
        Debug.Log("4 Pressed");
    }

    public void Pressed_5()
    {
        Debug.Log("5 Pressed");
    }

    private void SwitchToAsk()
    {
        askCanvas.SetActive(true);
        subjectCanvas.SetActive(false);
        this.enabled = false;
        askManager.enabled = true;
    }
}
