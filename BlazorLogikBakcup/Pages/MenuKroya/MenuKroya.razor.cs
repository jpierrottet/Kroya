using BlazorProyectoPostres.Data;
using BlazorProyectoPostres.Models.DatabaseModels;
using BlazorProyectoPostres.Models;
using Microsoft.AspNetCore.Components;
using Radzen;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BlazorProyectoPostres.Pages.MenuKroya
{
    public partial class MenuKroya : ComponentBase
    {
        [Inject]
        public PostresService ServicioPostre { get; set; }
        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        public List<MenuPrincipalViewModel> MenusActivos { get; set; } = new List<MenuPrincipalViewModel>();

        public MenuPrincipalViewModel? selectedMenu { get; set; }


        protected override async Task OnInitializedAsync()
        {

            MenusActivos = await ServicioPostre.ObtenerMenusActivos();
        }

        public void SelectMenu(MenuPrincipalViewModel menu)
        {
            selectedMenu = (selectedMenu == menu) ? null : menu;
            StateHasChanged(); 
        }

    }
}
