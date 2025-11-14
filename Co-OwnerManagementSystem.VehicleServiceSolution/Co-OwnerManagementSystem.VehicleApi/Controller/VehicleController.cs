using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.VehicleApplication.Models;
using Co_OwnerManagementSystem.VehicleApplication.Services;
using Co_OwnerManagementSystem.VehicleInfrastructure.Enum;
using Microsoft.AspNetCore.Mvc;

namespace Co_OwnerManagementSystem.VehicleApi.Controller;

[Route("api/v1/vehicles")]
public class VehicleController(IVehicleService vehicleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<VehicleModel>>> GetVehicles(
        string? model,
        VehicleStatus? status,
        int pageNumber = 1,
        int pageSize = 10
    )
    {
        var vehicles = await vehicleService.GetVehicles(pageNumber, pageSize, model, status);
        return Ok(vehicles);
    }

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetVehicle([FromRoute]int id)
    {
        try
        {
            var vehicle = await vehicleService.GetVehicle(id);
            return Ok(vehicle);
        }
        catch (ApiException e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult<VehicleModel>> CreateVehicle([FromBody]VehicleCreateModel vehicleModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdVehicle = await vehicleService.CreateVehicle(vehicleModel);
            return CreatedAtAction(nameof(GetVehicle), new { id = createdVehicle.VehicleId }, createdVehicle);
        }
        catch (ApiException e)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPut("{id:int}")]
    public async Task<ActionResult<VehicleModel>> UpdateVehicle([FromRoute]int id, [FromBody]VehicleUpdateModel vehicleModel)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var updatedVehicle = await vehicleService.UpdateVehicle(id, vehicleModel);
            return Ok(updatedVehicle);
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(e.Message);
        }catch (ApiException e) when(e.StatusCode == StatusCodes.Status400BadRequest)
        {
            return BadRequest(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpPatch("{id:int}/toggle")]
    public async Task<ActionResult> ToggleVehicle([FromRoute]int id)
    {
        try
        {
            var result = await vehicleService.ToggleVehicle(id);
            return Ok("Vehicle status toggled successfully.");
        }
        catch (ApiException e) when (e.StatusCode == StatusCodes.Status404NotFound)
        {
            return NotFound(e.Message);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    
}