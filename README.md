# URL Shortener Project (Backend)

## Overview

This project is the backend service for a URL shortener application. It is built with C# and .NET ASP.NET Core and uses SQL Server for data storage. The backend supports user registration, email verification, authentication, URL shortening, and link management with click count tracking. This service is consumed by the Angular frontend via HTTP requests.

## Features

- **User Registration:** Allows users to create accounts.
- **Email Verification:** Users must verify their email addresses to activate their accounts.
- **User Authentication:** Provides secure login for registered users.
- **URL Shortening:** Authenticated users can shorten URLs.
- **Link Management:** Users can view click counts and other details for their shortened URLs.

## Tech Stack

- **Backend:** C#, .NET ASP.NET Core
- **Database:** SQL Server
- **Authentication:** JWT (JSON Web Tokens)
- **Email Service:** (e.g., SendGrid, Mailgun)

## Getting Started

### Prerequisites

Ensure you have the following installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

### Installation

1. **Clone the repository:**
    ```sh
    git clone https://github.com/diegoambeliz/url-shortener-backend.git
    cd url-shortener-backend
    ```

2. **Install dependencies:**
    ```sh
    dotnet restore
    ```

### Running the Application

1. **Update Database:**
    ```sh
    dotnet ef database update
    ```

2. **Run the application:**
    ```sh
    dotnet run
    ```

3. **API Documentation:**
    Access the API documentation at `http://localhost:[PORT]/swagger`.


## Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository.
2. Create a new branch: `git checkout -b feature/your-feature`.
3. Commit your changes: `git commit -m 'Add some feature'`.
4. Push to the branch: `git push origin feature/your-feature`.
5. Open a pull request.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

## Contact

For any questions, please reach out to `your-email@example.com`.
