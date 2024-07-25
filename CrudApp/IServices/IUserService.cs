using CrudApp.Models;

namespace CrudApp.IServices;

public interface IUserService
{
    void CreateUser(string name, int age);
    List<User> GetAllUsers();
    void UpdateUser(int id, string name, int age);
    void DeleteUser(int id);
}
