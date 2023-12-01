using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ServiceManager.Core;
using ServiceManager.Core.Entities.Identity;
using ServiceManager.Core.Repositories;
using ServiceManager.UI.Config.Mapper;
using ServiceManager.UI.Facades.Requests;
using ServiceManager.UI.Facades.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

string connection = builder.Configuration.GetConnectionString("DefaultConnection")!;
builder.Services.AddDbContext<ServicesContext>(options => options.UseSqlServer(connection));

builder.Services.AddTransient<IdentityRepository>();

builder.Services.AddTransient<ServiceTypeFacade>();
builder.Services.AddTransient<ServiceFacade>();
builder.Services.AddTransient<MaterialFacade>();

builder.Services.AddTransient<ClientFacade>();
builder.Services.AddTransient<EmployeeFacade>();

builder.Services.AddTransient<RequestFacade>();
builder.Services.AddTransient<RequestNotifyFacade>();

builder.Services.AddRazorPages()
    .AddRazorRuntimeCompilation();

builder.Services.AddIdentity<User, Role>(options => {
    options.Password.RequireDigit = false;
    options.Password.RequireLowercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequiredLength = 1;
    options.Password.RequiredUniqueChars = 0;

    options.User.RequireUniqueEmail = false;
})
.AddEntityFrameworkStores<ServicesContext>();


builder.Services.AddAutoMapper(typeof(AppMappingProfile));

var app = builder.Build();

ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
