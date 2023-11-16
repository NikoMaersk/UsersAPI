
using UsersAPI.Repository;
using UsersAPI.Model;

namespace UsersAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<IUserRepository, UserRepository>();
			builder.Services.AddSingleton<INamesRepository, NamesRepository>();
			builder.Services.Configure<DBSettings>(builder.Configuration.GetSection(nameof(DBSettings)));

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			#region Users
			app.MapPost("/users", (IUserRepository ur, User user) =>
			{
				ur.Add(user);

			});

			app.MapDelete("/users/{id}", (Guid id, IUserRepository ur) =>
			{
				ur.Delete(id);
			});


			app.MapGet("/users/{id}", (Guid id, IUserRepository ur) =>
			{
				return ur.Get(id);
			});

			app.MapGet("/users", (IUserRepository ur) =>
			{
				return ur.GetAll();
			});
			#endregion

			#region Names
			app.MapPost("/names", (INamesRepository nr, Names name) =>
			{
				nr.Add(name);

			});

			app.MapDelete("/names/{id}", (Guid id, INamesRepository nr) =>
			{
				nr.Delete(id);
			});


			app.MapGet("/names/{id}", (Guid id, INamesRepository nr) =>
			{
				return nr.Get(id);
			});

			app.MapGet("/names", (INamesRepository nr) =>
			{
				return nr.GetAll();
			});

			app.MapGet("/names/sorted-by-name", (INamesRepository nr) =>
			{
				return nr.GetNamesSortedByName();
			});

			app.MapGet("/names/sorted-by-gender", (INamesRepository nr) =>
			{
				return nr.GetNamesSortedByGender();
			});
			#endregion


			app.Run();
		}
	}
}
