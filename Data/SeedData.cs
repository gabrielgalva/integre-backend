using Microsoft.AspNetCore.Identity;
using IntegreBackend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = "admin123@gmail.com";
        var admin = await userManager.FindByEmailAsync(adminEmail);

        if (admin == null)
        {
            var newAdmin = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                Cargo = "Administrador"
            };

            await userManager.CreateAsync(newAdmin, "Admin123@");
        }
    }
}
