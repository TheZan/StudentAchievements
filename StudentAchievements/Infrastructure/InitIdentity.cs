using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using StudentAchievements.Areas.Authorization.Models;

namespace StudentAchievements.Infrastructure
{
    public class InitIdentity
    {
        private IConfiguration configuration;
        private IUserRepository repository;

        public InitIdentity(IConfiguration _configuration, IUserRepository _repository)
        {
            configuration = _configuration;
            repository = _repository;
        }

        public async Task InitializeAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = configuration["Data:AdministratorAccount:Login"];
            string password = configuration["Data:AdministratorAccount:Password"];
            string adminName = configuration["Data:AdministratorAccount:Name"];

            if (await roleManager.FindByNameAsync("Admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }
            if (await roleManager.FindByNameAsync("Employer") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Employer"));
            }
            if (await roleManager.FindByNameAsync("Teacher") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Teacher"));
            }
            if (await roleManager.FindByNameAsync("Student") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("Student"));
            }

            if (await userManager.FindByNameAsync(adminEmail) == null)
            {
                IdentityUser admin = new IdentityUser() { Email = adminEmail, UserName = adminEmail };

                var result = await repository.AddUser(admin, password, new Administrator()
                {
                    Name = adminName,
                    Email = adminEmail
                });

                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, token);
                }
            }
        }
    }
}
