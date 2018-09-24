using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DESAPI.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace DESAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private ICryptoService _cryptoService;

        public CryptoController(ICryptoService cryptoService)
        {
            _cryptoService = cryptoService;
        }

        [HttpGet("encrypt")]
        public ActionResult<string> Encrypt([FromQuery]string input, [FromQuery]string keyString)
        {
            return _cryptoService.Encrypt(input, keyString);
        }

        [HttpGet("decrypt")]
        public ActionResult<string> Decrypt([FromQuery]string input, [FromQuery]string keyString)
        {
            return _cryptoService.Decrypt(input, keyString);
        }
    }
}
