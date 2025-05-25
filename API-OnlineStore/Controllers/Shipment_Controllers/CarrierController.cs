using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Shipment_F;
using BLL_OnlineStore.Interfaces.ShipmentBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ShipmentModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

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
                return BadRequest(ModelState);
            try
            {
                var Carriers = await _service.GetAllCarriers();
                if (Carriers == null || Carriers.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<CarrierDTO>>
                    {
                        Success = false,
                        Message = $" No Carriers found.",
                        Errors = ["Empty result"]
                    });


                }
                return Ok(new ApiResponse<IEnumerable<CarrierDTO>>
                {
                    Success = true,
                    Message = "Carriers retrieved successfully",
                    Data = Carriers
                });

            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Carriers.");

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
                return BadRequest(ModelState);

            var result = await _service.GetCarrierById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CarrierDTO>>
                {
                    Success = false,
                    Message = $"There is no Carrier With Id {id}",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<CarrierDTO>
            {
                Success = true,
                Message = "Carriers retrieved successfully",
                Data = result
            });
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
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Carrier == null || string.IsNullOrEmpty(Carrier.Name))
            {
                return BadRequest(new ApiResponse<IEnumerable<CarrierDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Carrier data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewCarrier = await _service.AddNewCarrier(Carrier);
            if (NewCarrier == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<CarrierDTO>>
                {
                    Success = false,
                    Message = $"Carrier Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CarrierDTO>
            {
                Success = true,
                Message = "Carrier Added successfully",
                Data = NewCarrier
            });
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
                return BadRequest(ModelState);

            var result = await _service.UpdateCarrierById(Carrier);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<CarrierDTO>>
                {
                    Success = false,
                    Message = $"Sorry Carrier Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<CarrierDTO>
            {
                Success = true,
                Message = "Carrier Updated Successfully",
                Data = Carrier
            });
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
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteCarrierById(id);
            if (!result)
                 return BadRequest(new ApiResponse<IEnumerable<CarrierDTO>>
                 {
                     Success = false,
                     Message = $"Sorry Carrier Didnot Deleted! Please Try Again!!",
                     Errors = ["Empty result"]
                 });

            return Ok(new ApiResponse<CarrierDTO>
            {
                Success = true,
                Message = $"Carrier With Id {id} Deleted Successfully",
                Data = null
            });
        }
    }
}
