using Domain.Classes;
using Domain.Data;
using Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repositories.Classes
{
    /// <summary>
    /// Repository implementation for user operations
    /// </summary>
    public class UserRepository : IUserRepository
    {
        // Data context
        private DataContext _context { get; }

        public UserRepository(DataContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves a list of all users.
        /// </summary>
        /// <returns>
        /// List of users as <see cref="Users"/> objects.
        /// </returns>
        public List<Users> AllUsers()
        {
            // gets users
            var users = _context.Users.Select(x => new Users
                                      {
                                          UserId    = x.UserId,
                                          UserName  = x.UserName,
                                          FirstName = x.FirstName,
                                          UserMail  = x.UserMail,
                                          IsAdmin   = x.IsAdmin
                                      })
                                      .Where(x => x.IsAdmin == false)
                                      .OrderBy(x => x.UserId)
                                      .ToList();

            if (users is null || !users.Any())
                return new List<Users>();

            // returns result
            return users;
        }

        /// <summary>
        /// Saves a new user to the system.
        /// </summary>
        /// <param name="data">The user data as a <see cref="Users"/> object.</param>
        /// <returns>
        /// A <see cref="Result"/> object indicating success or failure.
        /// </returns>
        public async Task<Result> SaveNewUserAsync(Users data)
        {
            // adds data to user table
            await _context.Users.AddAsync(data);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                     Message = "Ok.",
                    Success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    Message = ex.Message,
                    Success = false
                };
            }
        }
    }
}
