using HotelReservationAPi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HotelReservationAPi.Model.Entities;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservationAPi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;



        
        public ReservationsController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

        }






        /// <summary>
        /// This function lists all made reservations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await _reservationRepository.GetCustomers().ConfigureAwait(false));
        }



        /// <summary>
        /// This function returns the reservation whose "id" is given.
        /// </summary>
        /// <param name="id">It is a required area and so type is string</param>
        /// <returns>If function is succeded will be return Ok, than will be return NotFound</returns>
        [HttpGet("{id:length(24)}")]
        public async Task<IActionResult> GetById(string id)
        {
            var customer = await _reservationRepository.GetCustomer(id);

            if (customer is null)
            {
                return NotFound();
            }


            return Ok(customer);


        }

        /// <summary>
        /// You can add a new reservation using this method.
        /// </summary>
        /// <param></param>
        /// <returns>If function is succeded will be return CreatedAtAction, than will be return Bad Request</returns>        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservation customer)
        {
            if (customer is null)
            {
               return BadRequest();
            }

            await _reservationRepository.Create(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

        /// <summary>
        /// Using this method, you can edit and update the reservation whose "id" is specified.
        /// </summary>
        /// <param name="id">It is a required area and so type is int</param>
        /// <returns>If function is succeded will be return NoContent, than will be return Bad Request</returns>
        [HttpPut("{id:length(24)}")]
        public async Task<IActionResult> Update(string id, [FromBody] Reservation customer)
        {
            var existingCustomer = await _reservationRepository.GetCustomer(id);
            if (existingCustomer is null)
            {
                return BadRequest();
            }

            await _reservationRepository.Update(id, customer);

            return NoContent();
        }


        /// <summary>
        /// This function can remove your reservation. 
        /// </summary>
        /// <param name="id">It is a required area and so type is int</param>
        /// <returns>If function is succeded will be return NoContent, than will be return NotFound</returns>        
        [HttpDelete("{id:length(24)}")]
        public async Task<IActionResult> Delete(string id)
        {
            var customer = await _reservationRepository.GetCustomer(id);

            if (customer is null)
            {
                return NotFound();
            }

            await _reservationRepository.Delete(customer.Id);

            return NoContent();
        }
    }
}
