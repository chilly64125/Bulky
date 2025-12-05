using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BulkyBookWeb.Controllers.Api
{
    [Route("api/order")]
    [ApiController]
    [Authorize(Roles = SD.Role_Admin)]
    public class OrderApiController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderApiController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// GET /api/order
        /// Returns all orders with pagination
        /// </summary>
        [HttpGet]
        public ActionResult<dynamic> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            try
            {
                var orders = _unitOfWork.OrderHeader.GetAll().ToList();
                var pagedOrders = orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                var totalCount = orders.Count;

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        items = pagedOrders,
                        totalCount,
                        pageNumber,
                        pageSize,
                        totalPages = (totalCount + pageSize - 1) / pageSize
                    },
                    message = "Orders retrieved successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// GET /api/order/{id}
        /// Returns a specific order by ID
        /// </summary>
        [HttpGet("{id}")]
        public ActionResult<dynamic> GetById(int id)
        {
            try
            {
                var order = _unitOfWork.OrderHeader.Get(u => u.Id == id);
                if (order == null)
                    return NotFound(new { success = false, message = "Order not found" });

                return Ok(new { success = true, data = order, message = "Order retrieved successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// POST /api/order
        /// Creates a new order
        /// </summary>
        [HttpPost]
        public ActionResult<dynamic> Create([FromBody] OrderHeader orderHeader)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid order data", errors = ModelState.Values.SelectMany(v => v.Errors) });

                _unitOfWork.OrderHeader.Add(orderHeader);
                _unitOfWork.Save();

                return CreatedAtAction(nameof(GetById), new { id = orderHeader.Id },
                    new { success = true, data = orderHeader, message = "Order created successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// PUT /api/order/{id}
        /// Updates an existing order
        /// </summary>
        [HttpPut("{id}")]
        public ActionResult<dynamic> Update(int id, [FromBody] OrderHeader orderHeader)
        {
            try
            {
                if (id != orderHeader.Id)
                    return BadRequest(new { success = false, message = "ID mismatch" });

                if (!ModelState.IsValid)
                    return BadRequest(new { success = false, message = "Invalid order data", errors = ModelState.Values.SelectMany(v => v.Errors) });

                var existingOrder = _unitOfWork.OrderHeader.Get(u => u.Id == id);
                if (existingOrder == null)
                    return NotFound(new { success = false, message = "Order not found" });

                existingOrder.OrderDate = orderHeader.OrderDate;
                existingOrder.OrderTotal = orderHeader.OrderTotal;
                existingOrder.OrderStatus = orderHeader.OrderStatus;
                existingOrder.PaymentStatus = orderHeader.PaymentStatus;
                existingOrder.ApplicationUserId = orderHeader.ApplicationUserId;
                existingOrder.ShippingDate = orderHeader.ShippingDate;
                existingOrder.TrackingNumber = orderHeader.TrackingNumber;
                existingOrder.Carrier = orderHeader.Carrier;
                existingOrder.PaymentDate = orderHeader.PaymentDate;
                existingOrder.PaymentDueDate = orderHeader.PaymentDueDate;

                _unitOfWork.OrderHeader.Update(existingOrder);
                _unitOfWork.Save();

                return Ok(new { success = true, data = existingOrder, message = "Order updated successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        /// <summary>
        /// DELETE /api/order/{id}
        /// Deletes an order by ID
        /// </summary>
        [HttpDelete("{id}")]
        public ActionResult<dynamic> Delete(int id)
        {
            try
            {
                var order = _unitOfWork.OrderHeader.Get(u => u.Id == id);
                if (order == null)
                    return NotFound(new { success = false, message = "Order not found" });

                _unitOfWork.OrderHeader.Remove(order);
                _unitOfWork.Save();

                return Ok(new { success = true, data = order, message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
