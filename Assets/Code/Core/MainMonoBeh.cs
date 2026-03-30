using UnityEngine;
using Zenject;

namespace Core
{
    public class MainMonoBeh : MonoBehaviour
    {
        private Presenters _presenters;
        private GameFactory _gameFactory;

        [Inject]
        public void Construct(Presenters presenters, GameFactory gameFactory)
        {
            _presenters = presenters;
            _gameFactory = gameFactory;
        }

        private void Awake()
        {
            _gameFactory.Init();
            _presenters.PreInitialization();
        }
        void Start()
        {
            _presenters.Initialization();
        }
        private void Update()
        {
            float deltaTime = Time.deltaTime;
            _presenters.Execute(deltaTime);
        }
        private void FixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            _presenters.FixedExecute(fixedDeltaTime);
        }
        private void OnDestroy()
        {
            _presenters.Cleanup();
        }
    }
}
