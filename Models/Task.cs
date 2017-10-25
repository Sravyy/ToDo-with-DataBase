using System.Collections.Generic;
using MySql.Data.MySqlClient;
using System;

namespace ToDoList.Models
{
  public class Task
  {
    private int _id;
    private string _description;
    private int _categoryId;
    private string _date;

    public Task(string Description, string dueDate, int categoryId, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _date = dueDate;
      _categoryId = categoryId;
    }


    public override bool Equals(System.Object otherTask)
    {
      if (!(otherTask is Task))
      {
        return false;
      }
      else
      {
        Task newTask = (Task) otherTask;
        bool idEquality = (this.GetId() == newTask.GetId());
        bool descriptionEquality = (this.GetDescription() == newTask.GetDescription());
        bool categoryEquality = this.GetCategoryId() == newTask.GetCategoryId();
        bool dateEquality = this.GetDueDate() == newTask.GetDueDate();
        return (idEquality && descriptionEquality && categoryEquality && dateEquality);
      }
    }
    public override int GetHashCode()
    {
      return this.GetDescription().GetHashCode();
    }
    public string GetDescription()
    {
      return _description;
    }
    public int GetId()
    {
      return _id;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }
    public string GetDueDate()
    {
      return _date;
    }
    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO tasks (description, dueDate, category_id) VALUES (@description, @dueDate , @category_id);";

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@description";
      description.Value = this._description;
      cmd.Parameters.Add(description);

      MySqlParameter dueDate = new MySqlParameter();
      dueDate.ParameterName = "@dueDate";
      dueDate.Value = this._date;
      cmd.Parameters.Add(dueDate);

      MySqlParameter categoryId = new MySqlParameter();
      categoryId.ParameterName = "@category_id";
      categoryId.Value = this._categoryId;
      cmd.Parameters.Add(categoryId);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;
      // more logic will go here

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Task> GetAll()
    {
      List<Task> allTasks = new List<Task> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int taskId = rdr.GetInt32(0);
        string taskDescription = rdr.GetString(1);
        string taskDueDate = rdr.GetString(2);
        int taskCategoryId = rdr.GetInt32(3);
        Task newTask = new Task(taskDescription, taskDueDate, taskCategoryId,taskId );
        allTasks.Add(newTask);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allTasks;
    }

    public static Task Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM tasks WHERE id = (@searchId);";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int taskId = 0;
      string taskDescription = "";
      string taskDueDate = "";
      int taskCategoryId = 0;

      while (rdr.Read())
      {
        taskId = rdr.GetInt32(0);
        taskDescription = rdr.GetString(1);
        taskDueDate = rdr.GetString(2);
        taskCategoryId = rdr.GetInt32(3);
      }
      Task newTask = new Task(taskDescription, taskDueDate, taskCategoryId, taskId);
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return newTask;
    }

    public static List<Task> GetAlphaList()
        {
            List<Task> allTasks = new List<Task> {};
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"SELECT * FROM tasks ORDER BY dueDate;";
            var rdr = cmd.ExecuteReader() as MySqlDataReader;
            while(rdr.Read())
            {
              int taskId = rdr.GetInt32(0);
              string taskDescription = rdr.GetString(1);
              string taskDueDate = rdr.GetString(2);
              int taskCategoryId = rdr.GetInt32(3);
              Task newTask = new Task(taskDescription, taskDueDate, taskCategoryId, taskId );
              allTasks.Add(newTask);
            }
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
            return allTasks;
        }

        public static void DeleteTasks(int id)
        {
            MySqlConnection conn = DB.Connection();
            conn.Open();
            var cmd = conn.CreateCommand() as MySqlCommand;
            cmd.CommandText = @"DELETE FROM tasks WHERE category_id = @id;";

            MySqlParameter categoryId = new MySqlParameter();
            categoryId.ParameterName = "@id";
            categoryId.Value = id;
            cmd.Parameters.Add(categoryId);

            cmd.ExecuteNonQuery();
            conn.Close();
            if (conn != null)
            {
                conn.Dispose();
            }
        }

        public static void DeleteAll()
        {
          MySqlConnection conn = DB.Connection();
          conn.Open();

          var cmd = conn.CreateCommand() as MySqlCommand;
          cmd.CommandText = @"DELETE FROM tasks;";
          cmd.ExecuteNonQuery();
          conn.Close();
          if (conn != null)
          {
            conn.Dispose();
          }
        }


  }
}
