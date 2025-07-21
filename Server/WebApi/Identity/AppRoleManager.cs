using Microsoft.AspNetCore.Identity;

namespace WebApp.Identity;

public class AppRoleManager : RoleManager<AppRole>
{
    public AppRoleManager(IRoleStore<AppRole> store,
        IEnumerable<IRoleValidator<AppRole>> roleValidators, 
        ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, 
        ILogger<RoleManager<AppRole>> logger)
            : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }

}