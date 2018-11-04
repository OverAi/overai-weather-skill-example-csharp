using SkillExampleWebApi.Models;
using SkillExampleWebApi.Skills;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SkillExampleWebApi.Controllers
{
    
    public class WeatherController : ApiController
    {
        /// <summary>
        /// main entry point
        /// in the root of the pfoject there is a file called RequestExample.json 
        /// which contains an example body of a request to a skill you can use it 
        /// to test this skill.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult Post([FromBody]SkillRequestModel request)
        {
            WeatherSkill skill = new WeatherSkill();

            return Ok(skill.PerformAction(request));
        }
    }
}
