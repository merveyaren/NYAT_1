using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace NYAT_1.Data
{
    public static class Rol
    {
        public static async Task SeedRolesAndUsersAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();

            // 1. ROLLERİ OLUŞTURMA
            string[] roleNames = { "Admin", "DepoGorevlisi", "Kurye", "Musteri" };
            foreach (var roleName in roleNames)
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            // 2. TEST KULLANICILARINI OLUŞTURMA VE ROL ATAMA

            // --- YÖNETİCİ (ADMIN) HESABI ---
            var adminUser = new IdentityUser { UserName = "admin@lojistik.com", Email = "admin@lojistik.com", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != adminUser.UserName))
            {
                // Kullanıcıyı oluştur (Şifre kuralı: Büyük harf, küçük harf, rakam ve özel karakter içermeli)
                var createAdmin = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin"); // Rolü ver
                }
            }

            // --- DEPO GÖREVLİSİ HESABI ---
            var depoUser = new IdentityUser { UserName = "depo@lojistik.com", Email = "depo@lojistik.com", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != depoUser.UserName))
            {
                var createDepo = await userManager.CreateAsync(depoUser, "Depo123!");
                if (createDepo.Succeeded)
                {
                    await userManager.AddToRoleAsync(depoUser, "DepoGorevlisi");
                }
            }

            // --- KURYE HESABI ---
            var kuryeUser = new IdentityUser { UserName = "kurye@lojistik.com", Email = "kurye@lojistik.com", EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != kuryeUser.UserName))
            {
                var createKurye = await userManager.CreateAsync(kuryeUser, "Kurye123!");
                if (createKurye.Succeeded)
                {
                    await userManager.AddToRoleAsync(kuryeUser, "Kurye");
                }
            }
        }
    }
}