using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Security.Claims;
using System.Text.Json;
using UsersAPI.Model;
using UsersAPI.Repository;
using UsersAPI.Repository.Interfaces;
using UsersAPI.Util;

namespace UsersAPI
{
	public class Program
	{
		private const string authScheme = "token";

		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

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
			builder.Services.AddSingleton<INamesMatchRepository, NamesMatchRepository>();
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

			ConfigureRoutes(app);

			app.Run();
		}

		

		private static void ConfigureRoutes(WebApplication app)
		{
			#region Login

			app.MapPost("/login", async ([FromBody] LoginRequest request, IUserRepository ur, HttpContext context) =>
			{
				if (await ur.AuthenticateAsync(request.Email, request.Password))
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
				if (await ar.AuthenticateAsync(request.Email, request.Password))
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
				if (!EmailHelper.IsValidEmail(request.Email))
				{
					return Results.BadRequest("Must be a email");
				}

				await ur.AddAsync(request);
				return Results.Created();
			});


			app.MapDelete("/users/{id}", async (ObjectId id, IUserRepository ur) =>
			{
				await ur.DeleteAsync(id);
			}).RequireAuthorization("admin");


			app.MapGet("/users/{id}", async (ObjectId id, IUserRepository ur) =>
			{
				return await ur.GetAsync(id);
			}).RequireAuthorization();


			app.MapGet("/users", async (IUserRepository ur) =>
			{
				return await ur.GetAllAsync();
			}).RequireAuthorization("admin");


			app.MapGet("/users/email/{email}", async (string email, IUserRepository ur) =>
			{
				return await ur.GetByEmailAsync(email);
			}).RequireAuthorization();


			app.MapPost("/users/names/{email}", async (string email, JsonElement body, IUserRepository ur, INamesRepository nr) =>
			{
				var name = body.GetProperty("name").ToString();

				if (body.ValueKind == JsonValueKind.Undefined || name == null)
				{
					return Results.BadRequest("The 'name' property is missing in the request body.");
				}

				bool isValid = await nr.IsNameValidAsync(name);
				bool isNameStored = await ur.isNameAlreadyStoredAsync(email, name);

				if (!isValid)
				{
					return Results.BadRequest($"{name} is not a registered name");
				}

				if (isNameStored)
				{
					return Results.BadRequest($"{name} already exists for {email}");
				}

				await ur.AddNameToUserAsync(name, email);
				return Results.Created($"/users/{email}/names", $"{name} added succesfully");
			});


			app.MapPatch("users/partner/{email}", async (string email, JsonElement body, IUserRepository ur) =>
			{
				var linkedMail = body.GetProperty("email").ToString();
				

				if (body.ValueKind == JsonValueKind.Undefined || linkedMail == null)
				{
					return Results.BadRequest("The 'email' property is missing in the request body.");
				}

				bool isValid = await ur.CheckIfUserExistsAsync(email);

				if (!isValid && EmailHelper.IsValidEmail(email))
				{
					return Results.BadRequest("Invalid email");
				}
				else
				{
					await ur.PatchPartnerLinkAsync(email, linkedMail);
					return Results.Ok("Success");
				}
			});


			app.MapPatch("/users/names/clear/{email}", async (string email, IUserRepository ur) =>
			{
				if (!EmailHelper.IsValidEmail(email))
				{
					return Results.BadRequest("Invalid email");
				}

				bool modified = await ur.ClearNamesListAsync(email);

				return modified ? Results.NoContent() : Results.NotFound("List could not be cleared");
			});


			app.MapPatch("/users/names/remove/{email}", async (string email, [FromBody] RemoveNamesRequest request, IUserRepository ur) =>
			{
				if (!EmailHelper.IsValidEmail(email))
				{
					return Results.BadRequest("Invalid email");
				}

				if (request.Names == null)
				{
					return Results.BadRequest("Invalid or missing body");
				}

				bool modified = await ur.ClearNameFromListAsync(email, request.Names);

				return modified ? Results.NoContent() : Results.NotFound("List could not be cleared");
			});


			#endregion

			#region Names
			app.MapPost("/names", async (INamesRepository nr, Names name) =>
			{
				await nr.AddAsync(name);
				return Results.Created($"/names/{name.Name}", name);

			}).RequireAuthorization();


			app.MapDelete("/names/{Name}", (string name, INamesRepository nr) =>
			{
				nr.DeleteAsync(name);
			}).RequireAuthorization("admin");


			app.MapGet("/names/{Name}", (INamesRepository nr, string name) =>
			{
				return nr.GetByNameAsync(name);
			}).RequireAuthorization();


			app.MapGet("/names/international/{isInternational}", (INamesRepository nr, bool isInternational) =>
			{
				return nr.GetByInternationalAsync(isInternational);
			}).RequireAuthorization();


			app.MapGet("/names", (INamesRepository nr, [FromQuery] string sort, [FromQuery] string order) =>
			{
				return nr.GetNamesSortedAsync(sort, order);
			}).RequireAuthorization();


			app.MapGet("/names/gender/{gender}", (INamesRepository nr, Gender gender) =>
			{
				return nr.GetByGenderAsync(gender);
			}).RequireAuthorization();


			app.MapGet("/names/all", (INamesRepository nr) =>
			{
				return nr.GetAllAsync();
			}).RequireAuthorization();


			#endregion

			#region Admin


			app.MapPost("/admin", async ([FromBody] RegistrationRequest request, IAdminRepository ar) =>
			{
				if (!EmailHelper.IsValidEmail(request.Email))
				{
					return Results.BadRequest("Must be a email");
				}

				await ar.AddAsync(request);
				return Results.Created();
			});


			app.MapDelete("/admin/{id}", async (ObjectId id, IAdminRepository ar) =>
			{
				await ar.DeleteAsync(id);
			}).RequireAuthorization("admin");


			app.MapGet("/admin/{id}", async (ObjectId id, IAdminRepository ar) =>
			{
				return await ar.GetAsync(id);
			}).RequireAuthorization("admin");

			#endregion

			#region Match

			app.MapPost("/matches/{match}", async ([FromBody] NameMatch match, INamesMatchRepository nmr) =>
			{
				await nmr.AddAsync(match);
				return Results.Created($"/matches/{match.Id}", match);
			});

			app.MapGet("/matches", async (INamesMatchRepository nm) =>
			{
				return await nm.GetAllAsync();
			}).RequireAuthorization();


			app.MapGet("/matches/Name/{Name}", async (INamesMatchRepository nm, string name) =>
			{
				return await nm.GetAllByNameAsync(name);
			}).RequireAuthorization();

			#endregion
		}	
	}
}
