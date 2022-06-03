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
    public class MembersData
    {
        public static string config = ConfigurationManager.ConnectionStrings["cs"].ConnectionString;

        public List<MemberModel> GetAllMembers()
        {
            List<MemberModel> membersList = new List<MemberModel>();
            using(SqlConnection connection = new SqlConnection(config))
            {
                SqlCommand command = new SqlCommand("SP_GetAllMembers",connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter sda = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                sda.Fill(dt);
                connection.Open();
                foreach(DataRow dr in dt.Rows)
                {
                    membersList.Add(new MemberModel
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
                connection.Close();
            }
            return membersList;
        }
    }
}