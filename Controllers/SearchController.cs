using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RailwayReservation.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using RailwayReservation.Interfaces;     

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    private readonly ITrainService _trainService;

    public SearchController(ITrainService trainService) => _trainService = trainService;

    [HttpGet("trains")]
    public async Task<IActionResult> GetTrains([FromQuery] string source, [FromQuery] string destination)
    {
        var trains = await _trainService.GetAvailableTrainsAsync(source, destination);
        return Ok(trains);
    }

    [HttpGet("time-table")]
    public async Task<IActionResult> GetTimeTable()
    {
        var trains = await _trainService.GetAllTrainsAsync();
        return Ok(trains);
    }

    [HttpGet("fare-estimate")]
    public async Task<IActionResult> GetFare(int trainId, int adults, int children)
    {
        var totalFare = await _trainService.CalculateFareAsync(trainId, adults, children);
        return Ok(new { TotalFare = totalFare });
    }
}