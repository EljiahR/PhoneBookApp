using Spectre.Console;

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

                switch (option)
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
            var contacts = query.AllContacts();
            foreach (var contact in contacts)
            {
                Console.WriteLine(contact.Name);
                Console.WriteLine($"\t{contact.Email}");
                Console.WriteLine($"\t{contact.PhoneNumber}");
                Console.WriteLine($"\t{(contact.CategoryId == -1 ? "N/A" : contact.Category.Name)}\n");
            }

            Console.WriteLine("Press Enter to go back"); // Needs replaced to show contacts by category, not just categories
            Console.ReadLine();
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
            var categories = query.AllCategories();
            Console.Clear();
            
            // Getting new contact name
            Console.WriteLine("Please enter new contact's name:");
            string? newName = Console.ReadLine();
            while (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Name cannot be blank, please enter new contact name:");
                newName = Console.ReadLine();
            }

            // Getting new contact email or N/A if left blank
            Console.Clear();
            Console.WriteLine($"Name: {newName}\n");
            string? newEmail = Validation.Email();

            // Getting new contact phone number
            Console.Clear();
            Console.WriteLine($"Name: {newName}\n\tEmail: {newEmail}");
            string? newPhone = Validation.PhoneNumber();

            // Getting category fro new contact
            Console.Clear();
            int newCategoryId;
            Console.WriteLine($"Name: {newName}\n\tEmail: {newEmail}\tPhone: {newPhone}");
            var newCategory = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select a category:")
                    .PageSize(12)
                    .AddChoices(categories.Select(c => c.Name))
                    .AddChoices(new[] { "New Category", "Leave Blank"})
                    );
            if(newCategory == "Leave Blank")
            {
                newCategoryId = -1;
            } else if(newCategory != "New category")
            {
                newCategoryId = categories.Find(c => c.Name == newCategory).Id;
            } else
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
                
                newCategoryId = query.AddCategory(newCategoryEntry);
                    
            }

            query.AddContact(newName, newEmail, newPhone, newCategoryId);
            Console.WriteLine($"{newName} successfully added. Press Enter to return to main menu");
            Console.ReadLine();

        }

        private void AddCategoryMenu()
        {
            var categories = query.AllCategories();
            Console.Clear();
            Console.WriteLine("Enter new category name or leave blank to return to main menu:");
            string? newCategory = Console.ReadLine();
            while (!string.IsNullOrEmpty(newCategory) && categories.Exists(category => category.Name == newCategory))
            {
                Console.WriteLine($"\"{newCategory}\" already exists, please try again or leave black to return to main menu:");
                newCategory = Console.ReadLine();
            }
            if (!string.IsNullOrEmpty(newCategory))
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
            Console.Clear();
            var categories = query.AllCategories();
            var categoryNameToEdit = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Please select a category to edit/delete:")
                    .PageSize(12)
                    .AddChoices(categories.Select(c => c.Name))
                    .AddChoices("Go back")
                    );
            if(categoryNameToEdit != "Go back")
            {
                var categoryId = categories.Find(c => c.Name == categoryNameToEdit).Id;
                var option = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("Select option")
                        .PageSize(3)
                        .AddChoices(new[] { "Edit", "Delete" }));
                    
                if(option == "Edit")
                {
                    // Newborn baby is making it difficult to even think about making these new category names a single function
                    Console.WriteLine("Enter new category name");
                    string? newCategoryName = Console.ReadLine();
                    while (string.IsNullOrEmpty(newCategoryName) || categories.Exists(category => category.Name == newCategoryName))
                    {
                        if (string.IsNullOrEmpty(newCategoryName))
                            Console.WriteLine("Cannot be blank");
                        else
                            Console.WriteLine($"\"{newCategoryName}\" already exists, please try again");
                        newCategoryName = Console.ReadLine();
                    }
                    query.UpdateCategory(categoryId, newCategoryName);
                }
                else
                {
                    query.DeleteCategory(categoryId);
                }
                
            }
            
        }
    }
}
