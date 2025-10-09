using System.Collections.Generic;
using UnityEngine;

namespace SimpleAudioSystem
{
    [System.Serializable]
    public struct SFXInfo
    {
        public AudioData_SO audioData;
        public float audioStep;
        public float volumeScale;
    }
    public class AE_AudioHelper : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float overallVolume = 1f;
        [SerializeField] private SFXInfo[] audioArray;

        private Dictionary<string, float> audioTimeDict;

        #region Animation Event
        public void AE_PlaySFX(int index)
        {
            if (index < 0 || index >= audioArray.Length)
            {
                Debug.LogError($"Audio index {index} is out of range!");
                return;
            }
            var audioInfo = audioArray[index];
            var audioData = audioInfo.audioData;

            if (audioTimeDict == null)
                audioTimeDict = new Dictionary<string, float>();
            if (!audioTimeDict.ContainsKey(audioData.name))
            {
                audioTimeDict.Add(audioData.name, Time.time);
                AudioManager.Instance.PlaySFX(audioData.name, audioInfo.volumeScale * overallVolume);
            }
            else
            {
                if (Time.time - audioTimeDict[audioData.name] >= audioInfo.audioStep)
                {
                    audioTimeDict[audioData.name] = Time.time;
                    AudioManager.Instance.PlaySFX(audioData.name, audioInfo.volumeScale * overallVolume);
                }
            }
        }
        public void AE_PlaySFX(Object audioData)
        {
            if (audioData is AudioData_SO data)
            {
                AudioManager.Instance.PlaySFX(data.name, 1);
            }
            else
            {
                Debug.LogError("Passed parameter is not of type AudioData_SO");
            }
        }
        #endregion
    }
}
