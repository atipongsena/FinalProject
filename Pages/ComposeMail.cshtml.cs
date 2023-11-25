using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Identity;
using FinalProject.Areas.Identity.Data;

namespace FinalProject.Pages
{
    public class ComposeEmailModel : PageModel
    {
        public EmailFormModel Email = new EmailFormModel();
        public string ErrorMessage = "";
        public string SuccessMessage = "";

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            Email.EmailReceiver = Request.Form["emailreceiver"];
            Email.EmailSubject = Request.Form["emailsubject"];
            Email.EmailMessage = Request.Form["emailmessage"];
            Email.EmailSender = User.Identity.Name;

            if (string.IsNullOrEmpty(Email.EmailReceiver) || string.IsNullOrEmpty(Email.EmailSubject) || string.IsNullOrEmpty(Email.EmailMessage))
            {
                ErrorMessage = "All fields are required.";
                return Page();
            }
            var emailSender = User.Identity.Name;
            if (string.IsNullOrEmpty(emailSender))
            {
                ErrorMessage = "The sender's email address cannot be determined.";
                return Page();
            }


            try
            {
                string connectionString = "Data Source=(localdb)\\ProjectModels;Initial Catalog=master;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";
                using (var connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    string sql = "INSERT INTO emails (emailreceiver, emailsubject, emailmessage, emailsender) VALUES (@Receiver, @Subject, @Message, @Sender)";
                    using (var command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@Receiver", Email.EmailReceiver);
                        command.Parameters.AddWithValue("@Subject", Email.EmailSubject);
                        command.Parameters.AddWithValue("@Message", Email.EmailMessage);
                        command.Parameters.AddWithValue("@Sender", Email.EmailSender);
                        command.ExecuteNonQuery();
                    }
                }

                SuccessMessage = "Email sent successfully.";
                return RedirectToPage("EmailSent");
            }
            catch (Exception ex)
            {
                ErrorMessage = "Error sending email: " + ex.Message;
                return Page();
            }
        }
    }

    public class EmailFormModel
    {
        public string EmailReceiver { get; set; }
        public string EmailSubject { get; set; }
        public string EmailMessage { get; set; }
        public string EmailSender { get; set; }
    }
}
