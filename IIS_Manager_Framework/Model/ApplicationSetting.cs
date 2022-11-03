using System.Collections.Generic;

namespace IIS_Manager_Framework.Model
{
    public class ApplicationSetting
    {
        public ManageServer ManageServer { get; set; }
    }
    public class ManageServer
    {
        public ApplicationPool ApplicationPool { get; set; }
        public ApplicationSite ApplicationSite { get; set; }
    }
    public class ApplicationPool
    {
        public List<string> Start { get; set; }
        public List<string> Stop { get; set; }
    }
    public class ApplicationSite
    {
        public List<string> Start { get; set; }
        public List<string> Stop { get; set; }
    }
}
