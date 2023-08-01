using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicketingSolution.Domain.Domain;

namespace TicketingSolution.Persistance
{
    public class TicketingSolutionDbContext : DbContext
    {
        public TicketingSolutionDbContext(DbContextOptions<TicketingSolutionDbContext> options):
            base(options)
        {

        }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketBooking> TicketBookings { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Ticket>().HasData(
                new Ticket { Id = 1, Name="To Tehran"},
                new Ticket { Id = 1, Name="To Esfahan"},
                new Ticket { Id = 1, Name="To Rasht"}
                );
            modelBuilder.Entity<TicketBooking>().HasNoKey();
            modelBuilder.Entity<Ticket>()
                .HasMany(t => t.TicketBookings)
                .WithOne(tb => tb.Ticket)
                .HasForeignKey(tb => tb.TickectID);
        }
    }
}
