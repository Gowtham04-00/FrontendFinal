using Microsoft.AspNetCore.Mvc;
using System.Data;
using SavuProject.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using Microsoft.AspNetCore.Hosting.Server;

namespace SavuProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ModuleController : Controller
    {
        public IConfiguration Configuration { get; set; }

        public ModuleController(IConfiguration configuration)
        {
            Configuration = configuration;
        }



        [HttpDelete("Delete/{ID}")]
        public ActionResult Delete(int ID)
        {
            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                con.Open();
                string sql = "DELETE FROM VideoFiles WHERE ID = @ID";
                SqlCommand cmd = new SqlCommand(sql, con);
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.AddWithValue("@ID", ID);
                    cmd.ExecuteNonQuery();
                }
                // Successfully deleted the record
                return Ok(); // Redirect to the list view or any other appropriate action
            }
        }




        [HttpGet]
        public ActionResult UploadVideo()
        {
            //List<VideoFiles> videolist = new List<VideoFiles>();
            //string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

            //using (SqlConnection con = new SqlConnection(connectionString))
            //{
            //    SqlCommand cmd = new SqlCommand("spGetAllVideoFile", con);
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    con.Open();
            //    SqlDataReader rdr = cmd.ExecuteReader();
            //    while (rdr.Read())
            //    {
            //        VideoFiles video = new VideoFiles();
            //        video.ID = Convert.ToInt32(rdr["ID"]);
            //        video.Name = rdr["Name"].ToString();
            //        video.FileSize = Convert.ToInt32(rdr["FileSize"]);
            //        video.FilePath = rdr["FilePath"].ToString();

            //        videolist.Add(video);
            //    }
            //}
            //return Ok(videolist);
            return Ok();
        }


        //[HttpPost("UploadVideo")]
        //public ActionResult UploadVideo(IFormFile file)
        //{
        //    try
        //    {
        //        if (file != null && file.Length > 0)
        //        {
        //            string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
        //            int fileSize = (int)file.Length;

        //            int sizeInKB = (int)(fileSize / 1024.0);




        //            string uploadFolderPath = Path.Combine("wwwroot/VideoFileUpload");



        //            if (!Directory.Exists(uploadFolderPath))
        //            {
        //                Directory.CreateDirectory(uploadFolderPath);
        //            }

        //            string filePath = Path.Combine(uploadFolderPath, fileName); // Corrected filePath

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                file.CopyTo(stream);
        //            }

        //            filePath = Path.Combine("/VideoFileUpload/", fileName);


        //            //string connectionString = Configuration.GetConnectionString("DefaultConnection"); // Corrected connection string retrieval
        //            string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

        //            using (SqlConnection con = new SqlConnection(connectionString))
        //            {
        //                con.Open();
        //                SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
        //                cmd.CommandType = CommandType.StoredProcedure;
        //                cmd.Parameters.AddWithValue("@Name", fileName);
        //                cmd.Parameters.AddWithValue("@FileSize", sizeInKB);
        //                cmd.Parameters.AddWithValue("@FilePath", filePath);
        //                cmd.ExecuteNonQuery();
        //            }

        //            return Ok();
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "No file selected for upload.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        // Handle exceptions appropriately (log, show error message, etc.)
        //        // Example: Log.Error(ex.Message);
        //        ModelState.AddModelError("", $"An error occurred while uploading the file. {ex.Message}");
        //    }

        //    return Ok(); // You might want to return a view with an error message here
        //}




        [HttpPost("UploadVideo")]
        public ActionResult UploadVideo([FromForm] IFormFile file, [FromForm] int id)
        {
            try
            {
                if (file != null && file.Length > 0)
                {
                    string fileName = $"{Path.GetFileNameWithoutExtension(file.FileName)}_{DateTime.Now:yyyyMMddHHmmssfff}{Path.GetExtension(file.FileName)}";
                    int fileSize = (int)file.Length;
                    int sizeInKB = (int)(fileSize / 1024.0);
                    int ID = (int)id;

                    string uploadFolderPath = Path.Combine("wwwroot", "VideoFileUpload");

                    if (!Directory.Exists(uploadFolderPath))
                    {
                        Directory.CreateDirectory(uploadFolderPath);
                    }

                    string filePath = Path.Combine(uploadFolderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        file.CopyTo(stream);
                    }

                    filePath = Path.Combine("VideoFileUpload", fileName); // Corrected file path

                    string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        con.Open();
                        SqlCommand cmd = new SqlCommand("spAddNewVideoFile", con);
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ID", ID);
                        cmd.Parameters.AddWithValue("@Name", fileName);
                        cmd.Parameters.AddWithValue("@FileSize", sizeInKB);
                        cmd.Parameters.AddWithValue("@FilePath", filePath);
                        cmd.ExecuteNonQuery();
                    }

                    return Ok();
                }
                else
                {
                    ModelState.AddModelError("", "No file selected for upload.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception details for debugging
                Console.WriteLine($"Error: {ex.Message}");

                ModelState.AddModelError("", $"An error occurred while uploading the file. {ex.Message}");
            }

            return BadRequest(ModelState);
        }




        [HttpGet]
        [Route("Get/{ID}")]
        public ActionResult Get(int ID)
        {

            List<VideoFiles> videoList = new List<VideoFiles>();
            try
            {
                string connectionString = Configuration["ConnectionStrings:DefaultConnection"];

                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    con.Open();
                    string sql = "Select * From VideoFiles where ID=@ID";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.CommandType = CommandType.Text;
                    //cmd.Parameters.AddWithValue("@VideoId", videoId);
                    cmd.Parameters.AddWithValue("@ID", ID);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {

                        if (reader.Read())
                        {
                            VideoFiles video = new VideoFiles();
                            video.ID = Convert.ToInt32(reader["ID"]);
                            //video.VID = Convert.ToInt32(reader["VID"]);
                            video.Name = Convert.ToString(reader["Name"]);
                            video.FileSize = Convert.ToInt32(reader["FileSize"]);
                            video.FilePath = Convert.ToString(reader["FilePath"]);
                            //string filePath = Convert.ToString(reader["FilePath"]);
                           //string videoFolderPath = " C:\\dotnet\\SavuProject\\SavuProject\\VideoFileUpload\\VideoFileUpload\\Badass-MassTamilan.dev_20240108163";
                            //video.FilePath = Path.Combine(videoFolderPath, filePath);
                            videoList.Add(video);
                        }
                    }
                    con.Close();
                }
                return Ok(videoList);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while fetching the video. {ex.Message}");
                return Ok(); // Return a view with an error message
            }
        }

      




    }
}
