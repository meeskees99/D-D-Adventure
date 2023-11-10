using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.TextCore.Text;
using static UnityEditor.Progress;

public class SinglePlayer : MonoBehaviour
{
    public Material[] materials;
    public int currentTurnNmbr;
    public int currentRound;
    public bool DMTurn;
    [SerializeField] public List<GameObject> characters;
    [SerializeField] public List<Initiative> initiativeOrder;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetInitiativeOrder()
    {

        currentRound = 0;
        currentTurnNmbr = 0;

        SingleEClass[] combatEntities = FindObjectsOfType<SingleEClass>();
        foreach (var entity in combatEntities)
        {
            if (!characters.Contains(entity.gameObject))
            {
                characters.Add(entity.gameObject);
            }
        }
        if (initiativeOrder != null)
        {
            if (initiativeOrder.Count != 0)
            {
                initiativeOrder.Clear();
            }
            foreach (var character in characters)
            {
                character.GetComponent<MeshRenderer>().material = materials[0];
                Debug.Log($"{character.name} set to assigned");
                var count = 0;
                int _initiative = character.GetComponent<SingleEClass>().initiative + Random.Range(1, 20);
                if (initiativeOrder.Count != 0)
                {
                    foreach (var item in initiativeOrder)
                    {
                        
                        if (_initiative >= item.initiative)
                        {
                            var initiativeInstance = new Initiative { character = character };
                            initiativeInstance.initiative = _initiative;
                            
                            initiativeOrder.Insert(count, initiativeInstance);
                            break;
                        }
                        else
                        {
                            count++;
                            if (count == initiativeOrder.Count)
                            {
                                var initiativeInstance = new Initiative { character = character };
                                initiativeInstance.initiative = _initiative;
                                initiativeOrder.Add(initiativeInstance);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var initiativeInstance = new Initiative { character = character };
                    initiativeInstance.initiative = _initiative;
                    initiativeOrder.Add(initiativeInstance);
                }
                //Debug.Log("The amount of characters is: " + initiativeOrder.Count);
                //Debug.Log("Initiative of " + character + " is: " + _initiative);
                
            }
        }
        StartCombat();
    }

    private void StartCombat()
    {
        currentTurnNmbr = 0;
        initiativeOrder[currentTurnNmbr].character.GetComponent<MeshRenderer>().material = materials[2];
        foreach (var item in initiativeOrder)
        {
            item.character.GetComponent<SingleEClass>().playerCamera.enabled = false;
        }
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().isTurn = true;
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().playerCamera.enabled = true;
        Debug.Log($"Current turn number: {currentTurnNmbr}");
    }
    public void EndTurn()
    {
        Cursor.lockState = CursorLockMode.Locked;
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().isTurn = false;
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().playerCamera.enabled = false;
        initiativeOrder[currentTurnNmbr].character.GetComponent<MeshRenderer>().material = materials[3];
        currentTurnNmbr++;
        
        if (currentTurnNmbr > initiativeOrder.Count - 1)
        {
            foreach(var item in initiativeOrder)
            {
                item.character.GetComponent<MeshRenderer>().material = materials[1];
            }
            currentTurnNmbr = 0;
            currentRound++;
        }
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().isTurn = true;
        initiativeOrder[currentTurnNmbr].character.GetComponent<SingleEClass>().playerCamera.enabled = true;
        initiativeOrder[currentTurnNmbr].character.GetComponent<MeshRenderer>().material = materials[2];
        Debug.Log($"Current turn number: {currentTurnNmbr}");
        
    }
    [System.Serializable]
    public class Initiative
    {
        public GameObject character;
        public int initiative;
    }

}
