﻿using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using UsersAPI.Model;
using UsersAPI.Repository.Interfaces;
using UsersAPI.Util;

namespace UsersAPI.Repository
{
	public class AdminRepository : IAdminRepository
	{
		private readonly IMongoCollection<Admin> _admins;

        public AdminRepository(IOptions<DBSettings> mongoDB)
        {
            var mongoClient = new MongoClient(mongoDB.Value.ConnectionString);
			var mongoDatabase = mongoClient.GetDatabase(mongoDB.Value.DatabaseName);
			_admins = mongoDatabase.GetCollection<Admin>(mongoDB.Value.AdminCollectionName);
        }

		public async Task AddAsync(string name, string email, string password)
		{
			string hashedPassword = HashingHelper.HashPassword(password, out string salt);

			Admin admin = new()
			{
				Name = name,
				Email = email,
				HashedPassword = hashedPassword,
				Salt = salt
			};

			await _admins.InsertOneAsync(admin);
		}

		public async Task<bool> AuthenticateAsync(string email, string pwd)
		{
			Admin userMatch = await _admins.Find(u => u.Email == email).FirstAsync();

			if (userMatch == null || userMatch.HashedPassword == null || userMatch.Salt == null) { return false; }

			return HashingHelper.Verify(pwd, userMatch.HashedPassword, userMatch.Salt);
		}

		public async Task DeleteAsync(ObjectId id)
		{
			await _admins.DeleteOneAsync(a => a.Id == id);
		}

		public async Task<Admin> GetAsync(ObjectId id)
		{
			return await _admins.Find(a => a.Id == id).FirstOrDefaultAsync();
		}
	}
}
