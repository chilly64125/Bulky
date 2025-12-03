using BulkyBook.DataAcess.Data;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BulkyBook.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }


        public void Initialize()
        {


            //migrations if they are not applied
            try
            {
                if (_db.Database.GetPendingMigrations().Count() > 0)
                {
                    _db.Database.Migrate();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("_db.Database.Migrate() Error:" + ex.Message);
            }



            //create roles if they are not created
            if (!_roleManager.RoleExistsAsync(SD.Role_Customer).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Company)).GetAwaiter().GetResult();


                //if roles are not created, then we will create admin user as well
                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "admin@chen",
                    Email = "admin@chen",
                    Name = "管理員",
                    PhoneNumber = "1112223333",
                    StreetAddress = "台中巿****",
                    State = "IL",
                    PostalCode = "114341",
                    City = "Taichung"
                }, "Admin1788@").GetAwaiter().GetResult();

                ApplicationUser? user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@chen");
                if (user != null) {
                    _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
                }

                //_userManager.CreateAsync(new ApplicationUser
                //{
                //    UserName = "michael112246@yahoo.com.tw",
                //    Email = "michael112246@yahoo.com.tw",
                //    Name = "系統維護",
                //    PhoneNumber = "2223334444",
                //    StreetAddress = "台中巿****",
                //    State = "中華民國",
                //    PostalCode = "114342",
                //    City = "台中巿"
                //}, "123Aa*").GetAwaiter().GetResult();                           

                // user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "michael112246@yahoo.com.tw");
                //_userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();


                _userManager.CreateAsync(new ApplicationUser
                {
                    UserName = "kind@chen",
                    Email = "kind@chen",
                    Name = "穎川陳氏宗親",
                    PhoneNumber = "2223335555",
                    StreetAddress = "台中巿****",
                    State = "中華民國",
                    PostalCode = "114343",
                    City = "台中巿"
                }, "1788Aa@").GetAwaiter().GetResult();

                user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "kind@chen");
                if (user != null) {
                    _userManager.AddToRoleAsync(user, SD.Role_Customer).GetAwaiter().GetResult();
                }

                //_userManager.CreateAsync(new ApplicationUser
                //{
                //    UserName = "abc@def1",
                //    Email = "abc@def1",
                //    Name = "陳會員1",
                //    PhoneNumber = "2223336666",
                //    StreetAddress = "台中巿****",
                //    State = "中華民國",
                //    PostalCode = "114344",
                //    City = "台中巿"
                //}, "123Aa*").GetAwaiter().GetResult();

                //user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "abc@def1");
                //_userManager.AddToRoleAsync(user, SD.Role_Company).GetAwaiter().GetResult();


                //_userManager.CreateAsync(new ApplicationUser
                //{
                //    UserName = "abc@def2",
                //    Email = "abc@def2",
                //    Name = "陳會員2",
                //    PhoneNumber = "2223337777",
                //    StreetAddress = "台中巿****",
                //    State = "中華民國",
                //    PostalCode = "114341",
                //    City = "台中巿"
                //}, "123Aa*").GetAwaiter().GetResult();

                //user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "abc@def2");
                //_userManager.AddToRoleAsync(user, SD.Role_Company).GetAwaiter().GetResult();
            }

            return;
        }
    }
}
