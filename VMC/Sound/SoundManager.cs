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

        private bool isEnableSound;
        private bool isEnableMusic;
        private float valueSound;
        private float valueMusic;

        private const string KEY_SETTING_MUSIC = "VMC_Setting_Music";
        private const string KEY_SETTING_SOUND = "VMC_Setting_Sound";

        private const string KEY_VOLUME_MUSIC = "VMC_Volume_Music";
        private const string KEY_VOLUME_SOUND = "VMC_Volume_Sound";

        public bool IsEnableMusic
        {
            get
            {
                return isEnableMusic;
            }
            set
            {
                isEnableMusic = value;
                PlayerPrefsHelper.Set(KEY_SETTING_MUSIC, isEnableMusic);
                if (myMusic != null)
                {
                    myMusic.mute = !value;
                }
            }
        }
        public bool IsEnableSound
        {
            get
            {
                return isEnableSound;
            }
            set
            {
                isEnableSound = value;
                PlayerPrefsHelper.Set(KEY_SETTING_SOUND, isEnableSound);
                foreach (var sound in mySounds)
                {
                    sound.mute = !isEnableSound;
                }
            }
        }

        public float VolumeMusic
        {
            get
            {
                return valueMusic;
            }
            set
            {
                valueMusic = value;
                PlayerPrefsHelper.Set(KEY_VOLUME_MUSIC, value);
                if (myMusic != null)
                {
                    myMusic.volume = value;
                }
            }
        }
        public float VolumeSound
        {
            get
            {
                return valueSound;
            }
            set
            {
                valueSound = value;
                PlayerPrefsHelper.Set(KEY_VOLUME_SOUND, value);
            }
        }
        protected override void Awake()
        {
            base.Awake();
            if (Instance == this)
            {
                isEnableSound = PlayerPrefsHelper.Get(KEY_SETTING_SOUND, true);
                isEnableMusic = PlayerPrefsHelper.Get(KEY_SETTING_MUSIC, true);
                valueSound = PlayerPrefsHelper.Get(KEY_VOLUME_SOUND, 1f);
                valueMusic = PlayerPrefsHelper.Get(KEY_VOLUME_MUSIC, .8f);

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
        public static void PlaySoundAtPoint(string key, Vector3 position, float volume = 1f)
        {
            if (!Instance.isEnableSound)
                return;

            if (!Instance.listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found sound audioclip: " + key);
                return;
            }
            AudioSource.PlayClipAtPoint(Instance.listAudios[key], position, volume);
        }
        public static void PlaySoundAtPoint(AudioClip clip, Vector3 position, float volume = 1f)
        {
            if (!Instance.isEnableSound)
                return;
            AudioSource.PlayClipAtPoint(clip, position, volume);
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
            if (!listAudios.ContainsKey(clip.name))
            {
                listAudios.Add(clip.name, clip);
            }

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
                mySounds.Add(mySound);
            }
            mySound.volume = Mathf.Clamp01(volume) * valueSound;
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
        public static void StopSound(AudioClip clip)
        {
            if (Instance == null)
                return;
            if (clip == null) return;
            Instance._StopSound(clip.name);
        }
        private void _StopSound(string key)
        {
            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found sound audioclip: " + key);
                return;
            }
            string clipName = listAudios[key].name;
            foreach (var sound in mySounds)
            {
                if (sound.isPlaying && sound.clip.name.Equals(clipName))
                {
                    sound.loop = false;
                    sound.Stop();
                    break;
                }
            }
        }
    }
}
