Employment System Backend
=========================

This is the backend for an employment system that allows employers to create job vacancies and applicants to apply for open positions. The system supports user registration, authentication, and role-based access control for two types of users: Employers and Applicants. Additionally, caching is used to optimize performance for search operations.

Features
--------

-   **Employer Role**:

    -   CRUD operations for job vacancies.
    -   Set maximum number of applications per vacancy.
    -   Archive expired vacancies.
    -   View applicants for a specific vacancy.
    -   Automatically clear cache when vacancies are updated or deleted.
-   **Applicant Role**:

    -   Search for vacancies with keyword-based filtering.
    -   Apply for a vacancy.
    -   Limit one application per day per applicant.
    -   Cache search results for faster performance.
-   **Authentication & Authorization**:

    -   User registration and login with JWT token authentication.
    -   Role-based permissions for Employer and Applicant.
-   **Caching**:

    -   Memory caching for faster search performance.
    -   Cache invalidation on vacancy update or delete.

Technology Stack
----------------

-   **ASP.NET Core** (.NET 8.0)
-   **Entity Framework Core** (for database access)
-   **MS SQL Server** (database)
-   **JWT (JSON Web Tokens)** for authentication
-   **MemoryCache** for caching search results
-   **Swagger** for API documentation

Prerequisites
-------------

To run the project, you will need the following installed on your machine:

-   [.NET SDK](https://dotnet.microsoft.com/download)
-   [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)
-   A tool to test APIs like [Postman](https://www.postman.com/) or [curl](https://curl.se/).

Getting Started
---------------

Follow these steps to get the backend running locally.

### 1\. Clone the repository


`git clone https://github.com/your-repository/employment-system-backend.git
cd employment-system-backend`

### 2\. Set Up SQL Server

-   Ensure that SQL Server is running locally, and note the connection string.

### 3\. Set Up Configuration

In the `appsettings.json` file, ensure that the following configurations are set:


<pre>
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=employmentDB;User Id=your_username;Password=your_password;"
},
"Jwt": {
  "Key": "your_jwt_secret_key",  // Ensure this is at least 16 characters
  "Issuer": "yourissuer.com"
}
</pre>

### 4\. Apply Migrations

Before running the application, apply the database migrations to set up the schema:

`dotnet ef database update`

### 5\. Run the Application

`dotnet run`
*NOTE:you can run it with Visual Studio IDE*

The application will start, and Swagger will be available at `https://localhost:7040/swagger` or `http://localhost:5290/swagger` depending on the launch settings.

### 6\. API Documentation

The API documentation is available via **Swagger** at `https://localhost:7040/swagger`. This documentation provides information on all the available endpoints, their request/response models, and how to interact with the API.

How to Use
----------

### 1\. Register as Employer or Applicant

-   **Endpoint**: `/api/auth/register`
-   Send a `POST` request with the following payload:


<pre>
{
  "username": "yourusername",
  "password": "yourpassword",
  "role": "Employer"  // or "Applicant"
}
</pre>

### 2\. Login

-   **Endpoint**: `/api/auth/login`
-   Send a `POST` request with the following payload:


<pre>
{
  "username": "yourusername",
  "password": "yourpassword"
}
</pre>

-   The response will contain a JWT token, which you will use to authenticate requests.

### 3\. Perform Role-Specific Actions

#### Employer Role

-   **Create a Vacancy**: `/api/employers/vacancies` (POST)
-   **Update a Vacancy**: `/api/employers/vacancies/{vacancyId}` (PUT)
-   **Delete a Vacancy**: `/api/employers/vacancies/{vacancyId}` (DELETE)
-   **Get All Vacancies**: `/api/employers/vacancies` (GET)
-   **View Applicants for Vacancy**: `/api/employers/vacancies/{vacancyId}/applicants` (GET)

#### Applicant Role

-   **Search for Vacancies**: `/api/applicants/vacancies?keyword={your_keyword}` (GET)
-   **Apply for Vacancy**: `/api/applicants/vacancies/{vacancyId}/apply` (POST)
-   **View Applied Vacancies**: `/api/applicants/applied-vacancies` (GET)

Caching Strategy
----------------

-   Vacancy search results are cached for 60 minutes to improve performance.
-   Cache is invalidated when vacancies are updated or deleted to ensure up-to-date search results.

Development
-----------

### Logging

-   The project includes logging using `ILogger<T>`. Logs are written to the console by default.
-   You can view logs in the terminal when running the application to track the flow, warnings, and errors.
