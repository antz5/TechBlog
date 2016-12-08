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
    public class CategoryController : ApiController
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;

        [HttpPost]
        [Route("Post")]
        public IHttpActionResult AddCategory([FromBody]CategoryModels category)
        {
            if (sqlConnection==null || sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
            }
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter paramEmail = new SqlParameter();
            paramEmail.ParameterName = "@EmailId";
            paramEmail.Direction = System.Data.ParameterDirection.Input;
            paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
            paramEmail.Size = 50;
            paramEmail.Value = category.emailId;

            SqlParameter paramCategoryName = new SqlParameter();
            paramCategoryName.ParameterName = "@CategoryName";
            paramCategoryName.Direction = System.Data.ParameterDirection.Input;
            paramCategoryName.SqlDbType = System.Data.SqlDbType.VarChar;
            paramCategoryName.Size = 50;
            paramCategoryName.Value = category.categoryName;

            SqlParameter paramCategoryDescription = new SqlParameter();
            paramCategoryDescription.ParameterName = "@CategoryDescription";
            paramCategoryDescription.Direction = System.Data.ParameterDirection.Input;
            paramCategoryDescription.SqlDbType = System.Data.SqlDbType.VarChar;
            paramCategoryDescription.Size = 50;
            paramCategoryDescription.Value = category.categoryDescription;

            SqlParameter paramMessage = new SqlParameter();
            paramMessage.ParameterName = "@message";
            paramMessage.Direction = System.Data.ParameterDirection.Output;
            paramMessage.SqlDbType = System.Data.SqlDbType.VarChar;
            paramMessage.Size = 150;
            

            cmd.Parameters.AddRange(new SqlParameter[] { paramEmail, paramCategoryName, paramCategoryDescription, paramMessage});
            cmd.Connection = sqlConnection;
            const string procedureName = "AddCategory";
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.CommandText = procedureName;

            try
            {
                cmd.ExecuteNonQuery();
                if (paramMessage.Value.ToString().Equals("Category was added successfully"))
                    return Ok(paramMessage.Value.ToString());
                else
                {
                    var error = new HttpError("Problem inserting a new category");
                    var httpRespMsg = Request.CreateErrorResponse(HttpStatusCode.NotModified, error);
                    throw new HttpResponseException(httpRespMsg);
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            finally
            {
                SQLHelperClasses.SqlHelper.CloseConnection();
            }
            
        }

        [HttpGet]
        public IHttpActionResult GetCategories(string emailId)
        {
            List<CategoryModels> listOfCategories = new List<CategoryModels>();
            CategoryModels categoryModel = new CategoryModels();
            categoryModel.emailId = emailId;
            try
            {
                if (sqlConnection == null || sqlConnection.State == System.Data.ConnectionState.Closed)
                {
                    sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
                }
                cmd = new SqlCommand();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                cmd.Connection = sqlConnection;
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                const string storedProcedure = "GetCategories";
                cmd.CommandText = storedProcedure;

                SqlParameter paramEmail = new SqlParameter();
                paramEmail.ParameterName = "@EmailId";
                paramEmail.Direction = System.Data.ParameterDirection.Input;
                paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
                paramEmail.Size = 50;
                paramEmail.Value = categoryModel.emailId;

                cmd.Parameters.AddRange(new SqlParameter[] { paramEmail });
                da.Fill(dt);
                int rows = dt.Rows.Count;
                if (rows > 0)
                {
                    for (int i = 0; i < rows; i++)
                    {
                        categoryModel = new CategoryModels();                        
                        categoryModel.categoryDescription = dt.Rows[i]["CategoryDescription"].ToString();
                        categoryModel.categoryName = dt.Rows[i]["CategoryName"].ToString();
                        categoryModel.isActive = (bool)dt.Rows[i]["Active"];
                        listOfCategories.Add(categoryModel);
                    }
                    return Ok(listOfCategories);
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
