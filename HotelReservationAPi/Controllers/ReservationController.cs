using HotelReservationAPi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using HotelReservationAPi.Model;

namespace HotelReservationAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationsController : ControllerBase
    {
        private readonly IReservationRepository _reservationRepository;

        public ReservationsController(IReservationRepository reservationRepository)
        {
            _reservationRepository = reservationRepository;

        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(_reservationRepository.GetCustomers());
        }

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

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Reservation customer)
        {
            //if (customer is null)
            //{
            //    return BadRequest();
            //}

            await _reservationRepository.Create(customer);

            return CreatedAtAction(nameof(GetById), new { id = customer.Id }, customer);
        }

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
