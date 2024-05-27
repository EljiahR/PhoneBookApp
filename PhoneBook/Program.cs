using PhoneBook;

using var context = new PhoneBookContext();
var menu = new Menu(context);
menu.MainMenu();