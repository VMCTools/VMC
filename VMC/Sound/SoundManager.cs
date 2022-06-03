using System.Collections.Generic;
using UnityEngine;
using VMC.Ultilities;

namespace VMC.Sound
{
    public class SoundManager : Singleton<SoundManager>
    {
        [SerializeField] private List<SoundKey> keys;
        private Dictionary<string, AudioClip> listAudios = new Dictionary<string, AudioClip>();

        private AudioSource myMusic;
        private AudioSource mySound;
        private List<AudioSource> mySounds = new List<AudioSource>();

        [SerializeField] private bool isEnableSound;
        [SerializeField] private bool isEnableMusic;

        public const string KEY_SETTING_MUSIC = "VMC_Setting_Music";
        public const string KEY_SETTING_SOUND = "VMC_Setting_Sound";

        private void OnEnable()
        {
            UpdateSetting();
        }
        private void Start()
        {
            foreach (var item in keys)
            {
                if (!listAudios.ContainsKey(item.Key))
                    listAudios.Add(item.Key, item.Clip);
            }
            keys = null;
        }
        public void Set_EnableSound(bool isEnable)
        {
            isEnableSound = isEnable;
            foreach (var sound in mySounds)
            {
                sound.mute = !isEnableSound;
            }
        }
        public void Set_EnableMusic(bool isEnable)
        {
            isEnableMusic = isEnable;
            if (myMusic != null)
            {
                myMusic.mute = !isEnable;
            }
        }

        public void PlayMusic(string key)
        {
            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found music audioclip: " + key);
                return;
            }
            if (myMusic == null)
            {
                var musicObject = new GameObject("Music");
                musicObject.transform.SetParent(this.transform);
                myMusic = musicObject.AddComponent<AudioSource>();

                myMusic.loop = true;
                myMusic.volume = 0.4f;
                myMusic.mute = !isEnableMusic;
            }

            myMusic.clip = listAudios[key];
            myMusic.Play();
        }
        public void PlaySound(string key, bool isLoop = false)
        {
            if (!isEnableSound)
                return;

            if (!listAudios.ContainsKey(key))
            {
                Debug.LogError("Not found sound audioclip: " + key);
                return;
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

                mySound.loop = false;
                mySound.volume = 1f;

                mySounds.Add(mySound);
            }
            mySound.loop = isLoop;
            mySound.clip = listAudios[key];
            mySound.Play();
        }

        internal void UpdateSetting()
        {
            Set_EnableSound(PlayerPrefsHelper.Get(KEY_SETTING_SOUND,true));
            Set_EnableMusic(PlayerPrefsHelper.Get(KEY_SETTING_MUSIC, true));
        }

        public void StopSound(string key)
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
