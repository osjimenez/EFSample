
using System.Data;
using System.Numerics;
using System.Runtime.InteropServices.JavaScript;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddHostedService<Worker>();

builder.Services.AddDbContext<SampleDbContext>(options =>
{
	// Configure localdb
	options.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SampleDb;Trusted_Connection=True;MultipleActiveResultSets=true",
		sqlOptions =>
		{
			sqlOptions.MigrationsAssembly("Console");
		});
});

IHost host = builder.Build();
host.Run();

// Create worker
public class Worker(SampleDbContext db) : BackgroundService
{
	protected override async Task ExecuteAsync(CancellationToken ct)
	{
		//for (int i = 0; i < 300_000; i++)
		//{
		//	db.SampleEntities.Add(new SampleEntity
		//	{
		//		Name = "Sample Entity",
		//		ExternalId = i
		//	});
		//}
		//await db.SaveChangesAsync(ct);


		var ids = Enumerable.Range(1, 100).Select(x=>(long?)Random.Shared.Next(1, 300_000)).ToList();

		//var entities = await db.SampleEntities.Where(e => ids.Contains(e.ExternalId)).ToListAsync(ct);
		//var entities = await db.SampleEntities.Where(e => EF.Constant(ids).Contains(e.ExternalId)).ToListAsync(ct);

		//var entities = await db.SampleEntities.IgnoreQueryFilters().Where(e => ids.Contains(e.ExternalId)).ToListAsync(ct);
		//var entities = await db.SampleEntities.IgnoreQueryFilters().Where(e => EF.Constant(ids).Contains(e.ExternalId)).ToListAsync(ct);

		var entities = await db.SampleEntities.ToListAsync(ct);

		System.Console.WriteLine($"Count: {entities.Count}");
		//foreach (SampleEntity e in entities)
		//{
		//	System.Console.WriteLine($"Entity: {e.Id} - {e.Name}");
		//}
	}
}
