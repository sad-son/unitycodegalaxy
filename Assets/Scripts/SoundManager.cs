using UnityEngine;

namespace DefaultNamespace
{
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance { get; private set; }

        [SerializeField] private AudioSource ambientSource;
        [SerializeField] private AudioSource sfxSource;

        [SerializeField] private AudioClip ambientClip;
        [SerializeField] private AudioClip correctAnswerClip;
        [SerializeField] private AudioClip wrongAnswerClip;

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
                return;
            }

            SetupAudioSources();
        }

        private void Start()
        {
            PlayAmbient();
        }

        private void SetupAudioSources()
        {
            if (ambientSource == null)
            {
                ambientSource = gameObject.AddComponent<AudioSource>();
            }

            if (sfxSource == null)
            {
                sfxSource = gameObject.AddComponent<AudioSource>();
            }

            ambientSource.loop = true;
            ambientSource.playOnAwake = false;
            ambientSource.clip = ambientClip;

            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }

        public void PlayAmbient()
        {
            if (!ambientSource.isPlaying)
            {
                ambientSource.Play();
            }
        }

        public void StopAmbient()
        {
            if (ambientSource.isPlaying)
            {
                ambientSource.Stop();
            }
        }

        public void PlayCorrectAnswer()
        {
            sfxSource.PlayOneShot(correctAnswerClip);
        }

        public void PlayWrongAnswer()
        {
            sfxSource.PlayOneShot(wrongAnswerClip);
        }

        public void SetAmbientVolume(float volume)
        {
            ambientSource.volume = Mathf.Clamp01(volume);
        }

        public void SetSFXVolume(float volume)
        {
            sfxSource.volume = Mathf.Clamp01(volume);
        }
    }
}