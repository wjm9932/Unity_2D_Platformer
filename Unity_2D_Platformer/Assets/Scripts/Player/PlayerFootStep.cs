using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFootStep : MonoBehaviour
{
    [SerializeField] private List<AudioClip> footStepClips = new List<AudioClip>();
    private AudioSource playerFootStepSoundSource;

    private void Awake()
    {
        playerFootStepSoundSource = GetComponent<AudioSource>();
    }

    private void PlayFootStepSound()
    {
        int index = Random.Range(0, footStepClips.Count);
        playerFootStepSoundSource.PlayOneShot(footStepClips[index]);
    }
}
