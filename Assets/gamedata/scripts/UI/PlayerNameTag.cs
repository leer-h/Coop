using UnityEngine;
using Photon.Pun;
using TMPro;

public class PlayerNameTag : MonoBehaviourPun
{
    [SerializeField] private TextMeshPro nameText;
    [SerializeField] private Vector3 offset = new Vector3(0, 2f, 0);
    private Transform cam;

    private void Start()
    {
        cam = Camera.main.transform;

        nameText.text = photonView.Owner.NickName;

        if (photonView.IsMine)
        {
            nameText.gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        nameText.transform.position = transform.position + offset;

        Vector3 dir = nameText.transform.position - cam.position;
        if (dir.sqrMagnitude > 0.0001f)
        {
            nameText.transform.rotation = Quaternion.LookRotation(nameText.transform.position - cam.position);
        }
    }
}
