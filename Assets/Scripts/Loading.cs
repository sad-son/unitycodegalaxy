using System;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace DefaultNamespace
{
    public class Loading : MonoBehaviour
    {
        public float fakeLoadingTime = 1f;
        public float stageTime = 0.5f;
        public static Loading instance;

        public TMP_Text text;

        private int stage;
        private float nextStageTime;
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void EnableLoading()
        {
            FindObjectOfType<Loading>(true).gameObject.SetActive(true);
        }
        
        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LoadingAsync().Forget();
        }

        private async UniTaskVoid LoadingAsync()
        {
            await UniTask.DelayFrame(2, cancellationToken:this.GetCancellationTokenOnDestroy());
            LevelLoader.instance.Load();
            await UniTask.DelayFrame(2, cancellationToken:this.GetCancellationTokenOnDestroy());
            AdsManager.instance.Load();
            if (Application.platform != RuntimePlatform.WindowsEditor)
                await UniTask.Delay(TimeSpan.FromSeconds(fakeLoadingTime));
            Close();
        }

        private void Update()
        {
            if (Time.time <= nextStageTime)
            {
                return;
            }
            string dots = ".";
            string result = "Loading";
            int maxStages = 3;
            stage = Math.Clamp(stage, 0, maxStages);
            for (int i = 0; i < stage; i++)
            {
                result += dots;
            }
            
            text.text = result;
            stage++;
            if (stage > maxStages)
                stage = 0;
            
            nextStageTime = Time.time + stageTime;
        }

        public void Close()
        {
            gameObject.SetActive(false);
        }
    }
}