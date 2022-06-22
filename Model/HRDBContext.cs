using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HRCentralDataToSharePoint
{
    public class HRDBContext : DbContext
    {
        public HRDBContext()
            : base("name=HRDBContext")
        {
        }
    }


    public class Employee
    {
        public Employee()
        { }

        private int id;
        private string pid;
        private string ipin;
        private string cwid;
        private string bi_last_name;
        private string bi_first_name;
        private string el_last_name;
        private string el_first_name;
        private string gender;
        private string employee_status;
        private string company_code;
        private string org;
        private string org_text;
        private string position;
        private string position_text;
        private string cost_center;
        private string employee_group;
        private string staff_type;
        private string personal_area;
        private string personal_area_text;
        private string business_address;
        private string working_location;
        private string company_entry_date;
        private string group_entry_date;
        private string resource_manager;
        private string admin_manager;
        private string leaving_date;
        private string vendor_code;
        private string action;
        private string email_address;
        private string last_modify_date;
        private string del_flag;

        public int Id { get => id; set => id = value; }
        public string Pid { get => pid; set => pid = value; }
        public string Ipin { get => ipin; set => ipin = value; }
        public string Cwid { get => cwid; set => cwid = value; }
        public string Bi_last_name { get => bi_last_name; set => bi_last_name = value; }
        public string Bi_first_name { get => bi_first_name; set => bi_first_name = value; }
        public string El_last_name { get => el_last_name; set => el_last_name = value; }
        public string El_first_name { get => el_first_name; set => el_first_name = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Employee_status { get => employee_status; set => employee_status = value; }
        public string Company_code { get => company_code; set => company_code = value; }
        public string Org { get => org; set => org = value; }
        public string Org_text { get => org_text; set => org_text = value; }
        public string Position { get => position; set => position = value; }
        public string Position_text { get => position_text; set => position_text = value; }
        public string Cost_center { get => cost_center; set => cost_center = value; }
        public string Employee_group { get => employee_group; set => employee_group = value; }
        public string Staff_type { get => staff_type; set => staff_type = value; }
        public string Personal_area { get => personal_area; set => personal_area = value; }
        public string Personal_area_text { get => personal_area_text; set => personal_area_text = value; }
        public string Business_address { get => business_address; set => business_address = value; }
        public string Working_location { get => working_location; set => working_location = value; }
        public string Company_entry_date { get => company_entry_date; set => company_entry_date = value; }
        public string Group_entry_date { get => group_entry_date; set => group_entry_date = value; }
        public string Resource_manager { get => resource_manager; set => resource_manager = value; }
        public string Admin_manager { get => admin_manager; set => admin_manager = value; }
        public string Leaving_date { get => leaving_date; set => leaving_date = value; }
        public string Vendor_code { get => vendor_code; set => vendor_code = value; }
        public string Action { get => action; set => action = value; }
        public string Email_address { get => email_address; set => email_address = value; }
        public string Last_modify_date { get => last_modify_date; set => last_modify_date = value; }
        public string Del_flag { get => del_flag; set => del_flag = value; }
        public int Id2 { get => id; set => id = value; }
    }

}
