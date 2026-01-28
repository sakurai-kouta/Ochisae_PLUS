using UnityEngine;
using System.Collections.Generic;

public class NPCSerifController : MonoBehaviour
{
    [System.Serializable]
    public class NPCData
    {
        public NpcController npc;
        public string serif_JP;
        public string serif_EN;
    }
    public List<NPCData> npcList;

    private void Start() 
    {
        foreach (NPCData data in npcList)
        {
            if (data.npc == null) continue;
            data.npc.SetSerif(data.serif_JP, data.serif_EN);
        }
    }
}

