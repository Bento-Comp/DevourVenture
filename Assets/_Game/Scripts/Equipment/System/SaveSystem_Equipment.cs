using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem_Equipment
{

    public static void SaveEquipments(List<Equipment> headEquipmentList, List<Equipment> torsoEquipmentList, List<Equipment> toolEquipmentList)
    {
        BinaryFormatter formatter = new BinaryFormatter();

        string path = Application.persistentDataPath + "/EquipmentSave.txt";

        FileStream stream = new FileStream(path, FileMode.Create);

        InventorySaveData inventoryData = new InventorySaveData(headEquipmentList, torsoEquipmentList, toolEquipmentList);

        formatter.Serialize(stream, inventoryData);

        stream.Close();
    }


    public static InventorySaveData LoadEquipments()
    {
        string path = Application.persistentDataPath + "/EquipmentSave.txt";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();

            FileStream stream = new FileStream(path, FileMode.Open);

            InventorySaveData inventoryData = formatter.Deserialize(stream) as InventorySaveData;

            stream.Close();

            return inventoryData;
        }

        return null;
    }
}
