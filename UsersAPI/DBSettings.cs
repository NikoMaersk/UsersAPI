namespace UsersAPI
{
	public class DBSettings
	{
		public string? ConnectionString { get; set; } = null;
		public string? DatabaseName { get; set; } = null;
		public string? UserCollectionName { get; set; } = null;
		public string? NamesCollectionName { get; set; } = null;
		public string? AdminCollectionName { get; set; } = null;
		public string? MatchesCollectionName { get; set; } = null;
	}
}
