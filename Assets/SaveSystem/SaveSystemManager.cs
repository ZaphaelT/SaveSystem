using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using System;

namespace SaveSystem
{

    public class SaveSystemManager : MonoBehaviour
    {
        public SaveSystemManager Instance { get; private set; }
        private SaveData _saveData;

        private void Awake()
        {
            Instance = this;
        }

        private void NewGame()
        {

        }

        public void LoadGame()
        {
            string fullPath = Path.Combine(Application.persistentDataPath, "save.sv");
            if (!File.Exists(fullPath))
            {
                _saveData = new SaveData();
            }
            else
            {
                try
                {
                    string dataJson = "";

                    using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                    {
                        using (StreamReader streamReader = new StreamReader(fileStream))
                        {
                            dataJson = streamReader.ReadToEnd();
                        }
                    }
                    _saveData = JsonUtility.FromJson<SaveData>(dataJson);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error saving game. {e}");
                }
            }

        }

        public void SaveGame()
        {
            foreach (ISaveSystemElement saveSystemElement in _saveSystemElements)
            {
                saveSystemElement.SaveData(_saveData);
            }

            string fullPath = Path.Combine(Application.persistentDataPath, "save.sv");
            try
            {
                Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
                string data = JsonUtility.ToJson(_saveData, true);

                using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
                {
                    using (StreamWriter streamWriter = new StreamWriter(fileStream))
                    {
                        streamWriter.Write(data);
                    }
                }
            }
            catch(Exception e)
            {
                Debug.LogError($"Error saving game. {e}");
            }
        }

    }
}
