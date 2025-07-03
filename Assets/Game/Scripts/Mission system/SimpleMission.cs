using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Mission_system
{
    public class SimpleMission : MonoBehaviour, IMission, IDisposable
    {
        public event Action OnStarted;
        public event Action OnFinished;
        public event Action OnMissionPointReached;

        [SerializeField] private float missionDuration = 3f;
    
        private CancellationTokenSource _cancellationTokenSource;
    
        private void Log(string message)
        {
            Debug.Log($"[{gameObject.name}] {message}");
        }

        public void Begin()
        {
            if (_cancellationTokenSource != null)
            {
                Log("is already running");
                return;
            }

            _cancellationTokenSource = new CancellationTokenSource();
            StartAsync(_cancellationTokenSource.Token).Forget();
        }
    
        private async UniTaskVoid StartAsync(CancellationToken token)
        {
            Log("started");
            OnStarted?.Invoke();
        
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(missionDuration), cancellationToken: token);
                FinishMission();
            }
            catch (OperationCanceledException)
            {
                Log("canceled");
            }
        }

        private void FinishMission()
        {
            Log("finished");
            OnFinished?.Invoke();
            Destroy(gameObject);
        }
    
        private void OnDestroy()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_cancellationTokenSource == null) return;
        
            _cancellationTokenSource.Cancel();
            _cancellationTokenSource.Dispose();
        
            _cancellationTokenSource = null;
        }
    }
}