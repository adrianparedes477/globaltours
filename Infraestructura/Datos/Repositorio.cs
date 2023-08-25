using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Especificaciones;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Datos
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            
        }
        public async Task<T> ObtenerAsync(int id)
        {
            return await _db.Set<T>().FindAsync(id);
        }

        public async Task<IReadOnlyList<T>> ObtenerTodoAsync()
        {
            return await _db.Set<T>().ToListAsync();
        }

         public async Task<T> ObtenerEspec(IEspecificacion<T> espec)
        {
            return await AplicarEspecificaciones(espec).FirstOrDefaultAsync();
        }

        public async Task<IReadOnlyList<T>> ObtenerTodosEspec(IEspecificacion<T> espec)
        {
            return await AplicarEspecificaciones(espec).ToListAsync();
        }

        private IQueryable<T> AplicarEspecificaciones(IEspecificacion<T> espec)
        {
            return EvaluadorEspecificacion<T>.GetQuery(_db.Set<T>().AsQueryable(), espec);
        }
    }
}