using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FootballData.Services.Interfaces;
using FootballData.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace FootballData.Api.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class DefaultController : ControllerBase
    {
        private readonly IImportFootballDataService importFootballDataService;
        private readonly IPlayerService playerService;
        private readonly ILogger<DefaultController> logger;

        public DefaultController(IImportFootballDataService importFootballDataService, 
            IPlayerService playerService, ILogger<DefaultController> logger)
        {
            this.importFootballDataService = importFootballDataService;
            this.playerService = playerService;
            this.logger = logger;
        }

        [HttpGet("/import-league/{code}")]
        public async Task<IActionResult> Import(string code)
        {
            try
            {
                var result = (await this.importFootballDataService.ImportLeagueAsync(code)).GetSucceededValue();
                return result switch
                {
                    OperationResultType.Successfull => StatusCode(201, "Successfully imported"),
                    OperationResultType.AlreadyDone => StatusCode(409, "League already imported"),
                    OperationResultType.NotFound => StatusCode(404, "Not found"),
                    _ => StatusCode(504, "Server Error"),
                };
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error on Action Import with code: {code}", code);
                return StatusCode(504, "Server Error");
            }
        }

        [HttpGet("/total-players/{code}")]
        public async Task<IActionResult> CountPlayersOnCompetition(string code)
        {
            try
            {
                var count = await this.playerService.CountTotalPlayersOnCompetition(code);
                if (!count.HasValue) return StatusCode(404, "Not found");
                return Ok(new { total = count });
            }
            catch (Exception e)
            {
                this.logger.LogError(e, "Error on Action CountPlayersOnCompetition with code: {code}", code);
                return StatusCode(504, "Server Error");
            }
        }
    }
}