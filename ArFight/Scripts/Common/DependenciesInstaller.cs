using ArFight.Scripts.Game;
using UnityEngine;

namespace ArFight.Scripts
{
    [DefaultExecutionOrder(-5000)]
    public class DependenciesInstaller : MonoBehaviour
    {
        public GameDataSO GameDataSO;
        public TargetsController TargetsController;
        public GameUI GameUI;
        public SoundsController SoundsController;
        public void Awake()
        {
            ServiceLocator.Clear();
            ServiceLocator.Register(GameDataSO);
            ServiceLocator.Register(TargetsController);
            ServiceLocator.Register(GameUI);
            ServiceLocator.Register(SoundsController);
            GameController gameController = new GameController();
            ServiceLocator.Register(gameController);
            gameController.Initialize();
        }
    }
}