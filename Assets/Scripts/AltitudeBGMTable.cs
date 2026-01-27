using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "Audio/Altitude BGM Table")]
public class AltitudeBGMTable : ScriptableObject
{
    [System.Serializable]
    public class BGMData
    {
        public AudioClip bgm;
        public float minHeight;
        [Range(0f, 1f)]
        public float volume = 1.0f;
    }

    public List<BGMData> bgmList;
}
