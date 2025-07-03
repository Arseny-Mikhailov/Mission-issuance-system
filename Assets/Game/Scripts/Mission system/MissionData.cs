using UnityEngine;

namespace Game.Scripts.Mission_system
{
    [CreateAssetMenu(fileName = "NewMissionData", menuName = "Missions/MissionData")]
    public class MissionData : ScriptableObject
    {
        public string missionName;
        public GameObject missionPrefab;
        public float startDelay;
        public MissionData nextMission;
    }
}