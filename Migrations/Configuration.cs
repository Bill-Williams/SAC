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
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;

        }

        protected override void Seed(SacContext context)
        {

          
            NextEntityId = 0;
            SeedColors(context);
            // Save to get foreign keys

            context.SaveChanges();

            NextEntityId = 0;
            SeedRoles(context);

            NextEntityId = 0;
            SeedClubs(context);
            
            NextEntityId = 0;
            SeedClasses(context);

            // Save everying added to the context
            context.SaveChanges();
        }

        private void SeedRoles(SacContext context)
        {

            context.Roles.AddOrUpdate(x => x.Id,
                new AspNetRole()
                {
                    Id = "0c1c19f6-bbe1-467d-925b-b23b7a96d8ac",
                    Name = "Tech Admin"
                },

                new AspNetRole()
                {
                    Id = "0962b7ff-e0d3-4f51-8702-42310439ed5b",
                    Name = "Club Admin"
                },

                new AspNetRole()
                {
                    Id = "14d00ad1-c170-4a68-a63c-7d0d9d139e84",
                    Name = "Club User",
                }
            );

        }

        private void SeedClubs(SacContext context)
        {
            context.Clubs.AddOrUpdate(x => x.Id,
                new Club()
                {
                    Id = NextEntityId,
                    Name = "Robin Hood Archery Club",
                    Address = "3870 Homestead Rd",
                    CityStateZip = "Rock Hill, SC",
                    Contact = "Kenny Cobb",
                    Phone = "(803) 329-6639",
                    Email = "ken@huntintheworld.com",
                    Website = "https://www.facebook.com/pages/MECKLENBURG-BOWHUNTERS/132290430122624?ref=hl",
                    IconFileName = "RobinHood.png"
                },

                new Club()
                {
                    Id = NextEntityId,
                    Name = "Greenway Archery Club",
                    Address = "1300 Hwy. 21 By-pass",
                    CityStateZip = "Fort Mill, SC",
                    Contact = "Bill Steele",
                    Phone = "(803) 548-7252",
                    Email = "BillSteele@LeroySprings.com",
                    Website = "www.ascgreenway.org",
                    IconFileName = "Greenway.png"
                },

                new Club()
                {
                    Id = NextEntityId,
                    Name = "Lakeview Archery Club",
                    Address = "Chester State Park",
                    CityStateZip = "Chester, SC",
                    Contact = "Jason or Dianna Ghent",
                    Phone = "",
                    Email = "",
                    Website = "https://www.facebook.com/lakeview.archeryclub?fref=ts",
                    IconFileName = "Lakeview.png"
                },

                new Club()
                {
                    Id = NextEntityId,
                    Name = "Fort Mill Bowhunters",
                    Address = "1400 Williams Rd",
                    CityStateZip = "Fort Mill, SC",
                    Contact = "Adam McAnulty 803-242-3149  or  Billy Evans 803-412-1528",
                    Phone = "",
                    Email = "kitchensthatworkinc@gmail.com ",
                    Website = "https://www.facebook.com/pages/Fort-Mill-Bowhunters/361416573871861?fref=ts",
                    IconFileName = "FortMill.png"
                },

                new Club()
                {
                    Id = NextEntityId,
                    Name = "Indian Trail Bow Club",
                    Address = "",
                    CityStateZip = "Indian Trail, NC",
                    Contact = "Shane Kaylor",
                    Phone = "(704) 779-7253",
                    Email = "",
                    Website = "www.indiantrailbowclub.com",
                    IconFileName = "IndianTrail.png"
                },

                new Club()
                {
                    Id = NextEntityId,
                    Name = "Charlotte Rifle & Pistol Club",
                    Address = "9130 Kensington Dr.",
                    CityStateZip = "Waxhaw, NC 28173",
                    Contact = "Vance Tiller",
                    Phone = "(704) 930-8818",
                    Email = "Archery@CRPCPrograms.org",
                    Website = "http://crpc.clubexpress.com/",
                    IconFileName = "CRPC.png"
                }
            );
        }

        private void SeedColors(SacContext context)
        {

            context.Colors.AddOrUpdate(x => x.Id,
                new Color()
                {
                    Id = NextEntityId,
                    Name = "Green",
                    HexCode = "#00FF00"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "Yellow",
                    HexCode = "#FFFF00"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "Blue",
                    HexCode = "#0000FF"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "Red",
                    HexCode = "#FF0000"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "Purple",
                    HexCode = "#800080"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "Orange",
                    HexCode = "#FFA500"
                },

                new Color()
                {
                    Id = NextEntityId,
                    Name = "White",
                    HexCode = "#FFFFFF"
                }
            );
        }

        private void SeedClasses(SacContext context)
        {
            var colors = context.Colors.ToDictionary(p => p.Name, p => p.Id);

            context.Classes.AddOrUpdate(x => x.Id,
                new Class()
                {
                    Code = "PW",
                    Name = "PeeWee",
                    Description = "Age up to 8 years old.",
                    MaximumYardage = 10,
                    Known = false,
                    Restrictions = "None",
                    ColorId = colors["Green"]
                },

                new Class()
                {
                    Code = "CU",
                    Name = "Cub",
                    Description = "9 to 11 years old.",
                    MaximumYardage = 15,
                    Known = false,
                    Restrictions = "280 FPS",
                    ColorId = colors["Yellow"]
                },

                new Class()
                {
                    Code = "YO",
                    Name = "Youth",
                    Description = "12-14 years old.",
                    MaximumYardage = 25,
                    Known = false,
                    Restrictions = "280 FPS",
                    ColorId = colors["Blue"]
                },

                new Class()
                {
                    Code = "YA",
                    Name = "Young Adult",
                    Description = "15-17 years old.",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "280 FPS",
                    ColorId = colors["Red"]
                },

                new Class()
                {
                    Code = "TR",
                    Name = "Traditional",
                    Description = "15-17 years old.",
                    MaximumYardage = 25,
                    Known = false,
                    Restrictions =
                        "Recurve or longbow, No sight or sight marks, no release aid, no stabilizers, no overdraw.",
                    ColorId = colors["Blue"]
                },

                new Class()
                {
                    Code = "MN",
                    Name = "Mens Novice Hunter",
                    Description = "Mens Novice Hunter",
                    MaximumYardage = 30,
                    Known = true,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    ColorId = colors["Purple"]
                },

                new Class()
                {
                    Code = "WN",
                    Name = "Womens Novice Hunter",
                    Description = "Womens Novice Hunter",
                    MaximumYardage = 30,
                    Known = true,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    ColorId = colors["Purple"]
                },

                new Class()
                {
                    Code = "MH",
                    Name = "Mens Hunter",
                    Description = "Mens Hunter",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    ColorId = colors["Orange"]
                },

                new Class()
                {
                    Code = "WH",
                    Name = "Womens Hunter",
                    Description = "Womens Hunter",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer, 280 FPS.",
                    ColorId = colors["Orange"]
                },

                new Class()
                {
                    Code = "MO",
                    Name = "Mens Open",
                    Description = "Mens Open",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "280 FPS.",
                    ColorId = colors["White"]
                },

                new Class()
                {
                    Code = "WO",
                    Name = "Womens Open",
                    Description = "Womens Open",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "280 FPS.",
                    ColorId = colors["Red"]
                },

                new Class()
                {
                    Code = "WO",
                    Name = "Womens Open",
                    Description = "Womens Open",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "280 FPS.",
                    ColorId = colors["Red"]
                },

                new Class()
                {
                    Code = "MK",
                    Name = "Mens Known",
                    Description = "Mens Known",
                    MaximumYardage = 45,
                    Known = true,
                    Restrictions = "280 FPS.",
                    ColorId = colors["White"]
                },

                new Class()
                {
                    Code = "WK",
                    Name = "Womens Known",
                    Description = "Womens Known",
                    MaximumYardage = 40,
                    Known = true,
                    Restrictions = "280 FPS.",
                    ColorId = colors["Red"]
                },

                new Class()
                {
                    Code = "SO",
                    Name = "Senior Open",
                    Description = "50 Years +",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "280 FPS.",
                    ColorId = colors["White"]
                },

                new Class()
                {
                    Code = "OU",
                    Name = "Outlaw",
                    Description = "Outlaw",
                    MaximumYardage = 45,
                    Known = false,
                    Restrictions = "None.",
                    ColorId = colors["White"]
                },

                new Class()
                {
                    Code = "SE",
                    Name = "Senior Outlaw",
                    Description = "50 Years +",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "None.",
                    ColorId = colors["Red"]
                },

                new Class()
                {
                    Code = "HO",
                    Name = "Hunter Outlaw",
                    Description = "Hunter Outlaw",
                    MaximumYardage = 40,
                    Known = false,
                    Restrictions = "12 inch stabilizer.",
                    ColorId = colors["Orange"]
                }
            );
        }

        private int _entityId;
        private int NextEntityId {
            get
            {
                _entityId++;
                return _entityId;
            }
            set { _entityId = value; }
        }
    }
}
