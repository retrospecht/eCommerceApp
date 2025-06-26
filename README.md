# Full-Stack eCommerce Application
This is a MAUI application designed to provide a platform to manage items within an inventory with CRUD operations including name, price, and quantity as well as a shopping cart from a user perspective. 

# Features
### Inventory Management:
* View items in the inventory by ID, name, price, and quantity
* Search features including a search bar as well as a sort by feature for name and price
* Add, edit, and delete items which will seamlessly update in the shopping cart view

### Shopping Cart View:
* View available items by ID, name, price, and quantity
* Add and remove items from the shopping cart which seamlessly updates with the inventory
* An inline text box that allows the inline "+" button to add a specified number of items to the shopping cart with a single click
* A sort by feature for name and price
* The ability to check out and receive an itemized receipt with a customizable tax rate in the main menu

# Languages Used
* MAUI, XAML, C#, .NET

# Planned Features
* A "wishlist" feature implemented on the shopping cart view that allows users to choose a cart and add from the inventory to the selected cart
* Implement a working Shopping Cart WebAPI Service (Controller and EC)
* Add a persistence component that once implemented users do not lose data when both the client and server-side code are turned off
