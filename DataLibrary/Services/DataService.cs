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
        public DataContext _context { get; }

        public DataService(DataContext context)
        {
            _context = context;
        }

        // all tasks
        public List<TaskDto> AllTasks(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        }).ToList();

            // if user is an admin the return full list
            // otherwise filter list by user id

            if (isAdmin)
            {
                return taks.OrderBy(x => x.tsk_id).ToList();
            }

            return taks.Where(x => x.user_name == userName)
            .OrderBy(x => x.tsk_id)
            .ToList();
        }

        // pending tasks
        public List<TaskDto> Pendings(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.pending == true)
                .OrderBy(x => x.tsk_id)
                .ToList();
            }

            return taks.Where(x => x.pending == true & x.user_name == userName)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // completed tasks
        public List<TaskDto> Completed(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.complete == true)
                .OrderBy(x => x.tsk_id)
                .ToList();
            }

            return taks.Where(x => x.complete == true & x.user_name == userName)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // high priority tasks
        public List<TaskDto> HighPriority(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.prio_high == true)
                .OrderBy(x => x.tsk_id)
                .ToList();
            }

            return taks.Where(x => x.prio_high == true & x.user_name == userName)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // medium priority tasks
        public List<TaskDto> MediumPriority(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.prio_medium == true)
                .OrderBy(x => x.tsk_id)
                .ToList();
            }

            return taks.Where(x => x.prio_medium == true & x.user_name == userName)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // low priority tasks
        public List<TaskDto> LowPriority(string userName, bool isAdmin)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            user_name = us.user_name,
                            first_name = us.first_name

                        });

            if (isAdmin)
            {
                return taks.Where(x => x.prio_low == true)
                .OrderBy(x => x.tsk_id)
                .ToList();
            }

            return taks.Where(x => x.prio_low == true & x.user_name == userName)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // task categories
        public List<CategoryDto> Categories()
        {
            var categories = _context.category
            .Select(x => new CategoryDto
            {
                cat_id = x.cat_id,
                cat_name = x.cat_name,

            }).OrderBy(x => x.cat_id)
            .ToList();

            return categories;
        }

        // app users
        public List<UsersDto> Users()
        {
            var users = _context.users
            .Select(x => new UsersDto
            {
                user_id = x.user_id,
                user_name = x.user_name,
                first_name = x.first_name,
                user_mail = x.user_mail,
                is_admin = x.is_admin
            })
            .Where(x => x.is_admin == false)
            .OrderBy(x => x.user_id)
            .ToList();

            return users;
        }

        // save new user
        public async Task<Result> SaveNewUserAsync(UsersDto data)
        {
            var user = new Users()
            {
                user_name = data.user_name ?? string.Empty,
                first_name = data.first_name ?? string.Empty,
                user_mail = data.user_mail ?? string.Empty,
                is_admin = data.is_admin
            };

            _context.users.Add(user);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // save a new task
        public async Task<Result> SaveTaskAsync(TaskDto data)
        {
            var task = new Task()
            {
                tsk_title = data.tsk_title ?? string.Empty,
                deadline = data.deadline,
                tsk_note = data.tsk_note ?? string.Empty,
                prio_high = data.prio_high,
                prio_medium = data.prio_medium,
                prio_low = data.prio_low,
                cat_id = data.cat_id,
                user_id = data.user_id,
                pending = data.pending
            };

            _context.task.Add(task);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // task info
        public List<TaskDto> TaskInfo(int taskId)
        {
            var taks = (from us in _context.users
                        join tk in _context.task on us.user_id equals tk.users.user_id
                        join ct in _context.category on tk.category.cat_id equals ct.cat_id
                        select new TaskDto
                        {
                            tsk_id = tk.tsk_id,
                            tsk_title = tk.tsk_title,
                            deadline = tk.deadline,
                            tsk_note = tk.tsk_note,
                            pending = tk.pending,
                            complete = tk.complete,
                            prio_high = tk.prio_high,
                            prio_medium = tk.prio_medium,
                            prio_low = tk.prio_low,
                            user_note = tk.user_note,
                            cat_id = tk.cat_id,
                            user_id = tk.user_id,
                            first_name = us.first_name,
                            user_mail = us.user_mail,
                            cat_name = ct.cat_name

                        });

            return taks
            .Where(x => x.tsk_id == taskId)
            .OrderBy(x => x.tsk_id).ToList();
        }

        // edit task
        public async Task<Result> EditTaskAsync(TaskDto data)
        {
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);

            if (task is not null)
            {
                task.tsk_title = data.tsk_title ?? string.Empty;
                task.cat_id = data.cat_id;
                task.deadline = data.deadline;
                task.prio_high = data.prio_high;
                task.prio_medium = data.prio_medium;
                task.prio_low = data.prio_low;
                task.user_id = data.user_id;
                task.tsk_note = data.tsk_note ?? string.Empty;
            }

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // delete task
        public async Task<Result> DeleteTaskAsync(int taskId)
        {
            var task = _context.task.FirstOrDefault(x => x.tsk_id == taskId);
            _context.task.Remove(task);

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true,
                    id = taskId
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // add task note
        public async Task<Result> AddTaskNoteAsync(TaskDto data)
        {
            var task = _context.task.FirstOrDefault(x => x.tsk_id == data.tsk_id);
            if (task is not null)
            {
                task.user_note = data.user_note ?? string.Empty;
            }

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // mark as task done
        public async Task<Result> MarkasDone(int taskId)
        {
            var task = _context.task.FirstOrDefault(x => x.tsk_id == taskId);
            if (task is not null)
            {
                task.pending = false;
                task.complete = true;
            }

            try
            {
                await _context.SaveChangesAsync();
                return new Result
                {
                    message = "Ok",
                    success = true
                };
            }
            catch (Exception ex)
            {
                return new Result
                {
                    message = ex.Message,
                    success = false
                };
            }
        }

        // get task count of sign in user
        public List<TaskCount> TaskCount(string userName, bool isAdmin)
        {
            List<TaskCount> _task = new();

            if (isAdmin)
            {
                var count = _context.task.ToList();
                var task = new TaskCount
                {
                    all_task = count.Count(),
                    pendings = count.Where(x => x.pending == true).Count(),
                    complete = count.Where(x => x.complete == true).Count(),
                    high = count.Where(x => x.prio_high == true).Count(),
                    medium = count.Where(x => x.prio_medium == true).Count(),
                    low = count.Where(x => x.prio_low == true).Count(),
                };
                _task.Add(task);
                return _task.ToList();
            }
            else
            {
                var count = (from us in _context.users
                             join tk in _context.task on us.user_id equals tk.users.user_id
                             select new TaskDto
                             {
                                 tsk_id = tk.tsk_id,
                                 pending = tk.pending,
                                 complete = tk.complete,
                                 prio_high = tk.prio_high,
                                 prio_medium = tk.prio_medium,
                                 prio_low = tk.prio_low,
                                 user_name = us.user_name

                             }).Where(x => x.user_name == userName).ToList();

                var task = new TaskCount
                {
                    all_task = count.Count(),
                    pendings = count.Where(x => x.pending == true).Count(),
                    complete = count.Where(x => x.complete == true).Count(),
                    high = count.Where(x => x.prio_high == true).Count(),
                    medium = count.Where(x => x.prio_medium == true).Count(),
                    low = count.Where(x => x.prio_low == true).Count(),
                };
                _task.Add(task);
                return _task.ToList();
            }

        }
    }
}