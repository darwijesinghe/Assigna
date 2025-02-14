using Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repositories.Interfaces
{
    /// <summary>
    /// Repository interface for user related operations.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// List of users as <see cref="Users"/> objects.
        /// </returns>
        List<Users> AllUsers();

        /// <summary>
        /// Saves a new user to the system.
        /// </summary>
        /// <param name="data">The user data as a <see cref="Users"/> object.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure.
        /// </returns>
        Task<Result> SaveNewUserAsync(Users data);
    }
}
