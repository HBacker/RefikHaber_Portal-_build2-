using Microsoft.AspNetCore.Mvc;
using RefikHaber.Models; // Haber ve IHaberRepository için
using System.Collections.Generic;
using RefikHaber.Repostories;
using Microsoft.AspNetCore.Mvc;
using RefikHaber.Models;
using System.Collections.Generic;
using RefikHaber.Repostories;

namespace RefikHaber.Controllers
{
    // GET: api/v1/haber/{id}
    [Route("api/v1/[controller]")]  
    [ApiController]
    public class HaberController : ControllerBase
    {
        private readonly IHaberRepository _haberRepository;

        public HaberController(IHaberRepository haberRepository)
        {
            _haberRepository = haberRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Haber>> GetAllNews()
        {
            var haberler = _haberRepository.GetAll();
            return Ok(haberler);
        }
    }
}
