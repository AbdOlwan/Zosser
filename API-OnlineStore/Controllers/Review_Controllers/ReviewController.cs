using API_OnlineStore.Common;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Cart_F;
using BLL_OnlineStore.DTOs.EntitiesDTOs.Review_F;
using BLL_OnlineStore.Interfaces.ReviewBusServices;
using DAL_OnlineStore.Entities.Models.CartModels;
using DAL_OnlineStore.Entities.Models.ReviewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace API_OnlineStore.Controllers.Review_Controllers
{
    [Route("api/v1/reviews")]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewServices _service;

        public ReviewController(IReviewServices service)
        {
            _service = service;
        }

        // =================================================
        // Get All Reviews : 
        // =================================================
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<ReviewDTO>>> GetAllReviewsAsync()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            try
            {
                var Reviews = await _service.GetAllReviews();
                if (Reviews == null || Reviews.Count == 0)
                {
                    return NotFound(new ApiResponse<IEnumerable<ReviewDTO>>
                    {
                        Success = false,
                        Message = $"No Reviews found.",
                        Errors = ["Empty result"]
                    });
                }
                return Ok(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = true,
                    Message = " retrieved successfully",
                    Data = Reviews
                });
            }
            catch (Exception)
            {
                return StatusCode(500, "An error occurred while retrieving Reviews.");
            }
        }
        // =================================================
        // Get  Review By Id: 
        // =================================================

        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<ReviewDTO>> GetReviewByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.GetReviewById(id);
            if (result == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = false,
                    Message = $"There is no Review With Id {id}.",
                    Errors = ["Empty result"]
                });
            }

            return Ok(new ApiResponse<ReviewDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = result
            });
        }

        // =================================================
        // Add New Review : 
        // =================================================
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<ActionResult<ReviewDTO>> AddNewReviewAsync([FromBody] ReviewDTO Review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            // أولًا: التحقق من أن الكائن نفسه وبياناته الأساسية موجودة
            if (Review == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = false,
                    Message = $"Not Accepted: Review data is missing or Name is empty.",
                    Errors = ["Empty result"]
                });
            }
            //var PhoneNum = newDoctor.Phone.Trim().ToString();

            var NewReview = await _service.AddNewReview(Review);
            if (NewReview == null)
            {
                return BadRequest(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = false,
                    Message = $"Review Not Added",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<ReviewDTO>
            {
                Success = true,
                Message = " retrieved successfully",
                Data = NewReview
            });
        }

        // =================================================
        // Update Review By Id: 
        // =================================================

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> UpdateReviewByIdAsync(ReviewDTO Review)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.UpdateReviewById(Review);
            if (!result)
            {
                return BadRequest(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = false,
                    Message = $"Sorry Review Didnot Updated!!",
                    Errors = ["Empty result"]
                });
            }
            return Ok(new ApiResponse<ReviewDTO>
            {
                Success = true,
                Message = "Review Updated Successfully",
                Data = Review
            });
        }

        // =================================================
        // Delete Review By Id: 
        // =================================================
        //here we use HttpDelete method

        [HttpDelete("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<string>> DeleteReviewByIdAsync(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _service.DeleteReviewById(id);
            if (!result)
                return BadRequest(new ApiResponse<IEnumerable<ReviewDTO>>
                {
                    Success = false,
                    Message = $"Sorry Review Didnot Deleted! Please Try Again!!",
                    Errors = ["Empty result"]
                });

            return Ok(new ApiResponse<ReviewDTO>
            {
                Success = true,
                Message = $"Review With Id {id} Deleted Successfully",
                Data = null
            });
        }

    }
}
