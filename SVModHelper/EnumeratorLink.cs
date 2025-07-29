using Il2CppInterop.Runtime.Injection;
using System.Collections;

namespace SVModHelper
{
    internal class EnumeratorLink : Il2CppSystem.Object
    {
        private IEnumerator enumerator;

        public EnumeratorLink(IntPtr pointer) : base(pointer) {  }
        public EnumeratorLink(IEnumerator enumerator) : this(ClassInjector.DerivedConstructorPointer<EnumeratorLink>())
        {
            ClassInjector.DerivedConstructorBody(this);

            this.enumerator = enumerator;
        }

        public Il2CppSystem.Object Current => (Il2CppSystem.Object)enumerator.Current;
        public bool MoveNext()
        {
            return enumerator.MoveNext();
        }
        public void Reset()
        {
            enumerator.Reset();
        }
    }
}
