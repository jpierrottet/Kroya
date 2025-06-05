using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using BlazorProyectoPostres.Data;
using BlazorProyectoPostres.Clases;
using BlazorProyectoPostres.Models.DatabaseModels;
using System.Collections.Generic;
using Radzen;


namespace BlazorProyectoPostres.Pages.AdministracionKroya
{
    public partial class AdministracionKroya : ComponentBase
    {
        [Inject]
        public PostresService ServicioPostre { get; set; }
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }


        protected Radzen.Blazor.RadzenDataGrid<MENU> grid;
        protected MENU menuToInsert;

        public List<MENU> ListaMenus { get; set; } = new List<MENU>();

        protected override async Task OnInitializedAsync()
        {

            ListaMenus = await ServicioPostre.GetMenuPostrestAsync();
        }


        protected async Task AddNewMenuItem()
        {

            menuToInsert = new MENU();

            if (grid != null)
            {
                await grid.InsertRow(menuToInsert);
            }
            else
            {

                Console.WriteLine("El DataGrid (grid) es nulo.");
            }


        }


        protected async Task OnRowCreatePlaceholder(MENU nuevoMenu)
        {

            NotificationService.Notify(new NotificationMessage
            {
                Severity = NotificationSeverity.Info,
                Summary = "Información",
                Detail = "Guardado de nuevo ítem aún no implementado. El diálogo/fila se cerrará.",
                Duration = 4000
            });

            if (grid.IsRowInEditMode(nuevoMenu))
            {
                grid.CancelEditRow(nuevoMenu);
            }


            await Task.CompletedTask;
        }

        protected async Task OnRowEditPlaceholder(MENU EditarMenu)
        {

            await Task.CompletedTask;

        }

        protected async Task OnRowUpdate(MENU MenuEditar)
        {
            await grid.Reload();

        }

        protected async Task EditRow(MENU menuItem)
        {
            if (grid != null)
            {
                await grid.EditRow(menuItem); // Pone la fila en modo de edición inline
            }
        }

        protected async Task DeleteRow(MENU menuItem)
        {
            var confirmResult = await DialogService.Confirm(
                $"¿Estás seguro de que quieres eliminar '{menuItem.MEN_NOMBRE}'?",
                "Confirmar Eliminación",
                new ConfirmOptions() { OkButtonText = "Sí, Eliminar", CancelButtonText = "No" });

            if (confirmResult == true)
            {
                await ServicioPostre.EliminarMenu(menuItem);
                ListaMenus = await ServicioPostre.GetMenuPostrestAsync();
                await grid.Reload();

            }
        }

        protected async Task SaveRow(MENU menuItem)
        {

            if (grid != null)
            {
                await grid.UpdateRow(menuItem);
            }

        }

        private async Task AbrirPopup()
        {
            var result = await DialogService.OpenAsync<PopUpFormularioPostre>(
                "Añadir Nuevo Postre",
                null,
                new DialogOptions { Width = "700px", Height = "auto", Resizable = true, Draggable = true, CloseDialogOnEsc = true }
            );

            if (result != null)
            {
                var NuevoMenu = result.Menu as MENU;
                var ArchivosMenu = result.Archivos as List<ARCHIVO>;
                await ServicioPostre.InsertarMenuAsync(NuevoMenu, ArchivosMenu);
                if (grid != null)
                {
                    ListaMenus = await ServicioPostre.GetMenuPostrestAsync();
                    await grid.Reload();
                }
                FuncionesGenerales.Notificacion(NotificationService, "Postre añadido correctamente.", NotificationSeverity.Success);
                
                
            }
            else
            {
                Console.WriteLine("Diálogo cerrado sin resultado (cancelado).");
            }
        }

        private async Task AbrirPopupEdicion(MENU menuEditar)
        {
            var parameters = new Dictionary<string, object>();

            parameters.Add("MenuAEditar", menuEditar);
            var result = await DialogService.OpenAsync<PopUpFormularioPostre>(
                "Editar Postre",
                parameters,
                new DialogOptions { Width = "700px", Height = "auto", Resizable = true, Draggable = true, CloseDialogOnEsc = true }
            );

            if (result != null)
            {
                var MenuEditar = result.Menu as MENU;
                var ArchivosMenu = result.Archivos as List<ARCHIVO>;
                await ServicioPostre.EditarMenu(MenuEditar, ArchivosMenu);
                if (grid != null)
                {
                    ListaMenus = await ServicioPostre.GetMenuPostrestAsync();
                    await grid.Reload();
                }
            }

        }
        private async Task AbrirPopUpAcciones(MENU menuEditar)
        { 
            var AbrirPopUp = await DialogService.OpenAsync<PopUpAcciones>(
                "Acciones Postre",
                new Dictionary<string, object> { { "MenuAEditar", menuEditar } },
                new DialogOptions { Width = "400px", Height = "auto", Resizable = true, Draggable = true, CloseDialogOnEsc = true }
            );

        }




    }
}
