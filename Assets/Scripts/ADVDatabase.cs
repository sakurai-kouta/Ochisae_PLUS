using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ADVDatabase", menuName = "Scriptable Objects/ADVDatabase")]
public class ADVDatabase : ScriptableObject
{
    [System.Serializable]
    public class ADVData
    {
        public int id = 0;
        public Sprite speaker;
        public Sprite listener;
        public string serif_JP;
        public string serif_EN;
    }
    public List<ADVData> ADVSceneList;

    public ADVData GetData(int _id)
    {
        foreach (ADVData data in ADVSceneList) 
        {
            if (data.id == _id) return data;
        }
        return null;
    }
}
