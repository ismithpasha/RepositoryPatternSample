using RepositoryPatternSample.Api.ActionFilters;
using RepositoryPatternSample.Infrastructure.Core;
using RepositoryPatternSample.Services.IServices.Auth;
using RepositoryPatternSample.Services.Services.Auth;

public static class DependencyInjection
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilterAttribute>();

        services.AddTransient(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddTransient(typeof(IUserTypeService), typeof(UserTypeService));
        services.AddTransient(typeof(IUserService), typeof(UserService));
        services.AddScoped(typeof(IMenuService), typeof(MenuService));
        services.AddScoped(typeof(IRoleMenuPermissionService), typeof(RoleMenuPermissionService));
        services.AddScoped<IRoleService, RoleService>();
         

        services.AddScoped<IDapper, DapperContext>();

        return services;
    }
}