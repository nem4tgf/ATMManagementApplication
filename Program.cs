using Microsoft.EntityFrameworkCore;
using ATMManagementApplication.Data;

var builder = WebApplication.CreateBuilder(args);
//Add service to container => Thiet lap cau hinh data model
builder.Services.AddControllers();
builder.Services.AddDbContext<ATMContext>(options => 
options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"),
new MySqlServerVersion(new Version (8,0,403))
)
);

var app = builder.Build();

if(app.Environment.IsDevelopment()){
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.MapControllers();


app.Run();