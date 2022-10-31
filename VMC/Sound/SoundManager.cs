using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI.Extensions;
using VMC.Ultilities;

namespace VMC.Sound
{
    public class SoundManager : VMC.Ultilities.SingletonAdvance<SoundManager>
    {
        [SerializeField] private List<SoundKey> keys;
        private Dictionary<string, AudioClip> listAudios = new Dictionary<string, AudioClip>();

        private AudioSource myMusic;
        private AudioSource mySound;
        private List<AudioSource> mySounds = new List<AudioSource>();

        public bool isEnableSound { private set; get; } = true;
        public bool isEnableMusic { private set; get; } = true;

        private const string KEY_SETTING_MUSIC = "VMC_Setting_Music";
        private const string KEY_SETTING_SOUND = "VMC_Setting_Sound";

        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                Set_EnableSound(PlayerPrefsHelper.Get(KEY_SETTING_SOUND, true));
                Set_EnableMusic(PlayerPrefsHelper.Get(KEY_SETTING_MUSIC, true));
                if (keys != null)
                {
                    foreach (var item in keys)
                    {
                        if (!listAudios.ContainsKey(item.Key))
                            listAudios.Add(item.Key, item.Clip);
                    }
                }
#if !UNITY_EDITOR
            keys = null;
#endif
            }
        }

        public static void EnableSound(bool isEnable)
        {
            if (Instance == null) return;
            Instance.Set_EnableSound(isEnable);
        }
        private void Set_EnableSound(bool isEnable)
        {
            isEnableSound = isEnable;
            PlayerPrefsHelper.Set(KEY_SETTING_SOUND, isEnableSound);
            foreach (var sound in mySounds)
            {
                sound.mute = !isEnableSound;
            }
        }
        public static void EnableMusic(bool isEnable)
        {
            if (Instance == null) return;
            Instance.Set_EnableMusic(isEnable);
        }
        private void Set_EnableMusic(bool isEnable)
        {
            isEnableMusic = isEnable;
            PlayerPrefsHelper.Set(KEY_SETTING_MUSIC, isEnableMusic);
            if (myMusic != null)
            {
                myMusic.mute = !isEnable;
            }
        }

        public static void PlayMusic(string key, float volume = 0.4f)
        {
            if (Instance == null) return;
            Instance._PlayMusic(key, volume);
        }
        public static void PlayMusic(AudioClip clip, float volume = 0.4f)
        {
            if (Instance == null) return;
            Instance._PlayMusic(clip, volume);
        }

        private void _PlayMusic(AudioClip clip, float volume)
        {
            if (myMusic == null)
            {
                var musicObject = new GameObject("Music");
                musicObject.transform.SetParent(this.transform);
                myMusic = musicObject.AddComponent<AudioSource>();

                myMusic.loop = true;
            }
            myMusic.volume = volume;
            myMusic.mute = !isEnableMusic;
            myMusic.clip = clip;
            myMusic.Play();
        }

        private void _PlayMusic(string key, float volume = 0.4f)
        {
            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found music audioclip: " + key);
                return;
            }
            _PlayMusic(listAudios[key], volume);
        }
        public static void PlaySound(string key, float volume = 1f, bool isLoop = false)
        {
            if (Instance == null)
                return;
            Instance._PlaySound(key, volume, isLoop);
        }
        public static void PlaySound(AudioClip clip, float volume = 1f, bool isLoop = false)
        {
            if (Instance == null)
                return;
            Instance._PlaySound(clip, volume, isLoop);
        }
        private void _PlaySound(string key, float volume, bool isLoop = false)
        {
            if (!isEnableSound)
                return;

            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found sound audioclip: " + key);
                return;
            }
            _PlaySound(listAudios[key], volume, isLoop);
        }
        private void _PlaySound(AudioClip clip, float volume, bool isLoop = false)
        {
            if (!isEnableSound)
                return;

            mySound = null;
            foreach (var sound in mySounds)
            {
                if (!sound.isPlaying)
                {
                    mySound = sound;
                    break;
                }
            }
            if (mySound == null)
            {
                var soundObject = new GameObject("Sound");
                soundObject.transform.SetParent(this.transform);
                mySound = soundObject.AddComponent<AudioSource>();

                mySound.loop = false;
                mySound.volume = 1f;

                mySounds.Add(mySound);
            }
            mySound.volume = Mathf.Clamp01(volume);
            mySound.loop = isLoop;
            mySound.clip = clip;
            mySound.Play();
        }

        public static void StopSound(string key)
        {
            if (Instance == null)
                return;
            Instance._StopSound(key);
        }
        private void _StopSound(string key)
        {
            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found sound audioclip: " + key);
                return;
            }
            foreach (var sound in mySounds)
            {
                if (sound.isPlaying && sound.clip.name.Equals(key))
                {
                    sound.loop = false;
                    sound.Stop();
                    break;
                }
            }
        }
    }
}
