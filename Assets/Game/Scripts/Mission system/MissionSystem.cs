using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts.Mission_system
{
    public class MissionSystem : MonoBehaviour
    {
        [SerializeField] private List<MissionData> missionChainsRoots;

        private readonly List<Timer> _activeTimers = new();
        
        private void Start()
        {
            foreach (var firstMission in missionChainsRoots)
            {
                StartMissionChain(firstMission);
            }
        }

        private void StartMissionChain(MissionData missionData)
        {
            if (missionData == null) return;
            
            if (missionData.startDelay <= 0)
            {
                StartMission(missionData);
            }
            else
            {
                var delayMs = (int)(missionData.startDelay * 1000);
                var timer = new Timer();
                
                _activeTimers.Add(timer);

                void OnComplete()
                {
                    _activeTimers.Remove(timer);
                    StartMission(missionData);
                }

                timer.StartAsync(delayMs, OnComplete).Forget(); 
            }
        }

        private void StartMission(MissionData missionData) 
        {
            var missionObj = Instantiate(missionData.missionPrefab);
            missionObj.name = missionData.missionName;

            var mission = missionObj.GetComponent<IMission>();

            mission.OnFinished += OnComponentOnOnFinished;
            
            mission.Begin();
            return;

            void OnComponentOnOnFinished()
            {
                MissionCompleted(missionData);
            }
        }

        private void MissionCompleted(MissionData completedMissionData)
        {
            if (completedMissionData.nextMission != null)
            {
                StartMissionChain(completedMissionData.nextMission);
            }
        }

        private void OnDestroy()
        {
            foreach (var timer in _activeTimers)
            {
                timer.Cancel();
            }
            _activeTimers.Clear();
        }
    }
}