using System.Text.Json;
using InventoryManagementSystem.Models;

namespace InventoryManagementSystem.Services;

public class UserService
{
    public const string Seed_Username = "Admin123";
    public const string Seed_Email = "Admin123@gmail.com";
    public const string Seed_Password = "Admin123";

    public static void Seed_Users()
    {
        var users = Get_All_Users().FirstOrDefault(x => x.Role == Role.Admin);

        if (users == null)
        {
            Create(Guid.Empty, Seed_Username, Seed_Email, Seed_Password, Role.Admin);
        }
    }

    public static List<User> Get_All_Users()
    {
        var app_Users_FilePath = Utils.Get_Users_FilePath();

        if (!File.Exists(app_Users_FilePath))
        {
            return new List<User>();
        }

        var json = File.ReadAllText(app_Users_FilePath);

        return JsonSerializer.Deserialize<List<User>>(json);
    }

	public static User Get_By_Id(Guid id)
	{
		var users = Get_All_Users();
		return users.FirstOrDefault(x => x.Id == id);
	}

	private static void Save_All(List<User> users)
    {
        var app_Directory_Path = Utils.Get_Directory_Path();
        var appUsersPath = Utils.Get_Users_FilePath();

        if (!Directory.Exists(app_Directory_Path))
        {
            Directory.CreateDirectory(app_Directory_Path);
        }

        var json = JsonSerializer.Serialize(users);

        File.WriteAllText(appUsersPath, json);
    }

	public static User Login(string username, string password)
	{
		var login_Error_Message = "Please enter valid username or password.";

        var users_list = Get_All_Users();

		var user = users_list.FirstOrDefault(x => x.Username == username);

		if (user == null)
		{
			throw new Exception(login_Error_Message);
		}

		bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

		if (!passwordIsValid)
		{
			throw new Exception(login_Error_Message);
		}

		return user;
	}

	public static List<User> Create(Guid userId, string username, string email, string password, Role role)
    {
        var users_list = Get_All_Users();

        var user_name_Exists = users_list.Any(x => x.Username == username);
        
        var number_Of_Admins = users_list.Where(x => x.Role == Role.Admin).Count();

		if (username == "")
		{
			throw new Exception("Please insert username.");
		}
		if ( email == "")
		{
			throw new Exception("Please insert email address.");
		}
		if (password == "")
		{
			throw new Exception("Please insert Password.");
		}

		if (number_Of_Admins > 2 && role == Role.Admin) 
        {
            throw new Exception("The system cannot add more than two admins.");
		}

		if (user_name_Exists)
        {
            throw new Exception("Username already exists");
        }

		var user = new User()
        {
            Username = username,
            Email = email,
            PasswordHash = Utils.HashSecret(password),
            Role = role,
            CreatedBy = userId,
        };

        users_list.Add(user);

        Save_All(users_list);
        
        return users_list;
    }

    public static List<User> Delete(Guid id)
    {
        var users_list = Get_All_Users();
        
        var user = users_list.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not available.");
        }

        users_list.Remove(user);
        
        Save_All(users_list);

        return users_list;
    }
    public static User ChangePassword(Guid id, string password, string newPassword)
    {
        if (password == newPassword)
        {
            throw new Exception("New password must be different from current password.");
        }

        List<User> users = Get_All_Users();
        User user = users.FirstOrDefault(x => x.Id == id);

        if (user == null)
        {
            throw new Exception("User not found.");
        }

        bool passwordIsValid = Utils.VerifyHash(password, user.PasswordHash);

        if (!passwordIsValid)
        {
            throw new Exception("Incorrect current password.");
        }

        user.PasswordHash = Utils.HashSecret(newPassword);
        user.HasInitialPassword = false;
        Save_All(users);

        return user;
    }


}
