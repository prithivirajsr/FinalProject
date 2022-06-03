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
    public class EventController : Controller
    {
        public static string config = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;
        SqlConnection connection = new SqlConnection(config);
        int rowAffected;

        EventsData events = new EventsData();
        // GET: Event
        public ActionResult AllEvents(string searchBy, string search, int? i)
        {
            List<EventModel> searchEventList = new List<EventModel>();
            var eventList = events.GetAllEvents();
            if (searchBy != null && searchBy == "Name")
            {
                string query = "select * from T_Events where MemberName=@search";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", search);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    connection.Open();
                    foreach (DataRow dr in dt.Rows)
                    {
                        searchEventList.Add(new EventModel
                        {
                            EventId = Convert.ToInt16(dr["EventId"]),
                            MemberId = Convert.ToInt16(dr["MemberId"]),
                            MemberName = Convert.ToString(dr["MemberName"]),
                            Venue = Convert.ToString(dr["Venue"]),
                            Date = Convert.ToDateTime(dr["Date"]),
                            ECO = Convert.ToString(dr["ECO"]),
                            GDC = Convert.ToString(dr["GDC"]),
                            NSS = Convert.ToString(dr["NSS"])
                        });
                    }
                    ViewBag.EventsCount = searchEventList.Count;
                    connection.Close();
                }
                return View(searchEventList.ToPagedList(i ?? 1, 3));
            }
            else if (searchBy != null && searchBy == "Venue")
            {
                string query = "select * from T_Events where Venue=@search";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@search", search);
                    SqlDataAdapter sda = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    sda.Fill(dt);
                    connection.Open();
                    foreach (DataRow dr in dt.Rows)
                    {
                        searchEventList.Add(new EventModel
                        {
                            EventId = Convert.ToInt16(dr["EventId"]),
                            MemberId = Convert.ToInt16(dr["MemberId"]),
                            MemberName = Convert.ToString(dr["MemberName"]),
                            Venue = Convert.ToString(dr["Venue"]),
                            Date = Convert.ToDateTime(dr["Date"]),
                            ECO = Convert.ToString(dr["ECO"]),
                            GDC = Convert.ToString(dr["GDC"]),
                            NSS = Convert.ToString(dr["NSS"])
                        });
                    }
                    ViewBag.EventsCount = searchEventList.Count;
                    connection.Close();
                }
                return View(searchEventList.ToPagedList(i ?? 1, 3));
            }
            else
            {
                ViewBag.EventsCount = eventList.Count;
                TempData.Keep();
            }
            return View(eventList.ToPagedList(i ?? 1, 3));
        }
         
        // GET: Event/Details/5
        public ActionResult EventDetails(int eventid,int memberid)
        {
            EventModel model = new EventModel();
            string query = "select * from T_Events where EventId=@eventid and MemberId=@memberid";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@eventid", eventid);
                command.Parameters.AddWithValue("@memberid", memberid);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.MemberName = Convert.ToString(dr["MemberName"]);
                    model.Venue = Convert.ToString(dr["Venue"]);
                    model.Date = Convert.ToDateTime(dr["Date"]);
                    model.ECO = Convert.ToString(dr["ECO"]);
                    model.GDC = Convert.ToString(dr["GDC"]);
                    model.NSS = Convert.ToString(dr["NSS"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // GET: Event/Create
        public ActionResult AddEvent()
        {
            EventModel member = new EventModel();
            member.SelectMember = populateMembers();
            return View(member);
        }

        // POST: Event/Create
        [HttpPost]
        public ActionResult AddEvent(EventModel events)
        {

            events.SelectMember = populateMembers();
            var selectedItem = events.SelectMember.Find(p => p.Value == events.MemberName.ToString());
            if (selectedItem != null)
            {
                selectedItem.Selected = true;
                Response.Write(selectedItem.Text);
            }
            if (ModelState.IsValid)
            {
                if (!IsEventExist(selectedItem.Text,events.Venue,events.Date))
                {
                    using (SqlCommand command = new SqlCommand("SP_InsertEventData",connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@memberid",selectedItem.Value);
                        command.Parameters.AddWithValue("@membername",selectedItem.Text);
                        command.Parameters.AddWithValue("@venue", events.Venue);
                        command.Parameters.AddWithValue("@date", events.Date);
                        command.Parameters.AddWithValue("@eco", events.ECO);
                        command.Parameters.AddWithValue("@gdc", events.GDC);
                        command.Parameters.AddWithValue("@nss", events.NSS);
                        connection.Open();
                        rowAffected = command.ExecuteNonQuery();
                        if (rowAffected > 0)
                        {
                            TempData["Success"] = "Event Successfully Added";
                            return RedirectToAction("AllEvents", "Event");
                        }
                        else
                        {
                            TempData["Error"] = "Something went wrong please try again later";
                        }
                    }
                }
                else
                {
                    TempData["Error"] = "Event already exist for Venue:" 
                        + events.Venue + " ,Date:" + events.Date + " ,of Member:" + selectedItem.Text;
                }
            }

            return View(events);
        }

        // GET: Event/Edit/5
        public ActionResult EditEvent(int eventid, int memberid)
        {
            EventModel model = new EventModel();
            string query = "select * from T_Events where EventId=@eventid and MemberId=@memberid";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@eventid", eventid);
                command.Parameters.AddWithValue("@memberid", memberid);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.EventId = Convert.ToInt16(dr["EventId"]);
                    model.MemberId = Convert.ToInt16(dr["MemberId"]);
                    model.MemberName = Convert.ToString(dr["MemberName"]);
                    model.Venue = Convert.ToString(dr["Venue"]);
                    model.Date = Convert.ToDateTime(dr["Date"]);
                    model.ECO = Convert.ToString(dr["ECO"]);
                    model.GDC = Convert.ToString(dr["GDC"]);
                    model.NSS = Convert.ToString(dr["NSS"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // POST: Event/Edit/5
        [HttpPost]
        public ActionResult EditEvent(int eventid, int memberid,EventModel events)
        {
            string query = "update T_Events set Venue=@venue,Date=@date," +
                "ECO=@eco,GDC=@gdc,NSS=@nss where EventId=@eventid and MemberId=@memberid";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@venue", events.Venue);
                command.Parameters.AddWithValue("@date", events.Date);
                command.Parameters.AddWithValue("@eco", events.ECO);
                command.Parameters.AddWithValue("@gdc", events.GDC);
                command.Parameters.AddWithValue("@nss", events.NSS);
                command.Parameters.AddWithValue("@eventid", eventid);
                command.Parameters.AddWithValue("@memberid", memberid);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if (rowAffected > 0)
                {
                    TempData["Success"] = "Event Successfully Updated";
                    return RedirectToAction("AllEvents", "Event");
                }
                else
                {
                    TempData["Error"] = "Something went wrong please check your details";
                }
                return View();
            }
        }

        // GET: Event/Delete/5
        public ActionResult DeleteEvent(int eventid)
        {
            EventModel model = new EventModel();
            string query = "select * from T_Events where EventId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", eventid);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    model.EventId = Convert.ToInt16(dr["EventId"]);
                    model.MemberId = Convert.ToInt16(dr["MemberId"]);
                    model.MemberName = Convert.ToString(dr["MemberName"]);
                    model.Venue = Convert.ToString(dr["Venue"]);
                    model.Date = Convert.ToDateTime(dr["Date"]);
                    model.ECO = Convert.ToString(dr["ECO"]);
                    model.GDC = Convert.ToString(dr["GDC"]);
                    model.NSS = Convert.ToString(dr["NSS"]);
                }
                connection.Close();
            }
            return View(model);
        }

        // POST: Event/Delete/5
        [HttpPost]
        public ActionResult DeleteEvent(int eventid,EventModel events)
        {
            string query = "delete from T_Events where EventId=@id";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@id", eventid);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                if (rowAffected > 0)
                {
                    TempData["Success"] = "Event deleted for Member: " + events.MemberName;
                    return RedirectToAction("AllEvents", "Event");
                }
                else
                {
                    TempData["Error"] = "something wrong event not deleted";
                }
            }
            connection.Close();
            return View();
        }

        private List<SelectListItem> populateMembers()
        {
            List<SelectListItem> list = new List<SelectListItem>();
            string query = "select MemberId,FullName from T_Members";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                connection.Open();
                using (SqlDataReader dr = command.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        list.Add(new SelectListItem
                        {
                            Value = dr["MemberId"].ToString(),
                            Text = dr["FullName"].ToString(),

                        });
                    }
                }
                connection.Close();
            }
            return list;
        }

        private bool IsEventExist(string membername,string venue,DateTime date)
        {
            bool IsEventExist = false;
            string query = "select * from T_Events where MemberName=@membername" +
                " and Venue=@venue and Date=@date";
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                command.Parameters.AddWithValue("@membername", membername);
                command.Parameters.AddWithValue("@venue", venue);
                command.Parameters.AddWithValue("@date", date);
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                rowAffected = command.ExecuteNonQuery();
                connection.Close();
                if (dt.Rows.Count > 0)
                {
                    IsEventExist = true;
                }
            }
            return IsEventExist;
        }

        public ActionResult ExportAsExcel()
        {
            string query = "select * from T_Events";
            DataTable dt = new DataTable();
            SqlDataAdapter sda = new SqlDataAdapter(query, connection);
            sda.Fill(dt);
            if(dt.Rows.Count > 0)
            {
                string filepath = Server.MapPath("~/Content/ExportedEventData.xlsx");
                FileInfo files = new FileInfo(filepath);
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                ExcelPackage excel = new ExcelPackage(files);
                var sheetCreate = excel.Workbook.Worksheets.Add("EventsData");
                for(int i=0; i < dt.Columns.Count; i++)
                {
                    sheetCreate.Cells[1, i + 1].Value = dt.Columns[i].ColumnName.ToString();
                }
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    for(int j=0;j<dt.Columns.Count; j++)
                    {
                        sheetCreate.Cells[i+2,j+1].Value = dt.Rows[i][j].ToString();    
                    }
                }
                excel.Save();
                TempData["Success"] = "Event Data Successfully Exported To Excel";
                return RedirectToAction("AllEvents", "Event");
            }
            else
            {
                TempData["Error"] = "Something went wrong please try again";
            }
            return RedirectToAction("AllEvents","Event");
        }
    }
}
