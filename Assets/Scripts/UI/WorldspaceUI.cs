
    using Sirenix.OdinInspector;

    using UnityEngine;

    public class WorldspaceUI :MonoBehaviour
    {
        [SerializeField, InfoBox("Force disable on awake it interferes with Screen-space UI")]
        private bool _forceDisableOnAwake = true;
        
        protected virtual void Awake()
        {
            if (_forceDisableOnAwake)
            {
                gameObject.SetActive(false);
            }
        }
    }