📘 Dictionary API

A RESTful API built with ASP.NET Core for managing and querying a multilingual dictionary.
It allows users to search words by language and retrieve their definitions and synonyms in a structured format.

🚀 Features
🔍 Search words by language
📖 Retrieve definitions and synonyms
➕ Add new words with validation
🌐 Multi-language support
🗄️ SQLite database integration
🔄 Entity Framework Core migrations
🌱 Seed initial data (languages)

🛠️ Tech Stack
Backend: ASP.NET Core Web API
ORM: Entity Framework Core
Database: SQLite
Language: C#

📂 Project Structure
Dictionario.API/
│
├── Controllers/       # API endpoints
├── Data/              # DbContext and configuration
├── DTOs/              # Data Transfer Objects
├── Models/            # Entity models
├── Migrations/        # EF Core migrations
└── Program.cs         # App configuration

⚙️ Getting Started

1️⃣ Clone the repository
git clone https://github.com/your-username/dictionary-api.git
cd dictionary-api
2️⃣ Install dependencies
dotnet restore
3️⃣ Apply migrations & create database
dotnet ef database update
4️⃣ Run the API
dotnet run

The API will be available at:

http://localhost:5000

📡 API Endpoints
🔍 Search a word
GET /api/dictionary/search?word=house&lang=en
➕ Add a new word
POST /api/dictionary
Request body:
{
  "word": "house",
  "languageCode": "en",
  "definitions": [
    "A building for human habitation"
  ],
  "synonyms": [
    "home",
    "residence"
  ]
}
🌱 Seed Data

The project includes initial language data using Entity Framework Core:

English (en)
Spanish (es)
🧪 Testing the API

You can test the endpoints using:

Postman
Insomnia
cURL

⚠️ Notes
Ensure that the language exists before adding a word
Avoid duplicate entries for the same word and language
Migrations are required after model changes
📌 Future Improvements
🔎 Advanced search (partial matches, filters)
🔐 Authentication & authorization
📊 Pagination and performance optimization
🌍 Support for more languages
📥 Bulk import (CSV/JSON)
👨‍💻 Author

Developed by Oswaldo Rodriguez

