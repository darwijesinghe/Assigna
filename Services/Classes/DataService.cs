using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataLibrary.Data;
using DataLibrary.Interfaces;
using DataLibrary.Models;
using DataLibrary.Response;
using Task = DataLibrary.Models.Task;

namespace DataLibrary.Services
{
    public class DataService : IDataService
    {
        // data context
        public DataContext Context { get; }

        public DataService(DataContext context)
        {
            Context = context;
        }

        // all tasks
        public List<TaskDto> AllTasks(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        }).ToList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.OrderBy(x => x.TskId).ToList();
            }

            return taks.Where(x => x.UserName == userName)
            .OrderBy(x => x.TskId)
            .ToList();
        }

        // pending tasks
        public List<TaskDto> Pendings(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.Pending == true)
                .OrderBy(x => x.TskId)
                .ToList();
            }

            return taks.Where(x => x.Pending == true & x.UserName == userName)
            .OrderBy(x => x.TskId).ToList();
        }

        // completed tasks
        public List<TaskDto> Completed(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.Complete == true)
                .OrderBy(x => x.TskId)
                .ToList();
            }

            return taks.Where(x => x.Complete == true & x.UserName == userName)
            .OrderBy(x => x.TskId).ToList();
        }

        // high priority tasks
        public List<TaskDto> HighPriority(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.PrioHigh == true)
                .OrderBy(x => x.TskId)
                .ToList();
            }

            return taks.Where(x => x.PrioHigh == true & x.UserName == userName)
            .OrderBy(x => x.TskId).ToList();
        }

        // medium priority tasks
        public List<TaskDto> MediumPriority(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.PrioMedium == true)
                .OrderBy(x => x.TskId)
                .ToList();
            }

            return taks.Where(x => x.PrioMedium == true & x.UserName == userName)
            .OrderBy(x => x.TskId).ToList();
        }

        // low priority tasks
        public List<TaskDto> LowPriority(string userName, bool isAdmin)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            UserName = us.user_name,
                            FirstName = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.PrioLow == true)
                .OrderBy(x => x.TskId)
                .ToList();
            }

            return taks.Where(x => x.PrioLow == true & x.UserName == userName)
            .OrderBy(x => x.TskId).ToList();
        }

        // task categories
        public List<CategoryDto> Categories()
        {
            var categories = Context.category
            .Select(x => new CategoryDto
            {
                CatId = x.cat_id,
                CatName = x.cat_name,

            }).OrderBy(x => x.CatId)
            .ToList();

            return categories;
        }

        // app users
        public List<UsersDto> Users()
        {
            var users = Context.users
            .Select(x => new UsersDto
            {
                UserId = x.user_id,
                UserName = x.user_name,
                FirstName = x.first_name,
                UserMail = x.user_mail,
                IsAdmin = x.is_admin
            })
            .Where(x => x.IsAdmin == false)
            .OrderBy(x => x.UserId)
            .ToList();

            return users;
        }

        // save new user
        public async Task<Result> SaveNewUserAsync(UsersDto data)
        {
            var user = new Users()
            {
                user_name = data.UserName ?? string.Empty,
                first_name = data.FirstName ?? string.Empty,
                user_mail = data.UserMail ?? string.Empty,
                is_admin = data.IsAdmin
            };

            Context.users.Add(user);

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
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

        // save a new task
        public async Task<Result> SaveTaskAsync(TaskDto data)
        {
            var task = new Task()
            {
                tsk_title = data.TskTitle ?? string.Empty,
                deadline = data.Deadline,
                tsk_note = data.TskNote ?? string.Empty,
                prio_high = data.PrioHigh,
                prio_medium = data.PrioMedium,
                prio_low = data.PrioLow,
                cat_id = data.CatId,
                user_id = data.UserId,
                pending = data.Pending
            };

            Context.task.Add(task);

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
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

        // task info
        public List<TaskDto> TaskInfo(int taskId)
        {
            var taks = (from us in Context.users
                        join tk in Context.task on us.user_id equals tk.users.user_id
                        join ct in Context.category on tk.category.cat_id equals ct.cat_id
                        select new TaskDto
                        {
                            TskId = tk.tsk_id,
                            TskTitle = tk.tsk_title,
                            Deadline = tk.deadline,
                            TskNote = tk.tsk_note,
                            Pending = tk.pending,
                            Complete = tk.complete,
                            PrioHigh = tk.prio_high,
                            PrioMedium = tk.prio_medium,
                            PrioLow = tk.prio_low,
                            UserNote = tk.user_note,
                            CatId = tk.cat_id,
                            UserId = tk.user_id,
                            FirstName = us.first_name,
                            UserMail = us.user_mail,
                            CatName = ct.cat_name

                        });

            return taks
            .Where(x => x.TskId == taskId)
            .OrderBy(x => x.TskId).ToList();
        }

        // edit task
        public async Task<Result> EditTaskAsync(TaskDto data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);

            if (task is not null)
            {
                task.tsk_title = data.TskTitle ?? string.Empty;
                task.cat_id = data.CatId;
                task.deadline = data.Deadline;
                task.prio_high = data.PrioHigh;
                task.prio_medium = data.PrioMedium;
                task.prio_low = data.PrioLow;
                task.user_id = data.UserId;
                task.tsk_note = data.TskNote ?? string.Empty;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
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

        // delete task
        public async Task<Result> DeleteTaskAsync(int taskId)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == taskId);
            Context.task.Remove(task);

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
                    Success = true,
                    Id = taskId
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

        // add task note
        public async Task<Result> AddTaskNoteAsync(TaskDto data)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == data.TskId);
            if (task is not null)
            {
                task.user_note = data.UserNote ?? string.Empty;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
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

        // mark as task done
        public async Task<Result> MarkasDone(int taskId)
        {
            var task = Context.task.FirstOrDefault(x => x.tsk_id == taskId);
            if (task is not null)
            {
                task.pending = false;
                task.complete = true;
            }

            try
            {
                await Context.SaveChangesAsync();
                return new Result
                {
                    Message = "Ok",
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

        // get task count of sign in user
        public List<TaskCount> TaskCount(string userName, bool isAdmin)
        {
            List<TaskCount> _task = new();

            if (isAdmin)
            {
                var count = Context.task.ToList();
                var task = new TaskCount
                {
                    AllTask = count.Count(),
                    Pendings = count.Where(x => x.pending == true).Count(),
                    Complete = count.Where(x => x.complete == true).Count(),
                    High = count.Where(x => x.prio_high == true).Count(),
                    Medium = count.Where(x => x.prio_medium == true).Count(),
                    Low = count.Where(x => x.prio_low == true).Count(),
                };
                _task.Add(task);
                return _task.ToList();
            }
            else
            {
                var count = (from us in Context.users
                             join tk in Context.task on us.user_id equals tk.users.user_id
                             select new TaskDto
                             {
                                 TskId = tk.tsk_id,
                                 Pending = tk.pending,
                                 Complete = tk.complete,
                                 PrioHigh = tk.prio_high,
                                 PrioMedium = tk.prio_medium,
                                 PrioLow = tk.prio_low,
                                 UserName = us.user_name

                             }).Where(x => x.UserName == userName).ToList();

                var task = new TaskCount
                {
                    AllTask = count.Count(),
                    Pendings = count.Where(x => x.Pending == true).Count(),
                    Complete = count.Where(x => x.Complete == true).Count(),
                    High = count.Where(x => x.PrioHigh == true).Count(),
                    Medium = count.Where(x => x.PrioMedium == true).Count(),
                    Low = count.Where(x => x.PrioLow == true).Count(),
                };
                _task.Add(task);
                return _task.ToList();
            }

        }
    }
}