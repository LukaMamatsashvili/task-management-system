using TaskManagementSystem.Core.DataAccess;
using TaskManagementSystem.Core.Interfaces;
using TaskManagementSystem.Core.Services;
using TaskManagementSystem.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Add CORS
builder.Services.AddCors();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//Add Repositories
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
builder.Services.AddScoped<ITaskAttachmentRepository, TaskAttachmentRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserPermissionLinkRepository, UserPermissionLinkRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserRoleRepository, UserRoleRepository>();
//

//Add Services
builder.Services.AddScoped<IPermissionService, PermissionService>();
builder.Services.AddScoped<ITaskAttachmentService, TaskAttachmentService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<IUserAuthorizationService, UserAuthorizationService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRoleService, UserRoleService>();
//

//Add DbContext
builder.Services.AddDbContext<TaskManagementDbContext>();
//

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Use CORS
app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();