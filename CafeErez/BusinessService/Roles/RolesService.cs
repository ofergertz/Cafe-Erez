using CafeErez.Shared.BusinessComponents;
using CafeErez.Shared.BusinessService;
using CafeErez.Shared.BusinessService.Roles;
using CafeErez.Shared.Constants;
using CafeErez.Shared.Infrastructure;
using CafeErez.Shared.Model.Identity;
using DAL.Connectivity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static CafeErez.Shared.Constants.Constants;

namespace BusinessService.Roles
{
    public class RolesService : IRolesService
    {
        private readonly IStringLocalizer<RolesService> _localizer;
        private readonly IServiceProvider _serviceProvider;
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly UserManager<User> _userManager;

        public RolesService(
            IStringLocalizer<RolesService> localizer,
            ApplicationDbContext db,
            IServiceProvider serviceProvider,
            IHttpContextAccessor httpContextAccessor,
            RoleManager<AppRole> roleManager,
            UserManager<User> userManager)
        {
            _localizer = localizer;
            _db = db;
            _serviceProvider = serviceProvider;
            _httpContextAccessor = httpContextAccessor;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public async Task<ServiceWrapper<List<RolesResponse>>> GetAllAsync()
        {
            var roleClaims = await _db.Roles.ToListAsync();
            var mapper = (IMapper<List<AppRole>,List<RolesResponse>>)_serviceProvider.GetService(typeof(IMapper<List<AppRole>, List<RolesResponse>>));
            var roleClaimsResponse = mapper.Map(roleClaims);
            return await ServiceWrapper<List<RolesResponse>>.SuccessAsync(roleClaimsResponse);
        }

        public async Task<ServiceWrapper<string>> SaveAsync(RoleRequest request)
        {
            if (string.IsNullOrEmpty(request.Id))
            {
                var existingRole = await _roleManager.FindByNameAsync(request.Name);
                if (existingRole != null) return await ServiceWrapper<string>.FailAsync(_localizer["Similar Role already exists."]);
                var response = await _roleManager.CreateAsync(new AppRole() { Name = request.Name, Description = request.Description });
                if (response.Succeeded)
                {
                    return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Role {0} Created."], request.Name));
                }
                else
                {
                    return await ServiceWrapper<string>.FailAsync(response.Errors.Select(e => _localizer[e.Description].ToString()).ToList());
                }
            }
            else
            {
                var existingRole = await _roleManager.FindByIdAsync(request.Id);
                //if (existingRole.Name == Constants.Roles.AdministratorRole || existingRole.Name == Constants.Roles.BasicRole)
                //{
                //    return await ServiceWrapper<string>.FailAsync(string.Format(_localizer["Not allowed to modify {0} Role."], existingRole.Name));
                //}
                existingRole.Name = request.Name;
                existingRole.NormalizedName = request.Name.ToUpper();
                existingRole.Description = request.Description;
                await _roleManager.UpdateAsync(existingRole);
                return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Role {0} Updated."], existingRole.Name));
            }

            //if (string.IsNullOrWhiteSpace(request.Name))
            //{
            //    return await ServiceWrapper<string>.FailAsync(_localizer["RoleName is required."]);
            //}

            //var existingRoleClaim =
            //    await _db.Roles
            //        .SingleOrDefaultAsync(x =>
            //            x.Name == request.Name);
            //if (existingRoleClaim != null)
            //{
            //    return await ServiceWrapper<string>.FailAsync(_localizer["Similar Role already exists."]);
            //}
            //var mapper = (IMapper<RoleRequest, AppRole>)_serviceProvider.GetService(typeof(IMapper<RoleRequest, AppRole>));
            //var appRole = mapper.Map(request);
            //await _db.Roles.AddAsync(appRole);
            //var UserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            //await _db.SaveChangesAsync();
            //return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["RoleName {0} created."], request.Name));
        }

        public async Task<ServiceWrapper<string>> DeleteAsync(string id)
        {
            var existingRole = await _roleManager.FindByIdAsync(id);
            if (existingRole.Name != Constants.Roles.AdministratorRole && existingRole.Name != Constants.Roles.BasicRole)
            {
                bool roleIsNotUsed = true;
                var allUsers = await _userManager.Users.ToListAsync();
                foreach (var user in allUsers)
                {
                    if (await _userManager.IsInRoleAsync(user, existingRole.Name))
                    {
                        roleIsNotUsed = false;
                    }
                }
                if (roleIsNotUsed)
                {
                    await _roleManager.DeleteAsync(existingRole);
                    return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Role {0} Deleted."], existingRole.Name));
                }
                else
                {
                    return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Not allowed to delete {0} Role as it is being used."], existingRole.Name));
                }
            }
            else
            {
                return await ServiceWrapper<string>.SuccessAsync(string.Format(_localizer["Not allowed to delete {0} Role."], existingRole.Name));
            }
        }

    }
}
