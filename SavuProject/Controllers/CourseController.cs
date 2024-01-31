using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using SavuProject.Models;
using System.Data.SqlClient;
using Newtonsoft.Json;
using System.Reflection;

namespace SavuProject.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private IConfiguration Configuration;
        private readonly ILogger<CourseController> _logger;

        public CourseController(IConfiguration configuration, ILogger<CourseController> logger)
        {
            Configuration = configuration;
            _logger = logger; // Add this line
        }

        [HttpDelete("Delete/{CId}")]
        public async Task<IActionResult> Delete(int CId)
        {
            try
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync();

                    string sql = "DELETE FROM Course WHERE CId=@CId";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@CId", CId);
                        await command.ExecuteNonQueryAsync();
                    }
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        [Route("GetNotes")]
        public IActionResult GetNotes()
        {
            List<Course> courseList = new List<Course>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "Select * From Course";
                SqlCommand command = new SqlCommand(sql, connection);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);

                if (dataAdapter is not null)
                {
                    dataAdapter.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Course course = new Course();

                        course.CId = Convert.ToInt32(dr["CId"]);
                        course.CName = Convert.ToString(dr["CName"]);
                        course.CDuration = Convert.ToString(dr["CDuration"]);
                        course.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        course.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        course.CAvailability = Convert.ToString(dr["CAvailability"]);
                        course.CDescription = Convert.ToString(dr["CDescription"]);
                        course.CPre = Convert.ToString(dr["CPre"]);
                        course.OutCome = Convert.ToString(dr["OutCome"]);

                        courseList.Add(course);
                    }
                }

            }
            return Ok(courseList);
        }



        [HttpGet]
        [Route("GetNotesbyid/{id}")]
        public IActionResult GetNotesbyid(int id)
        {
            List<Course> courseList = new List<Course>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "Select * From Course where CId=@id";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);
                

                if (dataAdapter is not null)
                {
                    dataAdapter.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        Course course = new Course();

                        course.CId = Convert.ToInt32(dr["CId"]);
                        course.CName = Convert.ToString(dr["CName"]);
                        course.CDuration = Convert.ToString(dr["CDuration"]);
                        course.StartDate = Convert.ToDateTime(dr["StartDate"]);
                        course.EndDate = Convert.ToDateTime(dr["EndDate"]);
                        course.CAvailability = Convert.ToString(dr["CAvailability"]);
                        course.CDescription = Convert.ToString(dr["CDescription"]);
                        course.CPre = Convert.ToString(dr["CPre"]);
                        course.OutCome = Convert.ToString(dr["OutCome"]);

                        courseList.Add(course);
                    }
                }

            }
            return Ok(courseList);
        }



        [HttpGet]
        [Route("GetStudentCourse/{id}")]
        public IActionResult GetStudentCourse(int id)
        {
            List<VideoFiles> courseList = new List<VideoFiles>();
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                DataTable dataTable = new DataTable();

                string sql = "SELECT v.CId, v.FilePath, m.CId, m.SId, c.CName, c.startDate, c.endDate FROM VideoFiles v INNER JOIN MyLearning m ON v.CId = m.CId INNER JOIN Course c ON m.CId = c.CId WHERE m.CId = @Id;";
                SqlCommand command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@id", id);

                SqlDataAdapter dataAdapter = new SqlDataAdapter(command);


                if (dataAdapter is not null)
                {
                    dataAdapter.Fill(dataTable);
                    foreach (DataRow dr in dataTable.Rows)
                    {
                        VideoFiles course = new VideoFiles();

                        
                        course.FilePath = Convert.ToString(dr["FilePath"]);

                        courseList.Add(course);
                    }
                }

            }
            return Ok(courseList);
        }


        private Course MapDataRowToCourse(DataRow dr)
        {
            return new Course
            {
                CId = Convert.ToInt32(dr["CId"]),
                CName = Convert.ToString(dr["CName"]),
                CDuration = Convert.ToString(dr["CDuration"]),
                StartDate = Convert.ToDateTime(dr["StartDate"]),
                EndDate = Convert.ToDateTime(dr["EndDate"]),
                CAvailability = Convert.ToString(dr["CAvailability"]),
                CDescription = Convert.ToString(dr["CDescription"]),
                CPre = Convert.ToString(dr["CPre"]),
                OutCome = Convert.ToString(dr["OutCome"])
            };
        }






        [HttpGet("Create")]
        public IActionResult GetCreate()
        {
            return Ok();
        }


        [HttpPost("Create")]
        public IActionResult PostCreate([FromBody] Course course)
        {
            try
            {
                // Ensure you have valid data
                if (course == null)
                {
                    _logger.LogError("Invalid input data.");
                    return BadRequest("Invalid input data.");
                }

                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "InsertCourse";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Set up parameters
                        SqlParameter[] parameters = new SqlParameter[]
                        {
                    new SqlParameter("@CName", SqlDbType.NVarChar, 50) { Value = course.CName },
                    new SqlParameter("@CDuration", SqlDbType.NVarChar, 50) { Value = course.CDuration },
                    new SqlParameter("@StartDate", SqlDbType.DateTime) { Value = course.StartDate },
                    new SqlParameter("@EndDate", SqlDbType.DateTime) { Value = course.EndDate },
                    new SqlParameter("@CAvailability", SqlDbType.NVarChar, 50) { Value = course.CAvailability },
                    new SqlParameter("@CDescription", SqlDbType.NVarChar, -1) { Value = course.CDescription },
                    new SqlParameter("@CPre", SqlDbType.NVarChar, -1) { Value = course.CPre },
                    new SqlParameter("@OutCome", SqlDbType.NVarChar, -1) { Value = course.OutCome }
                        };

                        command.Parameters.AddRange(parameters);

                        connection.Open();

                        // Log the SQL command text before execution
                        _logger.LogInformation($"Executing SQL command: {command.CommandText}");

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Log success
                        _logger.LogInformation("Stored procedure executed successfully.");

                        connection.Close();
                    }
                }

                return Ok(new { Result = "Success" });
            }
            catch (SqlException sqlException)
            {
                // Log SQL exception details
                _logger.LogError($"SQL Error: {sqlException.Message}");
                _logger.LogError($"SQL Error Number: {sqlException.Number}");
                _logger.LogError($"SQL Error State: {sqlException.State}");
                _logger.LogError($"SQL Error Procedure: {sqlException.Procedure}");

                return StatusCode(500, "Internal Server Error. Please check the server logs for details.");
            }
            catch (Exception ex)
            {
                // Log other exceptions
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error. Please check the server logs for details.");
            }
        }



        //[HttpPost("Create")]
        //public IActionResult PostCreate([FromBody] Course course)
        //{

        //    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string sql = "InsertCourse";
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;


        //            SqlParameter parameter = new SqlParameter
        //            {
        //                ParameterName = "@CName",
        //                Value = course.CName,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CDuration",
        //                Value = course.CDuration,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@StartDate",
        //                Value = course.StartDate,
        //                SqlDbType = SqlDbType.DateTime
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@EndDate",
        //                Value = course.EndDate,
        //                SqlDbType = SqlDbType.DateTime
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CAvailability",
        //                Value = course.CAvailability,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CDescription",
        //                Value = course.CDescription,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CPre",
        //                Value = course.CPre,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@OutCome",
        //                Value = course.OutCome,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);


        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }

        //    return Ok(new { Result = "Success" });
        //}

        [HttpGet("Update/{CId}")]
        public IActionResult GetUpdateDetails(int CId)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
            Course course = new Course();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"Select * From Course Where CId='{CId}'";
                SqlCommand command = new SqlCommand(sql, connection);

                connection.Open();

                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        course.CId = Convert.ToInt32(dataReader["CId"]);
                        course.CName = Convert.ToString(dataReader["CName"]);
                        course.CDuration = Convert.ToString(dataReader["CDuration"]);
                        course.StartDate = Convert.ToDateTime(dataReader["StartDate"]);
                        course.EndDate = Convert.ToDateTime(dataReader["EndDate"]);
                        course.CAvailability = Convert.ToString(dataReader["CAvailability"]);
                        course.CDescription = Convert.ToString(dataReader["CDescription"]);
                        course.CPre = Convert.ToString(dataReader["CPre"]);
                        course.OutCome = Convert.ToString(dataReader["OutCome"]);
                    }
                }

                connection.Close();
            }

            return Ok(course);
        }


        [HttpPut("Update/{CId}")]
        public IActionResult Update([FromBody] Course course, int CId)
        {
            try
            {
                // Log the received data
                _logger.LogInformation($"Received data for update: {JsonConvert.SerializeObject(course)}");

                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    string sql = "UpdateCourse";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        // Add parameters
                        command.Parameters.AddWithValue("@CId", CId);
                        command.Parameters.AddWithValue("@CName", course.CName);
                        command.Parameters.AddWithValue("@CDuration", course.CDuration);
                        command.Parameters.AddWithValue("@StartDate", course.StartDate);
                        command.Parameters.AddWithValue("@EndDate", course.EndDate);
                        command.Parameters.AddWithValue("@CAvailability", course.CAvailability);
                        command.Parameters.AddWithValue("@CDescription", course.CDescription);
                        command.Parameters.AddWithValue("@CPre", course.CPre);
                        command.Parameters.AddWithValue("@OutCome", course.OutCome);

                        connection.Open();

                        // Log the SQL command text before execution
                        _logger.LogInformation($"Executing SQL command: {command.CommandText}");

                        // Execute the stored procedure
                        command.ExecuteNonQuery();

                        // Log success
                        _logger.LogInformation("Stored procedure executed successfully.");

                        connection.Close();
                    }
                }

                return Ok();
            }
            catch (SqlException sqlException)
            {
                // Log SQL exception details
                _logger.LogError($"SQL Error: {sqlException.Message}");
                _logger.LogError($"SQL Error Number: {sqlException.Number}");
                _logger.LogError($"SQL Error State: {sqlException.State}");
                _logger.LogError($"SQL Error Procedure: {sqlException.Procedure}");

                return StatusCode(500, "Internal Server Error. Please check the server logs for details.");
            }
            catch (Exception ex)
            {
                // Log other exceptions
                _logger.LogError($"Error: {ex.Message}");
                return StatusCode(500, "Internal Server Error. Please check the server logs for details.");
            }
        }

    }
}





        //[HttpPut("Update/{CId}")]
        //public IActionResult Update([FromBody] Course course, int CId)
        //{
        //    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];
        //    using (SqlConnection connection = new SqlConnection(connectionString))
        //    {
        //        string sql = "UpdateCourse";
        //        using (SqlCommand command = new SqlCommand(sql, connection))
        //        {
        //            command.CommandType = CommandType.StoredProcedure;


        //            SqlParameter parameter = new SqlParameter
        //            {
        //                ParameterName = "@CId",
        //                Value = CId,
        //                SqlDbType = SqlDbType.Int
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CName",
        //                Value = course.CName,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CDuration",
        //                Value = course.CDuration,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@StartDate",
        //                Value = course.StartDate.ToString("yyyy-MM-ddTHH:mm:ss"),
        //                SqlDbType = SqlDbType.DateTime
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@EndDate",
        //                Value = course.EndDate.ToString("yyyy-MM-ddTHH:mm:ss"),
        //                SqlDbType = SqlDbType.DateTime
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CAvailability",
        //                Value = course.CAvailability,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 50
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CDescription",
        //                Value = course.CDescription,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@CPre",
        //                Value = course.CPre,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);

        //            parameter = new SqlParameter
        //            {
        //                ParameterName = "@OutCome",
        //                Value = course.OutCome,
        //                SqlDbType = SqlDbType.VarChar,
        //                Size = 100
        //            };
        //            command.Parameters.Add(parameter);

        //            connection.Open();
        //            command.ExecuteNonQuery();
        //            connection.Close();
        //        }
        //    }

        //    return Ok();

        //}
    

