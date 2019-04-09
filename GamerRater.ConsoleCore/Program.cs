using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using GamerRater.DataAccess;
using GamerRater.Model;
using Newtonsoft.Json;

namespace GamerRater.ConsoleCore
{
	class Program
	{
		static void Main(string[] args)
		{
		}

		public void AddDummyData()
		{
			using (DataContext conn = new DataContext())
			{
				conn.Platforms.Add(new Platform() { Name = "Playstation 1" });
				conn.Platforms.Add(new Platform() { Name = "Playstation 2" });
				conn.Platforms.Add(new Platform() { Name = "Playstation 3" });
				conn.Platforms.Add(new Platform() { Name = "Playstation 4" });
				conn.Platforms.Add(new Platform() { Name = "Nintendo 64" });
				conn.Platforms.Add(new Platform() { Name = "Super Nintendo" });
				conn.Platforms.Add(new Platform() { Name = "Nintendo Switch" });
				conn.Platforms.Add(new Platform() { Name = "PC" });

				conn.Stars.Add(new Stars() { Value = "Awful" });
				conn.Stars.Add(new Stars() { Value = "Very bad" });
				conn.Stars.Add(new Stars() { Value = "Bad" });
				conn.Stars.Add(new Stars() { Value = "Unimpressive" });
				conn.Stars.Add(new Stars() { Value = "Average" });
				conn.Stars.Add(new Stars() { Value = "Fair" });
				conn.Stars.Add(new Stars() { Value = "Alright" });
				conn.Stars.Add(new Stars() { Value = "Good" });
				conn.Stars.Add(new Stars() { Value = "Great" });
				conn.Stars.Add(new Stars() { Value = "Superb" });

				conn.UserGroups.Add(new UserGroup() { Group = "User" });
				conn.UserGroups.Add(new UserGroup() { Group = "Admin" });

				conn.Users.Add(new User()
					{ Email = "Mail@mail.com", FirstName = "Magnus", LastName = "Pettersen", Username = "Magnus94" });

				conn.SaveChanges();
			}
		}
	}
}
