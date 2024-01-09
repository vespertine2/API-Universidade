using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using apiUniversidade.Model;
using Microsoft.AspNetCore.Mvc;
using apiUniversidade.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
namespace apiUniversidade.Controllers


{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [ApiController]
    [Route("api/[controller]")]

    public class AlunoController : Controller

    {
        private readonly ILogger<AlunoController> _logger;

         private readonly apiUniversidadeContext _context;

       public AlunoController(ILogger<AlunoController> logger, apiUniversidadeContext context)


        {
            _logger = logger;
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Aluno>> Get()
        {
            var Alunos = _context.Alunos.ToList();
            if (Alunos is null)
                return NotFound();
            
            return Alunos;

        } 

        [HttpPost]
        public ActionResult Post (Aluno aluno){

            _context.Alunos.Add(aluno);
            _context.SaveChanges();

            return new CreatedAtRouteResult ("GetAluno", new {id = aluno.Id},aluno);
                            
        }


         [HttpGet("{id:int}", Name = "GetAluno")]
        public ActionResult<Aluno> Get(int id)

        {
            var aluno = _context.Alunos.FirstOrDefault(p=>p.Id == id);
            if(aluno is null)
                return NotFound("Curso não encontrado");
            return aluno;

        }


        [HttpPut("id:int")]

        public ActionResult Put ( int id, Aluno aluno){
            if(id != aluno.Id)
                return BadRequest();
            
            _context.Entry(aluno).State = EntityState.Modified;
            _context.SaveChanges(); 

            return Ok (aluno);
        }

        [HttpDelete ("{id:int}")] 

         public ActionResult Delete(int id) {
            var aluno= _context.Alunos.FirstOrDefault (P=> P.Id == id);

            if (aluno is null) {

                return NotFound();
             }
                _context.Alunos.Remove(aluno);
                _context.SaveChanges();
                return Ok (aluno);
        }

    }
}