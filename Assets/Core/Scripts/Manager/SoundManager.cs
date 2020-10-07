using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace FoodZombie
{
    public class SoundManager : MonoBehaviour
    {
        [System.Serializable]
        public class Sound : IComparable<Sound>
        {
            public string fileName;
            public int id;
            public int limitNumber = 1;
            public AudioClip clip;

            public int CompareTo(Sound other)
            {
                return id.CompareTo(other.id);
            }
        }

        #region Members

        private static SoundManager mInstance;
        public static SoundManager Instance
        {
            get
            {
                if (mInstance == null)
                    mInstance = FindObjectOfType<SoundManager>();
                return mInstance;
            }
        }

        private const int TOTAL_UI_SOURCE = 5;
        private const int TOTAL_BATTLE_SOURCE = 10;

        public List<Sound> SFXClips;
        public List<Sound> musicClips;

        private AudioSource mMusicSource;
        private List<AudioSource> mSFXUISources = new List<AudioSource>();
        private List<AudioSource> mSFXBattleSources = new List<AudioSource>();
        private Queue<Action> mDelayedAction = new Queue<Action>();

        private bool mEnableMusic = true;
        private bool mEnableSFX = true;
        private bool mInitializedMusic;
        private bool mInitialziedSFX;
        private Tweener mMusicTweener;

        private static Dictionary<string, Sound> cachedSounds = new Dictionary<string, Sound>();

        #endregion

        //=============================================

        #region MonoBehaviour

        private void Awake()
        {
            if (mInstance == null)
                mInstance = this;
            else if (mInstance != this)
                Destroy(gameObject);
        }

        private IEnumerator Start()
        {
            mMusicSource = new GameObject("Music").AddComponent<AudioSource>();
            mMusicSource.transform.SetParent(transform);
            mMusicSource.loop = true;
            mMusicSource.playOnAwake = false;
            mMusicSource.mute = !mEnableMusic;
            mInitializedMusic = true;

            yield return null;

            mSFXUISources = new List<AudioSource>();
            for (int i = 0; i < TOTAL_UI_SOURCE; i++)
            {
                var audioSource = new GameObject("Sfx_UI").AddComponent<AudioSource>();
                audioSource.transform.SetParent(transform);
                audioSource.loop = false;
                audioSource.playOnAwake = false;
                audioSource.mute = !mEnableSFX;
                mSFXUISources.Add(audioSource);

                yield return null;
            }

            mSFXBattleSources = new List<AudioSource>();
            for (int i = 0; i < TOTAL_BATTLE_SOURCE; i++)
            {
                var audioSource = new GameObject("Sfx_Battle").AddComponent<AudioSource>();
                audioSource.transform.SetParent(transform);
                audioSource.loop = false;
                audioSource.playOnAwake = false;
                audioSource.mute = !mEnableSFX;
                mSFXBattleSources.Add(audioSource);

                yield return null;
            }

            mInitialziedSFX = true;

            GameData.Instance.GameConfigGroup.onEnableMuisic += OnEnableMusic;
            GameData.Instance.GameConfigGroup.onEnableSFX += OnEnableSFX;

            if (mDelayedAction != null && mDelayedAction.Count > 0)
            {
                foreach (var aaction in mDelayedAction)
                    aaction();

                mDelayedAction.Clear();
            }

            yield return new WaitForSeconds(1f);

            mEnableMusic = GameData.Instance.GameConfigGroup.EnableMusic;
            mEnableSFX = GameData.Instance.GameConfigGroup.EnableSFX;
            mMusicSource.mute = !mEnableMusic;
            foreach (var s in mSFXUISources)
                s.mute = !mEnableSFX;
            foreach (var s in mSFXBattleSources)
                s.mute = !mEnableSFX;
        }

        #endregion

        //=============================================

        #region Public

        //SFX

        public void PlaySFX(string pFileName, bool inBattle, bool pLoop = false)
        {
            if (!cachedSounds.ContainsKey(pFileName))
            {
                var sound = GetSound(pFileName, false);
                if (sound != null)
                    cachedSounds.Add(pFileName, sound);
                PlaySFX(sound, inBattle, pLoop);
            }
            else
            {
                var sound = cachedSounds[pFileName];
                PlaySFX(sound, inBattle, pLoop);
            }
        }

        public void PlaySFX(int pId, bool inBattle, bool pLoop = false)
        {
            PlaySFX(GetSound(pId, false), inBattle, pLoop);
        }

        public void StopSFX(int pId, bool pInBattle)
        {
            StopSFX(GetSound(pId, false), pInBattle);
        }
        public void StopAllSFX(int pId, bool pInBattle)
        {
            StopAllSFX(GetSound(pId, false), pInBattle);
        }

        public void StopAllBattleSFXs()
        {
            if (!mInitialziedSFX)
                return;

            foreach (var s in mSFXBattleSources)
                s.Stop();
        }

        //MUSIC

        public void PlayMusic(string pFileName, bool pFade = true)
        {
            PlayMusic(GetSound(pFileName, true), pFade);
        }

        public void PlayMusic(int pId, bool pFade = true)
        {
            PlayMusic(GetSound(pId, true), pFade);
        }

        public void StopMusic(bool pFade = false)
        {
            if (mMusicTweener != null)
                mMusicTweener.Kill();

            if (!pFade)
            {
                mMusicSource.Stop();
            }
            else
            {
                mMusicTweener = mMusicSource.DOFade(0, 1f)
                    .OnComplete(() =>
                    {
                        mMusicSource.volume = 1;
                        mMusicSource.Stop();
                    });
            }
        }

        //=========

        #endregion

        //==============================================

        #region Private

        private Sound GetSound(int pId, bool isMusic)
        {
            if (isMusic)
            {
                foreach (var m in musicClips)
                    if (m.id == pId)
                    {
#if UNITY_EDITOR
                        if (m.clip == null)
                            UnityEngine.Debug.LogError("No Sound for " + m.fileName);
#endif
                        return m;
                    }
            }
            else
            {
                foreach (var m in SFXClips)
                    if (m.id == pId)
                    {
#if UNITY_EDITOR
                        if (m.clip == null)
                            UnityEngine.Debug.LogError("No Sound for " + m.fileName);
#endif
                        return m;
                    }
            }
            return null;
        }

        private Sound GetSound(string pFileName, bool isMusic)
        {
            if (isMusic)
            {
                foreach (var m in musicClips)
                    if (m.fileName == pFileName)
                        return m;
            }
            else
            {
                foreach (var m in SFXClips)
                    if (m.fileName == pFileName)
                        return m;
            }
            return null;
        }

        private AudioSource GetSFXSource(AudioClip pClip, bool pInBattle, int pLimit)
        {
            return GetSFXSource(pClip.GetInstanceID(), pInBattle, pLimit);
        }

        private AudioSource GetSFXSource(int pClipInstanceId, bool pInBattle, int pLimit)
        {
            var sources = pInBattle ? mSFXBattleSources : mSFXUISources;
            //Find same sfx
            int countSame = 0;
            for (int i = sources.Count - 1; i >= 0; i--)
            {
                if (!sources[i].isPlaying)
                {
                    sources[i].clip = null;
#if UNITY_EDITOR
                    sources[i].name = pInBattle ? "SFX_Battle" : "SFX_UI";
#endif
                }

                if (sources[i].clip != null && sources[i].clip.GetInstanceID() == pClipInstanceId)
                {
                    countSame++;
                    if (countSame >= pLimit)
                    {
                        var source = sources[i];
                        sources.Remove(source);
                        sources.Add(source);
                        return source;
                    }
                }
            }

            //Find emppty sfx source
            for (int i = 0; i < sources.Count; i++)
            {
                if (!sources[i].isPlaying)
                {
                    //Move new SFX to last position
                    var source = sources[i];
                    sources.Remove(source);
                    sources.Add(source);
                    return source;
                }
            }

            return sources[0];
        }

        private void OnEnableSFX(bool pActive)
        {
            mEnableSFX = pActive;
            foreach (var s in mSFXUISources)
                s.mute = !mEnableSFX;
            foreach (var s in mSFXBattleSources)
                s.mute = !mEnableSFX;
        }

        private void OnEnableMusic(bool pActive)
        {
            mEnableMusic = pActive;
            if (pActive)
            {
                mMusicSource.mute = false;
                mMusicSource.Play();
            }
            else
            {
                mMusicSource.mute = true;
                mMusicSource.Stop();
            }
        }

        private void PlayMusic(Sound pSound, bool pFade)
        {
            if (!mInitializedMusic)
            {
                mDelayedAction.Enqueue(() => { PlayMusic(pSound, pFade); });
                return;
            }

            try
            {
                if (pSound == null) return;

                mMusicSource.clip = pSound.clip;

                if (!mEnableMusic || pSound.clip == null || !mInitializedMusic)
                    return;

                if (mMusicTweener != null)
                    mMusicTweener.Kill();
                if (!pFade)
                {
                    mMusicSource.volume = 1;
                }
                else
                {
                    mMusicSource.volume = 0;
                    mMusicTweener = mMusicSource.DOFade(1f, 3f);
                }
                mMusicSource.Play();
            }
            catch { }
        }

        private void PlaySFX(Sound pSound, bool pInBattle, bool pLoop)
        {
            if (!mInitialziedSFX)
                return;

            if (!mEnableSFX || pSound == null || pSound.clip == null || !mInitialziedSFX)
                return;

            var source = GetSFXSource(pSound.clip, pInBattle, pSound.limitNumber);
            source.volume = 1;
            source.loop = pLoop;
            source.clip = pSound.clip;
            if (!pLoop)
                source.PlayOneShot(pSound.clip);
            else
                source.Play();
#if UNITY_EDITOR
            source.name = (pInBattle ? "SFX_Battle" : "SFX_UI") + pSound.fileName;
#endif
        }

        private void StopSFX(Sound pSound, bool pInBattle)
        {
            if (pSound == null || pSound.clip == null)
                return;

            var sources = pInBattle ? mSFXBattleSources : mSFXUISources;
            for (int i = sources.Count - 1; i >= 0; i--)
            {
                if (sources[i].clip == pSound.clip)
                {
                    sources[i].Stop();
                    sources[i].clip = null;
#if UNITY_EDITOR
                    sources[i].name = pInBattle ? "SFX_Battle" : "SFX_UI";
#endif
                    break;
                }
            }
        }
        private void StopAllSFX(Sound pSound, bool pInBattle)
        {
            if (pSound == null || pSound.clip == null)
                return;

            var sources = pInBattle ? mSFXBattleSources : mSFXUISources;
            for (int i = sources.Count - 1; i >= 0; i--)
            {
                if (sources[i].clip == pSound.clip)
                {
                    sources[i].Stop();
                    sources[i].clip = null;
#if UNITY_EDITOR
                    sources[i].name = pInBattle ? "SFX_Battle" : "SFX_UI";
#endif
                }
            }
        }

        #endregion
    }
}