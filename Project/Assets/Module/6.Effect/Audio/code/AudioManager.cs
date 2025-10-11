using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Audio;

namespace SimpleAudioSystem
{
    public class AudioManager : Singleton<AudioManager>
    {
        [SerializeField] private AudioDataCollection audioDataCollection;

        [Header("Audio source")]
        [SerializeField] private AudioSource ambience_loop;
        [SerializeField] private AudioSource music_loop;
        [SerializeField] private AudioSource sfx_default;

        [Header("Audio mixer")]
        [SerializeField] private AudioMixer mainMixer;

        private bool ambience_crossfading = false;
        private bool music_crossfading = false;
        private CoroutineExcuter ambFader;
        private CoroutineExcuter musicFader;
        private List<SFXHandler> loopSources = new List<SFXHandler>();

        //由于SFX占用体积小，但是会频繁使用，因此做一个缓存字典
        public string current_music_name { get; private set; } = string.Empty;
        public string current_ambience_name { get; private set; } = string.Empty;

        private const string MASTER_VOLUME_KEY = "master_volume";
        private const string SFX_VOLUME_KEY = "sfx_volume";
        private const string AMB_VOLUME_KEY = "amb_volume";
        private const string BGM_VOLUME_KEY = "bgm_volume";

        #region 用户数据
        protected const float BGM_VOLUME = 0;
        protected const float AMB_VOLUME = 0;
        protected const float SFX_VOLUME = 0;
        protected bool bgm_mute = false;
        protected bool amb_mute = false;
        protected bool sfx_mute = false;
        #endregion

        protected const int MAX_LOOP_SOURCE = 10;
        protected const string SFX_GROUP = "SFX";

        #region 
        public void Init()
        {
            //@todo获取用户数据, 并设置音效的开关
        }
        #endregion

        #region 数据获取
        protected async UniTask<AudioClip> GetBGMClipByKey(string audioKey) => await GameAsset.GetAudioAsync(audioDataCollection.GetBGMRefByKey(audioKey));
        protected async UniTask<AudioClip> GetAMBClipByKey(string audioKey) => await GameAsset.GetAudioAsync(audioDataCollection.GetAMBRefByKey(audioKey));
        protected AudioClip GetSFXClipByKey(string audioKey) => audioDataCollection.GetSFXClipByKey(audioKey);
        #endregion

        #region 音量控制
        public void MuteMusic()
        {
            bgm_mute = true;
            amb_mute = true;
            mainMixer.SetFloat(BGM_VOLUME_KEY, -80);
            mainMixer.SetFloat(AMB_VOLUME_KEY, -80);
            //@todo存档数据
        }

        public void UnMuteMusic()
        {
            bgm_mute = false;
            amb_mute = false;
            mainMixer.SetFloat(BGM_VOLUME_KEY, BGM_VOLUME);
            mainMixer.SetFloat(AMB_VOLUME_KEY, AMB_VOLUME);
            RestartBGM();
            //@todo存档数据
        }

        public void MuteSFX()
        {
            sfx_mute = true;
            mainMixer.SetFloat(SFX_VOLUME_KEY, -80);
            //@todo存档数据
        }

        public void UnMuteSFX()
        {
            sfx_mute = false;
            mainMixer.SetFloat(SFX_VOLUME_KEY, SFX_VOLUME);
            //@todo存档数据
        }
        #endregion

