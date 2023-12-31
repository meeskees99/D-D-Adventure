using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class GameManager : NetworkBehaviour
{
    [SerializeField] GameObject dmCanvas;
    [SerializeField] GameObject playerCanvas;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (dmCanvas == null || playerCanvas == null) return;

        if (NetworkManager.Singleton.LocalClient.ClientId == 0)
        {
            dmCanvas.SetActive(true);
            playerCanvas.SetActive(false);
            Destroy(playerCanvas);
        }
        else
        {
            dmCanvas.SetActive(false);
            playerCanvas.SetActive(true);
            Destroy(dmCanvas);
        }
    }
}
