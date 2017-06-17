using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SAC.Domain;
using SAC.Domain.Models;

namespace SAC.Migrations
{
    public sealed class Configuration : DbMigrationsConfiguration<SacContext>
    {
        private bool initialMigration { get; set; }

        public Configuration()
        {
            AutomaticMigrationsEnabled = false;

            //Check if the initial migration is pending
            var migrator = new DbMigrator(this);
            initialMigration = migrator.GetPendingMigrations().Any(m => m.Contains("Initial"));
        }

        protected override void Seed(SacContext context)
        {
            //only seed if initial migration was performed this update
            if (initialMigration)
            {
                SeedColors(context);

                SeedRoles(context);

                SeedGroups(context);

                // Save to get foreign keys
                context.SaveChanges();

                SeedUsers(context);

                SeedClubs(context);

                SeedClasses(context);

                // Save everying added to the context
                context.SaveChanges();

                SeedUserRoles(context);

                SeedSchedules(context);

                // Save the added roles
                context.SaveChanges();
            }
        }

        private void SeedGroups(SacContext context)
        {
            int order = 0;

            context.Groups.AddOrUpdate(x => x.Id,
                new Group()
                {
                    Name = "Money",
                    SortOrder = order++
                },
                new Group()
                {
                    Name = "Mens",
                    SortOrder = order++
                },
                new Group()
                {
                    Name = "Womens",
                    SortOrder = order++
                },
                new Group()
                {
                    Name = "Juniors",
                    SortOrder = order++
                },
                new Group()
                {
                    Name = "Seniors",
                    SortOrder = order++
                }
            );
        }

        private void SeedRoles(SacContext context)
        {

            context.Roles.AddOrUpdate(x => x.Id,
                new AspNetRole()
                {
                    Name = "Tech Admin"
                },

                new AspNetRole()
                {
                    Name = "Scheduler"
                },

                new AspNetRole()
                {
                    Name = "Club Admin"
                },

                new AspNetRole()
                {
                    Name = "Club User"
                }
            );
        }

        private void SeedUsers(SacContext context)
        {
            context.Users.AddOrUpdate(x => x.Id,
                new AspNetUser()
                {
                    Email = "george.prado@outlook.com",
                    EmailConfirmed = true,
                    PasswordHash = "AIgbEEDXPWZVazTJQ5ZSSTUSlxMDDvJ8URKgl60fek7K+NDDDnJ4vzhEZk/gbwyhbg==",
                    SecurityStamp = "3a886fbb-6693-4890-b4dc-ddf19a756a7b",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "george.prado@outlook.com",
                },

                new AspNetUser()
                {
                    Email = "bill@billwilliams.biz",
                    EmailConfirmed = true,
                    PasswordHash = "ANP0iJZ4D3Tw+UD4ddXYiUD7G+95D00LR9+DuhHZaH9zS+PtOC91I0bhaNib423dIQ==",
                    SecurityStamp = "f7cdf28c-3c90-4de0-8f51-c657fd145ccd",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "bill@billwilliams.biz",
                }
            );
        }

        private void SeedUserRoles(SacContext context)
        {
            var admin = context.Roles.First(r => r.Name == "Tech Admin");

            context.Users.ToList().ForEach(u => u.AspNetRoles.Add(admin));
        }

