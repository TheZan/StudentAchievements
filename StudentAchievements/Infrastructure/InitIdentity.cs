using System.Text;
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

        public async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
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
                User admin = new User() { Email = adminEmail, UserName = adminEmail, Name = adminName};

                var result = await repository.AddUser(admin, password, new Administrator()
                {
                    User = admin
                });

                if (result.Succeeded)
                {
                    var token = await userManager.GenerateEmailConfirmationTokenAsync(admin);
                    await userManager.ConfirmEmailAsync(admin, token);
                }
            }
        }
    }
}
