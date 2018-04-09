using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using pokeinfo.Models;

namespace pokeinfo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("pokemon/{pokeid}")]
        public IActionResult QueryPoke(int pokeid)
        {
            var PokeInfo = new Dictionary<string, object>();
            PokeRequest.GetPokemonDataAsync(pokeid, ApiResponse =>
            {
                PokeInfo = ApiResponse;
            }
            ).Wait();
            
            Dictionary<string, object> pokecast = PokeInfo as Dictionary<string, object>;

            ViewBag.name = pokecast["name"];
            JArray types = (JArray)pokecast["types"];

            List<string> listTypes = new List<string>();
            for(int i =0; i<types.Count; i++)
            {
                listTypes.Add((string)types[i]["type"]["name"]);
            }

            ViewBag.type = listTypes;

            JObject sprites = (JObject)pokecast["sprites"];
            ViewBag.sprite = sprites["front_default"];
            ViewBag.height = pokecast["height"];
            ViewBag.weight = pokecast["weight"];

            return View("Index");
        }
        [HttpGet]
        [Route("pokemon/json/{pokeid}")]
        public JsonResult JQueryPoke(int pokeid)
        {
            var PokeInfo = new Dictionary<string, object>();
            PokeRequest.GetPokemonDataAsync(pokeid, ApiResponse =>
            {
                PokeInfo = ApiResponse;
            }
            ).Wait();

            return Json(PokeInfo);
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
