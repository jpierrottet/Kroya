using System.Linq;
using System.Threading.Tasks;
using System;
using BlazorProyectoPostres.Models.DatabaseModels;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using BlazorProyectoPostres.Models;

namespace BlazorProyectoPostres.Data
{
    public class PostresService
    {


        private readonly ContextoTablasPostres _context;

        public PostresService(ContextoTablasPostres context)
        {
            _context = context;
        }
        public async Task<List<MENU>> GetMenuPostrestAsync()
        {
            var Menus = await _context.MENUs
                .Include(c => c.ARCHIVO_POSTREs)
                .Where(a => a.MEN_ESTADO).ToListAsync();
            return Menus;
        }

        // Adminsitracion
        public async Task InsertarMenuAsync(MENU menu, List<ARCHIVO> ArchivosMenu)
        {
            menu.MEN_ESTADO = true;
            _context.MENUs.Add(menu);
            await _context.SaveChangesAsync();

            if (ArchivosMenu.Any())
            {
                foreach (var Archivo in ArchivosMenu)
                {

                    var ArchivoMenu = new ARCHIVO_POSTRE
                    {
                        ARP_ARCHIVONavigation = Archivo,
                        ARP_MENU = menu.MEN_ID
                    };
                    _context.ARCHIVO_POSTREs.Add(ArchivoMenu);
                    await _context.SaveChangesAsync();
                }
            }
        }

        public async Task EditarMenu(MENU menu, List<ARCHIVO> ArchivosMenu)
        {
            var MenuEditar = await _context.MENUs.FirstOrDefaultAsync(a => a.MEN_ID == menu.MEN_ID);
            if (MenuEditar != null)
            {
                MenuEditar.MEN_NOMBRE = menu.MEN_NOMBRE;
                MenuEditar.MEN_DESCRIPCION = menu.MEN_DESCRIPCION;
                MenuEditar.MEN_PRECIO = menu.MEN_PRECIO;
                if (ArchivosMenu.Any())
                {
                    foreach (var Archivo in ArchivosMenu)
                    {

                        var ArchivoMenu = new ARCHIVO_POSTRE
                        {
                            ARP_ARCHIVONavigation = Archivo,
                            ARP_MENU = MenuEditar.MEN_ID
                        };
                        _context.ARCHIVO_POSTREs.Add(ArchivoMenu);

                    }
                }

            }
            await _context.SaveChangesAsync();


        }

        public async Task<List<ARCHIVO>> ObtenerArchivosPostre(List<long> idsArchivos)
        {
            var ArchivosMenu = await _context.ARCHIVOs.Where(a => idsArchivos.Contains(a.ARC_ID)).ToListAsync();


            return ArchivosMenu;
        }


        public async Task EliminarMenu(MENU menu)
        {
            var MenuDesactivar = _context.MENUs.FirstOrDefaultAsync(a => a.MEN_ID == menu.MEN_ID);
            if (MenuDesactivar != null)
            {
                MenuDesactivar.Result.MEN_ESTADO = false;
            }
            await _context.SaveChangesAsync();
        }

        public async Task EliminarArchivoPostre(ARCHIVO ArchivoAeliminar)
        {
            var ArchivoPostre = await _context.ARCHIVO_POSTREs
                .FirstOrDefaultAsync(a => a.ARP_ARCHIVO == ArchivoAeliminar.ARC_ID);
            _context.ARCHIVO_POSTREs.Remove(ArchivoPostre);

            _context.ARCHIVOs.Remove(ArchivoAeliminar);
            await _context.SaveChangesAsync();
        }


        // Menu Principal
        public async Task<List<MenuPrincipalViewModel>> ObtenerMenusActivos()
        {
            var Archivos = await _context.ARCHIVO_POSTREs
                .Include(c => c.ARP_ARCHIVONavigation)
                .Include(c => c.ARP_MENUNavigation)
                .Include(c=> c.ARP_MENUNavigation.PORCIONs)
                .Where(a => a.ARP_MENUNavigation.MEN_ESTADO).ToListAsync();

            var Modelo = new List<MenuPrincipalViewModel>();
            if (Archivos.Any())
            {
                Modelo = Archivos.GroupBy(c => c.ARP_MENUNavigation).Select(a => new MenuPrincipalViewModel
                {
                    NombreMenu = a.Key.MEN_NOMBRE,
                    Descripcion = a.Key.MEN_DESCRIPCION,
                    Precio = a.Key.MEN_PRECIO.Value,
                    Imagenes = a.Select(b => b.ARP_ARCHIVONavigation.ARC_ARCHIVO).ToList(),
                    Porciones = a.Key.PORCIONs.ToList()

                }).ToList();
            }
            return Modelo;
        }


        // Porciones
        public async Task<List<PORCION>> ObtenerPorcionesMenu(int idMenu)
        {
            var Porciones = await _context.PORCIONs
                .Where(a => a.POR_MENU == idMenu).ToListAsync();
            return Porciones;
        }

        public async Task ActualizarPorcion(PORCION porcion)
        {
            await _context.SaveChangesAsync();
        }

        public async Task AgregarPorcion(PORCION porcion)
        {
            _context.PORCIONs.Add(porcion);
            await _context.SaveChangesAsync();
        }

        public async Task EliminarPorcion(PORCION porcion)
        {
            _context.PORCIONs.Remove(porcion);
            await _context.SaveChangesAsync();
        }
    }
}
