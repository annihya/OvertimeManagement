using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

public class DBModel : DbContext
{
    // You can add custom code to this file. Changes will not be overwritten.
    // 
    // If you want Entity Framework to drop and regenerate your database
    // automatically whenever you change your model schema, please use data migrations.
    // For more information refer to the documentation:
    // http://msdn.microsoft.com/en-us/data/jj591621.aspx

    public DBModel() : base("name=OvertimeManagement")
    {
    }

    public DbSet<OvertimeManagement.Models.Overtime> Overtimes { get; set; }
    public DbSet<OvertimeManagement.Models.Employee> Employees { get; set; }
    public DbSet<OvertimeManagement.Models.Department> Departments { get; set; }
    public DbSet<OvertimeManagement.Models.Position> Positions { get; set; }
}
