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


        //Authentication i�lemlerini ekledim
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
            options =>
            {
                options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                {
                    ValidateAudience = true,  //�zin verilecek sitelerin denetlenmesi
                    ValidateIssuer = true,
                    ValidateLifetime = true,        //Token�n s�resi olacakm�
                    ValidateIssuerSigningKey = true,  //token�n kontrol edilip edilmeyece�ini kontrol ediyor

                    ValidIssuer = builder.Configuration["Token:Issuer"],
                    ValidAudience = builder.Configuration["Token:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF32.GetBytes(builder.Configuration["Token:SecurityKey"])),  //SecurityKeyi byte �evirip olu�turuyor.
                    ClockSkew = TimeSpan.Zero
                };
            });
        builder.Services.AddControllers();
        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() { Title = "API Demo", Version = "v1" });
            // JWT i�in bir Bearer �emas� tan�mlay�n
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "L�tfen token'�n�z� 'Bearer {token}' format�nda girin.",
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

        // Middleware Pipeline Yap�land�rmas�
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

        //Tokenli giri�
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }

    private static async Task InitializeAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var userService = scope.ServiceProvider.GetRequiredService<UserService>();

        // UserService �zerindeki ba�lang�� metodunuzu �a��r�n
       
    }
}
