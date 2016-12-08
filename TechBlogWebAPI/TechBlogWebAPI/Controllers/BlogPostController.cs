using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechBlogWebAPI.Models;

namespace TechBlogWebAPI.Controllers
{
    public class BlogPostController : ApiController
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;

        [HttpPost]
        public IHttpActionResult AddPost([FromBody]PostsModels post)
        {
            sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            const string procedureName = "AddPost";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            SqlParameter paramEmail = new SqlParameter();
            paramEmail.ParameterName = "@EmailId";
            paramEmail.Direction = System.Data.ParameterDirection.Input;
            paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
            paramEmail.Size = 50;
            paramEmail.Value = post.EmailId;           

            SqlParameter paramPostTitle = new SqlParameter();
            paramPostTitle.ParameterName = "@PostTitle";
            paramPostTitle.Direction = System.Data.ParameterDirection.Input;
            paramPostTitle.SqlDbType = System.Data.SqlDbType.VarChar;            
            paramPostTitle.Value = post.PostTitle;

            SqlParameter paramContent = new SqlParameter();
            paramContent.ParameterName = "@Content";
            paramContent.Direction = System.Data.ParameterDirection.Input;
            paramContent.SqlDbType = System.Data.SqlDbType.VarChar;
            paramContent.Value = post.PostContent;

            SqlParameter paramPostedBy = new SqlParameter();
            paramPostedBy.ParameterName = "@PostedBy";
            paramPostedBy.Direction = System.Data.ParameterDirection.Input;
            paramPostedBy.SqlDbType = System.Data.SqlDbType.VarChar;
            paramPostTitle.Size = 50;
            paramPostedBy.Value = post.PostedBy;

            SqlParameter paramCategoryId = new SqlParameter();
            paramCategoryId.ParameterName = "@CategoryId_FK";
            paramCategoryId.Direction = System.Data.ParameterDirection.Input;
            paramCategoryId.SqlDbType = System.Data.SqlDbType.VarChar;
            paramPostedBy.Size = 50;
            paramCategoryId.Value = post.CategoryId;

            SqlParameter paramStatus = new SqlParameter();
            paramStatus.ParameterName = "@Status";
            paramStatus.Direction = System.Data.ParameterDirection.Input;
            paramStatus.SqlDbType = System.Data.SqlDbType.Char;
            paramStatus.Size = 1;
            paramStatus.Value = post.Status;

            SqlParameter paramMessage= new SqlParameter();
            paramMessage.ParameterName = "@message";
            paramMessage.Direction = System.Data.ParameterDirection.Output;
            paramMessage.Size = 100;
            paramMessage.SqlDbType = System.Data.SqlDbType.VarChar;

            cmd.Parameters.AddRange(new SqlParameter[] { paramEmail, paramCategoryId, paramContent, paramMessage, paramPostedBy, paramPostTitle, paramStatus });
          
            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
                    cmd.Connection = sqlConnection;
                }
               

                cmd.ExecuteNonQuery();

                if (paramMessage.Value.Equals("New Post added successfully"))
                    return Ok(paramMessage.Value.ToString());
                else
                {
                    HttpError error = new HttpError(paramMessage.Value.ToString());
                    var responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotModified, error);
                    throw new HttpResponseException(responseMessage);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            finally
            {
                sqlConnection.Close();
            }   
        }

        [HttpGet]
        public IHttpActionResult GetPost(string EmailId)
        {
            PostsModels post = new PostsModels();
            post.EmailId = EmailId;
            List<PostsModels> listOfPosts = new List<PostsModels>();
            sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            const string procedureName = "GetPosts";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dtPosts = new DataTable();
            SqlParameter paramEmail = new SqlParameter();
            paramEmail.ParameterName = "@EmailId";
            paramEmail.Direction = System.Data.ParameterDirection.Input;
            paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
            paramEmail.Size = 50;
            paramEmail.Value = post.EmailId;

            int rows = -1;

            try
            {
                if (sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
                }
                    cmd.Connection = sqlConnection;
                cmd.Parameters.Add(paramEmail);
                    da.Fill(dtPosts);
                    rows = dtPosts.Rows.Count;
                for (int i = 0; i < rows; i++)
                {
                    post = new PostsModels();
                    post.CategoryId = dtPosts.Rows[i]["CategoryId_FK"].ToString();
                   // post.EmailId = dtPosts.Rows[i]["EmailId"].ToString();
                    post.LastEditedDate = (DateTime)dtPosts.Rows[i]["LastEditedDate"];
                    post.PostContent = dtPosts.Rows[i]["PostContent"].ToString();
                    post.PostedBy = dtPosts.Rows[i]["PostedBy"].ToString();
                    post.PostedDate = dtPosts.Rows[i]["PostedDate"].ToString();
                    post.PostTitle = dtPosts.Rows[i]["PostTitle"].ToString();
                    listOfPosts.Add(post);
                }
                
                if (listOfPosts.Count > 0)
                {
                    return Ok(listOfPosts);
                }
                else
                {
                    HttpError error = new HttpError("No Rows found");
                    var responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotFound, error);
                    throw new HttpResponseException(responseMessage);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
    }
}
