using Microsoft.EntityFrameworkCore;

namespace Data;

public class SampleDbContext(DbContextOptions<SampleDbContext> options) : DbContext(options)
{
	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		var entityConfig = modelBuilder.Entity<SampleEntity>();
		entityConfig.ToTable("SampleEntities");

		var ids = Enumerable.Range(1, 100).Select(x => (long?)Random.Shared.Next(1, 300_000)).ToList();
		// Comment this line to test without query filter
		entityConfig.HasQueryFilter(e => ids.Contains(e.ExternalId));
	}

	public DbSet<SampleEntity> SampleEntities { get; set; }
}

public class SampleEntity
{
	public long Id { get; set; }
	public long? ExternalId { get; set; }
	public required string Name { get; set; }
}