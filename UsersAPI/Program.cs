using UsersAPI.Repository;
using UsersAPI.Model;
using MongoDB.Bson;
using UsersAPI.Repository.Interfaces;

namespace UsersAPI
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			//builder.Services.AddAuthorization();

			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddSingleton<IUserRepository, UserRepository>();
			builder.Services.AddSingleton<INamesRepository, NamesRepository>();
			builder.Services.AddSingleton<IAdminRepository, AdminRepository>();
			builder.Services.AddSingleton<INamesMatch, NamesMatchRepository>();
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

			app.MapDelete("/users/{id}", (ObjectId id, IUserRepository ur) =>
			{
				ur.Delete(id);
			});


			app.MapGet("/users/{id}", (ObjectId id, IUserRepository ur) =>
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


			app.MapDelete("/names/{id}", (ObjectId id, INamesRepository nr) =>
			{
				nr.Delete(id);
			});


			app.MapGet("/names/id={id}", (ObjectId id, INamesRepository nr) =>
			{
				return nr.Get(id);
			});


			app.MapGet("/names", (INamesRepository nr) =>
			{
				return nr.GetAll();
			});


			app.MapGet("/names/name={name}", (INamesRepository nr, string name) =>
			{
				return nr.GetByNames(name);
			});


			app.MapGet("/names/international={isInternational}", (INamesRepository nr, bool isInternational) => 
			{
				return nr.GetByInternational(isInternational);
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

			#region Admin

			app.MapPost("/admin", (IAdminRepository ar, Admin admin) =>
			{
				ar.Add(admin);
			});

			app.MapDelete("/admin/{id}", (ObjectId id, IAdminRepository ar) =>
			{
				ar.Delete(id);
			});

			app.MapGet("/admin/{id}", (ObjectId id, IAdminRepository ar) =>
			{
				return ar.Get(id);
			});

			#endregion

			#region Match

			app.MapGet("/matches", (INamesMatch nm) =>
			{
				return nm.GetAll();
			});

			app.MapGet("/matches/name={name}", (INamesMatch nm, string name) =>
			{
				return nm.GetAllByName(name);
			});

			#endregion

			app.Run();
		}
	}
}
