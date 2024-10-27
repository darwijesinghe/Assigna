using Domain.Classes;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    /// <summary>
    /// Service interface for user related operations
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// List of users as <see cref="UsersDto"/> objects.
        /// </returns>
        List<UsersDto> AllUsers();

        /// <summary>
        /// Saves a new user to the system.
        /// </summary>
        /// <param name="data">The user data as a <see cref="UsersDto"/> object.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure.
        /// </returns>
        Task<Result> SaveNewUserAsync(UsersDto data);
    }
}
