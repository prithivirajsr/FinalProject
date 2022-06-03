using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FinalProject.Data;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FinalProject.Models;
using PagedList.Mvc;
using PagedList;
using System.IO;
using OfficeOpenXml;



namespace FinalProject.Controllers
{
    public class MemberController : Controller
    {
        public static string config = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        SqlConnection connection = new SqlConnection(config);
        int rowAffected;

        MembersData members = new MembersData();
        // GET: Member
        public ActionResult AllMembers(string searchBy, string search, int? i)
        {
            List<MemberModel> searchMemberList = new List<MemberModel>();
            var membersList = members.GetAllMembers();
            if (searchBy != null && searchBy == "Name")
            {
                string query = "select * from T_Members where FullName=@search";
                using(SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", search);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    connection.Open();
                    foreach (DataRow dr in dt.Rows)
                    {
                        searchMemberList.Add(new MemberModel
                        {
                            MemberId = Convert.ToInt16(dr["MemberId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            City = Convert.ToString(dr["City"]),
                            Pincode = Convert.ToInt32(dr["Pincode"]),
                            Email = Convert.ToString(dr["Email"])
                        });
                    }
                    ViewBag.MembersCount = searchMemberList.Count;
                    connection.Close();
                }
                return View(searchMemberList.ToPagedList(i ?? 1, 3));
            }
            else if(searchBy != null && searchBy == "Email")
            {
                string query = "select * from T_Members where Email=@search";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", search);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    connection.Open();
                    foreach (DataRow dr in dt.Rows)
                    {
                        searchMemberList.Add(new MemberModel
                        {
                            MemberId = Convert.ToInt16(dr["MemberId"]),
                            FullName = Convert.ToString(dr["FullName"]),
                            Profession = Convert.ToString(dr["Profession"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]),
                            MobileNo = Convert.ToString(dr["MobileNo"]),
                            City = Convert.ToString(dr["City"]),
                            Pincode = Convert.ToInt32(dr["Pincode"]),
                            Email = Convert.ToString(dr["Email"])
                        });
                    }
                    ViewBag.MembersCount = searchMemberList.Count;
                    connection.Close();
                }
                return View(searchMemberList.ToPagedList(i ?? 1, 3));
            }
            else
            {
                ViewBag.MembersCount = membersList.Count;
                TempData.Keep("loggedInUserName");
            }
            return View(membersList.ToPagedList(i ?? 1, 3));
        }


        // GET: Member/Details/5
        public ActionResult MemberDetails(int id)
        {
            MemberModel model = new MemberModel();
            string query = "select * from T_Members where MemberId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.MemberId = Convert.ToInt16(dr["MemberId"]);
                    model.FullName = Convert.ToString(dr["FullName"]);
                    model.Profession = Convert.ToString(dr["Profession"]);
                    model.Gender = Convert.ToString(dr["Gender"]);
                    model.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    model.MobileNo = Convert.ToString(dr["MobileNo"]);
                    model.City = Convert.ToString(dr["City"]);
                    model.Pincode = Convert.ToInt32(dr["Pincode"]);
                    model.Email = Convert.ToString(dr["Email"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // GET: Member/Create
        public ActionResult AddMember()
        {
            return View();
        }

        // POST: Member/Create
        [HttpPost]
        public ActionResult AddMember(MemberModel member)
        {
            if (ModelState.IsValid)
            {
                if (!IsMemberExist(member.Email))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertMemberData", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@fullname", member.FullName);
                        command.Parameters.AddWithValue("@profession", member.Profession);
                        command.Parameters.AddWithValue("@gender", member.Gender);
                        command.Parameters.AddWithValue("@dateofbirth", member.DateOfBirth);
                        command.Parameters.AddWithValue("@mobileno", member.MobileNo);
                        command.Parameters.AddWithValue("@city", member.City);
                        command.Parameters.AddWithValue("@pincode", member.Pincode);
                        command.Parameters.AddWithValue("@email", member.Email);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            TempData["Success"] = "Member Successfully Added";
                            return RedirectToAction("AllMembers", "Member");
                        }
                        else
                        {
                            TempData["Error"] = "Something went wrong please try again later";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Member Already Exists with Email: " + member.Email;
                }
            }
            return View();
        }

        // GET: Member/Edit/5
        public ActionResult EditMember(int id)
        {
            MemberModel model = new MemberModel();
            string query = "select * from T_Members where MemberId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.MemberId = Convert.ToInt16(dr["MemberId"]);
                    model.FullName = Convert.ToString(dr["FullName"]);
                    model.Profession = Convert.ToString(dr["Profession"]);
                    model.Gender = Convert.ToString(dr["Gender"]);
                    model.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    model.MobileNo = Convert.ToString(dr["MobileNo"]);
                    model.City = Convert.ToString(dr["City"]);
                    model.Pincode = Convert.ToInt32(dr["Pincode"]);
                    model.Email = Convert.ToString(dr["Email"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // POST: Member/Edit/5
        [HttpPost]
        public ActionResult EditMember(int id,MemberModel model)
        {
            string query = "update T_Members set FullName=@fullname,Profession=@profession," +
                "Gender=@gender,DateOfBirth=@dateofbirth,MobileNo=@mobileno,City=@city,Pincode=@pincode," +
                "Email=@email where MemberId=@id";
            using (SqlCommand command = new SqlCommand(query,connection))
            {
                command.Parameters.AddWithValue("@fullname", model.FullName);
                command.Parameters.AddWithValue("@profession", model.Profession);
                command.Parameters.AddWithValue("@gender", model.Gender);
                command.Parameters.AddWithValue("@dateofbirth", model.DateOfBirth);
                command.Parameters.AddWithValue("@mobileno", model.MobileNo);
                command.Parameters.AddWithValue("@city", model.City);
                command.Parameters.AddWithValue("@pincode", model.Pincode);
                command.Parameters.AddWithValue("@email", model.Email);
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if(rowAffected > 0)
                {
                    TempData["Success"] = "Details Successfully Updated";
                    return RedirectToAction("AllMembers", "Member");
                }
                else
                {
                    TempData["Error"] = "Something went wrong please check your details";
                }
                return View();
            }
        }

        // GET: Member/Delete/5
        public ActionResult DeleteMember(int id)
        {
            MemberModel model = new MemberModel();
            string query = "select * from T_Members where MemberId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.MemberId = Convert.ToInt16(dr["MemberId"]);
                    model.FullName = Convert.ToString(dr["FullName"]);
                    model.Profession = Convert.ToString(dr["Profession"]);
                    model.Gender = Convert.ToString(dr["Gender"]);
                    model.DateOfBirth = Convert.ToDateTime(dr["DateOfBirth"]);
                    model.MobileNo = Convert.ToString(dr["MobileNo"]);
                    model.City = Convert.ToString(dr["City"]);
                    model.Pincode = Convert.ToInt32(dr["Pincode"]);
                    model.Email = Convert.ToString(dr["Email"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // POST: Member/Delete/5
        [HttpPost]
        public ActionResult DeleteMember(int id,MemberModel member)
        {
            deleteMemberFromEventTable(id);
                string query = "delete from T_Members where MemberId=@id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    rowAffected = command.ExecuteNonQuery();
                    if (rowAffected > 0)
                    {
                        TempData["Success"] = "Member: " + member.FullName + " Successfully deleted";
                        return RedirectToAction("AllMembers", "Member");
                    }
                    else
                    {
                        TempData["Error"] = "something wrong Member not deleted";
                    }
                }
                connection.Close();
            return View();
        }

        public void deleteMemberFromEventTable(int id)
        {
            string query = "delete from T_Events where MemberId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", id);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
            }
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

        public ActionResult ExportAsExcel()
        {
            string query = "select * from T_Members";
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(query, connection);
            sda.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                string filepath = Server.MapPath("~/Content/ExportedMemberData.xlsx");
                FileInfo files = new FileInfo(filepath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage(files);
                var sheetCreate = excel.Workbook.Worksheets.Add("EventsData");
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    sheetCreate.Cells[1, i + 1].Value = dt.Columns[i].ColumnName.ToString();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        sheetCreate.Cells[i + 2, j + 1].Value = dt.Rows[i][j].ToString();
                    }
                }
                excel.Save();
                TempData["Success"] = "Memeber Data Successfully Exported To Excel";
                return RedirectToAction("AllMembers", "Member");
            }
            else
            {
                TempData["Error"] = "Something went wrong please try again";
            }
            return RedirectToAction("AllMembers", "Member");
        }
       
    }
}