        private void SeedClubs(SacContext context)
        {
            context.Clubs.AddOrUpdate(x => x.Id,
                new Club()
                {
                    Name = "Robin Hood Archery Club",
                    Address = "3870 Homestead Rd",
                    CityStateZip = "Rock Hill, SC",
                    Contact = "Kenny Cobb",
                    Phone = "(803) 329-6639",
                    Email = "ken@huntintheworld.com",
                    Website = "https://www.facebook.com/pages/MECKLENBURG-BOWHUNTERS/132290430122624?ref=hl",
                    IconFileName = "RobinHood.png",
                    ShortName = "RobinHood"
                },

                new Club()
                {
                    Name = "Mecklenburg Wildlife Club",
                    Address = "2301 Wildlife Rd",
                    CityStateZip = "Charlotte, NC",
                    Contact = "Steven Walters",
                    Phone = "(704) 906-7651",
                    //Email = "",
                    Website = "https://www.facebook.com/pages/MECKLENBURG-BOWHUNTERS/132290430122624?ref=hl",
                    IconFileName = "Mecklenburg.png",
                    ShortName = "Mecklenburg"
                },

                new Club()
                {
                    Name = "Greenway Archery Club",
                    Address = "1300 Hwy. 21 By-pass",
                    CityStateZip = "Fort Mill, SC",
                    Contact = "Bill Steele",
                    Phone = "(803) 548-7252",
                    Email = "BillSteele@LeroySprings.com",
                    Website = "http://www.ascgreenway.org",
                    IconFileName = "Greenway.png",
                    ShortName = "Greenway"
                },

                new Club()
                {
                    Name = "Fort Mill Bowhunters",
                    Address = "1400 Williams Rd",
                    CityStateZip = "Fort Mill, SC",
                    Contact = "Adam McAnulty 803-242-3149  or  Billy Evans 803-412-1528",
                    Phone = "(803) 242-3149",
                    Email = "kitchensthatworkinc@gmail.com",
                    Website = "https://www.facebook.com/pages/Fort-Mill-Bowhunters/361416573871861?fref=ts",
                    IconFileName = "FortMill.png",
                    ShortName = "FortMill"
                },

                new Club()
                {
                    Name = "Indian Trail Bow Club",
                    Address = "",
                    CityStateZip = "Indian Trail, NC",
                    Contact = "Shane Kaylor",
                    Phone = "(704) 779-7253",
                    //Email = "",
                    Website = "http://www.indiantrailbowclub.com",
                    IconFileName = "IndianTrail.png",
                    ShortName = "IndianTrail"
                },

                new Club()
                {
                    Name = "Charlotte Rifle & Pistol Club",
                    Address = "9130 Kensington Dr.",
                    CityStateZip = "Waxhaw, NC 28173",
                    Contact = "Vance Tiller",
                    Phone = "(704) 930-8818",
                    Email = "Archery@CRPCPrograms.org",
                    Website = "http://crpc.clubexpress.com/",
                    IconFileName = "CRPC.png",
                    ShortName = "CRPC"
                }
            );
        }

        private void SeedColors(SacContext context)
        {
            context.Colors.AddOrUpdate(x => x.Id,
                new Color()
                {
                    Name = "Green",
                    HexCode = "#00FF00"
                },

                new Color()
                {
                    Name = "Yellow",
                    HexCode = "#FFFF00"
                },

                new Color()
                {
                    Name = "Blue",
                    HexCode = "#0000FF"
                },

                new Color()
                {
                    Name = "Red",
                    HexCode = "#FF0000"
                },

                new Color()
                {
                    Name = "Purple",
                    HexCode = "#800080"
                },

                new Color()
                {
                    Name = "Orange",
                    HexCode = "#FFA500"
                },

                new Color()
                {
                    Name = "White",
                    HexCode = "#FFFFFF"
                }
            );
        }

