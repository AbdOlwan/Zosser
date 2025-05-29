using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using System.Threading;

namespace API_OnlineStore.Controllers.Shipment_Controllers
{
    [Route("api/v1/carriers")]
    [ApiController]
    public class CarrierController : ControllerBase
    {
        private readonly ICarrierServices _service;

        public CarrierController(ICarrierServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Carriers : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<CarrierDTO>>> GetAllCarriersAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            try
            {
                var carier = await _service.GetAllCarriers();
                if (carier == null || !carier.Any())
                {
                    return NotFound(new ApiResponse(400, "No Carriers Found"));
                }
                return Ok(new ApiResponse(200, carier));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, $"An error occurred while retrieving Carriers: {ex.Message}"));
            }

        }



        // =================================================
        // Get  Carrier By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<CarrierDTO>> GetCarrierByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.GetCarrierById(id);
            if (result == null)
            {
                return NotFound(new ApiResponse(400, "No Carriers Found"));

            }
            return Ok(new ApiResponse(200, result));

        }

        // =================================================
        // Add New Carrier : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<CarrierDTO>> AddNewCarrierAsync([FromBody] CarrierDTO Carrier)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Carrier == null || string.IsNullOrEmpty(Carrier.Name))
            {
                return BadRequest(new ApiResponse(400, "Bad Request"));

            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCarrier = await _service.AddNewCarrier(Carrier);
            if (NewCarrier == null)
            {
                return BadRequest(new ApiResponse(400, "Bad Request"));

            }
            return Ok(new ApiResponse(200, NewCarrier));
        }


        // =================================================
        // Update Carrier By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateCarrierByIdAsync(CarrierDTO Carrier)
        {
            if (!ModelState.IsValid)
                return BadRequest(new ApiResponse(400, ModelState));

            var result = await _service.UpdateCarrierById(Carrier);
            if (!result)
            {
                return BadRequest(new ApiResponse(400, "Bad Request"));
            }
            return Ok(new ApiResponse(200, result));
        }

        // =================================================
        // Delete Carrier By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteCarrierByIdAsync(int id)
        {


            var result = await _service.DeleteCarrierById(id);
            if (!result)
                return BadRequest(new ApiResponse(400, "Bad Request"));

            return Ok(new ApiResponse(200, result));

        }
    }
}
