using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum SoundType
{
    ZOMBIE_DEAD,
    MELEE_HEAVY,
    MELEE_MISS,
    ZOMBIE_PAIN,
    MELEE_NORMAL,
    MELEE_NORMAL_MISS,
    STUNNED,
    UI_BTN_PRESSED,
    PLAYER_FOOTSTEP_GRASS,
    PLAYER_FOOTSTEP_STONE,
    PLAYER_RESPAWN,
    PLAYER_PAIN,
    PLAYER_DEATH,
    PLAYER_PICKUP_COIN,
    GAME_WIN,
    TELEPORTER_USE
}

[RequireComponent(typeof(AudioSource)), ExecuteInEditMode]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private SoundList[] soundList;
    public static SoundManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType sound, float volume = 1f)
    {
        AudioClip[] clips = instance.soundList[(int)sound].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.audioSource.PlayOneShot(randomClip, volume);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);
        for(int i = 0; i < soundList.Length; ++i)
            soundList[i].name = names[i];
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds { get => sounds; }
    [HideInInspector] public string name;
    [SerializeField] private AudioClip[] sounds;
}