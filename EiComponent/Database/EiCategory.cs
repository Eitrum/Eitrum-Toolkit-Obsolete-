using System;
using System.Collections.Generic;

namespace Eitrum {
    [Serializable]
    public class EiCategory {
        public string categoryName;
        public List<EiEntry> entries = new List<EiEntry>();
    }
}