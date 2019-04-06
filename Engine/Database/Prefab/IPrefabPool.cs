using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Eitrum.Database {
    public interface IPrefabPool {
        int PoolSize { set; }
        bool KeepPoolAlive { set; }
        bool FillAtAwake { set; }
        bool UsePoolCallbacks { set; }
        void LoadPrefab();
    }
}