using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System;

namespace FinalProject.Pages
{
    public class ReadEmailModel : PageModel
    {
        public EmailInfo Email { get; set; }

        public void OnGet(int emailid)
        {
            Email = new EmailInfo();

            try
            {
                String connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    
                    String updateSql = "UPDATE emails SET emailisread = 1 WHERE emailid = @EmailId";
                    using (SqlCommand updateCommand = new SqlCommand(updateSql, connection))
                    {
                        updateCommand.Parameters.AddWithValue("@EmailId", emailid);
                        updateCommand.ExecuteNonQuery();
                    }

                   
                    String selectSql = "SELECT * FROM emails WHERE emailid = @EmailId";
                    using (SqlCommand selectCommand = new SqlCommand(selectSql, connection))
                    {
                        selectCommand.Parameters.AddWithValue("@EmailId", emailid);
                        using (SqlDataReader reader = selectCommand.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Email.emailid = reader.GetInt32(0).ToString();
                                Email.emailsubject = reader.GetString(1);
                                Email.emailmessage = reader.GetString(2);
                                Email.emaildate = reader.GetDateTime(3).ToString();
                                Email.emailisread = "" + reader.GetInt32(4);
                                Email.emailsender = reader.GetString(5);
                                Email.emailreceiver = reader.GetString(6);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            
            }
        }
    }
    }