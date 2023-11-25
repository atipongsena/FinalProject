using FinalProject.Areas.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;

namespace FinalProject.Pages
{
    public class IndexModel : PageModel
    {

        public List<EmailInfo> listEmails = new List<EmailInfo>();

        private readonly ILogger<IndexModel> _logger;

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            _logger.LogInformation("OnGet called for user: {username}", User.Identity.Name);

            if (string.IsNullOrEmpty(User.Identity.Name))
            {
                _logger.LogWarning("User.Identity.Name is null or empty.");
                return;
            }

            try
            {
                String connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    string username = User.Identity.Name;

                    String sql = "SELECT * FROM emails WHERE emailreceiver = @username";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@username", username);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (!reader.HasRows)
                            {
                                _logger.LogInformation("No emails found for user: {username}", username);
                                return;
                            }

                            while (reader.Read())
                            {
                                EmailInfo emailInfo = new EmailInfo();
                                emailInfo.emailid = "" + reader.GetInt32(0);
                                emailInfo.emailsubject = reader.GetString(1);
                                emailInfo.emailmessage = reader.GetString(2);
                                emailInfo.emaildate = reader.GetDateTime(3).ToString();
                                emailInfo.emailisread = "" + reader.GetInt32(4);
                                emailInfo.emailsender = reader.GetString(5);
                                emailInfo.emailreceiver = reader.GetString(6);

                                listEmails.Add(emailInfo);

                            }
                            _logger.LogInformation("{count} emails retrieved for user: {username}", listEmails.Count, username);
                        }
                    }
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                _logger.LogError(ex, "Exception occurred while retrieving emails for user: {username}", User.Identity.Name);
            }
        }
        
        public IActionResult OnPostDeleteEmail(int emailid)
        {
            try
            {
                var connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    var sql = "DELETE FROM emails WHERE emailid = @EmailId";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@EmailId", emailid);
                        command.ExecuteNonQuery();
                    }
                }

                return RedirectToPage();
            }
            catch (Exception ex)
            {

                return Page();
            }
        }
    }
    public class EmailInfo
    {
        public String emailid;
        public String emailsubject;
        public String emailmessage;
        public String emaildate;
        public String emailisread;
        public String emailsender;
        public String emailreceiver;
    }
}



