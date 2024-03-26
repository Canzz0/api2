using apidenemesi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

internal class Program
{
    private static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        //Authentication iþlemlerini ekledim
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,  //Ýzin verilecek sitelerin denetlenmesi
                    ValidateIssuer = true,
                    ValidateLifetime = true,        //Tokenýn süresi olacakmý
                    ValidateIssuerSigningKey = true,  //tokenýn kontrol edilip edilmeyeceðini kontrol ediyor

                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidAudience = builder.Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(builder.Configuration["Token:SecurityKey"])),  //SecurityKeyi byte çevirip oluþturuyor.
                    ClockSkew = TimeSpan.Zero
                };
            });
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "API Demo", Version = "v1" });
            // JWT için bir Bearer þemasý tanýmlayýn
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Lütfen token'ýnýzý 'Bearer {token}' formatýnda girin.",
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] { }
        }
    });
        });

        // Database ve User servislerini ekleyin
        builder.Services.AddTransient<IDatabaseService, DatabaseService>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<CategoryService>();
        var app = builder.Build();

        // Middleware Pipeline Yapýlandýrmasý
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
        }

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1"));
        }

        //Tokenli giriþ
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        // UserService üzerindeki baþlangýç metodunuzu çaðýrýn
       
    }
}
