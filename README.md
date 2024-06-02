
# Book Store

An application for book store automation.

## Prerequisites

Before you begin, ensure you have the following installed:
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/)

## Getting Started

These instructions will get you a copy of the project up and running on your local machine for development and testing purposes.

### Installation

1. **Clone the Repository**

   ```bash
   git clone https://github.com/GeorgeRitchie/BookStore.git
   ```

2. **Open the Solution**

   Open the solution file (`BookStore.sln`) using Visual Studio 2022.

3. **Configure Settings**

   Navigate to the `appsettings.json` file and make necessary adjustments according to your environment settings, especially the database connection string:

   ```json
   "Infrastructure": {
	  "DbConnectionString": "Server=(localdb)\\LocalDb22;AttachDBFilename=[DataDirectory]\\BookStore.mdf;Initial Catalog=BookStore;Trusted_Connection=True;MultipleActiveResultSets=true"
   },
   ```

4. **Database Setup**

   Open the Package Manager Console in Visual Studio (View > Other Windows > Package Manager Console) and run the following command to update the database:

   ```powershell
   update-database -context AppDbContext
   ```

   This command applies existing migrations to the database which sets up the required database schema.

5. **Run the Application**

   - You can start the application by pressing `F5` or clicking on the `Start` button in Visual Studio. 
   - Ensure the web server is set up to start Kestrel.

## Usage

Once the application is running, you can access it by navigating to the URL indicated in Visual Studio's web server output, typically `https://localhost:7236` or a similar local URL.
