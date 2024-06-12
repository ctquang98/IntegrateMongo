using IntegrateMongo1.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IntegrateMongo1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBooksService booksService;

        public BookController(IBooksService booksService)
        {
            this.booksService = booksService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] GetAllParams _params)
        {
            return Ok(await booksService.GetAll(_params));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetById([FromRoute] string id)
        {
            if (string.IsNullOrWhiteSpace(id)) return NotFound();
            var book = await booksService.GetById(id);
            return book == null ? NotFound() : Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> InsertOne([FromBody] AppBookDto bookDto)
        {
            var book = await booksService.Insert(bookDto);
            return Ok(book);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> updateOne([FromRoute] string id, [FromBody] AppBookDto bookDto)
        {
            if (id == null) return NotFound();
            var book = await booksService.GetById(id);
            if (book == null) return NotFound();
            return Ok(await booksService.Update(book.Id, bookDto));
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> deleteOne([FromRoute] string id)
        {
            if (string.IsNullOrEmpty(id)) return NotFound();
            var book = await booksService.GetById(id);
            if (book == null) return NotFound();
            return Ok(await booksService.Delete(book.Id));
        }
    }
}
