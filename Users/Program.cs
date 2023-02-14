using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Users.Domain;
using Users.Infrastructure;
using Users.WebApi;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(Assembly.GetExecutingAssembly());
builder.Services.AddDbContext<UserDbContext>(ops =>
{
    string strconn = "Data Source=.;Initial Catalog=UserMg;Integrated Security=True; Encrypt=True;TrustServerCertificate=True;";
     
    ops.UseLoggerFactory(UserDbContext.MyLoggerFactory)
        .UseSqlServer(strconn);
});
builder.Services.Configure<MvcOptions>(ops =>
{
    ops.Filters.Add<UnitOfWorkFilter>();
});

builder.Services.AddScoped<IUserDomainRepository, UserDomainRepository>();
builder.Services.AddScoped<ISmsSendCodeService, MockSmsCodeSender>();
builder.Services.AddScoped<UserDomainService>();
builder.Services.AddDistributedMemoryCache();

//builder.Services.AddCors(options =>
//{
//    options.AddDefaultPolicy( opt => opt.WithOrigins(new[] { "http://localhost:3000", "http://123.sogo.com" }).AllowAnyHeader().AllowAnyMethod());
//});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        policy =>
        {
            policy.WithOrigins("http://localhost:3000",
                                "http://www.contoso.com").AllowAnyHeader().AllowAnyMethod();
        });
});



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.UseCors();

app.UseAuthorization();

app.MapControllers();

app.Run();
