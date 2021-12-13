namespace API.Entities;

public class Connection
{
	public Connection() { }

	public Connection(string connectionId, string username)
	{
		ConnectionId = connectionId;
		Username = username;
	}

	public string ConnectionId { get; set; } //{Class}Id considers this as primary key in EntityFramework
	public string Username { get; set; }
}