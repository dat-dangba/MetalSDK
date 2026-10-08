using UnityEngine;

namespace Metal
{
    public class GameBootstrap : BaseBootstrap
    {
        private static GameBootstrap Instance;
        private bool _shouldMerge;
        protected override ServiceContainer Container => ServiceLocator.Game;

        protected override void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                _shouldMerge = false;
            }
            else
            {
                _shouldMerge = true;
            }

            base.Awake();

            if (!_shouldMerge) return;

            MergeGameBootstrap();
        }

        private void MergeGameBootstrap()
        {
            Instance._Services.AddRange(_Services);
            Transform parent = Instance.transform;
            GameObject go = new GameObject("---------")
            {
                transform =
                {
                    parent = parent
                }
            };
            for (int i = transform.childCount - 1; i >= 0; i--)
            {
                transform.GetChild(i).parent = parent;
            }

            Destroy(gameObject);
        }
    }
}
