using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// Configuración de servicios y Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configuración del entorno de desarrollo para Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Almacenamiento en memoria para las tareas
var tareas = new List<Tarea>();

// Endpoints para la gestión de tareas

// Crear una tarea
app.MapPost("/tareas", (Tarea nuevaTarea) =>
{
    nuevaTarea.Id = tareas.Count > 0 ? tareas.Max(t => t.Id) + 1 : 1;
    tareas.Add(nuevaTarea);
    return Results.Created($"/tareas/{nuevaTarea.Id}", nuevaTarea);
})
.WithName("CrearTarea")
.WithOpenApi();

// Obtener todas las tareas
app.MapGet("/tareas", () =>
{
    return tareas;
})
.WithName("ObtenerTodasLasTareas")
.WithOpenApi();

// Obtener una tarea por ID
app.MapGet("/tareas/{id}", (int id) =>
{
    var tarea = tareas.FirstOrDefault(t => t.Id == id);
    return tarea is not null ? Results.Ok(tarea) : Results.NotFound();
})
.WithName("ObtenerTareaPorId")
.WithOpenApi();

// Actualizar una tarea por ID
app.MapPut("/tareas/{id}", (int id, Tarea tareaActualizada) =>
{
    var tarea = tareas.FirstOrDefault(t => t.Id == id);
    if (tarea is null) return Results.NotFound();

    tarea.Titulo = tareaActualizada.Titulo;
    tarea.Descripcion = tareaActualizada.Descripcion;
    tarea.Completada = tareaActualizada.Completada;

    return Results.Ok(tarea);
})
.WithName("ActualizarTarea")
.WithOpenApi();

// Eliminar una tarea por ID
app.MapDelete("/tareas/{id}", (int id) =>
{
    var tarea = tareas.FirstOrDefault(t => t.Id == id);
    if (tarea is null) return Results.NotFound();

    tareas.Remove(tarea);
    return Results.NoContent();
})
.WithName("EliminarTarea")
.WithOpenApi();

app.Run();

// Definición de la clase Tarea
record Tarea
{
    public int Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Completada { get; set; } = false;
}
