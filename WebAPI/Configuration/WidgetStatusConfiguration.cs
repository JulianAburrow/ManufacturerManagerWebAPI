namespace DataAccess.Configuration;

public class WidgetStatusConfiguration : IEntityTypeConfiguration<WidgetStatusModel>
{
    public void Configure(EntityTypeBuilder<WidgetStatusModel> builder)
    {
        builder.ToTable("WidgetStatus");
        builder.HasKey(nameof(WidgetStatusModel.StatusId));
        builder.HasMany(e => e.Widgets)
            .WithOne(e => e.Status)
            .HasForeignKey(e => e.StatusId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.NoAction);
    }
}
