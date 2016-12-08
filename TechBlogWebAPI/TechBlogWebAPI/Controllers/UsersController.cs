using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using TechBlogWebAPI.Models;
using NanoMessageBus.Serialization;
using Newtonsoft.Json.Linq;

namespace TechBlogWebAPI.Controllers
{
    public class UsersController : ApiController
    {
        SqlConnection sqlConnection = null;
        SqlCommand cmd = null;
       
        [HttpPost]     
        public IHttpActionResult AddUser([FromBody]NewUserModels newUserModel)
        {                    
            sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
               
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;

            SqlParameter paramEmail = new SqlParameter();
            paramEmail.ParameterName = "@emailId";
            paramEmail.Direction = System.Data.ParameterDirection.Input;
            paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
            paramEmail.Value = newUserModel.EmailId;

            SqlParameter paramPassword = new SqlParameter();
            paramPassword.ParameterName = "@password";
            paramPassword.Direction = System.Data.ParameterDirection.Input;
            paramPassword.SqlDbType = System.Data.SqlDbType.VarChar;
            paramPassword.Value = newUserModel.ChoosePassword;           

            SqlParameter paraSuccess = new SqlParameter();
            paraSuccess.ParameterName = "@success";
            paraSuccess.SqlDbType = System.Data.SqlDbType.Bit;
            paraSuccess.Direction = System.Data.ParameterDirection.Output;

            cmd.Parameters.AddRange(new SqlParameter[] { paramEmail, paramPassword, paraSuccess });
            const string storedProcedureName = "NewUser";
            cmd.CommandText = storedProcedureName;
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception exception) 
            {
                throw new Exception(exception.ToString());
            }
            finally
            {
                SQLHelperClasses.SqlHelper.CloseConnection();    
            }
            if (bool.Parse(paraSuccess.Value.ToString()))
                return Ok("User was successfully added");
            else
                return NotFound();
        }

        [HttpGet]      
       public IHttpActionResult IsUserValid(LoginModels loginUser)
        {
            
            cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            const string procName = "AuthenticateUser";
            cmd.CommandText = procName;

            SqlParameter paramEmail = new SqlParameter("@username", System.Data.SqlDbType.VarChar);
            paramEmail.Direction = System.Data.ParameterDirection.Input;
            paramEmail.Value = loginUser.EmailId;
           
            SqlParameter paramPassword = new SqlParameter("@password", System.Data.SqlDbType.VarChar);
            paramPassword.Direction = System.Data.ParameterDirection.Input;
            paramPassword.Value = loginUser.Password;

            SqlParameter paramisUserValid = new SqlParameter("@isUserValid", System.Data.SqlDbType.Bit);
            paramisUserValid.Direction = System.Data.ParameterDirection.Output;
            cmd.Parameters.Add(paramEmail);
            cmd.Parameters.Add(paramPassword);
            cmd.Parameters.Add(paramisUserValid);
            cmd.ExecuteNonQuery();

            if (bool.Parse(paramisUserValid.Value.ToString()))
                return Ok("User is valid");
            return NotFound();
            
        }                  
    }
}
