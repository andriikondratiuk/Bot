using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SashaGrayBot
{
    /// <summary>
    /// lastBuild/api/json
    /// </summary>
    public class JenkisBuildJson
    {
        public string _class { get; set; }
        public object[] actions { get; set; }
        public string[] artifacts { get; set; }
        public bool building { get; set; }
        public string description { get; set; }
        public string displayName { get; set; }
        public int duration { get; set; }
        public int estimatedDuration { get; set; }
        public object executor { get; set; }
        public string fullDisplayName { get; set; }
        public string id { get; set; }
        public bool keepLog { get; set; }
        public int number { get; set; }
        public int queueId { get; set; }
        public string result { get; set; }
        public long timestamp { get; set; }
        public string url { get; set; }
        public string builtOn { get; set; }
        public object changeSet { get; set; }
    }
}
