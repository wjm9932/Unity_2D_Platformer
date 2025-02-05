using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;
    [SerializeField] private GameObject audioSourcePrefab;
    [SerializeField] private GameObject loopAudioSourcePrefab;
    public enum InGameSoundEffectType
    {
        PLAYER_JUMP,
        PLAYER_DOUBLE_JUMP,
        PLAYER_DASH,
        PLAYER_LAND,
        PLAYER_SLIDE,
        PLAYER_ATTACK_1,
        PLAYER_ATTACK_2,
        PLAYER_HIT,
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

    private Dictionary<InGameSoundEffectType, float> lastPlayedTime = new Dictionary<InGameSoundEffectType, float>();

    private const float TIME_INTERVAL = 0.1f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this.gameObject);

        for (int i = 0; i < effectInfos.Length; i++)
        {
            List<AudioClip> clips = new List<AudioClip>(effectInfos[i].audioClips.Length);

            for (int j = 0; j < effectInfos[i].audioClips.Length; j++)
            {
                clips.Add(effectInfos[i].audioClips[j]);
            }

            inGameAudioClips.Add(effectInfos[i].effectType, clips);
            lastPlayedTime.Add(effectInfos[i].effectType, -TIME_INTERVAL);
        }
    }

    public void PlaySoundEffect(InGameSoundEffectType type, float volume)
    {
        if (Time.time - lastPlayedTime[type] < TIME_INTERVAL)
        {
            return;
        }

        lastPlayedTime[type] = Time.time;

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

    public AudioSource PlayLoopSoundEffect(InGameSoundEffectType type, float volume)
    {
        var audio = ObjectPoolManager.Instance.GetPoolableObject(loopAudioSourcePrefab).GetComponent<AudioSource>();

        if (inGameAudioClips[type].Count == 1)
        {
            audio.clip = inGameAudioClips[type][0];
        }
        else
        {
            int index = Random.Range(0, inGameAudioClips[type].Count);
            audio.clip = inGameAudioClips[type][index];
        }
        
        audio.loop = true;
        audio.volume = volume;
        audio.Play();

        return audio;
    }
}
