using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private AudioSource soundSource;
    [SerializeField] private List<AudioClip> soundEffects;

    private bool isReady = false;

    // Create an enum for sound effects to enable intellisense
    public enum SoundEffect
    {
        Attack,
        ItemPickup,
        ItemEquip,
        ItemConsume,
        ItemEnhance,
        UIHover,
        DoorOpen,
        DoorClose
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        StartCoroutine(SetReady());
    }

    public void PlaySound(SoundEffect sound, float pitch = 1f, bool randomPitch = false)
    {
        if (!isReady)
        {
            return;
        }

        int index = (int)sound;
        if (index >= 0 && index < soundEffects.Count)
        {
            soundSource.pitch = randomPitch ? UnityEngine.Random.Range(0.95f, 1.05f) : pitch;
            soundSource.PlayOneShot(soundEffects[index]);
        }
        else
        {
            Debug.LogWarning("Invalid sound effect index.");
        }  
    }

    private IEnumerator SetReady()
    {
        yield return new WaitForSeconds(1.5f);
        isReady = true;
    }
}