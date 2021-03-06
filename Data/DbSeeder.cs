namespace packt_asp_net_core_and_angular2.Data
{
  using System;
  using System.Linq;
  using System.Threading.Tasks;

  using Microsoft.EntityFrameworkCore;
  using Microsoft.EntityFrameworkCore.ChangeTracking;

  using packt_asp_net_core_and_angular2.Data;
  using packt_asp_net_core_and_angular2.Data.Items;
  using packt_asp_net_core_and_angular2.Data.Comments;
  using packt_asp_net_core_and_angular2.Data.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
  using Microsoft.Extensions.DependencyInjection;
  using OpenIddict.Core;
  using System.Threading;
  using OpenIddict.Models;
  using CryptoHelper;

  public class DbSeeder
  {
    private IServiceProvider Services;
    private ApplicationDbContext DbContext;
    private RoleManager<IdentityRole> RoleManager;
    private UserManager<ApplicationUser> UserManager;

    public DbSeeder(
      IServiceProvider services,
      ApplicationDbContext dbContext,
      RoleManager<IdentityRole> roleManager,
      UserManager<ApplicationUser> userManager)
    {
      Services = services;
      DbContext = dbContext;
      RoleManager = roleManager;
      UserManager = userManager;
    }

    public async Task SeedAsync()
    {
      this.DbContext.Database.EnsureCreated();
      if (await this.DbContext.Users.CountAsync() == 0)
      {
        await this.CreateUsers();
      }

      // Create default Application
      await CreateApplication();

      if (await this.DbContext.Items.CountAsync() == 0)
      {
        this.CreateItems();
      }
    }

    private async Task CreateApplication()
    {
        using (var scope = Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
        {
          var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
          await context.Database.EnsureCreatedAsync();

          var manager = scope.ServiceProvider.GetRequiredService<OpenIddictApplicationManager<OpenIddictApplication>>();

          if (await manager.FindByClientIdAsync("aurelia", CancellationToken.None) == null)
          {
              var descriptor = new OpenIddictApplicationDescriptor
              {
                  ClientId = "OpenGameList",
                  DisplayName = "OpenGameList",
                  PostLogoutRedirectUris = { new Uri("http://localhost:5000/") },
                  RedirectUris = { new Uri("http://localhost:5000/api/connect/token") },
                  ClientSecret = Crypto.HashPassword("1234567890_my_client_secret")
              };

              await manager.CreateAsync(descriptor, CancellationToken.None);
          }
      }
  }

    private void CreateItems()
    {
      var createdDate = new DateTime(2016, 03, 01, 12, 30, 00); 
      var lastModifiedDate = DateTime.Now; 
      var num = 100;

      var authorId = DbContext.Users.Where(u => u.UserName == "Admin").First().Id;
      for (int id = 1; id <= num; id++) 
      {
        DbContext.Items.Add(
          GetSampleItem(id, num - id, title: $"Game {id}", description: $"Description {id}", notes: $"Notes {id}"));
      }

      Item GetSampleItem(int id, int viewCount, string title, string description, string notes) {
        return new Item {
          UserId = authorId,
          Title = title,
          Description = description,
          ViewCount = viewCount,
          Notes = notes,
          CreatedDate = createdDate,
          LastModifiedDate = lastModifiedDate
        };
      }

      EntityEntry<Item> e1 = DbContext.Items.Add(new Item() 
        { 
            UserId = authorId, 
            Title = "Magarena", 
            Description = "Single-player fantasy card game similar to Magic: The Gathering", 
            Text = @"Loosely based on Magic: The Gathering, the game lets you play against a computer opponent or another human being.  
                                The game features a well-developed AI, an intuitive and clear interface and an enticing level of gameplay.", 
            Notes = "This is a sample record created by the Code-First Configuration class", 
            ViewCount = 2343, 
            CreatedDate = createdDate, 
            LastModifiedDate = lastModifiedDate 
        });

        EntityEntry<Item> e2 = DbContext.Items.Add(new Item() 
        { 
            UserId = authorId, 
            Title = "Minetest", 
            Description = "Open-Source alternative to Minecraft", 
            Text = @"The Minetest gameplay is very similar to Minecraft's: you are playing in a 3D open world, where you can create and/or remove various types of “blocks.
                        Minetest feature both single-player and multi-player game modes.  
                        It also has support for custom mods, additional texture packs and other custom/personalization options.  
                        Minetest has been released in 2015 under GNU Lesser General Public License.", 
            Notes = "This is a sample record created by the Code-First Configuration class", 
            ViewCount = 4180, 
            CreatedDate = createdDate, 
            LastModifiedDate = lastModifiedDate 
        });

        EntityEntry<Item> e3 = DbContext.Items.Add(new Item() 
        { 
            UserId = authorId, 
            Title = "Relic Hunters Zero", 
            Description = "A free game about shooting evil space ducks with tiny, cute guns.", 
            Text = @"Relic Hunters Zero is fast, tactical and also very smooth to play.  
                        It also enables the users to look at the source code, so they can can get creative and keep this game alive, fun and free for years to come. 
                        The game is also available on Steam.", 
            Notes = "This is a sample record created by the Code-First Configuration class",
            ViewCount = 5203, 
            CreatedDate = createdDate, 
            LastModifiedDate = lastModifiedDate 
        });

        EntityEntry<Item> e4 = DbContext.Items.Add(new Item() 
        { 
            UserId = authorId, 
            Title = "SuperTux", 
            Description = "A classic 2D jump and run, side-scrolling game similar to the Super Mario series.", 
            Text = @"The game is currently under Milestone 3. The Milestone 2, which is currently out, features the following: 
                        - a nearly completely rewritten game engine based on OpenGL, OpenAL, SDL2, ... 
                        - support for translations 
                        - in-game manager for downloadable add-ons and translations 
                        - Bonus Island III, a for now unfinished Forest Island and the development levels in Incubator Island 
                        - a final boss in Icy Island 
                        - new and improved soundtracks and sound effects 
                        ... and much more!  
                        The game has been released under the GNU GPL license.", 
            Notes = "This is a sample record created by the Code-First Configuration class", 
            ViewCount = 9602, 
            CreatedDate = createdDate,
            LastModifiedDate = lastModifiedDate 
        });

        EntityEntry<Item> e5 = DbContext.Items.Add(new Item() 
        { 
            UserId = authorId, 
            Title = "Scrabble3D", 
            Description = "A 3D-based revamp to the classic Scrabble game.", 
            Text = @"Scrabble3D extends the gameplay of the classic game Scrabble by adding a new whole third dimension.  
                        Other than playing left to right or top to bottom, you'll be able to place your tiles above or beyond other tiles.  
                        Since the game features more fields, it also uses a larger letter set. 
                        You can either play against the computer, players from your LAN or from the Internet.  
                        The game also features a set of game servers where you can challenge players from all over the world and get ranked into an official, ELO-based rating/ladder system. 
                        ", 
            Notes = "This is a sample record created by the Code-First Configuration class", 
            ViewCount = 6754, 
            CreatedDate = createdDate, 
            LastModifiedDate = lastModifiedDate 
        });

        if (DbContext.Comments.Count() == 0) 
        { 
            int numComments = 10;   // comments per item 
            for (int i = 1; i <= numComments; i++) DbContext.Comments.Add(GetSampleComment(i, e1.Entity.Id, authorId, createdDate.AddDays(i))); 
            for (int i = 1; i <= numComments; i++) DbContext.Comments.Add(GetSampleComment(i, e2.Entity.Id, authorId, createdDate.AddDays(i))); 
            for (int i = 1; i <= numComments; i++) DbContext.Comments.Add(GetSampleComment(i, e3.Entity.Id, authorId, createdDate.AddDays(i))); 
            for (int i = 1; i <= numComments; i++) DbContext.Comments.Add(GetSampleComment(i, e4.Entity.Id, authorId, createdDate.AddDays(i))); 
            for (int i = 1; i <= numComments; i++) DbContext.Comments.Add(GetSampleComment(i, e5.Entity.Id, authorId, createdDate.AddDays(i))); 
        } 
        DbContext.SaveChanges(); 
    }

    private Comment GetSampleComment(int n, int itemId, string authorId, DateTime createdDate) 
    { 
        return new Comment()
        { 
            ItemId = itemId, 
            UserId = authorId, 
            ParentId = null, 
            Text = String.Format("Sample comment #{0} for the item #{1}", n, itemId), 
            CreatedDate = createdDate, 
            LastModifiedDate = createdDate 
        }; 
    }

    private async Task CreateUsers()
    {
      var createdDate = new DateTime(2016, 03, 01, 12, 30, 00);
      var lastModified = DateTime.Now;
      var roleAdministrators = "Administrators";
      var roleRegistered = "Registered";

      //Create Roles (if they doesn't exist yet)
      if (!await RoleManager.RoleExistsAsync(roleAdministrators)) await
      RoleManager.CreateAsync(new IdentityRole(roleAdministrators));
      if (!await RoleManager.RoleExistsAsync(roleRegistered)) await
      RoleManager.CreateAsync(new IdentityRole(roleRegistered));

      await AddUser(createdDate, lastModified, "admin@opengamelist.com", "Admin", "Pass4User", roleAdministrators);
      await AddUser(createdDate, lastModified, "ryan@opengamelist.com", "Ryan", "Pass4User", roleRegistered);
      await AddUser(createdDate, lastModified, "solis@opengamelist.com", "Solis", "Pass4User", roleRegistered);
      await AddUser(createdDate, lastModified, "vodan@opengamelist.com", "Vodan", "Pass4User", roleRegistered);
      this.DbContext.SaveChanges();
    }

    async Task AddUser(DateTime createdDate, DateTime lastModified,
      string email, string name, string password, string role) {
      if (await UserManager.FindByEmailAsync(email) == null)
      {
        var user = new ApplicationUser(){
          Id = Guid.NewGuid().ToString(),
          UserName = name,
          Email = email,
          CreatedDate = createdDate,
          LastModifiedDate = lastModified
        };
        await UserManager.CreateAsync(user, password);
        await UserManager.AddToRoleAsync(user, role);
        // Remove Lockout and E-Mail confirmation.
        user.EmailConfirmed = true;
        user.LockoutEnabled = false;
      }
    }
  }
}