using EventRegistrationAPI_VishvaTyagi.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRegistrationAPI_VishvaTyagi.Data;

/// <summary>
/// EF Core database context for campus events and their registrations.
/// Seed data uses fixed dates so future events stay in the future while the work is marked.
/// </summary>
public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Event> Events => Set<Event>();

    public DbSet<Registration> Registrations => Set<Registration>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Event>()
            .HasMany(e => e.Registrations)
            .WithOne(r => r.Event)
            .HasForeignKey(r => r.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        // Event 1 is in the past. Event 3 has capacity 2 and two registrations, so it is full.
        // Events 2 and 4 are in 2027 and still have spots left.
        modelBuilder.Entity<Event>().HasData(
            new Event
            {
                EventId = 1,
                Title = "Winter Welcome Social",
                Location = "Student Centre Atrium",
                EventDate = new DateTime(2026, 1, 20, 18, 0, 0),
                Capacity = 80
            },
            new Event
            {
                EventId = 2,
                Title = "Spring Career Fair",
                Location = "Hazel McCallion Campus Gym",
                EventDate = new DateTime(2027, 3, 18, 10, 0, 0),
                Capacity = 150
            },
            new Event
            {
                EventId = 3,
                Title = "Resume Writing Workshop",
                Location = "Library Room 204",
                EventDate = new DateTime(2027, 2, 11, 14, 0, 0),
                Capacity = 2
            },
            new Event
            {
                EventId = 4,
                Title = "Campus Hackathon 2027",
                Location = "SCAET Lab 3",
                EventDate = new DateTime(2027, 4, 17, 9, 0, 0),
                Capacity = 40
            });

        modelBuilder.Entity<Registration>().HasData(
            new Registration
            {
                RegistrationId = 1,
                EventId = 1,
                StudentName = "Alex Chen",
                Email = "alex.chen@sheridancollege.ca",
                RegisteredOn = new DateTime(2026, 1, 5, 12, 0, 0)
            },
            new Registration
            {
                RegistrationId = 2,
                EventId = 2,
                StudentName = "Priya Shah",
                Email = "priya.shah@sheridancollege.ca",
                RegisteredOn = new DateTime(2026, 11, 1, 9, 30, 0)
            },
            new Registration
            {
                RegistrationId = 3,
                EventId = 3,
                StudentName = "Jordan Lee",
                Email = "jordan.lee@sheridancollege.ca",
                RegisteredOn = new DateTime(2026, 11, 10, 15, 0, 0)
            },
            new Registration
            {
                RegistrationId = 4,
                EventId = 3,
                StudentName = "Sam Patel",
                Email = "sam.patel@sheridancollege.ca",
                RegisteredOn = new DateTime(2026, 11, 12, 11, 15, 0)
            });
    }
}
