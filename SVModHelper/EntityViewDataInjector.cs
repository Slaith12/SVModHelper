using MelonLoader;
using System.Collections;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace SVModHelper
{
    internal class EntityViewDataInjector : AsyncOperationBase<AEntityViewDataSO>
    {
        public AEntityViewDataSO result;

        public EntityViewDataInjector(AEntityViewDataSO entityView)
        {
            result = entityView;
            MelonCoroutines.Start(WaitCoroutine());
        }

        public override void Execute()
        {

        }

        private IEnumerator WaitCoroutine()
        {
            yield return null;
            Complete(result, true, "");
        }
    }
}
