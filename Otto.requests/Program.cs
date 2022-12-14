using Microsoft.EntityFrameworkCore;
using Otto.models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OttoDbContext>();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseCors(
  options => options.WithOrigins("*").AllowAnyMethod().AllowAnyHeader()
);

app.MapGet("/api/requests", async (OttoDbContext db) =>
{
    var requests = await db.JoinRequests.ToListAsync();
    return Results.Ok(getResponses(requests));
});

app.MapGet("/api/requests/{id}", async (OttoDbContext db, int id) =>
{
    var request = await db.JoinRequests.FindAsync(id);
    if (request is not null)
        return Results.Ok(getResponse(request));
    return Results.NotFound();
});

app.MapGet("/api/requests/company/{id}", async (OttoDbContext db, int id) =>
{
    var requests = await db.JoinRequests.Where(r=> r.CompanyId == id).ToListAsync();
    if (requests is not null)
        return Results.Ok(getResponses(requests));
    return Results.NotFound();
});

app.MapGet("/api/requests/company/{id}/{state}", async (OttoDbContext db, string state ,int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState)) 
    {
        var requests = await db.JoinRequests.Where(r => r.CompanyId == id && r.State == newState).ToListAsync();
        if (requests is not null)
            return Results.Ok(getResponses(requests));
        return Results.NotFound();
    }
    else 
    {
        var posibleStates = Enum.GetValues(typeof(State)).Cast<State>()
                        .Select(d => (d, (int)d))
                        .ToList();

        return Results.NotFound($"Los estados posibles son: {String.Join(',', posibleStates)}"); 
    }
});

app.MapGet("/api/requests/user/{id}", async (OttoDbContext db, int id) =>
{
    var requests = await db.JoinRequests.Where(r => r.UserId == id).ToListAsync();
    if (requests is not null)
        return Results.Ok(getResponses(requests));
    return Results.NotFound();
});

app.MapGet("/api/requests/user/{id}/{state}", async (OttoDbContext db, string state, int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState))
    {
        var requests = await db.JoinRequests.Where(r => r.UserId == id && r.State == newState).ToListAsync();
        if (requests is not null)
            return Results.Ok(getResponses(requests));
        return Results.NotFound();
    }
    else
    {
        var posibleStates = Enum.GetValues(typeof(State)).Cast<State>()
                        .Select(d => (d, (int)d))
                        .ToList();

        return Results.NotFound($"Los estados posibles son: {String.Join(',', posibleStates)}");
    }
});

app.MapPost("/api/requests", async (OttoDbContext db, JoinRequest request) =>
{
    //check if userId exist
    var user = await db.Users.Where(u => u.Id == request.UserId).FirstOrDefaultAsync();
    if (user is null)
        return Results.Conflict("El usuario no existe");

    //check if companyId exist
    var company = await db.Companies.Where(c => c.Id == request.CompanyId).FirstOrDefaultAsync();
    if (company is null)
        return Results.Conflict("El fullfilment no existe");

    //check if request exist from that user to company id 
    var joinRequest = await db.JoinRequests.Where(r => r.UserId == request.UserId &&
                                                          r.CompanyId == request.CompanyId).FirstOrDefaultAsync();

    if (joinRequest is not null && (joinRequest.State == State.Confirmado || joinRequest.State == State.Pendiente))
        return Results.Conflict($"La solictud ya se encuentra creada y esta con un estado {joinRequest.State.ToString()}");
    //else if(joinRequest is not null && joinRequest.State == State.Rechazado)
    //else

    request.Created = DateTime.Now;

    await db.JoinRequests.AddAsync(request);
    await db.SaveChangesAsync();

    return Results.Created($"/api/requests/{request.Id}", getResponse(request));
});

app.MapPut("/api/requests/{id}", async (OttoDbContext db, JoinRequest request, int id) =>
{
    var joinRequest = await db.JoinRequests.FindAsync(id);
    if (joinRequest is null) return Results.NotFound();

    //// check if user is in company and is admin
    //var user = await db.Users.FindAsync(company.CreatedByUserId);

    UpdateFields(request, joinRequest);
    joinRequest.Modified = DateTime.Now;

    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.MapPut("/api/requests/{id}/{state}", async (OttoDbContext db, JoinRequestApplicant request, string state, int id) =>
{
    if (Enum.TryParse<State>(state, true, out State newState) && (newState == State.Confirmado || newState == State.Rechazado))
    {
        var joinRequest = await db.JoinRequests.FindAsync(id);
        if (joinRequest is null) return Results.NotFound("No existe un request con ese id");

        //verificar que el usuario aprobador exista
        var user = await db.Users.FindAsync(request.ApplicantUserId);
        if (user is null) return Results.NotFound("No existe un aprobador con ese id");

        //verificar que el usuario que esta aprobando, pertenece al fullfilment
        if (user.CompanyId == joinRequest.CompanyId && user.Rol.Equals("Administrador")) 
        {
            joinRequest.State = newState;
            joinRequest.Modified = DateTime.Now;
            await db.SaveChangesAsync();

            //update client user company id
            var clientUser = await db.Users.FindAsync(joinRequest.UserId);

            clientUser.CompanyId = joinRequest.CompanyId;
            await db.SaveChangesAsync();

            return Results.NoContent();
        }

        return Results.NotFound("El usuario aprobador no pertece a la misma compania que el solicitud o no tiene el rol apropiedo");

    }
    else
    {
        return Results.NotFound($"Los estados posibles son: {String.Join(',', new List<string>{State.Confirmado.ToString(), State.Rechazado.ToString()})}");
    }
});

app.MapDelete("/api/requests/{id}", async (OttoDbContext db, int id) =>
{
    var joinRequest = await db.JoinRequests.FindAsync(id);
    if (joinRequest is null)
        return Results.NotFound();

    db.JoinRequests.Remove(joinRequest);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();

static void UpdateFields(JoinRequest request, JoinRequest? joinRequest)
{
    if (joinRequest.State != request.State)
        joinRequest.State = request.State;
    if (joinRequest.UserId != request.UserId)
        joinRequest.UserId = request.UserId;
    if (joinRequest.CompanyId != request.CompanyId)
        joinRequest.CompanyId = request.CompanyId;
}

static IEnumerable<JoinRequestResponse> getResponses(IEnumerable<JoinRequest> joinRequests) 
{
    var resultado =new List<JoinRequestResponse>();
    foreach (var item in joinRequests)
    {
        resultado.Add(getResponse(item));
    }
    return resultado;
}

static JoinRequestResponse getResponse(JoinRequest joinRequest)
{
    var response = new JoinRequestResponse();
    response.State = joinRequest.State.ToString();
    response.Id = joinRequest.Id;
    response.Created = joinRequest.Created;
    response.Modified = joinRequest.Modified;
    response.UserId = joinRequest.UserId;
    response.CompanyId = joinRequest.CompanyId;
    return response;
}

public class JoinRequestApplicant
{
    [JsonPropertyName("modifiedByUserId")]
    public int ApplicantUserId { get; set; }
}

public class JoinRequestResponse 
{
    public int Id { get; set; }
    public DateTime? Created { get; set; }
    public DateTime? Modified { get; set; }
    public string State { get; set; }
    public int UserId { get; set; }
    public int CompanyId { get; set; }
}


