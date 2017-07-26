using System.Collections.Generic;

namespace WFPackCredsManager
{
    public class WFPackSettings
    {
        public class WorkflowInfo
        {
            public string Name { get; set; }
            public string Login { get; set; }
            public string EncryptedPassword { get; set; }
        }

        public string DefaultLogin { get; set; }
        public string DefaultEncPassword { get; set; }
        public List<WorkflowInfo> WorkflowList { get; set; }
    }
}
