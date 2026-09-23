namespace RadarPncp.Api.Data;

using Microsoft.EntityFrameworkCore;
using RadarPncp.Api.Data.Entities;

/// <summary>
/// Contexto para o banco de dados
/// </summary>
public class RadarDbContext(DbContextOptions<RadarDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Tabela de órgão
    /// </summary>
    public DbSet<GovernmentEntity> GovernmentEntities => Set<GovernmentEntity>();

    /// <summary>
    /// Tabela de unidade
    /// </summary>
    public DbSet<GovernmentUnit> GovernmentUnits => Set<GovernmentUnit>();

    /// <summary>
    /// Tabela de compra
    /// </summary>
    public DbSet<Procurement> Procurements => Set<Procurement>();

    /// <summary>
    /// Regras para criação do modelo
    /// </summary>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        //Regras de tamanho máximo e unicidade da entidade órgão
        modelBuilder.Entity<GovernmentEntity>(entity =>
        {
            entity.Property(e => e.TaxId).HasMaxLength(14);
            entity.HasIndex(e => e.TaxId).IsUnique();
        });

        //Regras de tamanho máximo, unicidade e exclusão restrita para a entidade unidade
        modelBuilder.Entity<GovernmentUnit>(entity =>
        {
            entity.Property(u => u.PncpUnitCode).HasMaxLength(9);
            entity.Property(u => u.StateCode).HasMaxLength(2);
            entity.HasIndex(u => new { u.GovernmentEntityId, u.PncpUnitCode }).IsUnique();

            entity.HasOne<GovernmentEntity>()
                .WithMany()
                .HasForeignKey(u => u.GovernmentEntityId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        //Regras de tamanho máximo, unicidade e exclusão restrita para a entidade compra
        modelBuilder.Entity<Procurement>(entity =>
        {
            entity.HasIndex(p => p.PncpControlNumber).IsUnique();

            entity.Property(p => p.EstimatedTotal).HasPrecision(18, 2);
            entity.Property(p => p.HomologatedTotal).HasPrecision(18, 2);

            entity.HasOne<GovernmentUnit>()
                .WithMany()
                .HasForeignKey(p => p.GovernmentUnitId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}