using Domain.Classes;
using Domain.Repositories.Interfaces;
using Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Services.Classes
{
    /// <summary>
    /// Service implementation for IUserService.
    /// </summary>
    public class UserService : IUserService
    {
        // Repositories
        private IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// List of users as <see cref="UsersDto"/> objects.
        /// </returns>
        public List<UsersDto> AllUsers()
        {
            // gets result
            var result = _userRepository.AllUsers();

            // returns converted result
            return result.Select(user => ConvertToUserDto(user)).ToList();
        }

        /// <summary>
        /// Saves a new user to the system.
        /// </summary>
        /// <param name="data">The user data as a <see cref="UsersDto"/> object.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure.
        /// </returns>
        public async Task<Result> SaveNewUserAsync(UsersDto data)
        {
            // conversion
            var user = new Users()
            {
                UserName  = data.UserName ?? string.Empty,
                FirstName = data.FirstName ?? string.Empty,
                UserMail  = data.UserMail ?? string.Empty,
                IsAdmin   = data.IsAdmin
            };

            // returns result
            return await _userRepository.SaveNewUserAsync(user);
        }

        /// <summary>
        /// Converts a single <see cref="Users"/> entity to a <see cref="UsersDto"/> object. Maps all 
        /// relevant properties from the <see cref="Users"/> entity to the corresponding 
        /// properties in <see cref="UsersDto"/>.
        /// </summary>
        /// <param name="task">The <see cref="Users"/> entity to be converted.</param>
        /// <returns>
        /// A <see cref="UsersDto"/> object containing the mapped data.
        /// </returns>
        private UsersDto ConvertToUserDto(Users users)
        {
            if (users is null)
                return new UsersDto();

            // returns converted list
            return new UsersDto
            {
                UserId    = users.UserId,
                UserName  = users.UserName,
                FirstName = users.FirstName,
                UserMail  = users.UserMail,
                IsAdmin   = users.IsAdmin
            };
        }
    }
}
