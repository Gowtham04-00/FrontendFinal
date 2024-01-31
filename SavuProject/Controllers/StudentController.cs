using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Data;
using SavuProject.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection;

namespace SavuProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private IConfiguration Configuration;


        public StudentController(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        [HttpGet]
        [Route("GetStudentbyid/{CId}")]
        public IActionResult GetStudentbyid(int CId)
        {
            List<Student> courseList = new List<Student>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "GetStudentDetails";
                SqlCommand command = new SqlCommand(sql, connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CId", CId);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);


                if (dataAdapter is not null)
                {
                    dataAdapter.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Student course = new Student();

                        course.SId = Convert.ToString(dr["SId"]);
                        course.SName = Convert.ToString(dr["SName"]);
                        course.CName = Convert.ToString(dr["CName"]);

                        courseList.Add(course);
                    }
                }

            }
            return Ok(courseList);
        }
    }
}
