﻿//
// Nitrocid KS  Copyright (C) 2018-2023  Aptivi
//
// This file is part of Nitrocid KS
//
// Nitrocid KS is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// Nitrocid KS is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY, without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <https://www.gnu.org/licenses/>.
//

using KS.Files;
using KS.Files.Operations;
using KS.Files.Operations.Querying;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace Nitrocid.Extras.ToDoList.ToDoList
{
    /// <summary>
    /// Manages your to-do list
    /// </summary>
    public static class ToDoManager
    {
        private static List<ToDoTask> toDoTasks = new();

        /// <summary>
        /// Adds a task
        /// </summary>
        /// <param name="taskName">Task name to add</param>
        public static void AddTask(string taskName)
        {
            if (!TaskExists(taskName))
            {
                var task = new ToDoTask()
                {
                    TaskName = taskName,
                };
                toDoTasks.Add(task);
            }
        }

        /// <summary>
        /// Removes a task
        /// </summary>
        /// <param name="taskName">Task name to remove</param>
        public static void RemoveTask(string taskName)
        {
            if (!TaskExists(taskName))
                toDoTasks.Remove(GetTask(taskName));
        }

        /// <summary>
        /// Gets a to-do task
        /// </summary>
        /// <param name="taskName">Task name to get</param>
        /// <returns>An instance of ToDoTask</returns>
        public static ToDoTask GetTask(string taskName) =>
            toDoTasks.Single((todo) => todo.TaskName == taskName);

        /// <summary>
        /// Gets a to-do task index
        /// </summary>
        /// <param name="taskName">Task name to get</param>
        /// <returns>An index of ToDoTask</returns>
        public static int GetTaskIndex(string taskName) =>
            toDoTasks.IndexOf(GetTask(taskName));

        /// <summary>
        /// Checks to see whether the to-do task exists
        /// </summary>
        /// <param name="taskName">Task name to check</param>
        /// <returns>True if found; false otherwise</returns>
        public static bool TaskExists(string taskName) =>
            toDoTasks.Any((todo) => todo.TaskName == taskName);

        /// <summary>
        /// Sets the task as done
        /// </summary>
        /// <param name="taskName">Task name to set</param>
        public static void SetDone(string taskName)
        {
            int idx = GetTaskIndex(taskName);
            toDoTasks[idx].TaskDone = true;
        }

        /// <summary>
        /// Sets the task as undone
        /// </summary>
        /// <param name="taskName">Task name to set</param>
        public static void SetUndone(string taskName)
        {
            int idx = GetTaskIndex(taskName);
            toDoTasks[idx].TaskDone = false;
        }

        /// <summary>
        /// Gets the task names
        /// </summary>
        /// <returns>An array of string containing task names</returns>
        public static string[] GetTaskNames() =>
            toDoTasks.Select((task) => task.TaskName).ToArray();

        /// <summary>
        /// Saves all tasks
        /// </summary>
        public static void SaveTasks()
        {
            string serializedTasks = JsonConvert.SerializeObject(toDoTasks, Formatting.Indented);
            Writing.WriteContentsText(Paths.GetKernelPath(KernelPathType.ToDoList), serializedTasks);
        }

        /// <summary>
        /// Loads all tasks
        /// </summary>
        public static void LoadTasks()
        {
            string path = Paths.GetKernelPath(KernelPathType.ToDoList);
            if (!Checking.FileExists(path))
                Making.MakeJsonFile(path, true, true);
            string serializedTasks = Reading.ReadContentsText(path);
            toDoTasks = (List<ToDoTask>)JsonConvert.DeserializeObject(serializedTasks, typeof(List<ToDoTask>));
        }
    }
}
