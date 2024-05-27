using Spectre.Console;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhoneBook
{
    internal class Menu
    {
        private Query query { get; set; }

        public Menu(PhoneBookContext context)
        {
            query = new Query(context);
        }
        
        private const string _ViewAllContacts = "View All Contacts";
        private const string _ViewAllCategories = "Filter Contacts By Category";
        private const string _AddNewContact = "Add New Contact";
        private const string _AddNewCategory = "Add New Category";
        private const string _EditDeleteContact = "Edit/Delete Contact";
        private const string _EditDeleteCategory = "Edit/Delete Category";
        private const string _ExitMenu = "Exit Program";

        private readonly List<string> _mainOptions = new()
        {
            _ViewAllContacts, _ViewAllCategories, _AddNewContact, _AddNewCategory
            , _EditDeleteContact, _EditDeleteCategory, _ExitMenu
        };
        public void MainMenu()
        {
            string? option;
            do
            {
                Console.Clear();
                option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(_mainOptions.Count)
                    .AddChoices(_mainOptions));

                switch(option)
                {
                    case _ViewAllContacts:
                        ViewContactsMenu();
                        break;
                    case _ViewAllCategories:
                        ViewCategoriesMenu();
                        break;
                    case _AddNewContact:
                        AddContactMenu();
                        break;
                    case _AddNewCategory:
                        AddCategoryMenu();
                        break;
                    case _EditDeleteContact:
                        EditContactsMenu();
                        break;
                    case _EditDeleteCategory:
                        EditCategoriesMenu();
                        break;
                }

            } while (option != _ExitMenu);

            Console.WriteLine("Goodbye!");
        }

        private void ViewContactsMenu()
        {
            throw new NotImplementedException();
        }

        private void ViewCategoriesMenu()
        {
            var categories = query.AllCategories();
            foreach (var category in categories)
            {
                Console.WriteLine(category.Name);
            }

            Console.WriteLine("Press Enter to go back"); // Needs replaced to show contacts by category, not just categories
            Console.ReadLine();
        }

        private void AddContactMenu()
        {
            throw new NotImplementedException();
        }

        private void AddCategoryMenu()
        {
            var categories = query.AllCategories();
            Console.Clear();
            Console.WriteLine("Enter new category name or leave blank to return to main menu:");
            string? newCategory = Console.ReadLine();
            while(!string.IsNullOrEmpty(newCategory) && categories.Exists(category => category.Name == newCategory))
            {
                Console.WriteLine($"\"{newCategory}\" already exists, please try again or leave black to return to main menu:");
                newCategory = Console.ReadLine();
            }
            if(!string.IsNullOrEmpty(newCategory))
            {
                Console.Clear();
                query.AddCategory(newCategory);
                Console.WriteLine($"\"{newCategory}\" added successfully\n");
                Console.WriteLine("Press Enter to return to main menu");
                Console.ReadLine();
            }
        }

        private void EditContactsMenu()
        {
            throw new NotImplementedException();
        }

        private void EditCategoriesMenu()
        {
            throw new NotImplementedException();
        }
    }
}
