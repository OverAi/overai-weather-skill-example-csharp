using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillExampleWebApi.Models
{
    public class SkillResponseModel
    {
        public Result Result { get; set; } = new Result();

        /// <summary>
        /// This action is the first priority. If filled, the Intro speak out will be ignored
        /// </summary>
        public ForceIntentModel ForceIntent { get; set; }
    }

    public class Result
    {
        public string IntroSpeakOut { get; set; }
    }

    public class ForceIntentModel
    {
        public string IntentName { get; set; }
        public IList<IntentParameter> Parameters { get; set; }
    }
}