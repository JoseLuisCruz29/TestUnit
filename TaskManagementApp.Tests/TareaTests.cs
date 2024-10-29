using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace TaskManagementApp.Tests
{
    [TestClass]
    public class TareaTests
    {
        private List<Tarea> tareas;

        [TestInitialize]
        public void Setup()
        {
            tareas = new List<Tarea>();
        }

        [TestMethod]
        public void CrearTarea_DeberiaAgregarTareaALaLista()
        {
            var nuevaTarea = new Tarea { Titulo = "Tarea de prueba", Descripcion = "Descripción de prueba" };
            nuevaTarea.Id = tareas.Count > 0 ? tareas.Max(t => t.Id) + 1 : 1;
            tareas.Add(nuevaTarea);

            Assert.AreEqual(1, tareas.Count);
            Assert.AreEqual("Tarea de prueba", tareas[0].Titulo);
        }

        [TestMethod]
        public void CrearTarea_SinDescripcion_DeberiaAgregarTareaConDescripcionNula()
        {
            var nuevaTarea = new Tarea { Titulo = "Tarea sin descripción" };
            nuevaTarea.Id = tareas.Count > 0 ? tareas.Max(t => t.Id) + 1 : 1;
            tareas.Add(nuevaTarea);

            Assert.AreEqual(1, tareas.Count);
            Assert.IsNull(tareas[0].Descripcion);
        }

        [TestMethod]
        public void ObtenerTareaPorId_DeberiaRetornarTareaCorrecta()
        {
            var tarea = new Tarea { Id = 1, Titulo = "Tarea 1", Descripcion = "Descripción 1" };
            tareas.Add(tarea);

            var tareaObtenida = tareas.FirstOrDefault(t => t.Id == 1);

            Assert.IsNotNull(tareaObtenida);
            Assert.AreEqual("Tarea 1", tareaObtenida.Titulo);
        }

        [TestMethod]
        public void ObtenerTareaPorId_Inexistente_DeberiaRetornarNulo()
        {
            var tareaObtenida = tareas.FirstOrDefault(t => t.Id == 99);

            Assert.IsNull(tareaObtenida);
        }

        [TestMethod]
        public void ActualizarTarea_DeberiaCambiarPropiedadesDeTarea()
        {
            var tarea = new Tarea { Id = 1, Titulo = "Tarea Inicial", Descripcion = "Descripción inicial" };
            tareas.Add(tarea);

            var tareaActualizada = tareas.FirstOrDefault(t => t.Id == 1);
            if (tareaActualizada != null)
            {
                tareaActualizada.Titulo = "Tarea Actualizada";
                tareaActualizada.Descripcion = "Descripción actualizada";
                tareaActualizada.Completada = true;
            }

            Assert.AreEqual("Tarea Actualizada", tareaActualizada.Titulo);
            Assert.AreEqual("Descripción actualizada", tareaActualizada.Descripcion);
            Assert.IsTrue(tareaActualizada.Completada);
        }

        [TestMethod]
        public void ActualizarTarea_NoExistente_NoDebeHacerNada()
        {
            var tarea = tareas.FirstOrDefault(t => t.Id == 99);
            Assert.IsNull(tarea); // Verifica que no se encontró la tarea
        }

        [TestMethod]
        public void EliminarTarea_DeberiaQuitarTareaDeLaLista()
        {
            var tarea = new Tarea { Id = 1, Titulo = "Tarea para eliminar", Descripcion = "Descripción de tarea" };
            tareas.Add(tarea);

            var tareaAEliminar = tareas.FirstOrDefault(t => t.Id == 1);
            if (tareaAEliminar != null)
            {
                tareas.Remove(tareaAEliminar);
            }

            Assert.AreEqual(0, tareas.Count);
        }

        [TestMethod]
        public void EliminarTarea_NoExistente_NoDebeCambiarLista()
        {
            int countInicial = tareas.Count;

            var tareaAEliminar = tareas.FirstOrDefault(t => t.Id == 99);
            if (tareaAEliminar != null)
            {
                tareas.Remove(tareaAEliminar);
            }

            Assert.AreEqual(countInicial, tareas.Count);
        }

        [TestMethod]
        public void CrearTarea_ConIdDuplicado_NoDebeAgregarTarea()
        {
            tareas.Add(new Tarea { Id = 1, Titulo = "Tarea existente" });

            var nuevaTarea = new Tarea { Id = 1, Titulo = "Tarea duplicada" };

            if (!tareas.Any(t => t.Id == nuevaTarea.Id))
            {
                tareas.Add(nuevaTarea);
            }

            Assert.AreEqual(1, tareas.Count);
        }

        [TestMethod]
        public void ContarTareasPendientes_DeberiaRetornarCantidadCorrecta()
        {
            tareas.Add(new Tarea { Id = 1, Titulo = "Tarea 1", Completada = false });
            tareas.Add(new Tarea { Id = 2, Titulo = "Tarea 2", Completada = true });
            tareas.Add(new Tarea { Id = 3, Titulo = "Tarea 3", Completada = false });

            int pendientes = tareas.Count(t => !t.Completada);

            Assert.AreEqual(2, pendientes);
        }

        [TestMethod]
        public void ContarTareasCompletadas_DeberiaRetornarCantidadCorrecta()
        {
            tareas.Add(new Tarea { Id = 1, Titulo = "Tarea 1", Completada = false });
            tareas.Add(new Tarea { Id = 2, Titulo = "Tarea 2", Completada = true });
            tareas.Add(new Tarea { Id = 3, Titulo = "Tarea 3", Completada = true });

            int completadas = tareas.Count(t => t.Completada);

            Assert.AreEqual(2, completadas);
        }

        [TestMethod]
        public void ActualizarEstadoTarea_DeberiaCambiarCompletada()
        {
            var tarea = new Tarea { Id = 1, Titulo = "Tarea a actualizar", Completada = false };
            tareas.Add(tarea);

            var tareaActualizada = tareas.FirstOrDefault(t => t.Id == 1);
            if (tareaActualizada != null)
            {
                tareaActualizada.Completada = true;
            }

            Assert.IsTrue(tareaActualizada.Completada);
        }

        [TestMethod]
        public void AgregarTarea_ConTituloVacio_NoDebeAgregar()
        {
            var nuevaTarea = new Tarea { Titulo = "" };

            if (!string.IsNullOrWhiteSpace(nuevaTarea.Titulo))
            {
                tareas.Add(nuevaTarea);
            }

            Assert.AreEqual(0, tareas.Count);
        }

        [TestMethod]
        public void ObtenerTodasLasTareas_DeberiaRetornarListaCompleta()
        {
            tareas.Add(new Tarea { Id = 1, Titulo = "Tarea 1" });
            tareas.Add(new Tarea { Id = 2, Titulo = "Tarea 2" });

            var todasLasTareas = tareas.ToList();

            Assert.AreEqual(2, todasLasTareas.Count);
        }

        [TestMethod]
        public void MarcarTodasComoCompletadas_DeberiaCambiarEstadoTodasLasTareas()
        {
            tareas.Add(new Tarea { Id = 1, Titulo = "Tarea 1", Completada = false });
            tareas.Add(new Tarea { Id = 2, Titulo = "Tarea 2", Completada = false });

            foreach (var tarea in tareas)
            {
                tarea.Completada = true;
            }

            Assert.IsTrue(tareas.All(t => t.Completada));
        }
    }

    public record Tarea
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool Completada { get; set; } = false;
    }
}
