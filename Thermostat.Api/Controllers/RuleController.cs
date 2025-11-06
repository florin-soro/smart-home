using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Thermostat.Application.Interactors;
using Thermostat.Application.Interactors.RequetsDto;

namespace Thermostat.Application.EnvironmentalConditions.Rule.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public partial class RuleController : ControllerBase
    {
        private readonly AddRuleInteractor addRuleInteractor;

        public RuleController(AddRuleInteractor addRuleInteractor)
        {
            this.addRuleInteractor = addRuleInteractor;
        }

        [HttpPost]
        public async Task<IActionResult> AddRule([FromBody] NewRuleDto ruleDto)
        {
            if (ruleDto == null)
                return BadRequest("Rule data is required.");

            await addRuleInteractor.HandleAsyncAsync(new InsertRuleRequest
            {
                Name = ruleDto.Name,
                HouseId = ruleDto.HouseId,
                SensorId = ruleDto.SensorId,
                RuleName = ruleDto.RuleName,
                RuleParameters = ruleDto.RuleParameters,
                Action = ruleDto.Action,
                ActionParameters = ruleDto.ActionParameters
            });
            return Ok();
        }
    }
}