using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Reflection.Metadata;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace PhoneBook
{
    internal class Validation
    {
        const string PhoneNumberRegex = @"^\d{3}-\d{3}-\d{4}$";
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

        public static string PhoneNumber() 
        { 
            Regex rx = new Regex(PhoneNumberRegex);
            Console.WriteLine("Please enter a valid phone number formatted as ###-###-#### or leave blank");
            string? phone = Console.ReadLine();
            MailAddress mailAddress;
            while (!string.IsNullOrEmpty(phone) && !rx.IsMatch(phone))
            {
                Console.WriteLine("Invalid format, try again or leave blank");
                phone = Console.ReadLine();
            }
            if (string.IsNullOrEmpty(phone)) phone = "N/A";
            return phone;

        }

        public static Category Category(List<Category> categories, Query query)
        {
            var newCategory = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select a category:")
                    .PageSize(12)
                    .AddChoices(categories.Select(c => c.Name))
                    .AddChoices(new[] { "New Category", "Leave Blank" })
                    );
            if (newCategory == "Leave Blank")
            {
                return new Category { Id = -1 };
            }
            else if (newCategory == "New category")
            {
                Console.WriteLine("Enter new category name");
                string? newCategoryEntry = Console.ReadLine();
                while (string.IsNullOrEmpty(newCategoryEntry) || categories.Exists(category => category.Name == newCategoryEntry))
                {
                    if (string.IsNullOrEmpty(newCategoryEntry))
                        Console.WriteLine("Cannot be blank");
                    else
                        Console.WriteLine($"\"{newCategoryEntry}\" already exists, please try again");
                    newCategoryEntry = Console.ReadLine();
                }

                return query.AddCategory(newCategoryEntry);
            }
            else
            {

                return categories.Find(c => c.Name == newCategory);

            }
        }
    }
}
