# carpooling-backend
Backend API for a carpooling application built with ASP.NET Core Web API using Clean Architecture.

### Prerequisites
Before running the project, set the following environment variable on your machine.

**Windows (PowerShell as Administrator):**
[System.Environment]::SetEnvironmentVariable(
  "ConnectionStrings__DefaultConnection",
  "Server=localhost;Database=CarpoolingDatabase;
   Trusted_Connection=True;TrustServerCertificate=True",
  "Machine"
)

After setting the variable:
1. Close PowerShell
2. Restart Visual Studio
3. Run the project