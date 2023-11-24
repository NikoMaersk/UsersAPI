using UsersAPI.Repository;
using UsersAPI.Model;
using MongoDB.Bson;
using UsersAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace UsersAPI
{
    public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			// Add services to the container.
			const string authScheme = "token";

			builder.Services.AddAuthentication(authScheme).AddCookie(authScheme, Options =>
			{
				Options.ExpireTimeSpan = TimeSpan.FromMinutes(20);
				Options.SlidingExpiration = true;
			});

			builder.Services.AddAuthorization(builder =>
			{
				builder.AddPolicy("user", pb =>
				{
					pb.RequireAuthenticatedUser()
					.AddAuthenticationSchemes(authScheme)
					.AddRequirements()
					.RequireClaim("user_type", "standard");
				});
			});

			builder.Services.AddAuthorization(builder =>
			{
				builder.AddPolicy("admin", pb =>
				{
					pb.RequireAuthenticatedUser()
					.AddAuthenticationSchemes(authScheme)
					.AddRequirements()
					.RequireClaim("user_type", "admin");
				});
			});

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
			app.UseAuthentication();
			app.UseAuthorization();

			#region Login

			app.MapPost("/login", async ([FromBody] LoginRequest request, IUserRepository ur, HttpContext context) =>
			{
				if (await ur.Authenticate(request.Email, request.Password))
				{
					List<Claim> claims = [new Claim("user_type", "standard")];
					var identity = new ClaimsIdentity(claims, authScheme);
					var userIdentity = new ClaimsPrincipal(identity);

					await context.SignInAsync(authScheme, userIdentity);
					return Results.Ok("Login success");
				}
				return Results.Unauthorized();
			}).AllowAnonymous();


			app.MapPost("/login/admin", async ([FromBody] LoginRequest request, IAdminRepository ar, HttpContext context) =>
			{
				if (await ar.Authenticate(request.Email, request.Password))
				{
					List<Claim> claims = [new Claim("user_type", "admin")];
					var identity = new ClaimsIdentity(claims, authScheme);
					var userIdentity = new ClaimsPrincipal(identity);

					await context.SignInAsync(authScheme, userIdentity);
					return Results.Ok("Login success");
				}
				return Results.Unauthorized();
			}).AllowAnonymous();


			app.MapPost("/logout", async (IUserRepository ur, HttpContext context) =>
			{
				await context.SignOutAsync();
			}).RequireAuthorization();

			#endregion

			#region Users
			
			app.MapPost("/users", async ([FromBody] RegistrationRequest request, IUserRepository ur) =>
			{
				await ur.Add(request);
			});


			app.MapDelete("/users/{id}", async (ObjectId id, IUserRepository ur) =>
			{
				await ur.Delete(id);
			}).RequireAuthorization("admin");


			app.MapGet("/users/{id}", async (ObjectId id, IUserRepository ur) =>
			{
				return await ur.Get(id);
			}).RequireAuthorization();


			app.MapGet("/users", async (IUserRepository ur) =>
			{
				return await ur.GetAll();
			}).RequireAuthorization("admin");


			app.MapGet("/users/email/{Email}", async (string email, IUserRepository ur) =>
			{
				return await ur.GetByEmail(email);
			}).RequireAuthorization();

			#endregion

			#region Names
			app.MapPost("/names", (INamesRepository nr, Names name) =>
			{
				nr.Add(name);

			}).RequireAuthorization();


			app.MapDelete("/names/{id}", (ObjectId id, INamesRepository nr) =>
			{
				nr.Delete(id);
			}).RequireAuthorization("admin");


			app.MapGet("/names/{id}", (ObjectId id, INamesRepository nr) =>
			{
				return nr.Get(id);
			}).RequireAuthorization();


			app.MapGet("/names/by-name/{name}", (INamesRepository nr, string name) =>
			{
				return nr.GetByName(name);
			}).RequireAuthorization();


			app.MapGet("/names/international/{isInternational}", (INamesRepository nr, bool isInternational) => 
			{
				return nr.GetByInternational(isInternational);
			}).RequireAuthorization();


			app.MapGet("/names", (INamesRepository nr, [FromQuery] string sort, [FromQuery] string order) =>
			{
				return nr.GetNamesSorted(sort, order);
			}).RequireAuthorization();


			app.MapGet("/names/gender/{gender}", (INamesRepository nr, Gender gender) =>
			{
				return nr.GetByGender(gender);
			}).RequireAuthorization();


			app.MapGet("/names/all", (INamesRepository nr) =>
			{
				return nr.GetAll();
			}).RequireAuthorization();


			#endregion

			#region Admin

			app.MapPost("/admin", async ([FromBody] RegistrationRequest request, IAdminRepository ar) =>
			{
				await ar.Add(request);
			});


			app.MapDelete("/admin/{id}", async (ObjectId id, IAdminRepository ar) =>
			{
				await ar.Delete(id);
			}).RequireAuthorization("admin");


			app.MapGet("/admin/{id}", async (ObjectId id, IAdminRepository ar) =>
			{
				return await ar.Get(id);
			}).RequireAuthorization("admin");

			#endregion

			#region Match

			app.MapGet("/matches", async (INamesMatch nm) =>
			{
				return await nm.GetAll();
			}).RequireAuthorization();


			app.MapGet("/matches/name/{name}", async (INamesMatch nm, string name) =>
			{
				return await nm.GetAllByName(name);
			}).RequireAuthorization();

			#endregion

			app.Run();
		}
	}
}
