using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using TechBlogWebAPI.Models;

namespace TechBlogWebAPI.Controllers
{

    [EnableCors(origins: "http://localhost:63779", headers: "*", methods: "*")]
    public class ProfileController : ApiController
    {
       
        
        ProfileModels profileModel = null;
        [HttpPost]
        public IHttpActionResult InsertProfile()
		{
            profileModel = updateModel();
           SqlConnection sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
           SqlCommand cmd = new SqlCommand();
            cmd.Connection = sqlConnection;
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            const string storedProcedureName = "InsertProfileDetails";
            cmd.CommandText = storedProcedureName;
            cmd.Parameters.Clear();

            SqlParameter paramFullName = new SqlParameter();
            SqlParameter paramDOB = new SqlParameter();
            SqlParameter paramOrganization = new SqlParameter();
            SqlParameter paramCity = new SqlParameter();
            SqlParameter paramCountry = new SqlParameter();
            SqlParameter paramMobile = new SqlParameter();
            SqlParameter paramAboutMe = new SqlParameter();
            SqlParameter paramGender = new SqlParameter();
            SqlParameter paramProfilePicture = new SqlParameter();
            SqlParameter paramEmailId = new SqlParameter();
            SqlParameter paramWebsite = new SqlParameter();
            SqlParameter paramSuccess = new SqlParameter();

            string response = string.Empty;

            // Check if the request contains multipart/form-data. 
            if (!Request.Content.IsMimeMultipartContent("form-data"))
            {
                HttpError error = new HttpError("Unsupported Media Type Error");
                var responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotModified, error);
                throw new HttpResponseException(responseMessage);
            }

            try
            {               

                paramFullName.ParameterName = "@Fullname";
                paramFullName.Direction = System.Data.ParameterDirection.Input;
                paramFullName.SqlDbType = System.Data.SqlDbType.VarChar;
                paramFullName.Size = 100;
                paramFullName.Value = profileModel.firstName +" "+profileModel.lastName;
                                
                paramDOB.ParameterName = "@DateOfBirth";
                paramDOB.Direction = System.Data.ParameterDirection.Input;
                paramDOB.SqlDbType = System.Data.SqlDbType.DateTime;
                paramDOB.Value = profileModel.DOB;


                paramOrganization.ParameterName = "@Organisation";
                paramOrganization.Direction = System.Data.ParameterDirection.Input;
                paramOrganization.SqlDbType = System.Data.SqlDbType.VarChar;
                paramOrganization.Value = profileModel.Organization;
                paramOrganization.Size = 50;


                paramCity.ParameterName = "@City";
                paramCity.Direction = System.Data.ParameterDirection.Input;
                paramCity.SqlDbType = System.Data.SqlDbType.VarChar;
                paramCity.Value = profileModel.City;
                paramCity.Size = 50;


                paramCountry.ParameterName = "@Country";
                paramCountry.Direction = System.Data.ParameterDirection.Input;
                paramCountry.SqlDbType = System.Data.SqlDbType.VarChar;
                paramCountry.Value = profileModel.Country;
                paramCountry.Size = 25;

                paramMobile.ParameterName = "@Mobile";
                paramMobile.Direction = System.Data.ParameterDirection.Input;
                paramMobile.SqlDbType = System.Data.SqlDbType.VarChar;
                paramMobile.Value = profileModel.Mobile;
                paramMobile.Size = 15;


                paramAboutMe.ParameterName = "@Aboutme";
                paramAboutMe.Direction = System.Data.ParameterDirection.Input;
                paramAboutMe.SqlDbType = System.Data.SqlDbType.VarChar;
                paramAboutMe.Value = profileModel.AboutMe;
                paramAboutMe.Size = 1000;


                paramGender.ParameterName = "@Gender";
                paramGender.Direction = System.Data.ParameterDirection.Input;
                paramGender.SqlDbType = System.Data.SqlDbType.VarChar;
                paramGender.Value = profileModel.Sex;
                paramGender.Size = 50;

                paramProfilePicture.ParameterName = "@profilePicture";
                paramProfilePicture.Direction = System.Data.ParameterDirection.Input;
                paramProfilePicture.SqlDbType = System.Data.SqlDbType.VarBinary;
                paramProfilePicture.Value = profileModel.ProfilePicture;

                paramEmailId.ParameterName = "@EmailId";
                paramEmailId.Direction = System.Data.ParameterDirection.Input;
                paramEmailId.SqlDbType = System.Data.SqlDbType.VarChar;
                paramEmailId.Value = profileModel.EmailId;
                paramEmailId.Size = 50;

                paramWebsite.ParameterName = "@website";
                paramWebsite.Direction = System.Data.ParameterDirection.Input;
                paramWebsite.SqlDbType = System.Data.SqlDbType.VarChar;
                paramWebsite.Value = profileModel.Website;
                paramWebsite.Size = 50;


                paramSuccess.ParameterName = "@success";
                paramSuccess.Direction = System.Data.ParameterDirection.Output;
                paramSuccess.SqlDbType = System.Data.SqlDbType.VarChar;
                paramSuccess.Size = 1000;


                cmd.Parameters.AddRange(new SqlParameter[] { paramFullName, paramDOB, paramMobile, paramOrganization, paramCountry, paramCity, paramAboutMe, paramGender,
                    paramSuccess, paramProfilePicture, paramWebsite, paramEmailId });

                cmd.ExecuteNonQuery();
                if (paramSuccess.Value.Equals("Profile Details for the user added successfully"))
                    response = paramSuccess.Value.ToString();
                else
                {
                    HttpError error = new HttpError(paramSuccess.Value.ToString());
                    var responseMessage = Request.CreateErrorResponse(HttpStatusCode.NotModified, error);
                    response = responseMessage.ToString();
                }
            }

