using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts
{
    public class MissionSystem : MonoBehaviour
    {
        [SerializeField] private List<MissionData> missionChains;

        private readonly List<Timer> _activeTimers = new();
        
        private void Start()
        {
            foreach (var firstMission in missionChains)
            {
                StartMissionChain(firstMission);
            }
        }

        private void StartMissionChain(MissionData missionData)
        {
            if (missionData == null) return;
            
            if (missionData.StartDelay <= 0)
            {
                StartMission(missionData);
            }
            else
            {
                //todo: converting to milliseconds for timer
                var delayMs = (int)(missionData.StartDelay * 1000);
                var timer = new Timer();
                
                _activeTimers.Add(timer);

                timer.StartAsync(delayMs, OnFinished).Forget(); 
                
                void OnFinished()
                {
                    _activeTimers.Remove(timer);
                    StartMission(missionData);
                }
            }
        }

        private void StartMission(MissionData missionData) 
        {
            var missionObj = Instantiate(missionData.MissionPrefab);
            missionObj.name = missionData.MissionName;

            var mission = missionObj.GetComponent<IMission>();

            mission.OnFinished += OnMissionFinished;
            
            mission.Begin();
            
            void OnMissionFinished()
            {
                MissionFinished(missionData);
            }
        }

        private void MissionFinished(MissionData finishedMissionData)
        {
            if (finishedMissionData.NextMission != null)
            {
                StartMissionChain(finishedMissionData.NextMission);
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