        private void SeedClasses(SacContext context)
        {
            var colors = context.Colors.ToDictionary(p => p.Name, p => p.Id);
            var groups = context.Groups.ToDictionary(p => p.Name, p => p.Id);

            context.Classes.AddOrUpdate(x => x.Id,
                new Class()
                {
                    Code = "PW",
                    Name = "PeeWee",
                    Description = "Age up to 8 years old.",
                    MaximumYardage = 10,
                    Known = false,
                    Restrictions = "None",
                    Group = context.Groups.Find(groups["Juniors"]),
                    Color = context.Colors.Find(colors["Green"])
                },

                new Class()
                {
                    Code = "CU",
                    Name = "Cub",
                    Description = "9 to 11 years old.",
                    MaximumYardage = 15,
                    Known = false,
                    Restrictions = "280 FPS",
                    Group = context.Groups.Find(groups["Juniors"]),
                    Color = context.Colors.Find(colors["Yellow"])
                },

                new Class()
                {
                    Code = "YO",
                    Name = "Youth",
                    Description = "12-14 years old.",
                    MaximumYardage = 25,
                    Known = false,
                    Restrictions = "280 FPS",
                    Group = context.Groups.Find(groups["Juniors"]),
                    Color = context.Colors.Find(colors["Blue"])
                },

                new Class()
                {
                    Code = "YA",
                    Name = "Young Adult",
                    Description = "15-17 years old.",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "280 FPS",
                    Group = context.Groups.Find(groups["Juniors"]),
                    Color = context.Colors.Find(colors["Red"])
                },

                new Class()
                {
                    Code = "TR",
                    Name = "Traditional",
                    Description = "15-17 years old.",
                    MaximumYardage = 25,
                    Known = false,
                    Restrictions = "Recurve or longbow, No sight or sight marks, no release aid, no stabilizers, no overdraw.",
                    Group = context.Groups.Find(groups["Juniors"]),
                    Color = context.Colors.Find(colors["Blue"])
                },

                new Class()
                {
                    Code = "MN",
                    Name = "Mens Novice Hunter",
                    Description = "Mens Novice Hunter",
                    MaximumYardage = 30,
                    Known = true,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    Group = context.Groups.Find(groups["Mens"]),
                    Color = context.Colors.Find(colors["Purple"])
                },

                new Class()
                {
                    Code = "WN",
                    Name = "Womens Novice Hunter",
                    Description = "Womens Novice Hunter",
                    MaximumYardage = 30,
                    Known = true,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    Group = context.Groups.Find(groups["Womens"]),
                    Color = context.Colors.Find(colors["Purple"])
                },

                new Class()
                {
                    Code = "MH",
                    Name = "Mens Hunter",
                    Description = "Mens Hunter",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    Group = context.Groups.Find(groups["Mens"]),
                    Color = context.Colors.Find(colors["Orange"])
                },

                new Class()
                {
                    Code = "WH",
                    Name = "Womens Hunter",
                    Description = "Womens Hunter",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    Group = context.Groups.Find(groups["Womens"]),
                    Color = context.Colors.Find(colors["Orange"])
                },

                new Class()
                {
                    Code = "MO",
                    Name = "Mens Open",
                    Description = "Mens Open",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "280 FPS.",
                    Group = context.Groups.Find(groups["Mens"]),
                    Color = context.Colors.Find(colors["White"])
                },

                new Class()
                {
                    Code = "WO",
                    Name = "Womens Open",
                    Description = "Womens Open",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "280 FPS.",
                    Group = context.Groups.Find(groups["Womens"]),
                    Color = context.Colors.Find(colors["Red"])
                },

                new Class()
                {
                    Code = "MK",
                    Name = "Mens Known",
                    Description = "Mens Known",
                    MaximumYardage = 45,
                    Known = true,
                    Restrictions = "280 FPS.",
                    Group = context.Groups.Find(groups["Mens"]),
                    Color = context.Colors.Find(colors["White"])
                },

                new Class()
                {
                    Code = "WK",
                    Name = "Womens Known",
                    Description = "Womens Known",
                    MaximumYardage = 40,
                    Known = true,
                    Restrictions = "280 FPS.",
                    Group = context.Groups.Find(groups["Womens"]),
                    Color = context.Colors.Find(colors["Red"])
                },

                new Class()
                {
                    Code = "SO",
                    Name = "Senior Open",
                    Description = "50 Years +",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "280 FPS.",
                    Group = context.Groups.Find(groups["Seniors"]),
                    Color = context.Colors.Find(colors["White"])
                },

                new Class()
                {
                    Code = "OU",
                    Name = "Outlaw",
                    Description = "Outlaw",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "None.",
                    Group = context.Groups.Find(groups["Money"]),
                    Color = context.Colors.Find(colors["White"])
                },

                new Class()
                {
                    Code = "SE",
                    Name = "Senior Outlaw",
                    Description = "50 Years +",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "None.",
                    Group = context.Groups.Find(groups["Money"]),
                    Color = context.Colors.Find(colors["Red"])
                },

                new Class()
                {
                    Code = "HO",
                    Name = "Hunter Outlaw",
                    Description = "Hunter Outlaw",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer.",
                    Group = context.Groups.Find(groups["Money"]),
                    Color = context.Colors.Find(colors["Orange"])
                }
            );
        }

        private void SeedSchedules(SacContext context)
        {
            context.Schedules.AddOrUpdate(
                new Schedule()
                {
                    Date = DateTime.Parse("06/04/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "Greenway")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("06/11/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "FortMill")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("06/24/2017"),
                    Description = "Coon Shoot Saturday Night",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "IndianTrail")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("06/25/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "IndianTrail")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("07/08/2017"),
                    Description = "ASA SC Championship",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "RobinHood")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("07/09/2017"),
                    Description = "ASA SC Championship & SAC",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "RobinHood")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("07/16/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "Mecklenburg")
                },
                new Schedule()
                {
                    Date = DateTime.Parse("01/14/2018"),
                    Description = "Polar Bear Shoot",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "CRPC")
                }
            );
        }
    }
}
