using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject audioSourcePrefab;
    public enum InGameSoundEffectType
    {
        ENEMY_HIT,
    }

    [System.Serializable]
    private struct InGameSoundEffectInfo
    {
        public InGameSoundEffectType effectType;
        public AudioClip[] audioClips;
    }

    [SerializeField] private InGameSoundEffectInfo[] effectInfos;
    private Dictionary<InGameSoundEffectType, List<AudioClip>> inGameAudioClips = new Dictionary<InGameSoundEffectType, List<AudioClip>>();

    private Dictionary<InGameSoundEffectType, int> lastPlayedFrame = new Dictionary<InGameSoundEffectType, int>();

    private int fixedUpdateFrameCount = 0;
    private const int FIXED_FRAME_RESET_INTERVAL = 5;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < effectInfos.Length; i++)
        {
            List<AudioClip> clips = new List<AudioClip>(effectInfos[i].audioClips.Length);

            for (int j = 0; j < effectInfos[i].audioClips.Length; j++)
            {
                clips.Add(effectInfos[i].audioClips[j]);
            }

            inGameAudioClips.Add(effectInfos[i].effectType, clips);
            lastPlayedFrame.Add(effectInfos[i].effectType, -FIXED_FRAME_RESET_INTERVAL); 
        }
    }

    public void PlaySoundEffect(InGameSoundEffectType type, float volume)
    {
        if (fixedUpdateFrameCount - lastPlayedFrame[type] < FIXED_FRAME_RESET_INTERVAL)
        {
            return;
        }

        lastPlayedFrame[type] = fixedUpdateFrameCount;

        var audio = ObjectPoolManager.Instance.GetPoolableObject(audioSourcePrefab).GetComponent<AudioSource>();
        audio.volume = volume;

        if (inGameAudioClips[type].Count == 1)
        {
            audio.PlayOneShot(inGameAudioClips[type][0]);
        }
        else
        {
            int index = Random.Range(0, inGameAudioClips[type].Count);
            audio.PlayOneShot(inGameAudioClips[type][index]);
        }
    }

    private void FixedUpdate()
    {
        fixedUpdateFrameCount++;
    }
}
