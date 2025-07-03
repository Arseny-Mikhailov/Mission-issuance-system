using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(fileName = "NewMissionData", menuName = "Missions/MissionData")]
    public class MissionData : ScriptableObject
    { 
        [field: Header("Название миссии")]
        [field: SerializeField] 
        public string MissionName { get; private set; }
        
        //todo: replace with custom property drawer for interface implementations
        [field: Header("Префаб с логикой миссии")]
        [field: SerializeField] 
        public GameObject MissionPrefab { get; private set; }
       
        [field: Header("Задержка перед началом(в секундах)")]
        [field: SerializeField] 
        [field: Min(0f)]
        public float StartDelay { get; private set; }
        
        [field: Header("Следующая миссия")]
        [field: SerializeField]
        public MissionData NextMission { get; private set; }
    }
}