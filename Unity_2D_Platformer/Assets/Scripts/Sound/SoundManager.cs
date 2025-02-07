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
        PLAYER_JUMP = 0,
        PLAYER_DOUBLE_JUMP = 1,
        PLAYER_DASH = 2,
        PLAYER_LAND = 3,
        PLAYER_SLIDE = 4,
        PLAYER_ATTACK_1 = 5,
        PLAYER_ATTACK_2 = 6,
        PLAYER_HIT = 7,
        PLAYER_DIE = 8,
        ENEMY_HIT = 9,
        ENEMY_WARRIOR_ATTACK = 10,
        ENEMY_SPEAR_ATTACK = 11,
        ENEMY_SHIELD_BLOCK = 12,
        ENEMY_SHIELD_ATTACK = 13,
        ENEMY_RANGE_ATTACK = 14,
        ENEMY_DIE = 15,
        CHEST_OPEN = 16,
        ITEM_ACQUIRE = 17,
        SWITCH_TRIGGER = 18,
        ENEMY_CHASING_READY = 19,
        ENEMY_CHASING_BULLET = 20,
        ENEMY_CHASING_LIGHNING = 21,
        BOUNCE_PAD,
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

    public void PlaySoundEffect(InGameSoundEffectType type, AudioSource audioSource)
    {
        if (Time.time - lastPlayedTime[type] < TIME_INTERVAL)
        {
            return;
        }

        lastPlayedTime[type] = Time.time;

        if (inGameAudioClips[type].Count == 1)
        {
            audioSource.PlayOneShot(inGameAudioClips[type][0]);
        }
        else
        {
            int index = Random.Range(0, inGameAudioClips[type].Count);
            audioSource.PlayOneShot(inGameAudioClips[type][index]);
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
