using Lines.Infrastructure.Context;
using Lines.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore; 
using System.Linq.Expressions;

namespace Lines.Infrastructure.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<ApplicationUser?> FindByConditionAsync(this UserManager<ApplicationUser> userManager,
            ApplicationDBContext context, Expression<Func<ApplicationUser, bool>> predicate)
        {
            return await context.ApplicationUsers.FirstOrDefaultAsync(predicate);
        }


        public static async Task<IEnumerable<T?>> SelectWhere<T>(this UserManager<ApplicationUser> userManager, 
          ApplicationDBContext context, Expression<Func<ApplicationUser, bool>> predicate
            , Expression<Func<ApplicationUser, T>> selector)
        {
            return await context.ApplicationUsers.Where(predicate)
                                                 .Select(selector)
                                                 .ToListAsync();
        }
    }

}
