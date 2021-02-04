using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class SavesManager : MonoBehaviour
{
    [System.Serializable]
    public class SavedData
    {
        public int starsRecord = 0;
        public float moneyRecord = 0f;
    }

    public static SavesManager Instance = null;

    private int slotSelected = 0;

    private SavedData[] savedData = new SavedData[3] { null, null, null};
    private string[] dates = new string[3];

    private void Awake()
    {
        Instance = this;
        slotSelected = InmutableData.Instance.slotSelected;

        Load();
    }

    public void SlotSelected(int index)
    {
        slotSelected = index;
    }

    public void Save()
    {
        SavedData data = new SavedData();
        data.moneyRecord = GameManager.Instance.currentMoney;
        data.starsRecord = GameManager.Instance.stars;

        if (savedData[slotSelected] != null && data.starsRecord < savedData[slotSelected].starsRecord)
            return;

        Directory.CreateDirectory(Application.persistentDataPath + "/Saves");

        FileStream dataStream = new FileStream(Application.persistentDataPath + "/Saves/save" + slotSelected.ToString() + ".flanStudio", FileMode.Create);

        BinaryFormatter formatter = new BinaryFormatter();
        formatter.Serialize(dataStream, data);

        dataStream.Close();
    }

    public void Load()
    {
        for(int i = 0; i < 3; ++i)
        {
            string path = Application.persistentDataPath + "/Saves/save" + i.ToString() + ".flanStudio";

            if (File.Exists(path))
            {
                FileStream dataStream = new FileStream(path, FileMode.Open);
                BinaryFormatter formatter = new BinaryFormatter();

                savedData[i] = (SavedData)formatter.Deserialize(dataStream);
                System.DateTime dateTime = File.GetLastWriteTime(path);
                dates[i] = dateTime.Day.ToString("00") + " / " + dateTime.Month.ToString("00") + " / " + dateTime.Year.ToString() + " - " + dateTime.Hour.ToString("00") + " : " + dateTime.Minute.ToString("00");

                dataStream.Close();
            }
        }
    }

    public bool HasSavedData(int slotIndex)
    {
        if (savedData[slotIndex] != null)
            return true;
        return false;
    }

    public SavedData GetSavedData(int slotIndex, out string date)
    {
        if(HasSavedData(slotIndex))
        {
            date = dates[slotIndex];
            return savedData[slotIndex];
        }

        date = null;
        return null;
    }

    public void OnSlotSelected(int index)
    {
        slotSelected = index;
        InmutableData.Instance.slotSelected = slotSelected;
    }
}