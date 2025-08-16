using UnityEngine;
using Photon.Pun;

public class HideMeshes : MonoBehaviourPun
{
    [SerializeField] private MeshRenderer meshRenderer;

    [SerializeField] private bool renderForLocal;
    [SerializeField] private bool renderForOthers;

    [SerializeField] private bool disableObj = false;

    void Start()
    {

        if (renderForOthers)
        {
            if (photonView.IsMine)
            {
                if (!disableObj)
                    meshRenderer.enabled = false;

                if (disableObj)
                    gameObject.SetActive(false);
            }
        }

        if (renderForLocal)
        {
            if (!photonView.IsMine)
            {
                if (!disableObj)
                    meshRenderer.enabled = false;

                if (disableObj)
                    gameObject.SetActive(false);
            }
        }
    }
}