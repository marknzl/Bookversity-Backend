# Bookversity-Backend
![Build Status](https://dev.azure.com/msa-devops/Bookversity/_apis/build/status/marknzl.Bookversity-Backend?branchName=master)

Backend for the Bookversity web application. Part of my submission for the Microsoft Student Accelerator 2020 Phase 2 project. Click [here](https://bookversity-backend.azurewebsites.net/) to view the backend API via Swagger.

# Intro
Bookversity (yes, not the greatest name, pls don't roast <3) is a simple web platform I built dedicated to facilitate the selling of second-hand university textbooks. This is the backend of the project, you can find the frontend [here](https://github.com/marknzl/bookversity-frontend). This backend is written in C# using the ASP.NET Core framework. This project utilizes CI/CD pipelines via Azure DevOps.

# Backend architecture

## Datastore and ORM
Data such as items, orders, users, etc.. is stored in a Microsoft SQL database running on Azure. Item image files are also stored on the cloud using Azure Blob Storage. I use EntityFrameworkCore as an ORM to interact with my database. For the image store, I have written my own service (`Services/ImageStoreService.cs`). I've *attempted* to implement the repository pattern to achieve looser coupling of dependencies. The concrete types are registered in `Startup.cs` and are injected into the controllers.

## Services
My backend has two services which I have written: a service for creation of JWT tokens (`Services/JwtService.cs`), and a service for interacting with my Image Store (`Services/ImageStoreService.cs`).

## Authentication and Authorization
Authentication is implemented via use of the ASP.Net Core Identity framework. When users login, their credentials are verified via Identity and a JWT (JSON Web Token) is issued to the client. This JWT token is stored in the client's browser's localStorage, and for each HTTP request, the token is put in the 'Authorization' header. Any API endpoints which require authorization are marked with the `[Authorization]` attribute either at controller-class level, or controller-method level.

## Controllers
I have four main controllers on my backend: `UserController`, `ItemController`, `CartController`, and `OrdersController`. 

- `UserController`:
Handles user login, registration, and has endpoints for fetching account information

- `ItemController`: 
Handles all interactions to do with items, such as item creation, item deletion, item fetching, etc...

- `CartController`:
Handles all interactions to do with cart actions, such as adding/removing items from a user's cart, and checking out items for purchase.

- `OrdersControllers`:
Facilitates fetching users' orders

## Real-time funtionality via SignalR
In order to provide real-time updates on the client-side to users, my backend makes use of a singular SignalR hub called 'RefreshHub' (`Hubs/RefreshHub.cs`). This hub is dead simple; clients just invoke the `refresh` method on the hub, and then the hub sends a refresh command to all other clients which instructs them to fetch new content.
### `Refresh()` method in `Hubs/RefreshHub.cs`:
```C#
public Task Refresh()
{
    return Clients.Others.SendAsync("refresh");
}
```