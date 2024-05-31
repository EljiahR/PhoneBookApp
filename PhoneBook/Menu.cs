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
                Console.WriteLine($"\t{(contact.Category == null ? "Uncatergorized" : contact.Category.Name)}\n");
            }

            Console.WriteLine("Press Enter to go back"); // Needs replaced to show contacts by category, not just categories
            Console.ReadLine();
        }

        private void ViewCategoriesMenu()
        {
            var categories = query.AllCategories();
            var option = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select a category to view")
                    .PageSize(5)
                    .AddChoices(categories.Select(c => c.Name))
                    .AddChoices("Go back"));
            if(option != "Go back")
            {
                var contacts = query.ContactByCategory(categories.Find(c => c.Name == option).Id);
                foreach(var contact in contacts)
                {
                    Console.WriteLine(contact.Name);
                    Console.WriteLine($"\t{contact.Email}");
                    Console.WriteLine($"\t{contact.PhoneNumber}");
                }
                Console.WriteLine("\nPress Enter to go back to main menu");
                Console.ReadLine();
            }
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
            Console.WriteLine($"Name: {newName}\tEmail: {newEmail}\n");
            string? newPhone = Validation.PhoneNumber();

            // Getting category fro new contact
            Console.Clear();
            int newCategoryId;
            Console.WriteLine($"Name: {newName}\tEmail: {newEmail}\tPhone: {newPhone}\n");
            var newCategory = Validation.Category(categories, query);

            query.AddContact(newName, newEmail, newPhone, newCategory.Id);
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
            var contacts = query.AllContacts();
            var categories = query.AllCategories();
            var contactName = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("Select contact to edit:")
                    .PageSize(10)
                    .AddChoices(contacts.Select(c => c.Name + " " + c.PhoneNumber))
                    .AddChoices("Go back"));
            if(contactName != "Go back")
            {
                var contact = contacts.Find(c => c.Name + " " + c.PhoneNumber == contactName);
                var editOrDelete = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                    .Title("Select an option")
                    .AddChoices(new[] {"Edit", "Delete"}));

                if(editOrDelete == "Edit")
                {
                    string? option;
                    string? newName = "";
                    string newEmail = "";
                    string newPhone = "";
                    int newCategoryId = 0;
                    do
                    {
                        Console.Clear();
                        Console.WriteLine("Current:");
                        Console.WriteLine($"\tName: {contact.Name} Email: {contact.Email} Phone Number: {contact.PhoneNumber} Category: {(contact.Category != null ? contact.Category.Name : "Uncatergorized")}");
                        if (!string.IsNullOrEmpty(newName) || !string.IsNullOrEmpty(newEmail) || !string.IsNullOrEmpty(newPhone) || newCategoryId > 0)
                        {
                            Console.WriteLine($"\t{(string.IsNullOrEmpty(newName) ? "" : $"Name: {newName} ")}{(string.IsNullOrEmpty(newEmail) ? "" : $"Email: {newEmail} ")}{(string.IsNullOrEmpty(newPhone) ? "" : $"Phone Number: {newPhone} ")}{(newCategoryId < 1 ? "" : $"category: {categories.Find(c => c.Id == newCategoryId).Name}")}");
                        }
                        option = AnsiConsole.Prompt(
                            new SelectionPrompt<string>()
                                .Title("Select category to edit or select Finished when done")
                                .PageSize(5)
                                .AddChoices(new[] { "Name", "Email", "Phone Number", "Category", "Finished" }));
                        switch (option)
                        {
                            case "Name":
                                Console.WriteLine("Enter a new name:");
                                newName = Console.ReadLine();
                                while (string.IsNullOrEmpty(newName))
                                {
                                    Console.WriteLine("Invalid entry, cannot be blank");
                                    newName = Console.ReadLine();
                                }
                                break;
                            case "Email":
                                newEmail = Validation.Email();
                                break;
                            case "Phone Number":
                                newPhone = Validation.PhoneNumber();
                                break;
                            case "Category":
                                var newCategory = Validation.Category(categories, query);
                                newCategoryId = newCategory.Id;
                                categories = query.AllCategories();
                                break;
                            case "Finished":
                                break;
                        }
                    } while (option != "Finished");
                    query.UpdateContact(contact.Id, newName, newEmail, newPhone, newCategoryId);
                    Console.WriteLine("Contact updated!");
                    Console.WriteLine("Press Enter to return to main menu");
                    Console.ReadLine();
                } else
                {
                    query.DeleteContact(contact.Id);
                    Console.WriteLine("Contact deleted.");
                    Console.WriteLine("Press Enter to return to main menu");
                    Console.ReadLine();
                }
                
            }
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
