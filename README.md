![ ](https://github.com/user-attachments/assets/49d9757c-9947-4d5e-b6ba-a72ee241e8c5)
# ResponseWrite

A lightweight middleware for ASP.NET Core to write content to the response stream **after** the execution of Razor Pages, MVC Views, or API Controllers.

## The Problem
In ASP.NET Core, once the headers are sent or the View starts rendering, you cannot easily write to the `Response.Body` without causing a `500 Internal Server Error` or `Response already started` exception.

## Microsoft's Partial Workaround (Not a Real Solution)

In ASP.NET Core, the common recommendation from Microsoft is to avoid writing directly to the response stream and instead pass data through mechanisms like:

* `ViewData`
* `ViewBag`
* `TempData`
* Partial Views
* View Components

Example:

```csharp
ViewData["FooterScript"] = "<script>...</script>";
```

And then render it inside the View:

```csharp
@ViewData["FooterScript"]
```

### Why This Is Incomplete

While this approach prevents the exception, it does **not actually solve the core problem**:

* It requires modifying the View in advance.
* It is not dynamic — injection points must already exist.
* It creates tight coupling between modules and Views.
* It cannot be used cleanly from Middleware or infrastructure layers.
* It breaks modular/plugin-based architectures.

This is a presentation-layer workaround, not a response-pipeline solution.

## The Solution
**ResponseWrite** allows you to queue your content during the request execution and automatically appends it to the final output just before the response is closed.

## Installation
Install via NuGet:
```bash
dotnet add package ResponseWrite
```

## How to Use

### 1. Register Middleware

In your `Program.cs`:

```csharp
app.UseResponseWrite(); // Add this before app.Run()
```

### 2. Write from anywhere

In your **Razor Pages**, **MVC Controllers**, or **Middleware**:

```csharp
public IActionResult OnGet()
{
    Response.Write("Hello from Elanat!"); // No error, no matter when you call it!
    return Page();
}
```

## Features

* Prevents `System.InvalidOperationException: Response already started`.
* Works perfectly with **Razor Pages**, **MVC**, and **Minimal APIs**.
* Extremely lightweight with zero dependencies.

## Compatibility with WebForms Core
This middleware is fully compatible with **WebForms Core**, a technology developed by [Elanat](https://elanat.net).

**Example:**
```csharp
public IActionResult OnGet()
{
    WebForms form = new WebForms();
    form.SetBackgroundColor("<body>", "violet");

    Response.Write(form.ExportToHtmlComment());
    return Page();
}

```
