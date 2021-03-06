﻿using System;
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
                context.SaveChanges();

                SeedContacts(context);

                SeedClasses(context);

                // Save everying added to the context
                context.SaveChanges();

                SeedUserRoles(context);

                // Save the added roles
                context.SaveChanges();

                const bool testData = true;
                if (testData)
                {
                    SeedSchedules(context);
                    context.SaveChanges();

                    SeedTournaments(context);
                    context.SaveChanges();

                    SeedCompetitors(context);
                    context.SaveChanges();
                }
            }
        }

        private void SeedGroups(SacContext context)
        {
            int order = 0;

            context.Groups.AddOrUpdate(x => x.Name,
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

            context.Roles.AddOrUpdate(x => x.Name,
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
            context.Users.AddOrUpdate(x => x.Email,
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
                },
                new AspNetUser()
                {
                    Email = "1@net.net",
                    EmailConfirmed = true,
                    PasswordHash = "ACLDEprUm/L+8S1X7FA0DXpULGiu8ihd0JUJ9bLTUgQNSSHmo96L3VanKmiXqk5MAA==",
                    SecurityStamp = "b816c11c-b2ae-4081-80ba-67d41f845472",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "1@net.net",
                },
                new AspNetUser()
                {
                    Email = "2@net.net",
                    EmailConfirmed = true,
                    PasswordHash = "AJpWas39byc9sh2RRhaZmJKpbgw8s4E4kp1Vcjgd1vgM0qSQ7LJq7WW0oM0DC3g3cw==",
                    SecurityStamp = "54355204-f9b8-4039-9816-6331247e86b4",
                    PhoneNumberConfirmed = false,
                    TwoFactorEnabled = false,
                    LockoutEnabled = true,
                    AccessFailedCount = 0,
                    UserName = "2@net.net",
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
            var contacts = context.Contacts;

            context.Clubs.AddOrUpdate(x => x.Name,
                new Club()
                {
                    Name = "Robin Hood Archery Club",
                    Address = "3870 Homestead Rd",
                    CityStateZip = "Rock Hill, SC",
                    Website = "https://www.facebook.com/pages/MECKLENBURG-BOWHUNTERS/132290430122624?ref=hl",
                    ShortName = "RobinHood",
                    GeoLocation = "34.996278, -81.053472",
                },

                new Club()
                {
                    Name = "Mecklenburg Wildlife Club",
                    Address = "2301 Wildlife Rd",
                    CityStateZip = "Charlotte, NC",
                    Website = "https://www.facebook.com/pages/MECKLENBURG-BOWHUNTERS/132290430122624?ref=hl",
                    ShortName = "Mecklenburg",
                    GeoLocation = "35.264250, -80.962083",
                },

                new Club()
                {
                    Name = "Greenway Archery Club",
                    Address = "1300 Hwy. 21 By-pass",
                    CityStateZip = "Fort Mill, SC",
                    Website = "http://www.ascgreenway.org",
                    ShortName = "Greenway",
                    GeoLocation = "35.043361, -80.918861",
                },

                new Club()
                {
                    Name = "Fort Mill Bowhunters",
                    Address = "1400 Williams Rd",
                    CityStateZip = "Fort Mill, SC",
                    Website = "https://www.facebook.com/pages/Fort-Mill-Bowhunters/361416573871861?fref=ts",
                    ShortName = "FortMill",
                    GeoLocation = "34.991583, -80.912861",
                },

                new Club()
                {
                    Name = "Indian Trail Bow Club",
                    Address = "",
                    CityStateZip = "Indian Trail, NC",
                    Website = "http://www.indiantrailbowclub.com",
                    ShortName = "IndianTrail",
                    GeoLocation = "35.108917, -80.614056",
                },

                new Club()
                {
                    Name = "Charlotte Rifle & Pistol Club",
                    Address = "9130 Kensington Dr.",
                    CityStateZip = "Waxhaw, NC 28173",
                    Website = "http://crpc.clubexpress.com/",
                    ShortName = "CRPC",
                    GeoLocation = "34.946250, -80.788056",
                },

                new Club()
                {
                    Name = "Lakeview Archery",
                    Address = "Chester State Park Hwy 72",
                    CityStateZip = "Chester SC 29706",
                    Website = "http://www.lakeviewarchery.org",
                    ShortName = "Lakeview",
                    GeoLocation = "34.683413,-81.248845",
                }
            );
        }

        private void SeedContacts(SacContext context)
        {
            var clubs = context.Clubs.ToDictionary(p => p.ShortName, p => p.Id);

            context.Contacts.AddOrUpdate(x => x.Name,
                new Contact()
                {
                    Name = "Kenny Cobb",
                    Phone = "(803) 329-6639",
                    Email = "ken@huntintheworld.com",
                    ClubId = clubs["RobinHood"],
                },
                new Contact()
                {
                    Name = "Steven Walters",
                    Phone = "(704) 906-7651",
                    //Email = "",
                    ClubId = clubs["Mecklenburg"],
                },
                new Contact()
                {
                    Name = "Bill Steele",
                    Phone = "(803) 548-7252",
                    Email = "BillSteele@LeroySprings.com",
                    ClubId = clubs["Greenway"],
                },
                new Contact()
                {
                    Name = "Billy Evans",
                    Phone = "(803) 412-1528",
                    Email = "kitchensthatworkinc@gmail.com",
                    ClubId = clubs["FortMill"],
                },
                new Contact()
                {
                    Name = "Adam McAnulty",
                    Phone = "(803) 242-3149",
                    Email = "kitchensthatworkinc@gmail.com",
                    ClubId = clubs["FortMill"],
                },
                new Contact()
                {
                    Name = "Shane Kaylor",
                    Phone = "(704) 779-7253",
                    //Email = "",
                    ClubId = clubs["IndianTrail"],
                },
                new Contact()
                {
                    Name = "Vance Tiller",
                    Phone = "(704) 930-8818",
                    Email = "Archery@CRPCPrograms.org",
                    ClubId = clubs["CRPC"],
                }
            );
        }

        private void SeedColors(SacContext context)
        {
            context.Colors.AddOrUpdate(x => x.Name,
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

            context.Classes.AddOrUpdate(x => x.Name,
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
            context.Schedules.AddOrUpdate(s => s.FromDate,
                new Schedule()
                {
                    FromDate = DateTime.Parse("06/04/2017"),
                    ToDate = DateTime.Parse("06/04/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "Greenway")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("06/11/2017"),
                    ToDate = DateTime.Parse("06/11/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "FortMill")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("06/24/2017"),
                    ToDate = DateTime.Parse("06/24/2017"),
                    Description = "Coon Shoot Saturday Night",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "IndianTrail")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("06/25/2017"),
                    ToDate = DateTime.Parse("06/25/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "IndianTrail")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("07/08/2017"),
                    ToDate = DateTime.Parse("07/09/2017"),
                    Description = "ASA SC Championship",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "RobinHood")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("07/09/2017"),
                    ToDate = DateTime.Parse("07/09/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "RobinHood")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("07/16/2017"),
                    ToDate = DateTime.Parse("07/16/2017"),
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "Mecklenburg")
                },
                new Schedule()
                {
                    FromDate = DateTime.Parse("01/14/2018"),
                    ToDate = DateTime.Parse("01/14/2018"),
                    Description = "Polar Bear Shoot",
                    Club = context.Clubs.FirstOrDefault(c => c.ShortName == "CRPC")
                }
            );
        }

        private void SeedTournaments(SacContext context)
        {
            var schedule = context.Schedules.FirstOrDefault();

            context.Tournaments.AddOrUpdate(
                new Tournament()
                {
                    Completed = true,
                    Schedules = {schedule}
                }
            );
        }

        private void SeedCompetitors(SacContext context)
        {
            var tournament = context.Tournaments.FirstOrDefault();
            var classes = context.Classes.ToDictionary(p => p.Name, p => p.Id);
            var currentDate = DateTime.Now;

            if (tournament != null)
                context.Competitors.AddOrUpdate(c => c.Archer,
                    new Competitor()
                    {
                        Archer = "Rosie",
                        Bonus = 0,
                        ClassId = classes["Cub"],
                        Score = 76,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(1)
                    },
                    new Competitor()
                    {
                        Archer = "Penny",
                        Bonus = 1,
                        ClassId = classes["Cub"],
                        Score = 110,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(2)
                    },
                    new Competitor()
                    {
                        Archer = "Bill",
                        Bonus = 3,
                        ClassId = classes["Mens Known"],
                        Score = 179,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(3)
                    },
                    new Competitor()
                    {
                        Archer = "George",
                        Bonus = 4,
                        ClassId = classes["Mens Known"],
                        Score = 179,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(4)
                    },
                    new Competitor()
                    {
                        Archer = "Abigail",
                        Bonus = 1,
                        ClassId = classes["Womens Novice Hunter"],
                        Score = 155,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(5)
                    },
                    new Competitor()
                    {
                        Archer = "Denise",
                        Bonus = 0,
                        ClassId = classes["Womens Novice Hunter"],
                        Score = 0,
                        TournamentId = tournament.Id,
                        CreatedDate = currentDate.AddSeconds(6)
                    }
                );
        }
    }
}
