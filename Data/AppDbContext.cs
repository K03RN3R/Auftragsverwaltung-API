using GUI_2.Models;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace GUI_2.Data
{
    public class AppDbContext : DbContext
    {

        private readonly IHttpContextAccessor _httpContextAccessor; // Für den Zugriff auf HTTP-Kontextinformationen wie Username usw /Asp.Net Core Funktion
        public AppDbContext(DbContextOptions<AppDbContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        // DbSet entspricht einer Tabelle in der Datenbank
        public DbSet<Kunde> Kunden => Set<Kunde>();
        public DbSet<Auftrag> Auftraege => Set<Auftrag>();
        public DbSet<Anlage> Anlagen => Set<Anlage>();
        public DbSet<Fehlerbericht> Fehlerberichte => Set<Fehlerbericht>();
        public DbSet<Schicht> Schichten => Set<Schicht>();
        public DbSet<AuftragSchicht> AuftragSchichten => Set<AuftragSchicht>();
        public DbSet<Spindel> Spindeln => Set<Spindel>();
        public DbSet<Station> Stationen => Set<Station>();
        public DbSet<Status> Stati => Set<Status>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            ApplyAuditFields();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangeOnSuccess, CancellationToken cancellationToken = default)
        {
            ApplyAuditFields();
            return base.SaveChangesAsync(acceptAllChangeOnSuccess, cancellationToken);
        }

        private void ApplyAuditFields()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            //kommt aus meiner Middleware
            var user = httpContext?.Items["CurrentUser"]?.ToString() ?? "system";
            var now = DateTime.UtcNow;

            //nur Models mit registriertem IAuditable Interface berücksichtigen
            foreach (var entry in ChangeTracker.Entries<IAuditable>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.ErstelltUtc = now;
                    entry.Entity.ErstelltVon ??= user;

                    //so bleibt Geaendert leer bei neuen Einträgen
                    entry.Entity.GeaendertUtc = null;
                    entry.Entity.GeaendertVon = null;
                }
                else if (entry.State == EntityState.Modified)
                {
                    //Schutz gegen Überschreiben von Erstellt
                    entry.Property(p => p.ErstelltUtc).IsModified = false;
                    entry.Property(p => p.ErstelltVon).IsModified = false;
                }
            }
        }
    }
}
