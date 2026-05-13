using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
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
            if (await userManager.FindByEmailAsync("admin@lojistik.com") == null)
            {
                var adminUser = new IdentityUser { UserName = "admin@lojistik.com", Email = "admin@lojistik.com", EmailConfirmed = true };
                var createAdmin = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createAdmin.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // --- DEPO GÖREVLİSİ HESABI ---
            if (await userManager.FindByEmailAsync("depo@lojistik.com") == null)
            {
                var depoUser = new IdentityUser { UserName = "depo@lojistik.com", Email = "depo@lojistik.com", EmailConfirmed = true };
                var createDepo = await userManager.CreateAsync(depoUser, "Depo123!");
                if (createDepo.Succeeded)
                {
                    await userManager.AddToRoleAsync(depoUser, "DepoGorevlisi");
                }
            }

            // --- KURYE HESABI ---
            if (await userManager.FindByEmailAsync("kurye@lojistik.com") == null)
            {
                var kuryeUser = new IdentityUser { UserName = "kurye@lojistik.com", Email = "kurye@lojistik.com", EmailConfirmed = true };
                var createKurye = await userManager.CreateAsync(kuryeUser, "Kurye123!");
                if (createKurye.Succeeded)
                {
                    await userManager.AddToRoleAsync(kuryeUser, "Kurye");
                }
            }

            // --- MÜŞTERİ HESABI (YENİ EKLENDİ) ---
            if (await userManager.FindByEmailAsync("musteri@lojistik.com") == null)
            {
                var musteriUser = new IdentityUser { UserName = "musteri@lojistik.com", Email = "musteri@lojistik.com", EmailConfirmed = true };
                var createMusteri = await userManager.CreateAsync(musteriUser, "Musteri123!");
                if (createMusteri.Succeeded)
                {
                    await userManager.AddToRoleAsync(musteriUser, "Musteri");
                }
            }
        }
    }
}