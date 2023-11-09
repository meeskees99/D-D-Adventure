using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class LoadCaveScene : NetworkBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.TryGetComponent(out ForceMovement move))
        {
            if (move != null)
            {
                LoadCaveSceneServerRpc();
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void LoadCaveSceneServerRpc(ServerRpcParams serverRpcParams = default)
    {
        NetworkManager.Singleton.SceneManager.LoadScene("Marnix Cave", UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
