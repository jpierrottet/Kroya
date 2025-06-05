using Radzen;

namespace BlazorProyectoPostres.Clases
{
    public class FuncionesGenerales
    {

        public static void Notificacion(NotificationService servicioNotificacion, string mensaje, NotificationSeverity severidad = NotificationSeverity.Success, string mensajeError = "", int duracion = 4000)
        {
            if (servicioNotificacion == null) return; // Buena práctica añadir una comprobación

            servicioNotificacion.Notify(new NotificationMessage
            {
                Severity = severidad,
                Summary = mensaje,
                Detail = mensajeError,
                Duration = duracion,
                Style = "min-width: 450px; max-width: 600px; line-height: 1.6; padding: 10px;"
            });
        }
    }
}
