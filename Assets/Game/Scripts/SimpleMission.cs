using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts
{
    public class SimpleMission : MonoBehaviour, IMission
    {
        public event Action OnStarted;
        public event Action OnFinished;
        public event Action OnMissionPointReached;

        [SerializeField] private float missionDuration = 3f;
    
        private CancellationTokenSource _cancellationTokenSource;

        public void Begin()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            StartAsync(_cancellationTokenSource.Token).Forget();
        }
    
        private async UniTaskVoid StartAsync(CancellationToken token)
        {
            Debug.Log($"[{gameObject.name}] started");
            OnStarted?.Invoke();
        
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(missionDuration), cancellationToken: token);
                FinishMission();
            }
            catch (OperationCanceledException)
            {
                Debug.Log($"[{gameObject.name}] canceled");
            }
        }

        private void FinishMission()
        {
            Debug.Log($"[{gameObject.name}] finished");
            OnFinished?.Invoke();
            Destroy(gameObject);
        }
    
        private void OnDestroy()
        {
            if (_cancellationTokenSource == null) return;
        
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        
            _cancellationTokenSource = null;
        }
    }
}