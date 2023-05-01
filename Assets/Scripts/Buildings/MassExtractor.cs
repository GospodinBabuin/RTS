using UnityEngine;

namespace Buildings
{
    public class MassExtractor : Building
    {
        [SerializeField] private Player _player;

        [SerializeField] private int massPerIteration = 5;
        private float _getMassTimer = 1.0f;
        private float _getMassTimerDelta;

        private void Start()
        {
            _player = GameObject.FindWithTag("Player").GetComponent<Player>();
        }
        
        private void FixedUpdate()
        {
            TryGetMass();
        }

        private void TryGetMass()
        {
            if (_getMassTimerDelta > 0)
            {
                _getMassTimerDelta -= Time.deltaTime;
                return;
            }
            
            _getMassTimerDelta = _getMassTimer;
            GetMass();
        }

        private void GetMass()
        {
            _player.Bank.AddMass(this, massPerIteration);
        }

        public override void ActivateMenu()
        {
            throw new System.NotImplementedException();
        }
    }
}
