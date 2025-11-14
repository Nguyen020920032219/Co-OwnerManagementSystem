using AutoMapper;
using AutoMapper.QueryableExtensions;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Co_OwnerManagementSystem.SharedLibrary.Paging;
using Co_OwnerManagementSystem.VehicleApplication.Models;
using Co_OwnerManagementSystem.VehicleInfrastructure.Entities;
using Co_OwnerManagementSystem.VehicleInfrastructure.Enum;
using Co_OwnerManagementSystem.VehicleInfrastructure.Repositories.Vehicles;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Co_OwnerManagementSystem.VehicleApplication.Services;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IMapper _mapper;

    public VehicleService(IVehicleRepository vehicleRepository, IMapper mapper)
    {
        _vehicleRepository = vehicleRepository;
        _mapper = mapper;
    }
    public async Task<PagedResult<VehicleOverallModel>> GetVehicles(int pageNumber, int pageSize, string? model, VehicleStatus? status)
    {
        var query = _vehicleRepository.DbSet()
            .AsQueryable()
            .AsNoTracking();
        if (!string.IsNullOrEmpty(model))
        {
            query = query.Where(v => v.Model.ToLower().Contains(model.ToLower()));
        }

        if (status != null)
        {
            query = query.Where(v => v.Status == (int)status.Value);
        }
        var totalCount = await query.CountAsync();
        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        query = query.OrderBy(v => v.VehicleId);
        
        var paginatedQuery = query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize);
        
        var projectedQuery = paginatedQuery
            .ProjectTo<VehicleOverallModel>(_mapper.ConfigurationProvider);
        var items = await projectedQuery.ToListAsync();
        return new PagedResult<VehicleOverallModel>(
            items,
            pageNumber,
            pageSize,
            totalCount,
            totalPages
        );
    }

    public async Task<VehicleModel> GetVehicle(int id)
    {
        var vehicle = await _vehicleRepository.DbSet()
            .FirstOrDefaultAsync(v => v.VehicleId == id);
        if (vehicle is null) throw new ApiException(StatusCodes.Status404NotFound,ErrorCodes.NotFound, "Vehicle not found");
        return _mapper.Map<VehicleModel>(vehicle);
    }

    public async Task<VehicleModel> CreateVehicle(VehicleCreateModel vehicleModel)
    {
        var vehicle = _mapper.Map<Vehicle>(vehicleModel);
        if(vehicle.Year > DateTime.Now.Year)
            throw new ApiException(StatusCodes.Status400BadRequest, ErrorCodes.Validation, "Vehicle year cannot be in the future");
        var result = await  _vehicleRepository.Add(vehicle);
        if(result is null) throw new Exception("Vehicle could not be created");
        return _mapper.Map<VehicleModel>(result);
    }

    public async Task<VehicleModel> UpdateVehicle(int id, VehicleUpdateModel vehicleModel)
    {
        var vehicle = await  _vehicleRepository.DbSet()
            .FirstOrDefaultAsync(v => v.VehicleId == id);
        if(vehicle is null) throw new ApiException(StatusCodes.Status404NotFound,ErrorCodes.NotFound, "Vehicle not found");
        if(vehicleModel.Year == null) vehicleModel.Year = vehicle.Year;
        if(vehicle.Year > DateTime.Now.Year)
            throw new ApiException(StatusCodes.Status400BadRequest, ErrorCodes.Validation, "Vehicle year cannot be in the future");
        if(vehicleModel.BatteryCapacity == null) vehicleModel.BatteryCapacity = vehicle.BatteryCapacity;
        if(vehicleModel.PurchasePrice == null) vehicleModel.PurchasePrice = vehicle.PurchasePrice;
        if(vehicleModel.CoOwnerGroupId == null) vehicleModel.CoOwnerGroupId = vehicle.CoOwnerGroupId;
        if(vehicleModel.Status == null) vehicleModel.Status = vehicle.Status;
        _mapper.Map(vehicleModel, vehicle);
        var result = await  _vehicleRepository.Update(vehicle);
        if(result is null) throw new Exception("Vehicle could not be updated");
        return _mapper.Map<VehicleModel>(result);
    }

    public async Task<bool> ToggleVehicle(int id)
    {
        var vehicle = await  _vehicleRepository.DbSet()
            .FirstOrDefaultAsync(v => v.VehicleId == id);
        if (vehicle is null) throw new ApiException(StatusCodes.Status404NotFound,ErrorCodes.NotFound, "Vehicle not found");
        if (vehicle.Status != 0) vehicle.Status = (int)VehicleStatus.Inactive;
        else vehicle.Status = (int)VehicleStatus.Available;
        await _vehicleRepository.Update(vehicle);
        return true;
    }
}