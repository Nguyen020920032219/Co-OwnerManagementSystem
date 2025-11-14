using System.Security.Claims;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Auths;
using Co_OwnerManagementSystem.AuthenticationBusiness.Models.Users;
using Co_OwnerManagementSystem.AuthenticationBusiness.Utils;
using Co_OwnerManagementSystem.AuthenticationData.Entities;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.Profiles;
using Co_OwnerManagementSystem.AuthenticationData.Repositories.Users;
using Co_OwnerManagementSystem.SharedLibrary.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace Co_OwnerManagementSystem.AuthenticationBusiness.Services.Auth.Impl;

public class AuthService : IAuthService
{
    private readonly ITokenService _tokenService;
    private readonly IUserRepository _userRepository;
    private readonly IProfileRepository _profileRepository;

   

    public AuthService(ITokenService tokenService, IUserRepository userRepository, IProfileRepository profileRepository)
    {
        _tokenService = tokenService;
        _userRepository = userRepository;

        _profileRepository = profileRepository;

    }
    public async Task<LoginResponseModel> Login(LoginRequestModel requestModel)
    {
        if (string.IsNullOrWhiteSpace(requestModel.PhoneNumber) || string.IsNullOrWhiteSpace(requestModel.Password))
            throw new ApiException(StatusCodes.Status400BadRequest, ErrorCodes.Validation,
                "Invalid or missing phone number or password");

        var existedUser = await _userRepository.DbSet().FirstOrDefaultAsync(u => u.PhoneNumber == requestModel.PhoneNumber);
        if (existedUser == null)
            throw new ApiException(StatusCodes.Status401Unauthorized, ErrorCodes.UnAuthorized,
                "Invalid phone number or password");
        var isMatchedPassword = PasswordHasher.VerifyPassword(requestModel.Password, existedUser.PasswordHash);
        if (!isMatchedPassword) throw new ApiException(StatusCodes.Status401Unauthorized, ErrorCodes.UnAuthorized,
            "Invalid phone number or password");
        var tokenResult = await _tokenService.CreateAuthToken(existedUser);
        return new LoginResponseModel()
        {
            AccessToken = tokenResult.AccessToken,
            RefreshToken = tokenResult.RefreshToken,
            Role = TypeConverter.GetRoleStringFromInt(existedUser.RoleId)
        };


    }

    public async Task<UserModel> Register(RegisterUserRequestModel request)
    {
        var existedUser = await _userRepository.DbSet().AnyAsync(u => u.PhoneNumber == request.PhoneNumber);
        if (existedUser)
        {
            throw new ApiException(StatusCodes.Status409Conflict, ErrorCodes.Conflict,
                "Phone number are existed");
        }
        
        if (request.Password != request.PasswordConfirm)
        {
            throw new ApiException(StatusCodes.Status400BadRequest, ErrorCodes.Validation, "Password does not match");
        }

        var existedEmail = await _profileRepository.DbSet().AnyAsync(p => p.Email == request.Email);
        if (existedEmail)
        {
            throw new ApiException(StatusCodes.Status409Conflict, ErrorCodes.Conflict, "Email are existed");
        }
        
        var user = new User()
        {
            PhoneNumber = request.PhoneNumber,
            PasswordHash = PasswordHasher.HashPassword(request.Password),
            RoleId = 3
        };
        var userCreated = await _userRepository.Add(user);
        if (userCreated is null)
        {
            throw new ApiException(StatusCodes.Status500InternalServerError, ErrorCodes.Internal,
                "Failed when create user");
        }

        var profile = new Profile()
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            Address = request.Address,
            Gender = request.Gender,
            DateOfBirth = request.DateOfBirth,
            UserId = userCreated.UserId,
            CitizenIdentification = "",
            DrivingLicense = ""
        };

        var profileCreated = await _profileRepository.Add(profile);
        if (profileCreated is null)
        {
            throw new ApiException(StatusCodes.Status500InternalServerError, ErrorCodes.Internal,
                "Failed when create profile");
        }
        
        return new UserModel()
        {
            UserId = userCreated.UserId,
            PhoneNumber = userCreated.PhoneNumber,
            Email = profileCreated.Email,
            FirstName = profileCreated.FirstName,
            LastName = profileCreated.LastName,
            Gender = profileCreated.Gender,
            DateOfBirth = profileCreated.DateOfBirth,
            Address = profileCreated.Address
        };
    }

    public async Task<UserModel> GetMyProfile(int userId)
    {
        var user = await _userRepository
            .DbSet()
            .Include(p => p.Profile)
            .Select(
                result => new UserModel()
                {
                    UserId = result.UserId,
                    PhoneNumber = result.PhoneNumber,
                    Email = result.Profile.Email,
                    FirstName = result.Profile.FirstName,
                    LastName = result.Profile.LastName,
                    Gender = result.Profile.Gender,
                    DateOfBirth = result.Profile.DateOfBirth,
                    Address = result.Profile.Address
                }
                )
            .FirstOrDefaultAsync(u => u.UserId == userId);
        if (user == null)
        {
            throw new ApiException(StatusCodes.Status404NotFound, ErrorCodes.NotFound, "User not found");
        }
        return user;
    }

    public async Task<bool> UpdateProfile(int userId, UpdateProfileModel model)
    {
        var existedProfile = await _profileRepository.DbSet().FirstOrDefaultAsync(u => u.UserId == userId);
        if (existedProfile == null)
        {
            throw new ApiException(StatusCodes.Status404NotFound, ErrorCodes.NotFound, "Profile not found");
        }
        if (!string.IsNullOrWhiteSpace(model.Email))
        {
            existedProfile.Email = model.Email;
        }
        if (!string.IsNullOrWhiteSpace(model.FirstName))
        {
            existedProfile.FirstName = model.FirstName;
        }
        if (!string.IsNullOrWhiteSpace(model.LastName))
        {
            existedProfile.LastName = model.LastName;
        }
        if (!string.IsNullOrWhiteSpace(model.Address))
        {
            existedProfile.Address = model.Address;
        }
        if (!string.IsNullOrWhiteSpace(model.Gender))
        {
            existedProfile.Gender = model.Gender;
        }
        if (model.DateOfBirth != null)
        {
            existedProfile.DateOfBirth = model.DateOfBirth;
        }
        var result = await _profileRepository.Update(existedProfile);

        return result != null;
    }

    
    
}