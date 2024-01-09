using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using apiUniversidade.Context;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace apiUniversidade.Controllers


{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/{v:apiversion}/curso")]
    public class CursoController : Controller
    
    {
       private readonly ILogger<CursoController> _logger;

         private readonly apiUniversidadeContext _context;

         public CursoController(ILogger<CursoController> logger, apiUniversidadeContext context)

         {
            _logger = logger;
            _context = context;
         }
         
        public string GetExemplo ()
        {
            return "Api v1";
        }

        [HttpGet]
        public ActionResult<IEnumerable<Curso>> Get()
        {
            var Cursos = _context.Cursos.ToList();
            if (Cursos is null)
                return NotFound();
            
            return Cursos;

        }   

        [HttpPost]
        public ActionResult Post (Curso curso){
            _context.Cursos.Add(curso);
            _context.SaveChanges();

            return new CreatedAtRouteResult ("GetCurso", new {id = curso.Id},curso);

        }

        //PROCURA ALGO NO BANCO DE DADOS. RETORNA NULO SE NÃO ENCONTRAR NADA

        [HttpGet("{id:int}", Name = "GetCurso")]
        public ActionResult<Curso> Get(int id)
        {
            var curso = _context.Cursos.FirstOrDefault(p=>p.Id == id);
            if(curso is null)
                return NotFound("Curso não encontrado");
            return curso;

        }

        [HttpPut("id:int")]
        public ActionResult Put ( int id, Curso curso){
            if(id != curso.Id)
                return BadRequest();
            
            _context.Entry(curso).State = EntityState.Modified;
            _context.SaveChanges(); 

            return Ok (curso);

        }

        [HttpDelete ("{id:int}")] 

         public ActionResult Delete(int id) {
            var curso= _context.Cursos.FirstOrDefault (P=> P.Id == id);

            if (curso is null) {

                return NotFound();
             }
                _context.Cursos.Remove(curso);
                _context.SaveChanges();
                return Ok (curso);
        }
    }
}