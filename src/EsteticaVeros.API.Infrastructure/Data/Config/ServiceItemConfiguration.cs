using EsteticaVeros.API.Core.ServiceAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EsteticaVeros.API.Infrastructure.Data.Config;

public class ServiceItemConfiguration : IEntityTypeConfiguration<ServiceItem>
{
  public void Configure(EntityTypeBuilder<ServiceItem> builder)
  {
    builder.ToTable("ServiceItems");

    // Primary Key
    builder.HasKey(si => si.Id);
    builder.Property(si => si.Id).ValueGeneratedOnAdd();

    // Foreign Keys
    builder.Property(si => si.ParentServiceId)
      .IsRequired();

    builder.Property(si => si.ChildServiceId)
      .IsRequired();

    // Properties
    builder.Property(si => si.Quantity)
      .IsRequired()
      .HasDefaultValue(1);

    builder.Property(si => si.DisplayOrder)
      .IsRequired()
      .HasDefaultValue(0);

    builder.Property(si => si.Notes)
      .HasMaxLength(500);

    builder.Property(si => si.CreatedAt)
      .IsRequired();

    // Indexes
    builder.HasIndex(si => si.ParentServiceId);
    builder.HasIndex(si => si.ChildServiceId);
    builder.HasIndex(si => new { si.ParentServiceId, si.ChildServiceId })
      .IsUnique(); // Un servicio hijo solo puede aparecer una vez en un padre

    // Relationship: ServiceItem → ChildService (Many-to-One)
    builder.HasOne(si => si.ChildService)
      .WithMany() // No necesitamos navegación inversa
      .HasForeignKey(si => si.ChildServiceId)
      .OnDelete(DeleteBehavior.Restrict); // No permitir eliminar un servicio que es parte de un combo/paquete

    // Check Constraint: No puede referenciar a sí mismo
    builder.ToTable(t => t.HasCheckConstraint(
      "CK_ServiceItem_NoSelfReference",
      "[ParentServiceId] <> [ChildServiceId]"));

    // TODO: Agregar constraint para validar que ChildService sea Simple
    // Esto se validará en la lógica de negocio por ahora
  }
}