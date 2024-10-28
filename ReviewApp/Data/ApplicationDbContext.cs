using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using ReviewApp.Models;

namespace ReviewApp.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemons { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //indirect many to many 
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PokemonCategory>().HasKey(pc => new { pc.PokemonId, pc.CategoryId });
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonCategory>()
                .HasOne(p => p.Category)
                .WithMany(pc => pc.PokemonCategories)
                .HasForeignKey(p => p.CategoryId);

            modelBuilder.Entity<PokemonOwner>().HasKey(pc => new { pc.PokemonId, pc.OwnerId });
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Pokemon)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.PokemonId);
            modelBuilder.Entity<PokemonOwner>()
                .HasOne(p => p.Owner)
                .WithMany(pc => pc.PokemonOwners)
                .HasForeignKey(p => p.OwnerId);

            // Seed data for PokemonOwners and related entities
            modelBuilder.Entity<Country>().HasData(
                new Country { Id = 1, Name = "Kanto" },
                new Country { Id = 2, Name = "Saffron City" },
                new Country { Id = 3, Name = "Millet Town" }
            );

            modelBuilder.Entity<Owner>().HasData(
              new Owner { Id = 1, FirstName = "Jack", LastName = "London", Gym = "Brocks Gym", CountryId = 1 },
              new Owner { Id = 2, FirstName = "Harry", LastName = "Potter", Gym = "Mistys Gym", CountryId = 2 },
              new Owner { Id = 3, FirstName = "Ash", LastName = "Ketchum", Gym = "Ashs Gym", CountryId = 3 }
          );

            modelBuilder.Entity<Category>().HasData(
             new Category { Id = 1, Name = "Electric" },
             new Category { Id = 2, Name = "Water" },
             new Category { Id = 3, Name = "Leaf" }
         );
            modelBuilder.Entity<Pokemon>().HasData(
             new Pokemon { Id = 1, Name = "Pikachu", BirthDate = new DateTime(1903, 1, 1) },
             new Pokemon { Id = 2, Name = "Squirtle", BirthDate = new DateTime(1903, 1, 1) },
             new Pokemon { Id = 3, Name = "Venasuar", BirthDate = new DateTime(1903, 1, 1) }
         );

            modelBuilder.Entity<PokemonOwner>().HasData(
            new PokemonOwner { PokemonId = 1, OwnerId = 1 },
            new PokemonOwner { PokemonId = 2, OwnerId = 2 },
            new PokemonOwner { PokemonId = 3, OwnerId = 3 }
         );

            modelBuilder.Entity<PokemonCategory>().HasData(
               new PokemonCategory { PokemonId = 1, CategoryId = 1 },
               new PokemonCategory { PokemonId = 2, CategoryId = 2 },
               new PokemonCategory { PokemonId = 3, CategoryId = 3 }
           );

            modelBuilder.Entity<Reviewer>().HasData(
             new Reviewer { Id = 1, FirstName = "Teddy", LastName = "Smith" },
             new Reviewer { Id = 2, FirstName = "Taylor", LastName = "Jones" },
             new Reviewer { Id = 3, FirstName = "Jessica", LastName = "McGregor" }
         );

            modelBuilder.Entity<Review>().HasData(
             new Review { Id = 1, Title = "Pikachu", Text = "Pickahu is the best pokemon, because it is electric", Rating = 5, ReviewerId = 1, PokemonId = 1 },
             new Review { Id = 2, Title = "Pikachu", Text = "Pickachu is the best at killing rocks", Rating = 5, ReviewerId = 2, PokemonId = 1 },
             new Review { Id = 3, Title = "Pikachu", Text = "Pickchu, pickachu, pikachu", Rating = 1, ReviewerId = 3, PokemonId = 1 },

             new Review { Id = 4, Title = "Squirtle", Text = "Squirtle is the best pokemon, because it is electric", Rating = 5, ReviewerId = 1, PokemonId = 2 },
             new Review { Id = 5, Title = "Squirtle", Text = "Squirtle is the best at killing rocks", Rating = 5, ReviewerId = 2, PokemonId = 2 },
             new Review { Id = 6, Title = "Squirtle", Text = "Squirtle, squirtle, squirtle", Rating = 1, ReviewerId = 3, PokemonId = 2 },

             new Review { Id = 7, Title = "Venasuar", Text = "Venasuar is the best pokemon, because it is electric", Rating = 5, ReviewerId = 1, PokemonId = 3 },
             new Review { Id = 8, Title = "Venasuar", Text = "Venasuar is the best at killing rocks", Rating = 5, ReviewerId = 2, PokemonId = 3 },
             new Review { Id = 9, Title = "Venasuar", Text = "Venasuar, Venasuar, Venasuar", Rating = 1, ReviewerId = 3, PokemonId = 3 }
         );
        }
    }
}
