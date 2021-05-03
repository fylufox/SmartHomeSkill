using System;
using System.Collections.Generic;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace SmartHomeSkill.JSON.discovery
{
    public class endpoint
    {
        public string endpointId { get; set; }
        public string manufacturerName { get; set; }
        public string description { get; set; }
        public string friendlyName { get; set; }
        public string[] displayCategories { get; set; }
        //public Additionalattributes additionalAttributes { get; set; }  //no requierd.
        public capabilities[] capabilities { get; set; }
        //public object[] connections { get; set; }   //no requierd.
        //public Relationships relationships { get; set; }    //no requierd.
        //public Cookie cookie { get; set; }  //no requierd.
    }

    public class capabilities
    {
        public string type { get; set; }
        public string @interface { get; set; }
        //public string instance { get; set; }    //no requierd
        public string version { get; set; }
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public Properties properties { get; set; }  //no requierd
        //public Capabilityresources capabilityResources { get; set; }    //no requierd
        //public Configuration configuration { get; set; }    //no requierd
        //public Semantics semantics { get; set; }    //no requierd
        //public object[] verificationsRequired { get; set; } //no requierd
    }

    public class Properties
    {
        public Supported[] supported { get; set; }
        public bool proactivelyReported { get; set; }
        public bool retrievable { get; set; }
    }

    public class Supported
    {
        public string name { get; set; }
    }

}
