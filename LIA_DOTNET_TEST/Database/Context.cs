using LIA_DOTNET_TEST.Models;
using Microsoft.EntityFrameworkCore;

namespace LIA_DOTNET_TEST.Database
{
    public class Context : DbContext
    {
        //Använde DbContextOptions för att ge flexibilitet i konfigurationen av databasanslutningar.
        //Detta tillvägagångssätt möjliggör enkel hantering av flera databaser inom samma lösning och följer bästa praxis för att separera konfiguration från kodbasen
        public Context(DbContextOptions<Context> options) : base(options)
        {
        }

        // protected override void OnConfiguring
        //(DbContextOptionsBuilder optionsBuilder)
        // {
        //     optionsBuilder.UseInMemoryDatabase(databaseName: "BookingDb");
        // }
        public DbSet<TimeSlot> TimeSlots { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}
