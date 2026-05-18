using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using RMA.Server.Services;
using System.Text;
using Google.Cloud.Firestore;
using RMA.Server.Entities;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Configure Firebase JWT Authentication
var projectId = builder.Configuration["Firebase:ProjectId"];
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = $"https://securetoken.google.com/{projectId}";
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = $"https://securetoken.google.com/{projectId}",
            ValidateAudience = true,
            ValidAudience = projectId,
            ValidateLifetime = true
        };
    })
    .AddJwtBearer("Local", options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "RMAServer",
            ValidAudience = "RMAServer",
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("RMA_SongLinh_SecretKey_For_Local_Testing_Only_12345"))
        };
    });

// Configure Authorization to accept both Firebase and Local tokens
builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme, "Local")
        .RequireAuthenticatedUser()
        .Build();
});

// Configure CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazorWasm",
        policy =>
        {
            policy.WithOrigins("http://localhost:5286", "https://localhost:7237")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

// Initialize Firestore
Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", "serviceAccountKey.json");
var firestoreDb = FirestoreDb.Create(projectId);
builder.Services.AddSingleton(firestoreDb);

// Register Repositories
builder.Services.AddScoped<FirestoreRepository<Customer>>(provider => new FirestoreRepository<Customer>(provider.GetRequiredService<FirestoreDb>(), "customers"));
builder.Services.AddScoped<FirestoreRepository<Device>>(provider => new FirestoreRepository<Device>(provider.GetRequiredService<FirestoreDb>(), "devices"));
builder.Services.AddScoped<FirestoreRepository<Vendor>>(provider => new FirestoreRepository<Vendor>(provider.GetRequiredService<FirestoreDb>(), "vendors"));
builder.Services.AddScoped<FirestoreRepository<Model>>(provider => new FirestoreRepository<Model>(provider.GetRequiredService<FirestoreDb>(), "models"));
builder.Services.AddScoped<FirestoreRepository<Category>>(provider => new FirestoreRepository<Category>(provider.GetRequiredService<FirestoreDb>(), "categories"));
builder.Services.AddScoped<FirestoreRepository<StatusMaster>>(provider => new FirestoreRepository<StatusMaster>(provider.GetRequiredService<FirestoreDb>(), "status_masters"));
builder.Services.AddScoped<FirestoreRepository<Location>>(provider => new FirestoreRepository<Location>(provider.GetRequiredService<FirestoreDb>(), "locations"));

// Firebase Cloud Messaging (FCM)
builder.Services.AddSingleton<IFcmService, FcmService>();
builder.Services.AddHostedService<RmaAlertBackgroundService>();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseCors("AllowBlazorWasm");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
