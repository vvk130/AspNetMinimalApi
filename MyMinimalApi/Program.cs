var builder = WebApplication.CreateBuilder(args);

var dbUrl = builder.Configuration.GetConnectionString("DefaultConnection");

builder.AddFluentValidationEndpointFilter();

builder.Services.AddDbContext<MyDbContext>(options => options.UseNpgsql(dbUrl));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddValidatorsFromAssemblyContaining<Program>();

var app = builder.Build();

app.UseSwagger();

app.UseSwaggerUI();

app.UseStatusCodePages();

app.MapBookApi();
app.MapAuthorApi();
app.MapWalletApi();
app.MapPurchaseApi();

app.Run();