            catch (Exception exception)
            {
                throw new Exception(HttpStatusCode.BadRequest + " " + exception.ToString());
            }
            finally
            {
                SQLHelperClasses.SqlHelper.CloseConnection();
                sqlConnection.Dispose();
                cmd.Dispose();
            }
            return Ok(response+System.Environment.NewLine+ paramSuccess.ToString());
        }

        private ProfileModels updateModel()
        {
            ProfileModels profilemodel = new ProfileModels();
            string sPath = "";

            //Server File Path
            sPath = System.Web.Hosting.HostingEnvironment.MapPath("~/Uploads/");

            try
            {
                //retreive the image from Request
                System.Web.HttpFileCollection hfc = System.Web.HttpContext.Current.Request.Files;
                HttpRequest request = HttpContext.Current.Request;

                if (hfc.Count > 0)
                {
                    System.Web.HttpPostedFile hpf = hfc[0];

                    if (hpf.ContentLength > 0)
                    {
                        // save the file in the directory
                        hpf.SaveAs(sPath + Path.GetFileName(hpf.FileName));

                        //Read the bytes of the file
                        byte[] imageData = File.ReadAllBytes(sPath + Path.GetFileName(hpf.FileName));

                        //bind to profilePicture model
                        profilemodel.ProfilePicture = imageData;

                        //Finally delete the saved file as we are saving it to the database
                        File.Delete(sPath + Path.GetFileName(hpf.FileName));

                    }
                }
                if (request.Form.Keys.Count > 0)
                {
                    profilemodel.AboutMe = request.Form["AboutMe"];
                    profilemodel.firstName = request.Form["firstName"];
                    profilemodel.lastName = request.Form["lastName"];
                    profilemodel.DOB = request.Form["DOB"];
                    profilemodel.Mobile = request.Form["Mobile"];
                    profilemodel.Organization = request.Form["Organization"];
                    profilemodel.Country = request.Form["Country"];
                    profilemodel.AboutMe = request.Form["AboutMe"];
                    profilemodel.Sex = request.Form["Sex"];
                    profilemodel.EmailId = request.Form["EmailId"];
                    profilemodel.City = request.Form["City"];
                    profilemodel.Website = request.Form["Website"];
                }

            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            return profilemodel;
        }

        [HttpGet]
        public HttpResponseMessage GetProfileImage(string emailId)
        {
            var response = Request.CreateResponse(HttpStatusCode.OK);
            profileModel = new ProfileModels();
            SqlConnection sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
            const string procedureName = "GetProfileImage";
            SqlCommand cmd = new SqlCommand(procedureName, sqlConnection);            
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter paramProfileImage = new SqlParameter();
            SqlParameter paramEmail = new SqlParameter();
            try
            {
                paramProfileImage.ParameterName = "@ProfileImage";
                paramProfileImage.SqlDbType = System.Data.SqlDbType.VarBinary;
                paramProfileImage.Direction = System.Data.ParameterDirection.Output;
                paramProfileImage.Size = 100000;

                paramEmail.ParameterName = "@EmailId";
                paramEmail.Direction = System.Data.ParameterDirection.Input;
                paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
                paramEmail.Value = emailId;
                paramEmail.Size = 50;
                cmd.Parameters.Add(paramEmail);
                cmd.Parameters.Add(paramProfileImage);

                cmd.ExecuteNonQuery();

                if (paramProfileImage.Value != null)
                {
                    profileModel.ProfilePicture = (byte[])paramProfileImage.Value;
                    MemoryStream ms = new MemoryStream(profileModel.ProfilePicture);
                    response.Content = new StreamContent(ms);
                    response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
                }
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
            finally
            {
                cmd.Dispose();                
                sqlConnection.Close();
                sqlConnection.Dispose();
            }
            return response;
        }

        [HttpGet]
        public IHttpActionResult GetProfileDetails(string emailId)
        {
            profileModel = new ProfileModels();
            SqlConnection sqlConnection = SQLHelperClasses.SqlHelper.OpenConnection();
            const string procedureName = "GetProfileDetails";
            SqlCommand cmd = new SqlCommand(procedureName, sqlConnection);
            SqlDataAdapter da = new SqlDataAdapter();
            DataTable dt = new DataTable();
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.Clear();

            SqlParameter paramEmail = new SqlParameter();
            try
            {
                paramEmail.ParameterName = "@EmailId";
                paramEmail.Direction = System.Data.ParameterDirection.Input;
                paramEmail.SqlDbType = System.Data.SqlDbType.VarChar;
                paramEmail.Value = emailId;
                paramEmail.Size = 50;
                cmd.Parameters.Add(paramEmail);
                da.SelectCommand = cmd;
                da.Fill(dt);
                int rows = dt.Rows.Count;
                string firstName = string.Empty;
                string lastName = string.Empty;

                for (int i = 0; i < rows; i++)
                {
                    profileModel.AboutMe = dt.Rows[i]["AboutMe"].ToString();
                    profileModel.City = dt.Rows[i]["City"].ToString();
                    profileModel.Country = dt.Rows[i]["Country"].ToString();
                    profileModel.Sex = dt.Rows[i]["Gender"].ToString();
                    string fullName = dt.Rows[i]["fullName"].ToString();
                    var name = fullName.Split(' ');
                    profileModel.firstName = name[0];
                    profileModel.lastName = name[1];

                    profileModel.Mobile = dt.Rows[i]["Mobile"].ToString();
                    profileModel.Organization = dt.Rows[i]["Organisation"].ToString();
                    profileModel.DOB = dt.Rows[i]["DateOfBirth"].ToString();
                }

                if (rows > 0)
                {
                    return Ok(profileModel);
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



