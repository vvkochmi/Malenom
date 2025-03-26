using MalenomAPI.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Web.Http;

namespace MalenomAPI.Controllers
{
    public class ImageController : ApiController
    {
        const string connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Malenom";

        [HttpGet]
        [Route("api/images/all")]
        public IHttpActionResult GetImages()
        {
            List<Image> images = new List<Image>();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT ID, Name, Img FROM Images";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = (int)reader["ID"];
                            string imgName = reader["Name"].ToString();
                            byte[] imgData = (byte[])reader["Img"];

                            images.Add(new Image { ID = id, Name = imgName, Img = imgData });
                        }
                    }
                }
            }
            return Ok(images);
        }

        [HttpPost]
        [Route("api/images/add")]
        public IHttpActionResult PostImage(Image image)
        {
            if (image == null)
            {
                return BadRequest();
            }

            int new_id = 0;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "INSERT INTO Images (Name, Img) VALUES (@ImgName, @ImgData)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImgName", image.Name);
                    command.Parameters.AddWithValue("@ImgData", image.Img);
                    command.ExecuteNonQuery();
                }

                query = "SELECT MAX(ID) FROM Images WHERE Name = @ImgName AND Img = @ImgData";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ImgName", image.Name);
                    command.Parameters.AddWithValue("@ImgData", image.Img);
                    new_id = (int)command.ExecuteScalar();
                }
            }

            return Ok(new_id);
        }

        [HttpPut]
        [Route("api/images/update/{id}")]
        public IHttpActionResult PutImage(int id, Image image)
        {
            if (image == null || image.ID != id)
            {
                return BadRequest();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "UPDATE Images SET Name = @ImgName, Img = @ImgData WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.Parameters.AddWithValue("@ImgName", image.Name);
                    command.Parameters.AddWithValue("@ImgData", image.Img);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }

        [HttpDelete]
        [Route("api/images/delete/{id}")]
        public IHttpActionResult DeleteImage(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string query = "DELETE Images WHERE ID = @ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
            return Ok();
        }
    }
}
