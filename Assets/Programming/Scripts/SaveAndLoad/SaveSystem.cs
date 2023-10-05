using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using UnityEngine;

public class SaveSystem : MonoBehaviour
{
    public InitiativeHandler initiativeHandler;
    public CombatHandler combbatHandler;
    public CampaignSlots campaignSlots;
    private string path;
    public int slotToLoad;
    private void Start()
    {
        path = Application.dataPath + "/DataXml.data";
        Save();
    }

    public void Load()
    {
        var serializer = new XmlSerializer(typeof(CampaignSlots));
        var stream = new FileStream(path, FileMode.Open);
        var loadedData = serializer.Deserialize(stream) as CampaignSlots;
        stream.Close();
    }

    public void Save()
    {
        campaignSlots.savedData[slotToLoad] = SetDataToSave(slotToLoad);
        var serializer = new XmlSerializer(typeof(CampaignSlots));
        var stream = new FileStream(path, FileMode.Create);
        serializer.Serialize(stream, campaignSlots);
        stream.Close();
    }

    public SavedData SetDataToSave(int slotToLoad)
    {

        campaignSlots.savedData[slotToLoad].playerStats.Add((1,1,1,1,1,1,1,1));
        return campaignSlots.savedData[slotToLoad];
    }
}
