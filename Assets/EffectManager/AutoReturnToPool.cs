using UnityEngine;

namespace ZroeX.ObjectPool.Design {
    public class AutoReturnToPool : MonoBehaviour {
        [SerializeField]
        private float returnTime = 2f;

        private float timer;

        private void Update() {
            timer += Time.deltaTime;
            if ( timer >= returnTime ) {
                EffectPool.Instance.Return( gameObject );
                timer = 0f;
            }
        }
    }
}