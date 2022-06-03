using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using FinalProject.Models;

namespace FinalProject.Data
{
    public class EventsData
    {
        public static string config = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public List<EventModel> GetAllEvents()
        {
            List<EventModel> eventsList = new List<EventModel>();
            using (SqlConnection connection = new SqlConnection(config))
            {
                SqlCommand command = new SqlCommand("SP_GetAllEvents", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach (DataRow dr in dt.Rows)
                {
                    eventsList.Add(new EventModel()
                    {
                        EventId = Convert.ToInt16(dr["EventId"]),
                        MemberId = Convert.ToInt16(dr["MemberId"]),
                        MemberName = Convert.ToString(dr["MemberName"]),
                        Venue = Convert.ToString(dr["Venue"]),
                        Date = Convert.ToDateTime(dr["Date"]),
                        ECO = Convert.ToString(dr["ECO"]),
                        GDC = Convert.ToString(dr["GDC"]),
                        NSS = Convert.ToString(dr["NSS"])
                    }) ;
                }
                connection.Close();
            }
            return eventsList;
        }
    }
}