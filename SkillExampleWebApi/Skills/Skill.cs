using SkillExampleWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SkillExampleWebApi.Skills
{
    public abstract class Skill
    {
        /// <summary>
        /// Main skill entry point.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        public abstract SkillResponseModel PerformAction(SkillRequestModel request);

        /// <summary>
        /// Gets the parameter from NLU parameter collection by name.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="parameters">The parameters.</param>
        /// <returns></returns>
        protected virtual IntentParameter GetParameter(string key, SkillRequestModel request)
        {
            var parameter = request.Intent.Parameters?.FirstOrDefault(p => p.Name.ToLower().Equals(key.ToLower()));

            return parameter;
        }

        /// <summary>
        /// Creates the skill response model.
        /// </summary>
        /// <param name="say">The say.</param>
        /// <param name="customValues">The custom values.</param>
        /// <returns></returns>
        protected SkillResponseModel CreateSkillResponse(string say)
        {
            SkillResponseModel res = new SkillResponseModel();

            res.Result.IntroSpeakOut = say;
            return res;
        }
    }
}