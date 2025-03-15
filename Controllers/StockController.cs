using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyWebApi.Data;
using MyWebApi.DTOs.Stock;
using MyWebApi.Helpers;
using MyWebApi.Intefaces;
using MyWebApi.Mappers;

namespace MyWebApi.Controllers
{
    

    [Route("api/Stock")]
    [ApiController]

    
    public class StockController : ControllerBase  
    {
        private readonly ApplicationDbContext _context;
        private readonly IStockRepository _stockRepo;

        public StockController(ApplicationDbContext context , IStockRepository stock)
        {
            _context = context;
            _stockRepo = stock;
            
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> GetAll([FromQuery] QueryObject query) {
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stocks = await _stockRepo.GetAllAsync(query) ;

            var stockModels = stocks.Select(a => a.ToStockDto()).ToList();

            return Ok(stockModels);
        }


        [HttpGet("{id}")]

        public async Task<IActionResult> GetById([FromRoute] int id){

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stock = await _stockRepo.GetByIdAsync(id);

            if(stock == null){
                return NotFound();
            }
            return Ok(stock.ToStockDto());
        }



        [HttpPost]

        public async  Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto){

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stockModel = stockDto.ToStockFromCreateDto();
            await _stockRepo.CreateAsync(stockModel);
            return CreatedAtAction(nameof(GetById) , new  {Id = stockModel.Id} , stockModel.ToStockDto() );
        }


        [HttpPut] 
        [Route("{id}")]
        
        public async Task<IActionResult> Update([FromRoute] int id , [FromBody] UpdateStockRequestDto updateDto) {

            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepo.UpdateAsync(id , updateDto );

            if(stockModel == null) {
                return NotFound();
            }

                stockModel.Symbol = updateDto.Symbol;
                stockModel.CompanyName = updateDto.CompanyName;
                stockModel.Purchase = updateDto.Purchase;
                stockModel.LastDiv = updateDto.LastDiv;
                stockModel.Industry = updateDto.Industry;
                stockModel.MarketCap = updateDto.MarketCap;

            _context.SaveChanges();

            return Ok(stockModel.ToStockDto());

        }


        [HttpDelete]
        [Route("{id}")]

        public async Task<IActionResult> Delete([FromRoute] int id){
            
            if(!ModelState.IsValid){
                return BadRequest(ModelState);
            }

            var stockModel = await _stockRepo.DeleteAsync(id);

            if(stockModel == null){
                return NotFound();
            }  
            
            return NoContent();
        }
        
    }
}