        #region Sound Play
        #region 背景音乐
        //播放背景音乐
        public async void PlayBGM(string audioKey, float volume = 1)
        {
            current_music_name = audioKey;
            if (string.IsNullOrEmpty(audioKey))
            {
                music_loop.Stop();
                return;
            }

            music_loop.clip = await GetBGMClipByKey(audioKey);
            if (music_loop.clip != null)
            {
                music_loop.volume = volume;
                music_loop.Play();
            }
        }
        //从当前音乐过渡到指定背景音乐，若不指定音乐，则渐隐音乐
        public async void PlayBGM(string audioKey, bool startOver, float transitionTime, float volume = 1, bool forcePlay = false)
        {
            //If no audio name, fade out the ambience
            if (string.IsNullOrEmpty(audioKey))
            {
                FadeAudio(music_loop, 0, transitionTime, true);
                current_music_name = string.Empty;
            }
            //If the audio name is the same, only fade the volume to the target value
            if (current_music_name == audioKey)
            {
                FadeAudio(music_loop, volume, transitionTime);
            }
            else
            {
                if (current_music_name == string.Empty || !music_loop.isPlaying)
                {
                    music_loop.clip = await GetAMBClipByKey(audioKey);
                    if (music_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    music_loop.volume = volume;
                    music_loop.Play();
                }
                else
                {
                    if (music_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    await CrossFadeMusic(audioKey, volume, startOver, transitionTime, forcePlay);
                }
                current_music_name = audioKey;
            }
        }
        public void RestartBGM()
        {
            if (!string.IsNullOrEmpty(current_music_name))
            {
                PlayBGM(current_music_name, 1);
            }
        }
        #endregion

        #region 环境音效
        //从当前环境音效过渡到指定环境音效
        public async void PlayAMB(string audioKey, bool startOver, float transitionTime, float volume = 1, bool forcePlay = false)
        {
            //If no audio name, fade out the ambience
            if (string.IsNullOrEmpty(audioKey))
            {
                FadeAudio(ambience_loop, 0, transitionTime, true);
                current_ambience_name = string.Empty;
            }
            //If the audio name is the same, only fade the volume to the target value
            if (current_ambience_name == audioKey)
            {
                FadeAudio(ambience_loop, volume, transitionTime);
            }
            else
            {
                if (current_ambience_name == string.Empty || !ambience_loop.isPlaying)
                {
                    ambience_loop.clip = await GetAMBClipByKey(audioKey);
                    if (ambience_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    ambience_loop.volume = volume;
                    ambience_loop.Play();
                }
                else
                {
                    if (ambience_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    await CrossFadeAmbience(audioKey, volume, startOver, transitionTime, forcePlay);
                }
                current_ambience_name = audioKey;
            }
        }
        //播放指定环境音效，若已经存在相同环境音效，则改变音量，若存在不同环境音效，则过渡
        public async void PlayAMB(string audioKey, bool startOver, float volume = 1, bool forcePlay = false)
        {
            //If no audio name, fade out the ambience
            if (string.IsNullOrEmpty(audioKey))
            {
                FadeAudio(ambience_loop, 0, 0.5f, true);
                current_ambience_name = string.Empty;
            }
            //If the audio name is the same, only fade the volume to the target value
            if (current_ambience_name == audioKey)
            {
                FadeAudio(ambience_loop, volume, 0.5f);
            }
            else
            {
                if (current_ambience_name == string.Empty || !ambience_loop.isPlaying)
                {
                    ambience_loop.clip = await GetAMBClipByKey(audioKey);
                    if (ambience_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    ambience_loop.volume = volume;
                    ambience_loop.Play();
                }
                else
                {
                    if (ambience_loop.clip == null)
                    {
                        Debug.LogWarning("No clip found, nothing will be done for ambient");
                        return;
                    }
                    await CrossFadeAmbience(audioKey, volume, startOver, 0.5f, forcePlay);
                }
                current_ambience_name = audioKey;
            }
        }
        #endregion

        #region SFX音效
        //默认AudioSource播放音频
        public AudioClip PlaySFX(string sfxKey, float volumeScale = 1) => PlaySFX(sfx_default, sfxKey, volumeScale);
        //指定AudioSource播放音频
        public AudioClip PlaySFX(AudioSource targetSource, string sfxKey, float volumeScale)
        {
            if (string.IsNullOrEmpty(sfxKey))
                return null;
            AudioClip clip = GetSFXClipByKey(sfxKey);
            if (clip != null)
                targetSource.PlayOneShot(clip, volumeScale);
            else
                Debug.LogAssertion($"No Clip found:{sfxKey}");
            return clip;
        }
        //指定AudioSource播放循环音频
        AudioClip PlaySFXLoop(AudioSource targetSource, string sfxKey, float volumeScale, float transition = 1f)
        {
            if (string.IsNullOrEmpty(sfxKey))
            {
                targetSource.Stop();
                return null;
            }
            AudioClip clip = GetSFXClipByKey(sfxKey);
            targetSource.clip = clip;
            targetSource.loop = true;
            targetSource.Play();
            FadeAudio(targetSource, volumeScale, transition);

            return clip;
        }
        public AudioClip PlaySFXLoop(string sfxKey, float volumeScale = 1)
        {
            if (loopSources == null)
            {
                loopSources = new List<SFXHandler>();
            }
            //寻找已经播放同名clip的AudioSource
            var source = loopSources.First(x => x.sfxKey == sfxKey);
            if (source != null)
            {
                source.volume = volumeScale;
                return source.PlaySFX(sfxKey);
            }
            
            source = loopSources.First(x => !x.isPlaying);
            if (source != null)
            {
                source.volume = volumeScale;
                return source.PlaySFX(sfxKey);
            }

            if (loopSources.Count < MAX_LOOP_SOURCE)
            {
                source = new SFXHandler(gameObject.AddComponent<AudioSource>(), sfxKey, volumeScale);
                source.SetMixer(mainMixer.FindMatchingGroups(SFX_GROUP)[0]);
                loopSources.Add(source);

                source.volume = volumeScale;
                return source.PlaySFX(sfxKey);
            }
            else
            {
                source = loopSources[0];
                source.volume = volumeScale;
                return source.PlaySFX(sfxKey);
            }
        }
        public void StopSFXLoop(string sfxKey)
        {
            var source = loopSources.First(x => x.sfxKey == sfxKey);
            if(source != null)
            {
                source.Stop();
            }
        }
        //将目标AudioSource从当前播放的SFX逐渐过渡到指定SFX
        public void FadeInAndOutSFX(AudioSource targetSource, string sfxKey, float maxVolume, float duration, float fadeIn, float fadeOut) =>
            StartCoroutine(coroutineFadeInAndOutSFX(targetSource, sfxKey, maxVolume, duration, fadeIn, fadeOut));
        #endregion
        #endregion

        #region Helper function
        public void FadeAmbience(float targetVolume, float transitionTime, bool StopOnFadeOut = false) => FadeAudio(ambience_loop, targetVolume, transitionTime, StopOnFadeOut);
        public void FadeMusic(float targetVolume, float transitionTime, bool StopOnFadeOut = false) => FadeAudio(music_loop, targetVolume, transitionTime, StopOnFadeOut);
        public void FadeAudio(AudioSource m_audio, float targetVolume, float transitionTime, bool StopOnFadeOut = false)
        {
            StartCoroutine(coroutineFadeAudio(m_audio, targetVolume, transitionTime, StopOnFadeOut));
        }
        public void ChangeMasterVolume(float targetVolume)
        {
            mainMixer.SetFloat(MASTER_VOLUME_KEY, targetVolume);
        }
        async Task CrossFadeAmbience(string audioKey, float targetVolume, bool startOver, float transitionTime, bool forceCrossFade = false)
        {
            if (!forceCrossFade && ambience_crossfading) return;
            if (ambFader == null) ambFader = new CoroutineExcuter(this);
            AudioClip ambClip = await GetAMBClipByKey(audioKey);
            ambFader.Excute(coroutineCrossFadeAmbience(ambClip, targetVolume, startOver, transitionTime));
        }
        async Task CrossFadeMusic(string audioKey, float targetVolume, bool startOver, float transitionTime, bool forceCrossFade = false)
        {
            if (!forceCrossFade && music_crossfading) return;
            if (musicFader == null) musicFader = new CoroutineExcuter(this);
            AudioClip musicClip = await GetBGMClipByKey(audioKey);
            musicFader.Excute(coroutineCrossFadeMusic(musicClip, targetVolume, startOver, transitionTime));
        }
        #endregion

        #region Coroutine
        IEnumerator coroutineFadeInAndOutSFX(AudioSource m_audio, string clip, float maxVolume, float duration, float fadeIn, float fadeOut)
        {
            AudioSource tempAudio = Instantiate(m_audio);
            tempAudio.name = "_TempSFX";
            tempAudio.volume = 0;
            tempAudio.loop = true;
            tempAudio.clip = GetSFXClipByKey(clip);
            tempAudio.Play();

            yield return new WaitForLoop(fadeIn, (t) => tempAudio.volume = Mathf.Lerp(0, maxVolume, t));
            yield return new WaitForSeconds(duration);
            yield return new WaitForLoop(fadeOut, (t) => tempAudio.volume = Mathf.Lerp(maxVolume, 0, t));

            Destroy(tempAudio.gameObject);
        }
        IEnumerator coroutineCrossFadeAmbience(AudioClip to_clip, float targetVolume, bool startOver, float transitionTime)
        {
            ambience_crossfading = true;
            yield return coroutineCrossFadeAudio(ambience_loop, to_clip, targetVolume, startOver, transitionTime);
            ambience_crossfading = false;
        }
        IEnumerator coroutineCrossFadeMusic(AudioClip to_clip, float targetVolume, bool startOver, float transitionTime)
        {
            music_crossfading = true;
            yield return coroutineCrossFadeAudio(music_loop, to_clip, targetVolume, startOver, transitionTime);
            music_crossfading = false;
        }
        IEnumerator coroutineCrossFadeAudio(AudioSource targetSource, AudioClip to_clip, float targetVolume, bool startOver, float transitionTime)
        {
            if (targetSource.isPlaying && targetSource.clip != null)
            {
                StartCoroutine(coroutineFadeAudio(targetSource, 0, transitionTime));
            }

            AudioSource tempAudio = new GameObject($"[_Temp_{targetSource.name}]").AddComponent<AudioSource>();
            Destroy(tempAudio.gameObject, transitionTime); //Schedule an auto Destruction;

            tempAudio.volume = 0;
            tempAudio.loop = true;

            //Fade In Clip
            tempAudio.clip = to_clip;
            if (!startOver) tempAudio.time = targetSource.time;
            tempAudio.outputAudioMixerGroup = targetSource.outputAudioMixerGroup;
            tempAudio.Play();
            yield return coroutineFadeAudio(tempAudio, targetVolume, transitionTime);

            //Swap audio
            targetSource.clip = tempAudio.clip;
            targetSource.time = tempAudio.time;
            targetSource.volume = tempAudio.volume;
            targetSource.Play();
        }
        IEnumerator coroutineFadeAudio(AudioSource source, float targetVolume, float transition, bool StopOnFadeOut = false)
        {
            float initVolume = source.volume;
            yield return new WaitForLoop(transition, (t) =>
            {
                source.volume = Mathf.Lerp(initVolume, targetVolume, t);
            });
            yield return null;

            if (StopOnFadeOut && source.volume == 0) source.Stop();
        }
        #endregion

        internal class SFXHandler
        {
            private AudioSource source;
            public string sfxKey{ get; private set; }
            public float volume
            {
                get => source.volume;
                set => source.volume = value;
            }
            public bool isPlaying => source.isPlaying;

            public SFXHandler(AudioSource source, string sfxKey, float volume = 1)
            {
                this.sfxKey = sfxKey;
                this.source = source;
                this.source.volume = volume;
                source.loop = true;
            }
            public void SetMixer(AudioMixerGroup group)
            {
                source.outputAudioMixerGroup = group;
            }
            public AudioClip PlaySFX(string sfxKey)
            {
                this.sfxKey = sfxKey;
                return AudioManager.Instance.PlaySFXLoop(source, sfxKey, volume, 0);
            }
            public void Stop()
            {
                source.Stop();
            }
        }
    }
}
