using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    internal class Validation
    {
        
        public static string Email()
        {
            Console.WriteLine("Please enter a valid email address or leave blank");
            string? email = Console.ReadLine();
            MailAddress mailAddress;
            while(!string.IsNullOrEmpty(email) && !MailAddress.TryCreate(email, out mailAddress))
            {
                Console.WriteLine("Invalid email, try again or leave blank");
                email = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(email)) email = "N/A";
            return email;
        }
    }
}
