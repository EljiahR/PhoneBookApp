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
        private const string _ViewAllContacts = "View All Contacts";
        private const string _ViewAllCategories = "Filter Contacts By Category";
        private const string _AddNewContact = "Add New Contact";
        private const string _AddNewCategory = "Add New Category";
        private const string _EditDeleteContact = "Edit/Delete Contact";
        private const string _EditDeleteCategory = "Edit/Delete Category";
        private const string _ExitMenu = "Exit Program";

        private readonly static List<string> _mainOptions = new()
        {
            _ViewAllContacts, _ViewAllCategories, _AddNewContact, _AddNewCategory
            , _EditDeleteContact, _EditDeleteCategory, _ExitMenu
        };
        public static void MainMenu()
        {
            string? option;
            do
            {
                option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Main Menu")
                    .PageSize(_mainOptions.Count)
                    .AddChoices(_mainOptions));

                switch(option)
                {
                    case _ViewAllContacts:
                        ContactsMenu();
                        break;
                    case _ViewAllCategories:
                        CategoriesMenu();
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

        public static void ContactsMenu()
        {
            throw new NotImplementedException();
        }

        public static void CategoriesMenu()
        {
            throw new NotImplementedException();
        }

        public static void AddContactMenu()
        {
            throw new NotImplementedException();
        }

        public static void AddCategoryMenu()
        {
            throw new NotImplementedException();
        }

        public static void EditContactsMenu()
        {
            throw new NotImplementedException();
        }

        public static void EditCategoriesMenu()
        {
            throw new NotImplementedException();
        }
    }
}
