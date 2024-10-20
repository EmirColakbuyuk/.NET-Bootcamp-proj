
# TechMarket MVC

### Overview

TechMarket MVC is a .NET-based web application designed to manage and showcase products such as computers, phones, and smartwatches in a tech market environment. The project allows administrators to add, edit, and delete product entries, while end-users can browse, filter, and view product details from various categories. It follows an MVC (Model-View-Controller) architecture and uses Entity Framework for data access.

### Features

- **Admin Dashboard:**
  - Add, update, and delete products (computers, phones, and smartwatches).
  - Manage inventory and product details.
- **Product Listing:**
  - Display products by category (computers, phones, smartwatches).
  - Search and filter functionality for each product category.
- **Detailed Product Pages:**
  - View detailed product information (specs, price, and availability).
  - Navigate easily between categories.
- **Responsive Design:**
  - Clean, responsive interface for both administrators and users.
  
### Technologies

- **Backend:**
  - ASP.NET Core MVC
  - Entity Framework Core
  - C#
- **Frontend:**
  - Bootstrap 4 for responsive design
  - HTML5 & CSS3
- **Database:**
  - SQLite (for development)
- **Hosting/Environment:**
  - .NET 8.0

### Installation and Setup

1. **Clone the Repository:**
   ```bash
   git clone https://github.com/your-username/techmarket-mvc.git
   cd techmarket-mvc
   ```

2. **Install .NET SDK:**
   Ensure you have the .NET SDK installed. You can download it from [here](https://dotnet.microsoft.com/download).

3. **Install Dependencies:**
   Inside the project directory, run the following command to restore dependencies:
   ```bash
   dotnet restore
   ```

4. **Database Setup:**
   The project uses Entity Framework Core with SQLite. To set up the database, run:
   ```bash
   dotnet ef database update
   ```

5. **Run the Application:**
   You can run the application using:
   ```bash
   dotnet run
   ```

   Then, navigate to `http://localhost:5000` to access the application.

### Admin Access

To access the admin dashboard, log in with the following credentials (or create your own):

- **Username:** `admin`
- **Password:** `admin123`

Once logged in, you can manage products across different categories.

### Project Structure

- `Controllers/`: Contains the controllers for handling HTTP requests for computers, phones, smartwatches, and admin actions.
- `Models/`: Defines the database models for Computers, Phones, Smartwatches, and Users.
- `Views/`: Contains the views responsible for rendering the user interface, organized by category (e.g., Computers, Phones, Smartwatches).
- `ViewModels/`: Holds view models used to manage data between controllers and views.
- `wwwroot/`: Contains static files like images, CSS, and JavaScript assets.
- `Data/`: Contains the `TechMarketContext` class used for Entity Framework Core database operations.

### Usage

- **Admin Management:**
  - After logging in, admins can manage the products in each category.
  - Add new products with images, specs, and descriptions, and update or remove products as needed.

- **End User:**
  - Browse products from different categories.
  - Filter and search within categories.
  - View detailed product descriptions, prices, and stock availability.


