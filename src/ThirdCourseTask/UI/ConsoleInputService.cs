using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThirdCourseTask.Infrastructure.Services.Abstractions;

namespace ThirdCourseTask.UI
{
    public class ConsoleInputService : IInputService
    {
        private ITaskService _taskService;

        public ConsoleInputService(ITaskService taskService)
        {
            _taskService = taskService;
        }

        public async Task RunAsync()
        {
            var key = Console.ReadLine();
            Menu();
            switch (key)
            {
                case "1":
                    {
                        await AddTaskAsync();
                        break;
                    }


                case "2":
                    {
                        await ListTasksAsync();
                        break;
                    }


                case "3":
                    {
                        await ChangeTaskStatus();
                        break;
                    }

                case "4":
                    {
                        await RemoveTaskByIdAsync();
                        break;
                    }

                case "0":
                    return;

                default:
                    Console.WriteLine("The operation you selected does not exist. Please try again.");
                    break;
            }
        }

        private void Menu()
        {
            Console.WriteLine("\n=== Task Manager ===");
            Console.WriteLine("1) Add Task");
            Console.WriteLine("2) Show All Tasks");
            Console.WriteLine("3) Change Task status");
            Console.WriteLine("4) Remove Task By Id");
        }

        public async Task AddTaskAsync()
        {
            try
            {
                Console.Write("Title: ");
                string? title = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(title))
                {
                    Console.WriteLine("Title cannot be empty.");
                    return;
                }

                Console.Write("Description: ");
                string? description = Console.ReadLine();

                int createdTaskId = await _taskService.AddAsync(title, description);
                Console.WriteLine("Created. Id=" + createdTaskId);
            }

            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        private async Task ListTasksAsync()
        {
            try
            {
                var tasks = await _taskService.GetAllAsync();

                if (tasks.Count == 0)
                {
                    Console.WriteLine("Empty.");
                    return;
                }

                foreach (var task in tasks)
                {
                    Console.WriteLine("Id: " + task.Id + ", Title: " + task.Title + ", Completed: " + task.IsCompleted + ", CreatedAt: " + task.CreatedAt);
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }
        }

        private async Task ChangeTaskStatus()
        {
            try
            {
                Console.Write("Id: ");
                string? input = Console.ReadLine();

                int taskId;
                bool parsed = int.TryParse(input, out taskId);

                if (!parsed)
                {
                    Console.WriteLine("Invalid Id.");
                    return;
                }

                Console.Write("Completed? (y/n): ");
                string? ans = Console.ReadLine();

                bool isCompleted = false;

                if (ans == "y" || ans == "1")
                {
                    isCompleted = true;
                }

                bool updated = await _taskService.CompleteAsync(taskId, isCompleted);

                if (updated)
                {
                    Console.WriteLine("Updated.");
                }
                else
                {
                    Console.WriteLine("Not found.");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

        }

        private async Task RemoveTaskByIdAsync()
        {
            try
            {
                Console.Write("Id: ");
                string? input = Console.ReadLine();

                int taskId;
                bool parsed = int.TryParse(input, out taskId);

                if (!parsed)
                {
                    Console.WriteLine("Invalid Id.");
                    return;
                }

                bool deleted = await _taskService.RemoveAsync(taskId);

                if (deleted)
                {
                    Console.WriteLine("Deleted.");
                }
                else
                {
                    Console.WriteLine("Not found.");
                }
            }

            catch (Exception ex)
            {
                Console.WriteLine("Unexpected error: " + ex.Message);
            }

        }
    }
}
