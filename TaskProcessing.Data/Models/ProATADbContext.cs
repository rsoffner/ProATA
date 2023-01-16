using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TaskProcessing.Core.Models;

namespace TaskProcessing.Data.Models;

public partial class ProATADbContext : DbContext
{
    public ProATADbContext()
    {
    }

    public ProATADbContext(DbContextOptions<ProATADbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Schedule> Schedules { get; set; }
    public virtual DbSet<Scheduler> Schedulers { get; set; }
    public virtual DbSet<APITask> Tasks { get; set; }

    //public virtual DbSet<Url> Urls { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Schedule>().ToTable("Schedules")
            .Ignore(x => x.Events);

        modelBuilder.Entity<Scheduler>().ToTable("Schedulers")
            .Ignore(x => x.Events);

        modelBuilder.Entity<APITask>().ToTable("Tasks")
            .Ignore(x => x.Events); 

        /*
        modelBuilder.Entity<Url>(entity =>
        {
            entity.HasNoKey();

            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExternalUrl)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });
        */

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
