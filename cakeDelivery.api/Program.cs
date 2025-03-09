
using System.Text;
using cakeDelivery.api;
using cakeDelivery.api.Authorization;
using cakeDelivery.Business;
using cakeDelivery.Business.Authorization;
using cakeDelivery.Mappers;
using cakeDelivery.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using UserDelivery.Business;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(); 
builder.Services.AddEndpointsApiExplorer();

//  Swagger Config
builder.Services.AddSwaggerConfig();


// Get MongoDB settings from configuration
var mongoDBSettings = builder.Configuration.GetSection("MongoDBSettings").Get<MongoDBSettings>();
builder.Services.AddScoped<IMongoClient>(sp => new MongoClient(mongoDBSettings.AtlasURI));
builder.Services.AddScoped<IMongoDatabase>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    return client.GetDatabase(mongoDBSettings.DatabaseName);
});

//  FluentValidation
builder.Services.AddFluentValidationAutoValidation()
    .AddFluentValidationClientsideAdapters();
builder.Services.AddValidatorsFromAssemblyContaining<CakeValidator>();



// Register AutoMapper
builder.Services.AddAutoMapper(typeof(CakeMappingProfile));
builder.Services.AddAutoMapper(typeof(OrderMappingProfile));
builder.Services.AddAutoMapper(typeof(OrderItemMappingProfile));
builder.Services.AddAutoMapper(typeof(CategoryMappingProfile));
builder.Services.AddAutoMapper(typeof(CustomerMappingProfile));
builder.Services.AddAutoMapper(typeof(DeliveryMappingProfile));
builder.Services.AddAutoMapper(typeof(FeedbackMappingProfile));
builder.Services.AddAutoMapper(typeof(PaymentMappingProfile));
builder.Services.AddAutoMapper(typeof(UserMappingProfile));

// Register Services
builder.Services.AddScoped<CakeService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<OrderItemService>();
builder.Services.AddScoped<CustomerService>();
builder.Services.AddScoped<CustomerFeedbackService>();
builder.Services.AddScoped<DeliveryService>();
builder.Services.AddScoped<PaymentService>();
builder.Services.AddScoped<UserService>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<UniqueValidatorService>();
builder.Services.AddScoped<IAuthorizationHandler, PermissionAuthorizationHandler>();


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(Permissions.View.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.View)));
    options.AddPolicy(Permissions.ManageUsers.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageUsers)));
    options.AddPolicy(Permissions.ManageCustomers.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageCustomers)));
    options.AddPolicy(Permissions.ManageCakes.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageCakes)));
    options.AddPolicy(Permissions.ManageCategories.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageCategories)));
    options.AddPolicy(Permissions.ManageOrders.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageOrders)));
    options.AddPolicy(Permissions.ManageDeliveries.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManageDeliveries)));
    options.AddPolicy(Permissions.ManagePayments.ToString(), 
        policy => policy.Requirements.Add(new PermissionRequirement(Permissions.ManagePayments)));
});

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>();
builder.Services.AddSingleton(jwtOptions);
builder.Services.AddAuthentication().AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
{
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidateAudience = true,
        ValidAudience = jwtOptions.Audience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SignKey))
    };
});

var app = builder.Build();

//  Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseHttpsRedirection();
app.Run();