# Tech Stack
Backend (CO.API)

    ASP.NET Core Web API (.NET 8)
    
    Entity Framework Core (EF Core) → SQL Server

    Prometheus-net → for API metrics and monitoring

    Custom Exception Handling Middleware → friendly error responses

Frontend (React SPA)

    Vite + React (JavaScript)

    Fetches data from the API

    Displays it in a user-friendly manner

API Endpoints - From Swagger
GET /Customers

    Returns list of all customers

    Response:

    [
      {
        "customerID": "ALFKI",
        "companyName": "Alfreds Futterkiste"
      },
      ...
    ]

GET /Customers/customer/{id}

    Returns details about a specific customer

    Path parameter: id (string, required)

    Response:

    {
      "customerID": "ALFKI",
      "companyName": "Alfreds Futterkiste",
      "contactName": "Maria Anders",
      "contactTitle": "Sales Representative",
      "address": "Obere Str. 57",
      "city": "Berlin",
      "region": null,
      "postalCode": "12209",
      "country": "Germany",
      "phone": "030-0074321",
      "fax": "030-0076545"
    }

GET /Customers/customer/{id}/orders

    Returns list of orders for a specific customer

    Path parameter: id (string, required)

    Response:

[
  {
    "orderId": 10248,
    "orderDate": "1996-07-04T00:00:00",
    "total": 440.00,
    "numberOfProducts": 3,
    "potentialProblem": false
  },
  ...
]



# UI
- Main Customers window
![image](https://github.com/user-attachments/assets/4047adf8-2591-4bec-abb4-ec2e00bc057f)

- Selected Customer window
![image](https://github.com/user-attachments/assets/30ddbf35-a315-434e-a730-3a420fd92405)

- Customers Search 
![image](https://github.com/user-attachments/assets/605cdeaa-01ef-4b8c-8d29-86f40c7eee34)
