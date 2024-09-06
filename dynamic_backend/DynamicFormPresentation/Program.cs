using Microsoft.EntityFrameworkCore;
using DynamicFormPresentation.Models;
using DynamicFormRepo.DynamicFormRepoImplementation;
using DynamicFormRepo.DynamicFormRepoInterface;
using DynamicFormService.DynamicFormServiceImplementation;
using DynamicFormService.DynamicFormServiceInterface;
using DynamicFormServices.DynamicFormServiceImplementation;
using DynamicFormServices.DynamicFormServiceInterface;
using DynamicFormRepos.DynamicFormRepoImplementation;
using DynamicFormRepos.DynamicFormRepoInterface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


builder.Services.AddSwaggerGen(opt =>
{
    opt.SwaggerDoc("v1", new OpenApiInfo { Title = "MyAPI", Version = "v1" });
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Please enter token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type=ReferenceType.SecurityScheme,
                    Id="Bearer"
                }
            },
            new string[]{}
        }
    });
});



// Configure JSON options
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});







builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };
});


// Register DynamicForm repositories and services
builder.Services.AddScoped<IDynamicFormRepoInterface, DynamicFormRepoImplementation>();
builder.Services.AddScoped<IDynamicFormServiceInterface, DynamicFormServiceImplementation>();

// Register Question repositories and services
builder.Services.AddScoped<IQuestionServiceInterface, QuestionServiceImplementation>();
builder.Services.AddScoped<IQuestionRepoInterface, QuestionRepoImplementation>();

// Register Response repositories and services
builder.Services.AddScoped<IResponseFormServiceInterface, ResponseFormServiceImplementation>();
builder.Services.AddScoped<IResponseFormRepoInterface, ResponseFormRepoImplementation>();

// Register Form repositories and services
builder.Services.AddScoped<IFormRepo, FormRepo>();
builder.Services.AddScoped<IFormService, FormService>();

// Register AnswerOption repositories and services
builder.Services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>();
builder.Services.AddScoped<IAnswerOptionService, AnswerOptionService>();

//Register login file
builder.Services.AddScoped<ILoginService, LoginService>();

// Answer type repositories and services
builder.Services.AddScoped<IAnswerTypeRepository, AnswerTypeRepository>();
builder.Services.AddScoped<IAnswerTypeService, AnswerTypeService>();

// Configure Entity Framework Core
builder.Services.AddDbContext<SDirectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add CORS policy
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                   .AllowAnyMethod()
                   .AllowAnyHeader();
        });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Use CORS policy
app.UseCors("AllowAll");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
