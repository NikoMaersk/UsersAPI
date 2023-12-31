﻿using System.Text.RegularExpressions;

namespace UsersAPI.Util
{
	public static class EmailHelper
	{
		public static bool IsValidEmail(string email)
		{
			const string pattern = @"^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

			return Regex.IsMatch(email, pattern);
		}
	}
}
