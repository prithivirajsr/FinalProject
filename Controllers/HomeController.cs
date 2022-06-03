using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Models;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FinalProject.Controllers
{
    public class HomeController : Controller
    {
        public static string config = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        SqlConnection connection = new SqlConnection(config);
        int rowAffected,Attended,NotAttended;
        string loggedInUserName;

        public ActionResult Signup()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Signup(UserModel user)
        {
            if (ModelState.IsValid)
            {
                if (!IsUserExist(user.Email))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertUserData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@name", user.Name);
                        command.Parameters.AddWithValue("@dateofbirth", user.DateOfBirth);
                        command.Parameters.AddWithValue("@mobileno", user.MobileNo);
                        command.Parameters.AddWithValue("@email", user.Email);
                        command.Parameters.AddWithValue("@password", user.Password);
                        command.Parameters.AddWithValue("@confirmpassword", user.ConfirmPassword);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                        if(rowAffected > 0)
                        {
                            TempData["Success"] = "User Successfully Created";
                            return RedirectToAction("Signin", "Home");
                        }
                        else
                        {
                            TempData["Error"] = "Something went wrong please try again later";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "User Already Exists with Email: " + user.Email;
                }
            }
            return View();
        }
        public ActionResult Signin()
        {
            return View();
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Signin(SigninModel signinData)
        {
            if(ModelState.IsValid)
            {
                if (IsValidUser(signinData.Email, signinData.Password))
                {
                    TempData["Success"] = "Signin Successfully Done";
                    return RedirectToAction("AllMembers", "Member");
                }
                else
                {
                    TempData["Error"] = "User not found, Please signup first";
                }
            }
            return View();
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost,ValidateAntiForgeryToken]
        public ActionResult ForgotPassword(ForgotPasswordModel forgotPasswordData)
        {
            if (ModelState.IsValid)
            {
                if (IsUserExist(forgotPasswordData.Email))
                {
                    string query = "update T_Users set Password=@newpassword," +
                        "ConfirmPassword=@newpassword where Email=@email";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@email",forgotPasswordData.Email);
                        command.Parameters.AddWithValue("@newpassword", forgotPasswordData.NewPassword);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                        connection.Close();
                        if(rowAffected > 0)
                        {
                            TempData["Success"] = "Password Successfully Reset";
                            return RedirectToAction("Signin", "Home");
                        }
                        else
                        {
                            TempData["Error"] = "Something went wrong please try again later";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "User not found, Please signup first";
                }
            }
            return View();
        }

        

        public ActionResult Report()
        {
            string query = "select COUNT(*) from T_Events where ECO=@attended";
            using(SqlCommand command = new SqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@attended", "Attended");
                connection.Open();
                Attended = (int)command.ExecuteScalar();
                ViewBag.ECOAttended = Attended;
                connection.Close();
            }
            string query1 = "select COUNT(*) from T_Events where GDC=@attended";
            using (SqlCommand command = new SqlCommand(query1, connection))
            {
                command.Parameters.AddWithValue("@attended", "Attended");
                connection.Open();
                Attended = (int)command.ExecuteScalar();
                ViewBag.GDCAttended = Attended;
                connection.Close();
            }
            string query2 = "select COUNT(*) from T_Events where NSS=@attended";
            using (SqlCommand command = new SqlCommand(query2, connection))
            {
                command.Parameters.AddWithValue("@attended", "Attended");
                connection.Open();
                Attended = (int)command.ExecuteScalar();
                ViewBag.NSSAttended = Attended;
                connection.Close();
            }
            string query3 = "select COUNT(*) from T_Events where ECO=@notattended";
            using (SqlCommand command = new SqlCommand(query3, connection))
            {
                command.Parameters.AddWithValue("@notattended", "Not Attended");
                connection.Open();
                NotAttended = (int)command.ExecuteScalar();
                ViewBag.ECONotAttended = NotAttended;
                connection.Close();
            }
            string query4 = "select COUNT(*) from T_Events where GDC=@notattended";
            using (SqlCommand command = new SqlCommand(query4, connection))
            {
                command.Parameters.AddWithValue("@notattended", "Not Attended");
                connection.Open();
                NotAttended = (int)command.ExecuteScalar();
                ViewBag.GDCNotAttended = NotAttended;
                connection.Close();
            }
            string query5 = "select COUNT(*) from T_Events where NSS=@notattended";
            using (SqlCommand command = new SqlCommand(query5, connection))
            {
                command.Parameters.AddWithValue("@notattended", "Not Attended");
                connection.Open();
                NotAttended = (int)command.ExecuteScalar();
                ViewBag.NSSNotAttended = NotAttended;
                connection.Close();
            }
            return View();
        }

        private bool IsUserExist(string email)
        {
            bool IsUserExist = false;
            string query = "select * from T_Users where Email = @email";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email",email);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if(dt.Rows.Count > 0)
                {
                    IsUserExist = true;
                }
            }
            return IsUserExist;
        }

        private bool IsValidUser(string email,string password)
        {
            bool IsValidUser = false;
            string query = "select * from T_Users where Email = @email and Password = @password";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                command.Parameters.AddWithValue("@password", password);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if(dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    loggedInUserName = row.ItemArray[1].ToString();
                    TempData["loggedInUserName"] = loggedInUserName;
                    IsValidUser = true;
                }
            }
            return IsValidUser;
        }

        private bool IsMemberExist(string email)
        {
            bool IsMemberExist = false;
            string query = "select * from T_Members where Email = @email";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@email", email);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if (dt.Rows.Count > 0)
                {
                    IsMemberExist = true;
                }
            }
            return IsMemberExist;
        }
    }
}