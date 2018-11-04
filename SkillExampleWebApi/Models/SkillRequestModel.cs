using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillExampleWebApi.Models
{
    public class SkillRequestModel
    {
        public string SessionId { get; set; }
        public string UserId { get; set; }
        public string AgentId { get; set; }
        public string Language { get; set; }
        public float TimezoneOffset { get; set; } = 0;

        public Intent Intent { get; set; }

        public List<KeyValuePair<string, string>> CustomValues { get; set; }

        public List<KeyValuePair<string, string>> Extra { get; set; }
    }

    public class Intent
    {
        public string Action { get; set; }
        public string Response { get; set; }
        public string Name { get; set; }
        public IList<IntentParameter> Parameters { get; set; }
    }

    public class IntentParameter
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string FormattedValue { get; set; }
    }
}