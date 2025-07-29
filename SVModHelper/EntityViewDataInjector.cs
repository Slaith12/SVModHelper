using MelonLoader;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
