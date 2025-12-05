using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulkyBookWeb.Controllers.Api
{
    [ApiController]
    [Route("api/kindness")]
    [Authorize]
    public class KindnessApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public KindnessApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        /// <summary>
        /// Get all kindness positions
        /// </summary>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var positions = _unitOfWork.Kindness.GetAll().ToList();
                var paginatedPositions = positions
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                return Ok(new
                {
                    success = true,
                    data = paginatedPositions,
                    total = positions.Count,
                    page = page,
                    pageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Query kindness positions by floor and section
        /// </summary>
        [HttpGet("positions/query")]
        [AllowAnonymous]
        public IActionResult QueryPositions([FromQuery] string? floor = null, [FromQuery] string? section = null)
        {
            try
            {
                var positions = _unitOfWork.Kindness.GetAll().ToList();

                if (!string.IsNullOrEmpty(floor))
                    positions = positions.Where(p => p.Floor == floor).ToList();

                if (!string.IsNullOrEmpty(section))
                    positions = positions.Where(p => p.Section == section).ToList();

                return Ok(new
                {
                    success = true,
                    data = positions,
                    total = positions.Count
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Get kindness position by ID
        /// </summary>
        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetById(int id)
        {
            try
            {
                var position = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == id);
                if (position == null)
                    return NotFound(new { success = false, message = "Position not found" });

                return Ok(new { success = true, data = position });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Create new kindness position (admin only)
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public IActionResult Create([FromBody] KindnessPosition position)
        {
            try
            {
                if (position == null)
                    return BadRequest(new { success = false, message = "Position data is required" });

                _unitOfWork.Kindness.Add(position);
                _unitOfWork.Save();

                return Ok(new { success = true, data = position, message = "Position created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Update kindness position (admin only)
        /// </summary>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id, [FromBody] KindnessPosition position)
        {
            try
            {
                if (position == null)
                    return BadRequest(new { success = false, message = "Position data is required" });

                var existingPosition = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == id);
                if (existingPosition == null)
                    return NotFound(new { success = false, message = "Position not found" });

                existingPosition.Floor = position.Floor;
                existingPosition.Section = position.Section;
                existingPosition.Level = position.Level;
                existingPosition.Position = position.Position;

                _unitOfWork.Kindness.Update(existingPosition);
                _unitOfWork.Save();

                return Ok(new { success = true, data = existingPosition, message = "Position updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// Delete kindness position (admin only)
        /// </summary>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            try
            {
                var position = _unitOfWork.Kindness.Get(u => u.KindnessPositionId == id);
                if (position == null)
                    return NotFound(new { success = false, message = "Position not found" });

                _unitOfWork.Kindness.Remove(position);
                _unitOfWork.Save();

                return Ok(new { success = true, message = "Position deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
