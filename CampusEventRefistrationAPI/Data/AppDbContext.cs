using Microsoft.EntityFrameworkCore;
using CampusEventRefistrationAPI.Models;

namespace CampusEventRefistrationAPI.Data;

// EF Core database context for campus events and student registrations.
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();
    public DbSet<Registration> Registrations => Set<Registration>();


        //setting relationships and seeding data
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {

        base.OnModelCreating(modelBuilder);

        // One event can have many registrations; deleting an event deletes its registrations.
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Registrations)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);


        
        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                EventId = 1,
                Title = "Orientation",
                Location = "Learning Commons",
                EventDate = new DateTime(2025, 9, 15, 9, 0, 0, DateTimeKind.Utc),
                Capacity = 10
            },
            new Event
            {
                EventId = 2,
                Title = "Peer Mentor Event",
                Location = "B124",
                EventDate = new DateTime(2026, 12, 15, 18, 0, 0),
                Capacity = 20
            },
            // event with capacity 2 
            new Event
            {
                EventId = 3,
                Title = "Puppy Yoga",
                Location = "Gym",
                EventDate = new DateTime(2027, 1, 18, 12, 0, 0),
                Capacity = 2
            },
            new Event
            {
                EventId = 4,
                Title = "Graduation Photos",
                Location = "Conference Centre",
                EventDate = new DateTime(2027, 4, 1, 13, 0, 0),
                Capacity = 50
            }
            
        );

        modelBuilder.Entity<Registration>().HasData(
            new Registration
            {
                RegistrationId = 1,
                EventId = 1,
                StudentName = "Jordan Patel",
                RegisteredOn = new DateTime(2025, 8, 8, 18, 0, 0)
            }, 
            //Event 3 has 2 registrations, matching its capacity of 2
            new Registration
            {
                RegistrationId = 2,
                EventId = 3,
                StudentName = "Yaya Vibes",
                RegisteredOn = new DateTime(2025, 7, 9, 12, 0, 0)
            }, 
            new Registration
            {
                RegistrationId = 3,
                EventId = 3,
                StudentName = "Holy Ten",
                RegisteredOn = new DateTime(2025, 9, 12, 12, 0, 0)
            },
            new Registration
            {
                RegistrationId = 4,
                EventId = 1,
                StudentName = "Maloti Hulu",
                RegisteredOn = new DateTime(2025, 9, 8, 12, 0, 0)
            },
            new Registration
            {
                RegistrationId = 5,
                EventId = 2,
                StudentName = "Nyasha Mutasa",
                RegisteredOn = new DateTime(2026, 11, 10, 11, 20, 0)
            },
            new Registration
            {
                RegistrationId = 6,
                EventId = 2,
                StudentName = "Kudakwashe Chikwanda",
                RegisteredOn = new DateTime(2026, 11, 12, 13, 5, 0)
            },
            new Registration
            {
                RegistrationId = 7,
                EventId = 2,
                StudentName = "Chiedza Dube",
                RegisteredOn = new DateTime(2026, 11, 15, 17, 30, 0)
            },
            new Registration
            {
                RegistrationId = 8,
                EventId = 4,
                StudentName = "Tafadzwa Gumbo",
                RegisteredOn = new DateTime(2026, 11, 20, 9, 40, 0)
            },
            new Registration
            {
                RegistrationId = 9,
                EventId = 4,
                StudentName = "Farai Ncube",
                RegisteredOn = new DateTime(2026, 11, 22, 15, 10, 0)
            }
            
        );
    }
}