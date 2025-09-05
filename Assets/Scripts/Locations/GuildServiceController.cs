
    using System;

    using UnityEngine;

    public abstract class GuildServiceController : MonoBehaviour
    {
        protected virtual void Awake()
        {
            GuildManager.RegisterNewService(this);
        }
    }