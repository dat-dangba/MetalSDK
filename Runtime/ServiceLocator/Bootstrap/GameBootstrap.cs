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
        }

        protected override void MergeBootstrap()
        {
            if (!_shouldMerge) return;

            Instance._Services.AddRange(_Services);
            Transform parent = Instance.transform;
            GameObject go = new GameObject("---------")
            {
                transform =
                {
                    parent = parent
                }
            };

            int count = transform.childCount;
            for (int i = 0; i < count; i++)
            {
                transform.GetChild(0).parent = parent;
            }

            Destroy(gameObject);
        }
    }
}
