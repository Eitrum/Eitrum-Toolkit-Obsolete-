using System;

namespace Eitrum.Engine.Core {
    public enum UpdateMode {
        None,
        PreUpdate,
        Update,
        LateUpdate,
        PostUpdate,
        FixedUpdate,
        ThreadedUpdate
    }
}
